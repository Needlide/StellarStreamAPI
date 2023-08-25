namespace StellarStreamAPI.POCOs.Models.Content
{
    public class MarsPhotosRoverModel
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
        public DateTime landing_date { get; set; }
        public DateTime launch_date { get; set; }
        public string status { get; set; } = null!;
    }
}
