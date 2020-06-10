using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LinkShortner.Functions
{
    public static class GetLink
    {
        [FunctionName("GetLink")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get" , Route = "{shortUrl}")] HttpRequest req,
            string shortUrl,
            ILogger log)
        {
            try
            {
                return new OkObjectResult(shortUrl);
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
