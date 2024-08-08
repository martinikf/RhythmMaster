using System.Security.Cryptography;
using System.Text;
using Application.Interfaces.Providers;

namespace Infrastructure.Authentication;

public class PasswordHashProvider : IPasswordHashProvider
{
    public string Hash(string password, string username)
    {
        var salt = Encoding.UTF8.GetBytes(username);
            
        // Parameters for the PBKDF2 function
        const int iterations = 10000; // Number of iterations, recommended value is at least 10,000
        const int hashByteSize = 32; // Size of the hash in bytes

        // Generate the hash
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        
        var hash = pbkdf2.GetBytes(hashByteSize);
        return Convert.ToBase64String(hash);
    }
}