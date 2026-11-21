using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OBE.Models;
using OBE.Security;

namespace OBE.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserPasswordService _passwords;
        public UserController(AppDbContext db, UserPasswordService passwords)
        {
            _db = db;
            _passwords = passwords;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
         return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(string email,string password,int programId,string name_ar,string name_en)
        {
            if (password.Length < 6)
            {
                return BadRequest("Password must be at least 6 characters long.");
            }
            if (_db.Users.Any(u => u.Email == email))
            {
                return BadRequest("Email is already registered.");
            }
            if (string.IsNullOrEmpty(email)|| string.IsNullOrEmpty(password)|| string.IsNullOrEmpty(name_en)|| string.IsNullOrEmpty(name_ar))
            {
                return BadRequest("please enter All fileds");
            }
            var user = new User
            {
                Email = email,
                ProgramId = programId,
                NameAr = name_ar,
                NameEn = name_en,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                // OBE-05: public registration always gets the least-privileged role.
                Role = AppRoles.Instructor
            };
            // OBE-02: store only the hash, never the plaintext password.
            user.Password = _passwords.Hash(user, password);
            _db.Users.Add(user);
            _db.SaveChanges();
            return Ok("User registered successfully.");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login (string email , string password)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "you dont have account");
                return View();
            }

            // OBE-02: look the user up by email, then verify the password hash
            // (legacy plaintext rows are verified once and re-hashed).
            var user = string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)
                ? null
                : _db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || !_passwords.Verify(user, password))
            {
                ModelState.AddModelError("Email", "Email and password is not true");
                return View();
            }

            // OBE-01: issue an authentication cookie with identity and role claims.
            var role = AppRoles.Normalize(user.Role);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.NameEn ?? user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("GetProgram","Program");
        }

        // OBE-05: only administrators can change a user's role (e.g. promote to Admin).
        [HttpPost]
        [Authorize(Policy = AppPolicies.AdminOnly)]
        public IActionResult SetRole(int userId, string role)
        {
            var canonical = AppRoles.All.FirstOrDefault(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));
            if (canonical == null)
            {
                return BadRequest("Invalid role.");
            }
            var user = _db.Users.Find(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            user.Role = canonical;
            user.UpdatedAt = DateTime.Now;
            _db.SaveChanges();
            return Ok();
        }
    }
}
