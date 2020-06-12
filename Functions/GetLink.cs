using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;
using LinkShortner.Code;

namespace LinkShortner.Functions
{
    public static class GetLink
    {
        [FunctionName("GetLink")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{shortUrl}")] HttpRequest req,
            [Table("ShortUrl", Connection = "AzureWebJobsStorage")] CloudTable inputTable,
            [Queue(queueName: "ClickQueue")] IAsyncCollector<string> queue,
            ILogger log,
            string shortUrl)
        {
            var defaultUrl = "https://evancjohnson.com";
            try
            {
                //return to the default url on bad input.
                if(string.IsNullOrEmpty(shortUrl))
                    return new RedirectResult(defaultUrl);

                //get the url
                var url = await StorageMethods.GetLink(inputTable, shortUrl, log);

                //null check the url object
                if (url != null)
                {
                    await queue.AddAsync(url.RowKey);
                    return new RedirectResult(url.LongUrl);
                }
                else
                {
                    log.LogInformation($"The given shortUrl: {shortUrl} was not found");
                    return new RedirectResult(defaultUrl);
                }
            }
            catch (Exception ex)
            {
                var message = "Unable to Shorten Link. Please try again later.";
                log.LogError(ex, $"{message} {ex.Message}");
                return new RedirectResult(defaultUrl);
            }
        }
    }
}
