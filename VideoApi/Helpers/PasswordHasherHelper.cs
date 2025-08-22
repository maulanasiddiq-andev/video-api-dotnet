using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace VideoApi.Helpers
{
    public sealed class HashingOptions
    {
        public int Iterations { get; set; } = 10000;
    }

    public interface IPasswordHasher
    {
        string Hash(string password);
        // (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
    }
    public sealed class PasswordHasherHelper : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        public PasswordHasherHelper(IOptions<HashingOptions> options)
        {
            Options = options.Value;
        }

        public PasswordHasherHelper()
        {
            Options = new HashingOptions();
        }

        private HashingOptions Options { get; }

        public string Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Options.Iterations, HashAlgorithmName.SHA512))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{salt}.{key}";
            }
        }
    }
}