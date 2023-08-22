using System.Security.Cryptography;

namespace StellarStreamAPI.Security
{
    public class SymmetricEncryptor
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
                using Aes aes = Aes.Create();

                var IV = aes.IV;

                using ICryptoTransform encryptor = aes.CreateEncryptor();

                var plainText = System.Text.Encoding.Default.GetBytes(data);

                var ciphertext = encryptor.TransformFinalBlock(plainText, 0, plainText.Length);

                byte[] result = new byte[IV.Length + ciphertext.Length];
                Buffer.BlockCopy(IV, 0, result, 0, IV.Length);
                Buffer.BlockCopy(ciphertext, 0, result, IV.Length, ciphertext.Length);

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
                aes.Key = System.Text.Encoding.Default.GetBytes(_configuration.GetSection("Security")["EncryptionKey"]);
                byte[] encryptedData = System.Text.Encoding.Default.GetBytes(data);
                byte[] IV = new byte[16];
                Buffer.BlockCopy(encryptedData, 0, IV, 0, 16);
                byte[] ciphertext = new byte[encryptedData.Length - IV.Length];
                Buffer.BlockCopy(encryptedData, 16, ciphertext, 0, encryptedData.Length - IV.Length);
                aes.IV = IV;

                using ICryptoTransform decryptor = aes.CreateDecryptor();
                var plainText = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);

                return Convert.ToBase64String(plainText);
            }
            catch(ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException in SymmetricEncryptor.Decrypt(string data). ParamName: {ParamName}, Message: {Message}", ex.ParamName, ex.Message);
                return string.Empty;
            }
        }
    }
}
