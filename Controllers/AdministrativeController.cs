using Microsoft.AspNetCore.Mvc;
using StellarStreamAPI.Database;
using StellarStreamAPI.Security;
using StellarStreamAPI.Security.POCOs;
using System.Net;

namespace StellarStreamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrativeController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly SymmetricEncryptor _symmetricEncryptor;

        public AdministrativeController(DatabaseContext dbContext, SymmetricEncryptor symmetricEncryptor)
        {
            _dbContext = dbContext;
            _symmetricEncryptor = symmetricEncryptor;
        }

        [HttpPost("/user")]
        public async Task<IActionResult> UserRegistration([FromBody]ApiKeyConsumerRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string encryptedEmail = _symmetricEncryptor.Encrypt(model.Email);

            Result<bool> emailExistResult = await _dbContext.ApiKeyConsumerExistAsync(encryptedEmail);

            if (!emailExistResult.IsSuccess)
            {
                return Problem("Error occured while checking is email registered.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            if (emailExistResult.Value)
            {
                return Conflict(new { message = "Email already registered." });
            }

            ApiKeyConsumer consumer = new() { Email = encryptedEmail };
            Result<bool> saveResult = await _dbContext.SaveApiKeyConsumerAsync(consumer);

            if (!saveResult.IsSuccess)
            {
                return Problem("Error occured while saving user.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(new { message = "Registered successfully." });
        }
    }
}
