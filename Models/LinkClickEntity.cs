using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkShortner.Models
{
    public class LinkClickEntity : TableEntity
    {
        public LinkClickEntity() { }

        public LinkClickEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey) { }

        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
