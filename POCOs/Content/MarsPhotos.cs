using StellarStreamAPI.Abstraction;
using StellarStreamAPI.POCOs.Content.MarsPhotosSubmodels;

namespace StellarStreamAPI.POCOs.Content
{
    public class MarsPhotos : DbEntityBase
    {
        internal int Id { get; set; }
        internal int Sol { get; set; }
        internal MarsPhotosCameraModel Camera { get; set; }
        internal string ImgSrc { get; set; }
        internal string EarthDate { get; set; }
        internal MarsPhotosRoverModel Rover { get; set; }
    }
}
