using StellarStreamAPI.POCOs.Content;

namespace StellarStreamAPI.Interfaces
{
    public interface IMongoContentDatabaseContext
    {
        public Task<Result<List<NewsThumbnails>>> GetNews(int count, int offset, string? title, string? newsSite, DateTime? startDateP, DateTime? endDateP, DateTime? startDateU, DateTime? endDateU);
        public Task<Result<List<NasaImages>>> GetNasaImages(int count, int offset, string? title, string? center, string? nasaId, string? mediaType, string[]? keywords, DateTime? startDate, DateTime? endDate, string? secondaryDescription, string? secondaryCreator, string? description);
        public Task<Result<List<MarsPhotos>>> GetMarsPhotos(int count, int offset, int? startSol, int? endSol, string? cameraName, DateTime? startDate, DateTime? endDate, string? roverName);
        public Task<Result<List<Apod>>> GetApods(int count, int offset, string? title, string? explanation, DateTime? startDate, DateTime? endDate, string? copyright, string? mediaType);
        public Task<Result<long>> GetNewsCount();
        public Task<Result<long>> GetNasaImagesCount();
        public Task<Result<long>> GetMarsPhotosCount();
        public Task<Result<long>> GetApodsCount();
    }
}
