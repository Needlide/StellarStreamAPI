using MongoDB.Bson;

namespace StellarStreamAPI.POCOs.Models.Content
{
    public class NasaImagesUserFriendlyModel
    {
        public int Id { get; set; }
        public string Center { get; set; }
        public string Title { get; set; }
        public string NASAId { get; set; }
        public string MediaType { get; set; }
        public BsonArray Keywords { get; set; }
        public DateTime DateCreated { get; set; }
        public string SecondaryDescription { get; set; }
        public string SecondaryCreator { get; set; }
        public string Description { get; set; }
        public string Href { get; set; }
    }
}
