﻿using MongoDB.Bson;
using StellarStreamAPI.Abstraction;

namespace StellarStreamAPI.POCOs.Content
{
    public class NasaImages : DbEntityBase
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
