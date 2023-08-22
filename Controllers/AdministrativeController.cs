using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StellarStreamAPI.Database;
using StellarStreamAPI.Security;
using StellarStreamAPI.Security.JWT;
using StellarStreamAPI.Security.POCOs;
using StellarStreamAPI.Security.POCOs.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace StellarStreamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrativeController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly SymmetricEncryptor _symmetricEncryptor;
        private readonly ILogger _logger;

        public AdministrativeController(DatabaseContext dbContext, SymmetricEncryptor symmetricEncryptor, ILogger logger)
        {
            _dbContext = dbContext;
            _symmetricEncryptor = symmetricEncryptor;
            _logger = logger;
        }

        [HttpPost("/user")]
        public async Task<IActionResult> UserRegistration([FromBody]ApiKeyConsumerRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string encryptedPass = _symmetricEncryptor.Encrypt(model.Password);

            Result<bool> emailExistResult = await _dbContext.ApiKeyConsumerExistAsync(model.Email);

            if (!emailExistResult.IsSuccess)
            {
                return Problem("Error occured while checking is email registered.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            if (emailExistResult.Value)
            {
                return Conflict(new { message = "Email already registered." });
            }

            ApiKeyConsumer consumer = new() { Email = model.Email, Password = encryptedPass };
            Result<bool> saveResult = await _dbContext.SaveApiKeyConsumerAsync(consumer);

            if (!saveResult.IsSuccess)
            {
                return Problem("Error occured while saving user.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            string JWT = GenerateJWT(model.Email);

            return Ok(new { message = "Registered successfully.", token = JWT });
        }

        [HttpPost("/user/login")]
        public async Task<IActionResult> UserLogin([FromBody] ApiKeyConsumerRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Result<ApiKeyConsumer> apiKeyConsumerResult = await _dbContext.GetApiKeyConsumerAsync(model.Email);

            if(!apiKeyConsumerResult.IsSuccess)
            {
                return Problem("Error occured while checking is email registered.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            if (apiKeyConsumerResult.Value == null || !VerifyPassword(model.Password, apiKeyConsumerResult.Value.Password))
            {
                return BadRequest(new { message = "Invalid credentials." });
            }

            string JWT = GenerateJWT(model.Email);
            if (string.IsNullOrEmpty(JWT))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return Ok(new { token = JWT });
        }

        [HttpPost("/user/apikey")]
        public async Task<IActionResult> UserRegisterApiKey()
        {
            string userEmail = User.FindFirstValue(ClaimTypes.Email);
            Result<bool> userExists = await _dbContext.ApiKeyConsumerExistAsync(userEmail);

            if (!userExists.IsSuccess)
            {
                return Problem("Error occured while checking is email registered.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            if(!userExists.Value)
            {
                return BadRequest(new { message = "Invalid credentials." });
            }

            Result<ApiKeyConsumer> apiKeyConsumerResult = await _dbContext.GetApiKeyConsumerAsync(userEmail);

            if (!apiKeyConsumerResult.IsSuccess)
            {
                return Problem("Error occured while retrieving user.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            ApiKeyConsumer consumer = apiKeyConsumerResult.Value;

            string apiKey = GenerateApiKey();

            ApiKey userApiKey = new()
            {
                KeyName = userEmail,
                KeyValue = _symmetricEncryptor.Encrypt(apiKey),
                UserId = consumer.UserId,
            };

            Result<bool> savingApiKey = await _dbContext.SaveApiKeyAsync(userApiKey);

            if(!savingApiKey.IsSuccess)
            {
                return Problem("Error occured while saving API key.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(new { apikey = apiKey, message = "IMPORTANT: Save your API key immediately. You won't be able to retrieve it again." });
        }

        [HttpGet("/user/apikeys")]
        public async Task<IActionResult> UserGetApiKeys()
        {
            string userEmail = User.FindFirstValue(ClaimTypes.Email);
            Result<bool> userExists = await _dbContext.ApiKeyConsumerExistAsync(userEmail);

            if (!userExists.IsSuccess)
            {
                return Problem("Error occured while checking is email registered.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            if(!userExists.Value)
            {
                return BadRequest(new { message = "Invalid credentials." });
            }

            Result<List<ApiKey>> userApiKeysResult = await _dbContext.GetApiKeysAsync(userEmail);

            if(!userApiKeysResult.IsSuccess)
            {
                return Problem("Error occured while retrieving keys.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            List<ApiKey> userApiKeys = userApiKeysResult.Value;

            var responseKeys = userApiKeys.Select(apiKey => new
            {
                keyId = apiKey.KeyId,
                creationDate = apiKey.CreationDate,
                expirationDate = apiKey.ExpiryDate,
                status = apiKey.Status,
                usageCount = apiKey.UsageCount,
                requestsThisHour = apiKey.RequestsThisHour
            }).ToList();

            return Ok(responseKeys);
        }

        [HttpDelete("/user/apikey")]
        public async Task<IActionResult> UserRevokeApiKey([FromBody] ApiKeyRevokingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userEmail = User.FindFirstValue(ClaimTypes.Email);
            Result<bool> userExists = await _dbContext.ApiKeyConsumerExistAsync(userEmail);

            if (!userExists.IsSuccess)
            {
                return Problem("Error occured while checking is email registered.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            if (!userExists.Value)
            {
                return BadRequest(new { message = "Invalid credentials." });
            }

            Result<bool> apiKeyExistsResult = await _dbContext.ApiKeyExistsAsync(model.KeyId);

            if (!apiKeyExistsResult.IsSuccess)
            {
                return Problem("Error occured while checking is key exists.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            if (!apiKeyExistsResult.Value)
            {
                return BadRequest(new { message = "API key with given ID does not exist." });
            }

            Result<bool> keyRevokeResult = await _dbContext.DeleteApiKeyAsync(model.KeyId);

            if (!keyRevokeResult.IsSuccess)
            {
                return Problem("Error occured while revoking API key.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(new { message = "API key successfully revoked." });
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string storedPassword = _symmetricEncryptor.Decrypt(storedHash);
            if (storedPassword.Equals(enteredPassword))
                return true;
            return false;
        }

        private string GenerateJWT(string email)
        {
            try
            {
                SecurityTokenDescriptor descriptor = new()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email, email)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new RsaSecurityKey(JWTKeyReader.ReadPrivateKey("private_key.pem")), SecurityAlgorithms.RsaSha256)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(descriptor);

                return tokenHandler.WriteToken(token);
            }
            catch(ArgumentOutOfRangeException ex) { _logger.LogError(ex, "ArgumentOutOfException in AdministrativeController.GenerateJWT(string email). ParamName: {ParamName}, ActualValue: {ActualValue}, Message: {Message}", ex.ParamName, ex.ActualValue, ex.Message); return string.Empty; }
            catch(ArgumentException ex) { _logger.LogError(ex, "ArgumentException in AdministrativeController.GenerateJWT(string email). ParamName: {ParamName}, Message: {Message}", ex.ParamName, ex.Message); return string.Empty; }
            catch(SecurityTokenEncryptionFailedException ex) { _logger.LogError("SecurityTokenEncryptionFailedException in AdministrativeController.GenerateJWT(string email). Message: {Message}", ex.Message); return string.Empty; }
        }

        private string GenerateApiKey(int size = 32)
        {
            try
            {
                byte[] randomNumber = new byte[size];
                RandomNumberGenerator.Create().GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
            catch(ArgumentNullException ex) {_logger.LogError(ex, "ArgumentNullException in GenerateApiKey(int size = 32). ParamName: {ParamName}, Message: {Message}", ex.ParamName, ex.Message); return string.Empty; }
        }
    }
}
