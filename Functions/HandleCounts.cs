using System;
using LinkShortner.Code;
using LinkShortner.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace LinkShortner.Functions
{
    public static class HandleCounts
    {
        [FunctionName("HandleCounts")]
        public static async System.Threading.Tasks.Task Run([QueueTrigger("ClickQueue", Connection = "AzureWebJobsStorage")] string shortUrl,
                               [Table("ShortUrl", Connection = "AzureWebJobsStorage")] CloudTable inputTable,
                               [Table("LinkClick", Connection = "AzureWebJobsStorage")] CloudTable linkClick,
                               ILogger log)
        {
            try
            {
                var parts = shortUrl.Split("|");
                var slug = parts[0];
                var ipAddress = parts[1];
                var userAgent = parts[2];

                //get the url based on the given storage url
                var url = await StorageMethods.GetLink(inputTable, slug, log);

                if (url != null)
                {
                    var linkClickEnttiy = new LinkClickEntity(slug, Guid.NewGuid().ToString())
                    {
                        IpAddress = ipAddress,
                        UserAgent = userAgent
                    };

                    await StorageMethods.InsertLinkClick(linkClick, linkClickEnttiy, log);

                    //increment the clicks from the queue
                    url.Clicks++;

                    //insert or update (in this case update) the given url
                    await StorageMethods.InsertLink(inputTable, url, log);
                }

            }
            catch (Exception ex)
            {
                //log the error
                log.LogError(ex, $"Unable to count clicks for given shortUrl: {shortUrl}");
            }
        }
    }
}
