﻿using StellarStreamAPI.Abstraction;

namespace StellarStreamAPI.POCOs.Content
{
    public class NewsThumbnails : EntityBase
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string NewsSite { get; set; }
        public string Summary { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
