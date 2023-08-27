namespace StellarStreamAPI.POCOs.Models.Content
{
    public class MarsPhotosUserFriendlyModel
    {
        public int Id { get; set; }
        public int Sol { get; set; }
        public object Camera { get; set; }
        public string ImgSrc { get; set; }
        public string EarthDate { get; set; }
        public object Rover { get; set; }
    }
}
