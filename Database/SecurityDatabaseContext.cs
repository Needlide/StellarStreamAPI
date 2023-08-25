using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using StellarStreamAPI.Interfaces;
using StellarStreamAPI.POCOs.Models.Security;
using StellarStreamAPI.POCOs.Security;

namespace StellarStreamAPI.Database
{
    public class SecurityDatabaseContext : IMongoSecurityDatabaseContext
    {
        private readonly IMongoCollection<ApiKey> ApiKeys;
        private readonly IMongoCollection<ApiKeyConsumer> ApiKeyConsumers;

        public SecurityDatabaseContext(IOptions<StellarStreamApiSecurityDBSettings> databaseSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            ApiKeys = database.GetCollection<ApiKey>(databaseSettings.Value.ApiKeysCollectionName);
            ApiKeyConsumers = database.GetCollection<ApiKeyConsumer>(databaseSettings.Value.ApiKeysConsumersCollectionName);
        }

        public async Task<Result<bool>> DeleteApiKeyAsync(ObjectId keyId)
        {
            try
            {
                await ApiKeys.DeleteOneAsync(x => x.KeyId.Equals(keyId));
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
                await ApiKeyConsumers.DeleteOneAsync(x => x.Email == consumer.Email);
                return Result<bool>.Success(true);
            }
            catch (MongoException ex) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }

        public async Task<Result<List<ApiKey>>> GetApiKeysAsync(string apiKeyName)
        {
            try
            {
                var value = await ApiKeys.FindAsync(x => x.KeyName == apiKeyName);
                return Result<List<ApiKey>>.Success(value.ToList());
            }
            catch (MongoException ex) { return Result<List<ApiKey>>.Fail(ex); }
            catch (Exception ex) { return Result<List<ApiKey>>.Fail(ex); }
        }

        public async Task<Result<List<ApiKey>>> GetApiKeysAsync()
        {
            try
            {
                var result = await ApiKeys.Find(_ => true).ToListAsync();
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
                await ApiKeys.InsertOneAsync(key);
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
                await ApiKeyConsumers.InsertOneAsync(consumer);
                return Result<bool>.Success(true);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }

        public async Task<Result<List<ApiKeyConsumer>>> GetApiKeyConsumersAsync()
        {
            try
            {
                var result = await ApiKeyConsumers.Find(_ => true).ToListAsync();
                return Result<List<ApiKeyConsumer>>.Success(result);
            }
            catch (MongoException ex) { return Result<List<ApiKeyConsumer>>.Fail(ex); }
            catch (Exception ex) { return Result<List<ApiKeyConsumer>>.Fail(ex); }
        }

        public async Task<Result<bool>> ApiKeyConsumerExistAsync(string email)
        {
            try
            {
                var result = await ApiKeyConsumers.FindAsync(x => x.Email.Equals(email));
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

        public async Task<Result<ApiKeyConsumer>> GetApiKeyConsumerAsync(string email)
        {
            try
            {
                var result = await ApiKeyConsumers.FindAsync(x => x.Email.Equals(email));
                return Result<ApiKeyConsumer>.Success(result.First());
            }
            catch (MongoException ex) { return Result<ApiKeyConsumer>.Fail(ex); }
            catch (Exception ex) { return Result<ApiKeyConsumer>.Fail(ex); }
        }

        public async Task<Result<bool>> ApiKeyExistsAsync(ObjectId keyId)
        {
            try
            {
                var result = await ApiKeys.FindAsync(x => x.KeyId.Equals(keyId));
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

        public Result<bool> UpdateApiKey(ApiKey replacement)
        {
            try
            {
                return Result<bool>.Success(ApiKeys.ReplaceOne(k => k.KeyId == replacement.KeyId, replacement).IsAcknowledged);
            }
            catch (MongoException ex) { return Result<bool>.Fail(ex); }
            catch (Exception ex) { return Result<bool>.Fail(ex); }
        }
    }
}
