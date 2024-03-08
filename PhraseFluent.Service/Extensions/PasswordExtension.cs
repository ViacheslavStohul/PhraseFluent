using System.Security.Cryptography;
using System.Text;

namespace PhraseFluent.Service.Extensions;

public static class PasswordExtension
{
    /// <summary>
    /// Hashes the input string using SHA256 algorithm and returns the hashed value as a hexadecimal string.
    /// </summary>
    /// <param name="toHash">The string to hash.</param>
    /// <returns>The hashed value as a hexadecimal string.</returns>
    public static string Hash(this string toHash)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(toHash));

        var builder = new StringBuilder();
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }
        
        return builder.ToString();
    }

    /// <summary>
    /// Compares a hashed string with the original string to determine if they are equal.
    /// </summary>
    /// <param name="toCompare">The original string to be compared.</param>
    /// <param name="hashedString">The hashed string to be compared with the original string.</param>
    /// <returns>
    /// <c>true</c> if the hashed string is equal to the original string;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool IsHashStringsEqual(this string toCompare, string hashedString)
    {
        var hashToCompare = toCompare.Hash();
        return hashToCompare == hashedString;
    }
}