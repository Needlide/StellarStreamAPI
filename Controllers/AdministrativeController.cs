using Microsoft.AspNetCore.Mvc;
using StellarStreamAPI.Database;
using StellarStreamAPI.Security.POCOs;
using System.Net;

namespace StellarStreamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrativeController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public AdministrativeController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("/user")]
        public async Task<IActionResult> UserRegistration([FromBody]ApiKeyConsumerRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Result<bool> emailExistResult = await _dbContext.ApiKeyConsumerExistAsync(model.Email);

            if (!emailExistResult.IsSuccess)
            {
                return Problem("Error checking email existence.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            if (emailExistResult.Value)
            {
                return Conflict(new { message = "Email already registered." });
            }

            ApiKeyConsumer consumer = new() { Email = model.Email };
            Result<bool> saveResult = await _dbContext.SaveApiKeyConsumerAsync(consumer);

            if (!saveResult.IsSuccess)
            {
                return Problem("Error saving user.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(new { message = "Registered successfully." });
        }
    }
}
