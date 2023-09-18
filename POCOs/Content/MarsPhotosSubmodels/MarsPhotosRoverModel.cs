namespace StellarStreamAPI.POCOs.Content.MarsPhotosSubmodels
{
    public class MarsPhotosRoverModel
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
        public DateTime landing_date { get; set; }
        public DateTime launch_date { get; set; }
        public string status { get; set; } = null!;
        internal int max_sol;
        internal DateTime max_date;
        internal int total_photos;
        internal List<MarsPhotosRoverCameraModel> cameras;
    }
}
