namespace OBE.Security;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Instructor = "Instructor";

    /// <summary>Roles that may be assigned. Public registration always uses <see cref="Instructor"/>.</summary>
    public static readonly IReadOnlyList<string> All = new[] { Admin, Instructor };

    /// <summary>
    /// Maps a stored role value to a canonical role name. Unknown/empty values fall back to
    /// the least-privileged role (Instructor), matching the DB default for users.role.
    /// </summary>
    public static string Normalize(string? role)
    {
        var match = All.FirstOrDefault(r => string.Equals(r, role?.Trim(), StringComparison.OrdinalIgnoreCase));
        return match ?? Instructor;
    }
}

public static class AppPolicies
{
    public const string AdminOnly = "AdminOnly";
    public const string AdminOrInstructor = "AdminOrInstructor";
}
