using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StellarStreamAPI.Interfaces;
using StellarStreamAPI.POCOs.Content;
using StellarStreamAPI.POCOs.Models.Security;

namespace StellarStreamAPI.Database
{
    public class ContentDatabaseContext : IMongoContentDatabaseContext
    {
        private readonly IMongoCollection<NewsThumbnails> News;
        private readonly IMongoCollection<Item> NasaImages;
        private readonly IMongoCollection<MarsPhotos> MarsPhotos;
        private readonly IMongoCollection<Apod> Apods;

        public ContentDatabaseContext(IMongoClient mongoClient, IOptions<AcuDbContentDBSettings> databaseSettings)
        {
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            News = database.GetCollection<NewsThumbnails>(databaseSettings.Value.NewsCollectionName);
            NasaImages = database.GetCollection<Item>(databaseSettings.Value.NASAGalleryCollectionName);
            MarsPhotos = database.GetCollection<MarsPhotos>(databaseSettings.Value.MarsPhotosCollectionName);
            Apods = database.GetCollection<Apod>(databaseSettings.Value.PictureOfTheDayCollectionName);
        }

        public async Task<Result<List<NewsThumbnails>>> GetNews(int count, int offset, string? title, string? newsSite, DateTime? startDateP, DateTime? endDateP, DateTime? startDateU, DateTime? endDateU)
        {
            try
            {
                var filter = Builders<NewsThumbnails>.Filter.Empty;

                if (startDateP.HasValue)
                {
                    filter &= Builders<NewsThumbnails>.Filter.Gte(n => n.PublishedAt, startDateP.Value);
                }
                if (endDateP.HasValue)
                {
                    filter &= Builders<NewsThumbnails>.Filter.Lte(n => n.PublishedAt, endDateP.Value);
                }
                if (startDateU.HasValue)
                {
                    filter &= Builders<NewsThumbnails>.Filter.Gte(n => n.UpdatedAt, startDateU.Value);
                }
                if (endDateU.HasValue)
                {
                    filter &= Builders<NewsThumbnails>.Filter.Lte(n => n.UpdatedAt, endDateU.Value);
                }

                var projection = await News.Find(filter).Project(n => new NewsThumbnails
                {
                    Title = n.Title,
                    Url = n.Url,
                    ImageUrl = n.ImageUrl,
                    NewsSite = n.NewsSite,
                    Summary = n.Summary,
                    PublishedAt = n.PublishedAt,
                    UpdatedAt = n.UpdatedAt,
                    Featured = n.Featured,
                    Launches = n.Launches
                }).ToListAsync();

                var query = projection.AsQueryable();

                if (!string.IsNullOrEmpty(title))
                    query = query.Where(x => x.Title != null && x.Title.Contains(title));
                if (!string.IsNullOrEmpty(newsSite))
                    query = query.Where(query => query.NewsSite == newsSite);

                count = count < 0 ? 10 : count;

                var result = query.Skip(offset).Take(count).ToList();//negative offset considered as zero

                return Result<List<NewsThumbnails>>.Success(result);
            }
            catch (MongoException ex) { return Result<List<NewsThumbnails>>.Fail(ex); }
            catch (Exception ex) { return Result<List<NewsThumbnails>>.Fail(ex); }
        }

        public async Task<Result<List<Item>>> GetNasaImages(int count, int offset, string? title, string? center, string? nasaId, string? mediaType, [FromQuery] string[]? keywords, DateTime? startDate, DateTime? endDate, string? description, string? contentType)
        {
            try
            {
                var filter = Builders<Item>.Filter.Empty;

                var filterBuilder = Builders<Item>.Filter;

                if (startDate.HasValue)
                {
                    var dateFilter = filterBuilder.ElemMatch(
                        x => x.Data,
                        data => data.DateCreated >= startDate.Value
                    );

                    filter &= dateFilter;
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    var dateFilter = filterBuilder.ElemMatch(
                        x => x.Data,
                        data => data.DateCreated <= endDate.Value
                    );

                    filter &= dateFilter;
                }

                var projection = await NasaImages.Find(filter)
                                  .Project<Item>("{ _id: 0 }")
                                  .ToListAsync();

                var query = projection.AsQueryable();

                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(x => x.Data.Where(d => d.Title != null) != null && x.Data.Where(d => d.Title.Contains(title)) != null);
                }
                if (!string.IsNullOrEmpty(center))
                {
                    query = query.Where(x => x.Data.Where(d => d.Center != null) != null && x.Data.Where(d => d.Center.Contains(center)) != null);
                }
                if (!string.IsNullOrEmpty(description))
                {
                    query = query.Where(x => x.Data.Where(d => d.Description != null) != null && x.Data.Where(d => d.Description.Contains(description)) != null);
                }
                if (!string.IsNullOrEmpty(mediaType))
                {
                    query = query.Where(x => x.Data.Where(d => d.MediaType != null) != null && x.Data.Where(d => d.MediaType.Contains(mediaType)) != null);
                }
                if (!string.IsNullOrEmpty(nasaId))
                {
                    query = query.Where(x => x.Data.Where(d => d.NasaId != null) != null && x.Data.Where(d => d.NasaId.Contains(nasaId)) != null);
                }
                if (keywords != null && keywords.Any())
                {
                    query = query.Where(x => x.Data.Where(d => d.Keywords.Any(keyword => keywords.Contains(keyword))) != null);
                }

                var result = query.Skip(offset).Take(count).ToList();
                return Result<List<Item>>.Success(result);
            }
            catch (InvalidCastException ex) { return Result<List<Item>>.Fail(ex); }
            catch (MongoException ex) { return Result<List<Item>>.Fail(ex); }
            catch (Exception ex) { return Result<List<Item>>.Fail(ex); }
        }

        public async Task<Result<List<MarsPhotos>>> GetMarsPhotos(int count, int offset, int? startSol, int? endSol, string? cameraName, DateTime? startDate, DateTime? endDate, string? roverName)
        {
            try
            {
                var filter = Builders<MarsPhotos>.Filter.Empty;

                if (startDate.HasValue)
                {
                    filter &= Builders<MarsPhotos>.Filter.Gte(m => m.EarthDate, startDate.Value);
                }
                if (endDate.HasValue)
                {
                    filter &= Builders<MarsPhotos>.Filter.Lte(m => m.EarthDate, endDate.Value);
                }

                var projection = await MarsPhotos.Find(filter).Project(n => new MarsPhotos
                {
                    Sol = n.Sol,
                    ImgSrc = n.ImgSrc,
                    Camera = n.Camera,
                    EarthDate = n.EarthDate,
                    Rover = n.Rover
                }).ToListAsync();

                var query = projection.AsQueryable();

                if (startSol.HasValue && endSol.HasValue)
                {
                    if (startSol > endSol)
                    {
                        query = query.Where(x => x.Sol == endSol);
                    }
                    else
                    {
                        query = query.Where(x => x.Sol > startSol && x.Sol < endSol);
                    }
                }
                else if (endSol.HasValue)
                {
                    query = query.Where(x => x.Sol == endSol);
                }
                else if (startSol.HasValue)
                {
                    query = query.Where(x => x.Sol == startSol);
                }

                if (!string.IsNullOrEmpty(cameraName))
                {
                    query = query.Where(x => x.Camera.Name != null && x.Camera.Name.Contains(cameraName));
                }
                if (!string.IsNullOrEmpty(roverName))
                {
                    query = query.Where(x => x.Rover.Name != null && x.Rover.Name.Contains(roverName));
                }

                var result = query.Skip(offset).Take(count).ToList();
                return Result<List<MarsPhotos>>.Success(result);
            }
            catch (FormatException ex) { return Result<List<MarsPhotos>>.Fail(ex); }
            catch (ArgumentException ex) { return Result<List<MarsPhotos>>.Fail(ex); }
            catch (OverflowException ex) { return Result<List<MarsPhotos>>.Fail(ex); }
            catch (MongoException ex) { return Result<List<MarsPhotos>>.Fail(ex); }
            catch (Exception ex) { return Result<List<MarsPhotos>>.Fail(ex); }
        }

        public async Task<Result<List<Apod>>> GetApods(int count, int offset, string? title, string? explanation, DateTime? startDate, DateTime? endDate, string? copyright, string? mediaType)
        {
            try
            {
                var filter = Builders<Apod>.Filter.Empty;

                if (startDate.HasValue)
                {
                    filter &= Builders<Apod>.Filter.Gte(a => a.Date, startDate.Value);
                }
                if (endDate.HasValue)
                {
                    filter &= Builders<Apod>.Filter.Lte(a => a.Date, endDate.Value);
                }

                var projection = await Apods.Find(filter).Project(n => new Apod
                {
                    Copyright = n.Copyright,
                    Date = n.Date,
                    Explanation = n.Explanation,
                    HdUrl = n.HdUrl,
                    MediaType = n.MediaType,
                    ServiceVersion = n.ServiceVersion,
                    Title = n.Title,
                    Url = n.Url,
                    Concepts = n.Concepts
                }).ToListAsync();

                var query = projection.AsQueryable();

                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(x => x.Title != null && x.Title.Contains(title));
                }
                if (!string.IsNullOrEmpty(copyright))
                {
                    query = query.Where(x => x.Copyright != null && x.Copyright.Contains(copyright));
                }
                if (!string.IsNullOrEmpty(explanation))
                {
                    query = query.Where(x => x.Explanation != null && x.Explanation.Contains(explanation));
                }
                if (!string.IsNullOrEmpty(mediaType))
                {
                    query = query.Where(x => x.MediaType != null && x.MediaType.Contains(mediaType));
                }

                var result = query.Skip(offset).Take(count).ToList();
                return Result<List<Apod>>.Success(result);
            }
            catch (MongoException ex) { return Result<List<Apod>>.Fail(ex); }
            catch (Exception ex) { return Result<List<Apod>>.Fail(ex); }
        }

        public async Task<Result<long>> GetNewsCount()
        {
            return Result<long>.Success(await News.CountDocumentsAsync(_ => true));
        }

        public async Task<Result<long>> GetNasaImagesCount()
        {
            return Result<long>.Success(await NasaImages.CountDocumentsAsync(_ => true));
        }

        public async Task<Result<long>> GetMarsPhotosCount()
        {
            return Result<long>.Success(await MarsPhotos.CountDocumentsAsync(_ => true));
        }

        public async Task<Result<long>> GetApodsCount()
        {
            return Result<long>.Success(await Apods.CountDocumentsAsync(_ => true));
        }
    }
}
