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
        public async Task<IActionResult> News(string? title, string? newsSite, DateTime? startDateP, DateTime? endDateP, DateTime? startDateU, DateTime? endDateU, int count = 10, int offset = 0)
        {
            var result = await _dbContext.GetNews(count, offset, title, newsSite, startDateP, endDateP, startDateU, endDateU);

            if(!result.IsSuccess)
            {
                return Problem("Error occured while retrieving data.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Value);
        }

        [HttpGet("/nasa-library")]
        public async Task<IActionResult> NasaLibrary(string? title, string? center, string? nasaId, string? mediaType, string[]? keywords, DateTime? startDate, DateTime? endDate, string? secondaryDescription, string? secondaryCreator, string? description, int count = 10, int offset = 0)
        {
            var result = await _dbContext.GetNasaImages(count, offset, title, center, nasaId, mediaType, keywords, startDate, endDate, secondaryDescription, secondaryCreator, description);

            if (!result.IsSuccess)
            {
                return Problem("Error occured while retrieving data.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Value);
        }

        [HttpGet("/mars-rover-photos")]
        public async Task<IActionResult> MarsPhotos(int? startSol, int? endSol, string? cameraName, DateTime? startDate, DateTime? endDate, string? roverName, int count = 10, int offset = 0)
        {
            var result = await _dbContext.GetMarsPhotos(count, offset, startSol, endSol, cameraName, startDate, endDate, roverName);

            if (!result.IsSuccess)
            {
                return Problem("Error occured while retrieving data.", statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Value);
        }

        [HttpGet("/apod")]
        public async Task<IActionResult> Apod(string? title, string? explanation, DateTime? startDate, DateTime? endDate, string? copyright, string? mediaType, int count = 10, int offset = 0)
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