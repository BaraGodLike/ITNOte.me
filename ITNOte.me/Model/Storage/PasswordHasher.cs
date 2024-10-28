using System.Buffers.Binary;
using System.Security.Cryptography;

namespace ITNOte.me.Model.Storage;

public class PasswordHasher
{
    // https://www.rfc-editor.org/rfc/rfc8018.html#section-5.2
    private const int IterationsSize = 4;
    private const int SaltSize = 32;
    private const int SubKeySize = 64;
    private const int HashSize = IterationsSize + SaltSize + SubKeySize;
  
    private const int MinimumIterations = 100_000;
    private const int MaximumIterations = 110_000;
    
    private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

    public bool VerifyHashedPassword(
        ReadOnlySpan<char> providedPassword,
        ReadOnlySpan<byte> hashedPassword)
    {
        if (hashedPassword.Length < HashSize)
        {
            return false;
        }

        hashedPassword = hashedPassword[..HashSize];
        
        var expectedSubKey = hashedPassword.Slice(IterationsSize + SaltSize, SubKeySize);
        Span<byte> actualSubKey = stackalloc byte[SubKeySize];
        Rfc2898DeriveBytes.Pbkdf2(providedPassword, 
            hashedPassword.Slice(IterationsSize, SaltSize), 
            actualSubKey, 
            (int) BinaryPrimitives.ReadUInt32BigEndian(hashedPassword[..IterationsSize]), 
            HashAlgorithmName.SHA512);
        var equals = CryptographicOperations.FixedTimeEquals(actualSubKey, expectedSubKey);
        actualSubKey.Clear();
        return equals;
    }
    
    public byte[] HashPassword(ReadOnlySpan<char> password)
    {
        Span<byte> salt = stackalloc byte[SaltSize];
        Span<byte> subKey = stackalloc byte[SubKeySize];
        Rng.GetBytes(salt);
        var iterationCount = RandomNumberGenerator.GetInt32(MinimumIterations, MaximumIterations);
        Rfc2898DeriveBytes.Pbkdf2(password, salt, subKey, iterationCount, HashAlgorithmName.SHA512);

        var result = new byte[HashSize];
        var resultIterations = result.AsSpan()[..IterationsSize];
        var resultSalt = result.AsSpan().Slice(IterationsSize, SaltSize);
        var resultSubKey = result.AsSpan().Slice(IterationsSize + SaltSize, SubKeySize);
        BinaryPrimitives.WriteUInt32BigEndian(resultIterations, (uint) iterationCount);
        salt.CopyTo(resultSalt);
        subKey.CopyTo(resultSubKey);
        salt.Clear();
        subKey.Clear();
        return result;
    }
}
