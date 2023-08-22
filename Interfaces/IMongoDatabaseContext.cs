using StellarStreamAPI.POCOs;

namespace StellarStreamAPI.Interfaces
{
    public interface IMongoDatabaseContext
    {
        public Task<Result<bool>> SaveApiKeyAsync(ApiKey key);
        public Task<Result<bool>> SaveApiKeyConsumerAsync(ApiKeyConsumer consumer);
        public Task<Result<List<ApiKey>>> GetApiKeysAsync();
        public Task<Result<List<ApiKey>>> GetApiKeysAsync(string apiKeyName);
        public Task<Result<bool>> DeleteApiKeyAsync(long keyId);
        public Task<Result<bool>> DeleteApiKeyConsumerAsync(ApiKeyConsumer consumer);
        public Task<Result<List<ApiKeyConsumer>>> GetApiKeyConsumersAsync();
        public Task<Result<bool>> ApiKeyConsumerExistAsync(string email);
        public Task<Result<ApiKeyConsumer>> GetApiKeyConsumerAsync(string email);
        public Task<Result<bool>> ApiKeyExistsAsync(long keyId);
    }
}
