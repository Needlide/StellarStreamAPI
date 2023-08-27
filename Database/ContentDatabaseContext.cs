using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StellarStreamAPI.Interfaces;
using StellarStreamAPI.POCOs.Content;
using StellarStreamAPI.POCOs.Models.Content;
using StellarStreamAPI.POCOs.Models.Security;

namespace StellarStreamAPI.Database
{
    public class ContentDatabaseContext : IMongoContentDatabaseContext
    {
        private readonly IMongoCollection<NewsThumbnails> News;
        private readonly IMongoCollection<NasaImages> NasaImages;
        private readonly IMongoCollection<MarsPhotos> MarsPhotos;
        private readonly IMongoCollection<Apod> Apods;

        public ContentDatabaseContext(IMongoClient mongoClient, IOptions<AcuDbContentDBSettings> databaseSettings)
        {
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            News = database.GetCollection<NewsThumbnails>(databaseSettings.Value.NewsCollectionName);
            NasaImages = database.GetCollection<NasaImages>(databaseSettings.Value.NASAGalleryCollectionName);
            MarsPhotos = database.GetCollection<MarsPhotos>(databaseSettings.Value.MarsPhotosCollectionName);
            Apods = database.GetCollection<Apod>(databaseSettings.Value.PictureOfTheDayCollectionName);
        }

        public async Task<Result<List<NewsThumbnailsUserFriendlyModel>>> GetNews(int count, int offset, string? title, string? newsSite, DateTime? startDateP, DateTime? endDateP, DateTime? startDateU, DateTime? endDateU)
        {
            try
            {
                var projection = await News.Find(_ => true).Project(n => new NewsThumbnailsUserFriendlyModel
                {
                    Id = n.Id,
                    Title = n.Title,
                    Url = n.Url,
                    ImageUrl = n.ImageUrl,
                    NewsSite = n.NewsSite,
                    Summary = n.Summary,
                    PublishedAt = n.PublishedAt,
                    UpdatedAt = n.UpdatedAt
                }).ToListAsync();

                var query = projection.AsQueryable();

                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(x => x.Title.Contains(title));
                }
                if (!string.IsNullOrEmpty(newsSite))
                {
                    query = query.Where(query => query.NewsSite == newsSite);
                }
                if (startDateP.HasValue && endDateP.HasValue)
                {
                    if (startDateP > endDateP)
                    {
                        query = query.Where(x => x.PublishedAt == endDateP);
                    }
                    else
                    {
                        query = query.Where(x => x.PublishedAt > startDateP && x.PublishedAt < endDateP);
                    }
                }
                else if (endDateP.HasValue)
                {
                    query = query.Where(x => x.PublishedAt == endDateP);
                }
                else if (startDateP.HasValue)
                {
                    query = query.Where(x => x.PublishedAt == startDateP);
                }

                if (startDateU.HasValue && endDateU.HasValue)
                {
                    if (startDateU > endDateU)
                    {
                        query = query.Where(x => x.UpdatedAt == endDateU);
                    }
                    else
                    {
                        query = query.Where(x => x.UpdatedAt > startDateU && x.UpdatedAt < endDateU);
                    }
                }
                else if (endDateU.HasValue)
                {
                    query = query.Where(x => x.UpdatedAt == endDateU);
                }
                else if (startDateU.HasValue)
                {
                    query = query.Where(x => x.UpdatedAt == startDateU);
                }

                count = count < 0 ? 10 : count;

                var result = query.Skip(offset).Take(count).ToList();//negative offset considered as zero

                return Result<List<NewsThumbnailsUserFriendlyModel>>.Success(result);
            }
            catch (MongoException ex) { return Result<List<NewsThumbnailsUserFriendlyModel>>.Fail(ex); }
            catch (Exception ex) { return Result<List<NewsThumbnailsUserFriendlyModel>>.Fail(ex); }
        }

        public async Task<Result<List<NasaImagesUserFriendlyModel>>> GetNasaImages(int count, int offset, string? title, string? center, string? nasaId, string? mediaType, [FromQuery]string[]? keywords, DateTime? startDate, DateTime? endDate, string? secondaryDescription, string? secondaryCreator, string? description)
        {
            try
            {
                var projection = await NasaImages.Find(_ => true).Project(n => new NasaImagesUserFriendlyModel
                {
                    Id = n.Id,
                    NASAId = n.NASAId,
                    Center = n.Center,
                    DateCreated = n.DateCreated,
                    Description = n.Description,
                    Href = n.Href,
                    Keywords = n.Keywords,
                    MediaType = n.MediaType,
                    SecondaryCreator = n.SecondaryCreator,
                    SecondaryDescription = n.SecondaryDescription,
                    Title = n.Title
                }).ToListAsync();

                var query = projection.AsQueryable();

                if(!string.IsNullOrEmpty(title))
                {
                    query = query.Where(query => query.Title.Contains(title));
                }
                if(!string.IsNullOrEmpty(center))
                {
                    query = query.Where(query=> query.Center.Contains(center));
                }
                if(!string.IsNullOrEmpty(description))
                {
                    query = query.Where(x => x.Description.Contains(description));
                }
                if(!string.IsNullOrEmpty(secondaryCreator))
                {
                    query = query.Where(x => x.SecondaryCreator.Contains(secondaryCreator));
                }
                if(!string.IsNullOrEmpty(mediaType))
                {
                    query = query.Where(x => x.MediaType.Contains(mediaType));
                }
                if(!string.IsNullOrEmpty(nasaId))
                {
                    query = query.Where(x => x.NASAId.Contains(nasaId));
                }
                if (!string.IsNullOrEmpty(secondaryDescription))
                {
                    query = query.Where(x => x.SecondaryDescription.Contains(secondaryDescription));
                }
                if (keywords != null && keywords.Any())
                {
                    query = query.Where(image => image.Keywords.Any(keyword => keywords.Contains(keyword.AsString)));
                }
                if (startDate.HasValue && endDate.HasValue)
                {
                    if (startDate > endDate)
                    {
                        query = query.Where(x => x.DateCreated == endDate);
                    }
                    else
                    {
                        query = query.Where(x => x.DateCreated > startDate && x.DateCreated < endDate);
                    }
                }
                else if (endDate.HasValue)
                {
                    query = query.Where(x => x.DateCreated == endDate);
                }
                else if (startDate.HasValue)
                {
                    query = query.Where(x => x.DateCreated == startDate);
                }

                var result = query.Skip(offset).Take(count).ToList();
                return Result<List<NasaImagesUserFriendlyModel>>.Success(result);
            }
            catch (InvalidCastException ex) { return Result<List<NasaImagesUserFriendlyModel>>.Fail(ex); } 
            catch (MongoException ex) { return Result<List<NasaImagesUserFriendlyModel>>.Fail(ex); }
            catch (Exception ex) { return Result<List<NasaImagesUserFriendlyModel>>.Fail(ex); }
        }

        public async Task<Result<List<MarsPhotosUserFriendlyModel>>> GetMarsPhotos(int count, int offset, int? startSol, int? endSol, string? cameraName, DateTime? startDate, DateTime? endDate, string? roverName)
        {
            try
            {
                var projection = await MarsPhotos.Find(_ => true).Project(n => new MarsPhotosUserFriendlyModel
                {
                    Camera = n.Camera,
                    EarthDate = n.EarthDate,
                    Id = n.Id,
                    ImgSrc = n.ImgSrc,
                    Rover = n.Rover,
                    Sol = n.Sol
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

                if (startDate.HasValue && endDate.HasValue)
                {
                    if (startDate > endDate)
                    {
                        query = query.Where(x => DateTime.Parse(x.EarthDate) == endDate);
                    }
                    else
                    {
                        query = query.Where(x => DateTime.Parse(x.EarthDate) > startDate && DateTime.Parse(x.EarthDate) < endDate);
                    }
                }
                else if (endDate.HasValue)
                {
                    query = query.Where(x => DateTime.Parse(x.EarthDate) == endDate);
                }
                else if (startDate.HasValue)
                {
                    query = query.Where(x => DateTime.Parse(x.EarthDate) == startDate);
                }
                
                if(!string.IsNullOrEmpty(cameraName))
                {
                    query = query.Where(x => Newtonsoft.Json.JsonConvert.DeserializeObject<MarsPhotosCameraModel>((string)x.Camera).name.Contains(cameraName));
                }
                if(!string.IsNullOrEmpty(roverName))
                {
                    query = query.Where(x => Newtonsoft.Json.JsonConvert.DeserializeObject<MarsPhotosRoverModel>((string)x.Rover).name.Contains(roverName));
                }

                var result = query.Skip(offset).Take(count).ToList();
                return Result<List<MarsPhotosUserFriendlyModel>>.Success(result);
            }
            catch (FormatException ex) { return Result<List<MarsPhotosUserFriendlyModel>>.Fail(ex); }
            catch (ArgumentException ex) { return Result<List<MarsPhotosUserFriendlyModel>>.Fail(ex); }
            catch (OverflowException ex) { return Result<List<MarsPhotosUserFriendlyModel>>.Fail(ex); }
            catch (MongoException ex) { return Result<List<MarsPhotosUserFriendlyModel>>.Fail(ex); }
            catch (Exception ex) { return Result<List<MarsPhotosUserFriendlyModel>>.Fail(ex); }
        }

        public async Task<Result<List<ApodUserFriendlyModel>>> GetApods(int count, int offset, string? title, string? explanation, DateTime? startDate, DateTime? endDate, string? copyright, string? mediaType)
        {
            try
            {
                var projection = await Apods.Find(_ => true).Project(n => new ApodUserFriendlyModel
                {
                    Copyright = n.Copyright,
                    Date = n.Date,
                    Explanation = n.Explanation,
                    HdUrl = n.HdUrl,
                    Id = n.Id,
                    MediaType = n.MediaType,
                    ServiceVersion = n.ServiceVersion,
                    Title = n.Title,
                    Url = n.Url
                }).ToListAsync();

                var query = projection.AsQueryable();

                if(!string.IsNullOrEmpty(title))
                {
                    query = query.Where(x => x.Title.Contains(title));
                }
                if (!string.IsNullOrEmpty(copyright))
                {
                    query = query.Where(x => x.Copyright.Contains(copyright));
                }
                if (!string.IsNullOrEmpty(explanation))
                {
                    query = query.Where(x => x.Explanation.Contains(explanation));
                }
                if(!string.IsNullOrEmpty(mediaType))
                {
                    query = query.Where(x => x.MediaType.Contains(mediaType));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    if (startDate > endDate)
                    {
                        query = query.Where(x => DateTime.Parse(x.Date) == endDate);
                    }
                    else
                    {
                        query = query.Where(x => DateTime.Parse(x.Date) > startDate && DateTime.Parse(x.Date) < endDate);
                    }
                }
                else if (endDate.HasValue)
                {
                    query = query.Where(x => DateTime.Parse(x.Date) == endDate);
                }
                else if (startDate.HasValue)
                {
                    query = query.Where(x => DateTime.Parse(x.Date) == startDate);
                }

                var result = query.Skip(offset).Take(count).ToList();
                return Result<List<ApodUserFriendlyModel>>.Success(result);
            }
            catch (MongoException ex) { return Result<List<ApodUserFriendlyModel>>.Fail(ex); }
            catch (Exception ex) { return Result<List<ApodUserFriendlyModel>>.Fail(ex); }
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
