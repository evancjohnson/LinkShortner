using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkShortner.Models
{
    public class LinkEntity : TableEntity
    {

        public LinkEntity(string partitionKey, string shortUrl) : base(partitionKey, shortUrl)
        {
            ShortUrl = shortUrl;

        }
        public string ShortUrl { get; set; }
        public string LongUrl { get; set; }
        public int Clicks { get; set; }
    }
}
