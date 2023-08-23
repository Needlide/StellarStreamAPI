using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StellarStreamAPI.Interfaces;
using StellarStreamAPI.POCOs;

namespace StellarStreamAPI.Database
{
    public class DatabaseContext : IMongoDatabaseContext
    {
        private readonly ILogger<DatabaseContext> _logger;
        private readonly IMongoCollection<ApiKey> _apiKeyCollection;
        private readonly IMongoCollection<ApiKeyConsumer> _apiKeyConsumerCollection;

        public DatabaseContext(IOptions<StellarStreamApiSecurityDBSettings> databaseSettings, ILogger<DatabaseContext> logger)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _apiKeyCollection = database.GetCollection<ApiKey>(databaseSettings.Value.ApiKeysCollectionName);
            _apiKeyConsumerCollection = database.GetCollection<ApiKeyConsumer>(databaseSettings.Value.ApiKeysConsumersCollectionName);

            _logger = logger;
        }

        public async Task<Result<bool>> DeleteApiKeyAsync(long keyId)
        {
            try
            {
                await _apiKeyCollection.DeleteOneAsync(x => x.KeyId.Equals(keyId));
                return Result<bool>.Success(true);
            }
            catch (MongoException ex) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }

        public async Task<Result<bool>> DeleteApiKeyConsumerAsync(ApiKeyConsumer consumer)
        {
            try
            {
                if (consumer == null)
                {
                    return Result<bool>.Fail(new ArgumentNullException(nameof(consumer)));
                }
                await _apiKeyConsumerCollection.DeleteOneAsync(x => x.Email == consumer.Email);
                return Result<bool>.Success(true);
            }
            catch (MongoException ex) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }

        public async Task<Result<List<ApiKey>>> GetApiKeysAsync(string apiKeyName)
        {
            try
            {
                var value = await _apiKeyCollection.FindAsync(x => x.KeyName == apiKeyName);
                return Result<List<ApiKey>>.Success(value.ToList());
            }
            catch (MongoException ex) { return Result<List<ApiKey>>.Fail(ex); }
            catch (Exception ex) { return Result<List<ApiKey>>.Fail(ex); }
        }

        public async Task<Result<List<ApiKey>>> GetApiKeysAsync()
        {
            try
            {
                var result = await _apiKeyCollection.Find(_ => true).ToListAsync();
                return Result<List<ApiKey>>.Success(result);
            }
            catch (MongoException ex) { return Result<List<ApiKey>>.Fail(ex); }
            catch (Exception ex) { return Result<List<ApiKey>>.Fail(ex); }
        }

        public async Task<Result<bool>> SaveApiKeyAsync(ApiKey key)
        {
            try
            {
                if (key == null)
                {
                    return Result<bool>.Fail(new ArgumentNullException(nameof(key)));
                }
                await _apiKeyCollection.InsertOneAsync(key);
                return Result<bool>.Success(true);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }

        public async Task<Result<bool>> SaveApiKeyConsumerAsync(ApiKeyConsumer consumer)
        {
            try
            {
                if (consumer == null)
                {
                    return Result<bool>.Fail(new ArgumentNullException(nameof(consumer)));
                }
                await _apiKeyConsumerCollection.InsertOneAsync(consumer);
                return Result<bool>.Success(true);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }

        public async Task<Result<List<ApiKeyConsumer>>> GetApiKeyConsumersAsync()
        {
            try
            {
                var result = await _apiKeyConsumerCollection.Find(_ => true).ToListAsync();
                return Result<List<ApiKeyConsumer>>.Success(result);
            }
            catch (MongoException ex) { return Result<List<ApiKeyConsumer>>.Fail(ex); }
            catch (Exception ex) { return Result<List<ApiKeyConsumer>>.Fail(ex); }
        }

        public async Task<Result<bool>> ApiKeyConsumerExistAsync(string email)
        {
            try
            {
                var result = await _apiKeyConsumerCollection.FindAsync(x => x.Email.Equals(email));
                if(result.Any())
                {
                    return Result<bool>.Success(true);
                }
                else
                {
                    return Result<bool>.Success(false);
                }
            }
            catch (MongoException ex) { _logger.LogError(ex, "An error occurred in ApiKeyConsumerExistAsync method: {Message}", ex.Message); return Result<bool>.Fail(ex); }
            catch (Exception ex) { _logger.LogError(ex, "An error occurred in ApiKeyConsumerExistAsync method: {Message}", ex.Message); return Result<bool>.Fail(ex); }
        }

        public async Task<Result<ApiKeyConsumer>> GetApiKeyConsumerAsync(string email)
        {
            try
            {
                var result = await _apiKeyConsumerCollection.FindAsync(x => x.Email.Equals(email));
                return Result<ApiKeyConsumer>.Success(result.First());
            }
            catch (MongoException ex) { return Result<ApiKeyConsumer>.Fail(ex); }
            catch (Exception ex) { return Result<ApiKeyConsumer>.Fail(ex); }
        }

        public async Task<Result<bool>> ApiKeyExistsAsync(long keyId)
        {
            try
            {
                var result = await _apiKeyCollection.FindAsync(x => x.KeyId.Equals(keyId));
                if(result.Any())
                {
                    return Result<bool>.Success(true);
                }
                else
                {
                    return Result<bool>.Success(false);
                }
            }
            catch (MongoException ex) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }
    }
}
