using StellarStreamAPI.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace StellarStreamAPI.Security
{
    public class SymmetricEncryptor : IEncryptor
    {
        private readonly ILogger<SymmetricEncryptor> _logger;
        private readonly IConfiguration _configuration;

        public SymmetricEncryptor(ILogger<SymmetricEncryptor> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public string Encrypt(string data)
        {
            try
            {
                byte[] salt = new byte[32];
                RandomNumberGenerator.Create().GetBytes(salt);
                using var pbkdf2 = new Rfc2898DeriveBytes(_configuration.GetSection("Security")["EncryptionPassword"], salt, 1000, HashAlgorithmName.SHA256);

                using Aes aes = Aes.Create();
                aes.Key = pbkdf2.GetBytes(32);

                var IV = aes.IV;

                using ICryptoTransform encryptor = aes.CreateEncryptor();

                var plainText = Encoding.UTF8.GetBytes(data);

                var ciphertext = encryptor.TransformFinalBlock(plainText, 0, plainText.Length);

                byte[] result = new byte[IV.Length + ciphertext.Length + salt.Length];
                Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
                Buffer.BlockCopy(IV, 0, result, salt.Length, IV.Length);
                Buffer.BlockCopy(ciphertext, 0, result, IV.Length + salt.Length, ciphertext.Length);

                return Convert.ToBase64String(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "ArgumentException in SymmetricEncryptor.Encrypt(string data). ParamName: {ParamName}, Message: {Message}", ex.ParamName, ex.Message);
                return string.Empty;
            }
        }

        public string Decrypt(string data)
        {
            try
            {
                using Aes aes = Aes.Create();

                byte[] encryptedData = Convert.FromBase64String(data);

                byte[] salt = new byte[32];
                Buffer.BlockCopy(encryptedData, 0, salt, 0, salt.Length);

                byte[] IV = new byte[16];
                Buffer.BlockCopy(encryptedData, salt.Length, IV, 0, 16);

                byte[] ciphertext = new byte[encryptedData.Length - (IV.Length + salt.Length)];
                Buffer.BlockCopy(encryptedData, salt.Length + IV.Length, ciphertext, 0, encryptedData.Length - (IV.Length + salt.Length));

                using var pbkdf2 = new Rfc2898DeriveBytes(_configuration.GetSection("Security")["EncryptionPassword"], salt, 1000, HashAlgorithmName.SHA256);
                aes.Key = pbkdf2.GetBytes(32);

                aes.IV = IV;

                using ICryptoTransform decryptor = aes.CreateDecryptor();
                var plainText = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);

                return Encoding.UTF8.GetString(plainText);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException in SymmetricEncryptor.Decrypt(string data). ParamName: {ParamName}, Message: {Message}", ex.ParamName, ex.Message);
                return string.Empty;
            }
        }
    }
}
