using StellarStreamAPI.Security.POCOs;

namespace StellarStreamAPI.Interfaces
{
    internal interface IApiKeyConsumerRepository
    {
        internal Task<ApiKeyConsumer> RegisterKeyAsync(string email);
        internal Task<ApiKeyConsumer> UnregisterKeyAsync(string apiKeyName);
        internal Task<ApiKeyConsumer> GetKeyAsync(string email);
    }
}
