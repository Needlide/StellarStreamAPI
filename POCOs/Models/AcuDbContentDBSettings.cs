namespace StellarStreamAPI.POCOs.Models
{
    public class AcuDbContentDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string PictureOfTheDayCollectionName { get; set; } = null!;
        public string MarsPhotosCollectionName { get; set; } = null!;
        public string NASAGalleryCollectionName { get; set; } = null!;
        public string NewsCollectionName { get; set; } = null!;
    }
}
