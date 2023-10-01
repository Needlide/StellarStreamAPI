using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarStreamAPI.Interfaces;
using System.Net;

namespace StellarStreamAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly IMongoContentDatabaseContext _dbContext;

        public ContentController(IMongoContentDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("/news-count")]
        public async Task<IActionResult> NewsCount()
        {
            Result<long> result = await _dbContext.GetNewsCount();

            if (!result.IsSuccess)
            {
                return Problem("Error occured while retrieving count.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Value);
        }

        [HttpGet("/nasa-library-count")]
        public async Task<IActionResult> GalleryCount()
        {
            Result<long> result = await _dbContext.GetNasaImagesCount();

            if (!result.IsSuccess)
            {
                return Problem("Error occured while retrieving count.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Value);
        }

        [HttpGet("/mars-rover-photos-count")]
        public async Task<IActionResult> MarsPhotosCount()
        {
            Result<long> result = await _dbContext.GetMarsPhotosCount();

            if (!result.IsSuccess)
            {
                return Problem("Error occured while retrieving count.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Value);
        }

        [HttpGet("/apod-count")]
        public async Task<IActionResult> ApodCount()
        {
            Result<long> result = await _dbContext.GetApodsCount();

            if (!result.IsSuccess)
            {
                return Problem("Error occured while retrieving count.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Value);
        }

        [HttpGet("/news")]
        public async Task<IActionResult> News(string? title, string? newsSite, string? startDateP, string? endDateP, string? startDateU, string? endDateU, int count = 10, int offset = 0)
        {
            var result = await _dbContext.GetNews(count, offset, title, newsSite, startDateP, endDateP, startDateU, endDateU);

            if (!result.IsSuccess)
            {
                return Problem("Error occured while retrieving data.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Value);
        }

        [HttpGet("/nasa-library")]
        public async Task<IActionResult> NasaLibrary(string? title, string? center, string? nasaId, string? mediaType, [FromQuery]string[]? keywords, string? startDate, string? endDate, string? description, int count = 10, int offset = 0)
        {
            var result = await _dbContext.GetNasaImages(count, offset, title, center, nasaId, mediaType, keywords, startDate, endDate, description);

            if (!result.IsSuccess)
            {
                return Problem("Error occured while retrieving data.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Value);
        }

        [HttpGet("/mars-rover-photos")]
        public async Task<IActionResult> MarsPhotos(int? startSol, int? endSol, string? cameraName, string? startDate, string? endDate, string? roverName, int count = 10, int offset = 0)
        {
            var result = await _dbContext.GetMarsPhotos(count, offset, startSol, endSol, cameraName, startDate, endDate, roverName);

            if (!result.IsSuccess)
            {
                return Problem("Error occured while retrieving data.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Value);
        }

        [HttpGet("/apod")]
        public async Task<IActionResult> Apod(string? title, string? explanation, string? startDate, string? endDate, string? copyright, string? mediaType, int count = 10, int offset = 0)
        {
            var result = await _dbContext.GetApods(count, offset, title, explanation, startDate, endDate, copyright, mediaType);

            if (!result.IsSuccess)
            {
                return Problem("Error occured while retrieving data.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Value);
        }
    }
}