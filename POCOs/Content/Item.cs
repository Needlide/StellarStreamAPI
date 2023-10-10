using StellarStreamAPI.POCOs.Content.NasaImagesSubmodels;

namespace StellarStreamAPI.POCOs.Content
{
    public class Item
    {
        public List<string> Assets { get; set; }
        public List<Data> Data { get; set; }
        public List<Link> Links { get; set; }
    }
}
