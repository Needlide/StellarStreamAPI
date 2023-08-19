using MongoDB.Driver;
using StellarStreamAPI.Interfaces;
using StellarStreamAPI.Security.POCOs;
using System.Security.Authentication;

namespace StellarStreamAPI.Database
{
    public class DatabaseContext : IMongoDatabaseContext
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<ApiKey> _apiKeyCollection;
        private readonly IMongoCollection<ApiKeyConsumer> _apiKeyConsumerCollection;

        public DatabaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(_configuration.GetConnectionString("AzureCosmosDBMongoDB")));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            _client = new MongoClient(settings);
            _database = _client.GetDatabase("stellarstreamapisecuritydb");
            _apiKeyCollection = _database.GetCollection<ApiKey>("ApiKeys");
            _apiKeyConsumerCollection = _database.GetCollection<ApiKeyConsumer>("ApiKeysConsumers");
        }

        async Task<Result<bool>> IMongoDatabaseContext.DeleteApiKeyAsync(ApiKey key)
        {
            try
            {
                if (key == null)
                {
                    return Result<bool>.Fail(new ArgumentNullException(nameof(key)));
                }
                await _apiKeyCollection.DeleteOneAsync(x => x.KeyId == key.KeyId);
                return Result<bool>.Success();
            }
            catch (MongoException ex) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }

        async Task<Result<bool>> IMongoDatabaseContext.DeleteApiKeyConsumerAsync(ApiKeyConsumer consumer)
        {
            try
            {
                if (consumer == null)
                {
                    return Result<bool>.Fail(new ArgumentNullException(nameof(consumer)));
                }
                await _apiKeyConsumerCollection.DeleteOneAsync(x => x.Email == consumer.Email);
                return Result<bool>.Success();
            }
            catch (MongoException ex) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }

        async Task<Result<ApiKey>> IMongoDatabaseContext.GetApiKeyAsync(string apiKeyName)
        {
            try
            {
                var value = await _apiKeyCollection.FindAsync(x => x.KeyName == apiKeyName);
                Result<ApiKey>.Value = value.First();
                return Result<ApiKey>.Success();
            }
            catch (MongoException ex) { return Result<ApiKey>.Fail(ex); }
            catch (Exception ex) { return Result<ApiKey>.Fail(ex); }
        }

        async Task<Result<List<ApiKey>>> IMongoDatabaseContext.GetApiKeysAsync()
        {
            try
            {
                Result<List<ApiKey>>.Value = await _apiKeyCollection.Find(_ => true).ToListAsync();
                return Result<List<ApiKey>>.Success();
            }
            catch (MongoException ex) { return Result<List<ApiKey>>.Fail(ex); }
            catch (Exception ex) { return Result<List<ApiKey>>.Fail(ex); }
        }

        async Task<Result<bool>> IMongoDatabaseContext.SaveApiKeyAsync(ApiKey key)
        {
            try
            {
                if (key == null)
                {
                    return Result<bool>.Fail(new ArgumentNullException(nameof(key)));
                }
                await _apiKeyCollection.InsertOneAsync(key);
                return Result<bool>.Success();
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }

        async Task<Result<bool>> IMongoDatabaseContext.SaveApiKeyConsumerAsync(ApiKeyConsumer consumer)
        {
            try
            {
                if (consumer == null)
                {
                    return Result<bool>.Fail(new ArgumentNullException(nameof(consumer)));
                }
                await _apiKeyConsumerCollection.InsertOneAsync(consumer);
                return Result<bool>.Success();
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }
    }
}
