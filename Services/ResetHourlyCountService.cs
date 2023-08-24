using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StellarStreamAPI.POCOs.Models;
using StellarStreamAPI.POCOs.Security;

namespace StellarStreamAPI.Services
{
    public class ResetHourlyCountService : BackgroundService
    {
        private readonly IMongoCollection<ApiKey> _apiKeyCollection;

        public ResetHourlyCountService(IMongoClient client, IOptions<StellarStreamApiSecurityDBSettings> databaseSettings)
        {
            _apiKeyCollection = client.GetDatabase(databaseSettings.Value.DatabaseName).GetCollection<ApiKey>(databaseSettings.Value.ApiKeysCollectionName);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = TimeSpan.FromHours(1) - DateTime.Now.TimeOfDay;

                if (delay.TotalMilliseconds <= 0)
                {
                    delay = TimeSpan.FromHours(1);
                }

                await Task.Delay(delay, stoppingToken);

                var update = Builders<ApiKey>.Update.Set(k => k.RequestsThisHour, 0);
                
                _apiKeyCollection.UpdateMany(_ => true, update);
            }
        }
    }
}
