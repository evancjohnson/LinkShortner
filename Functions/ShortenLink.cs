using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Azure.Cosmos.Table;
using LinkShortner.Models;
using LinkShortner.Code;
using System.Net.Http;

namespace LinkShortner.Functions
{
    public static class ShortenLink
    {
        [FunctionName("ShortenLink")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Table("ShortUrl", Connection = "AzureWebJobsStorage")] CloudTable inputTable,
            ILogger log)
        {
            try
            {
                string longUrl;
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                longUrl = data?.longUrl;

                if (string.IsNullOrEmpty(longUrl))
                    return new BadRequestObjectResult("Unable to shorten null or whitesapce");

                var newUrl = new LinkEntity("ShortUrl", RandomString.GetRandomString(6))
                {
                    LongUrl = longUrl,
                    Clicks = 1
                };

                await StorageMethods.InsertLink(inputTable, newUrl, log);

                return new OkObjectResult(newUrl.RowKey);
            }
            catch (Exception ex)
            {
                var message = "Unable to Shorten Link. Please try again later.";
                log.LogError(ex, $"{message} {ex.Message}");
                return new BadRequestObjectResult(message);
            }
        }
    }
}
