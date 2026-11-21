using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using OBE.Models;

namespace OBE.Security;

/// <summary>
/// OBE-02: password hashing and verification, including a migration path for accounts
/// whose users.password column still holds a legacy plaintext value.
/// Passwords are never logged.
/// </summary>
public class UserPasswordService
{
    private readonly IPasswordHasher<User> _hasher;
    private readonly AppDbContext _db;

    public UserPasswordService(IPasswordHasher<User> hasher, AppDbContext db)
    {
        _hasher = hasher;
        _db = db;
    }

    public string Hash(User user, string password) => _hasher.HashPassword(user, password);

    /// <summary>
    /// Verifies a password. A legacy plaintext value is compared in constant time and, on success,
    /// replaced with a hash (the caller's SaveChanges is not needed; this method saves).
    /// Hashes produced with outdated parameters are upgraded the same way.
    /// </summary>
    public bool Verify(User user, string password)
    {
        var stored = user.Password ?? string.Empty;

        if (IsIdentityHash(stored))
        {
            var result = _hasher.VerifyHashedPassword(user, stored, password);
            if (result == PasswordVerificationResult.Failed)
                return false;
            if (result == PasswordVerificationResult.SuccessRehashNeeded)
                SaveNewHash(user, password);
            return true;
        }

        // Legacy plaintext row.
        if (!FixedTimeEquals(stored, password))
            return false;

        SaveNewHash(user, password);
        return true;
    }

    /// <summary>Hashes every remaining plaintext password. Returns the number of rows changed.</summary>
    public int MigrateAllLegacyPasswords()
    {
        var count = 0;
        foreach (var user in _db.Users.ToList())
        {
            if (string.IsNullOrEmpty(user.Password) || IsIdentityHash(user.Password))
                continue;
            user.Password = _hasher.HashPassword(user, user.Password);
            user.UpdatedAt = DateTime.Now;
            count++;
        }
        if (count > 0)
            _db.SaveChanges();
        return count;
    }

    private void SaveNewHash(User user, string password)
    {
        user.Password = _hasher.HashPassword(user, password);
        user.UpdatedAt = DateTime.Now;
        _db.SaveChanges();
    }

    /// <summary>
    /// True when the value has the exact structure of an ASP.NET Core Identity hash
    /// (V2: 0x00 + 16-byte salt + 32-byte subkey; V3: 0x01 + header + salt + subkey).
    /// </summary>
    public static bool IsIdentityHash(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length % 4 != 0)
            return false;

        var buffer = new byte[value.Length];
        if (!Convert.TryFromBase64String(value, buffer, out var len) || len == 0)
            return false;

        if (buffer[0] == 0x00)
            return len == 1 + 16 + 32;

        if (buffer[0] == 0x01 && len >= 13)
        {
            var prf = ReadUInt32BE(buffer, 1);
            var saltLength = (int)ReadUInt32BE(buffer, 9);
            return prf <= 2 && saltLength >= 16 && len - 13 - saltLength >= 16;
        }
        return false;
    }

    private static uint ReadUInt32BE(byte[] b, int offset) =>
        (uint)(b[offset] << 24 | b[offset + 1] << 16 | b[offset + 2] << 8 | b[offset + 3]);

    private static bool FixedTimeEquals(string a, string b) =>
        CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(a), Encoding.UTF8.GetBytes(b));
}
