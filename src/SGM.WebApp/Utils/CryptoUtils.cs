using System.Security.Cryptography;
using System.Text;

namespace SGM.WebApp.Utils;

public static class CryptoUtils
{
    public static string Md5Hex(string input)
        => Convert.ToHexStringLower(MD5.HashData(Encoding.UTF8.GetBytes(input)));

    public static string HmacSha256Hex(string input, string key)
        => Convert.ToHexStringLower(
            HMACSHA256.HashData(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(input)));

    /// <summary>Constant-time, case-insensitive comparison of two hex signature strings.</summary>
    public static bool FixedTimeEqualsHex(string expected, string actual)
    {
        if (string.IsNullOrEmpty(actual) || expected.Length != actual.Length)
        {
            return false;
        }

        var a = Encoding.ASCII.GetBytes(expected.ToLowerInvariant());
        var b = Encoding.ASCII.GetBytes(actual.ToLowerInvariant());
        return CryptographicOperations.FixedTimeEquals(a, b);
    }

    /// <summary>Constant-time exact comparison; false when either side is empty.</summary>
    public static bool FixedTimeEquals(string? a, string? b)
    {
        if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
        {
            return false;
        }

        var x = Encoding.UTF8.GetBytes(a);
        var y = Encoding.UTF8.GetBytes(b);
        return x.Length == y.Length && CryptographicOperations.FixedTimeEquals(x, y);
    }
}
