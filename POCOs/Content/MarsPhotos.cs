using StellarStreamAPI.Abstraction;
using StellarStreamAPI.POCOs.Content.MarsPhotosSubmodels;

namespace StellarStreamAPI.POCOs.Content
{
    public class MarsPhotos : EntityBase
    {
        public int Sol { get; set; }
        public MarsPhotosCameraModel Camera { get; set; }
        public string ImgSrc { get; set; }
        public string EarthDate { get; set; }
        public MarsPhotosRoverModel Rover { get; set; }
    }
}
