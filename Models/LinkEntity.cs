
using Microsoft.Azure.Cosmos.Table;

namespace LinkShortner.Models
{
    public class LinkEntity : Microsoft.Azure.Cosmos.Table.TableEntity
    {
        public LinkEntity() { }

        public LinkEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey) { }

        public string LongUrl { get; set; }

        public int Clicks { get; set; }
    }
}
