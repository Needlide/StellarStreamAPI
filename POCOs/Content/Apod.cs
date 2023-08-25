﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace StellarStreamAPI.POCOs.Content
{
    public class Apod
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public string Copyright { get; set; }
        public string Date { get; set; }
        public string Explanation { get; set; }
        public string HdUrl { get; set; }
        public string MediaType { get; set; }
        public string ServiceVersion { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
