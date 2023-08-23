namespace StellarStreamAPI.POCOs.Models
{
    public class StellarStreamApiSecurityDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ApiKeysCollectionName { get; set; } = null!;
        public string ApiKeysConsumersCollectionName { get; set; } = null!;
    }
}
