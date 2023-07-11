using System.Security.Cryptography;

namespace Dify.Core.Application.IdentityServer;

public static class PasswordHashManager
{
    private const int SaltSize = 0x10; // 16 bytes
    private const int HashSize = 0x20; // 32 bytes
    private const int Iterations = 0x3e8; // 1000 iterations
    
    public static string HashPassword(string password)
    {
        if (password == null)
        {
            throw new ArgumentNullException(nameof(password));
        }
        byte[] salt, hash;
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256))
        {
            salt = pbkdf2.Salt;
            hash = pbkdf2.GetBytes(HashSize);
        }
        byte[] hashBytes = new byte[SaltSize + HashSize + 1];
        Buffer.BlockCopy(salt, 0, hashBytes, 1, SaltSize);
        Buffer.BlockCopy(hash, 0, hashBytes, SaltSize + 1, HashSize);
        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyHashedPassword(string hashedPassword, string password)
    {
        if (hashedPassword == null || password == null)
        {
            throw new ArgumentNullException(hashedPassword == null ? nameof(hashedPassword) : nameof(password));
        }
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);
        if (hashBytes.Length != SaltSize + HashSize + 1 || hashBytes[0] != 0)
        {
            return false;
        }
        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 1, salt, 0, SaltSize);
        byte[] storedHash = new byte[HashSize];
        Buffer.BlockCopy(hashBytes, SaltSize + 1, storedHash, 0, HashSize);
        byte[] computedHash;
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
        {
            computedHash = pbkdf2.GetBytes(HashSize);
        }
        return storedHash.SequenceEqual(computedHash);
    }
}
