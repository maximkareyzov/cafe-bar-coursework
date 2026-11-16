using System.Globalization;
using System.Security.Cryptography;

namespace CafeBar.Modules.StaffManagement.Logic;

/// <summary>
/// Хэширование паролей (ТЗ, п. 4 «Безопасность»): в базе хранится только хэш, сам пароль не сохраняется.
/// Алгоритм — PBKDF2 с HMAC-SHA256 и случайной солью для каждого пароля.
/// </summary>
public static class PasswordHasher
{
    private const string Algorithm = "PBKDF2-SHA256";
    private const int Iterations = 600_000;
    private const int SaltSize = 16;
    private const int HashSize = 32;

    /// <summary>Возвращает строку для Employees.PasswordHash: алгоритм$итерации$соль$хэш.</summary>
    public static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return string.Join('$', Algorithm, Iterations.ToString(CultureInfo.InvariantCulture), Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    /// <summary>Проверяет пароль по сохранённой строке. Число итераций берётся из самой строки.</summary>
    public static bool Verify(string password, string passwordHash)
    {
        string[] parts = passwordHash.Split('$');
        if (parts.Length != 4 || parts[0] != Algorithm) return false;
        if (!int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out int iterations) || iterations <= 0) return false;

        byte[] salt, expected;
        try
        {
            salt = Convert.FromBase64String(parts[2]);
            expected = Convert.FromBase64String(parts[3]);
        }
        catch (FormatException)
        {
            return false;
        }

        if (salt.Length == 0 || expected.Length == 0) return false;

        byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}
