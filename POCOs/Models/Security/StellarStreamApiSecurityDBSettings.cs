namespace StellarStreamAPI.POCOs.Models.Security
{
    public class StellarStreamApiSecurityDBSettings
    {
        public string DatabaseName { get; set; } = null!;
        public string ApiKeysCollectionName { get; set; } = null!;
        public string ApiKeysConsumersCollectionName { get; set; } = null!;
    }
}
