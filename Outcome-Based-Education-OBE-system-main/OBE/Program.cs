using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using OBE.Models;
using OBE.Security;

var builder = WebApplication.CreateBuilder(args);

// OBE-03: the connection string comes only from configuration
// (User Secrets in Development, environment variable ConnectionStrings__MvcTest elsewhere).
var connectionString = builder.Configuration.GetConnectionString("MvcTest");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'MvcTest' is not configured. Set it with " +
        "'dotnet user-secrets set \"ConnectionStrings:MvcTest\" \"<connection string>\"' " +
        "or the environment variable 'ConnectionStrings__MvcTest'.");
}

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // OBE-01: every endpoint requires an authenticated user unless marked [AllowAnonymous].
    var requireAuthenticated = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(requireAuthenticated));

    // OBE-11: validate antiforgery tokens on all unsafe HTTP methods (POST/PUT/PATCH/DELETE).
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// OBE-11: fetch() calls send the token in this header.
builder.Services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// OBE-02: password hashing (PBKDF2, ASP.NET Core Identity format).
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<UserPasswordService>();

// OBE-01: cookie authentication with claims.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.AccessDeniedPath = "/User/Login";
        options.Cookie.Name = "OBE.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;

        // fetch() POSTs must get a real 401/403 instead of a redirect to an HTML page
        // (a followed redirect would look like a successful response to the scripts).
        options.Events.OnRedirectToLogin = context =>
        {
            if (HttpMethods.IsGet(context.Request.Method))
                context.Response.Redirect(context.RedirectUri);
            else
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppPolicies.AdminOnly, p => p.RequireRole(AppRoles.Admin));
    options.AddPolicy(AppPolicies.AdminOrInstructor, p => p.RequireRole(AppRoles.Admin, AppRoles.Instructor));
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // مدة بقاء البيانات
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// OBE-02 (optional, opt-in): hash all remaining plaintext passwords once at startup.
// Enable with configuration key Security:MigrateLegacyPasswordsOnStartup=true.
if (app.Configuration.GetValue<bool>("Security:MigrateLegacyPasswordsOnStartup"))
{
    using var scope = app.Services.CreateScope();
    var migrated = scope.ServiceProvider.GetRequiredService<UserPasswordService>().MigrateAllLegacyPasswords();
    app.Logger.LogInformation("Legacy password migration: {Count} account(s) re-hashed.", migrated);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
