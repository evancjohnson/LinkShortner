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
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.Cosmos.Table;

namespace LinkShortner.Functions
{
    public static class ShortenLink
    {
        [FunctionName("ShortenLink")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ShortenLink/{longUrl}")] HttpRequest req,
            string longUrl,
            ILogger log)
        {
            try
            { 

                return new OkObjectResult(longUrl);
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
