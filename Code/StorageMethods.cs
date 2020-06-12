using LinkShortner.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LinkShortner.Code
{
    public static class StorageMethods
    {
        /// <summary>
        /// Insert a link into the table storage
        /// <param name="table">The table to insert data into</param>
        /// <param name="entity">The link entity that is going to be inserted or updated</param>
        /// <param name="logger">The log instance</param>
        /// </summary>
        public static async Task InsertLink(CloudTable table, LinkEntity entity, ILogger logger)
        {
            try
            {
                await table.CreateIfNotExistsAsync();

                //merge the op
                var mergeOperation = TableOperation.InsertOrMerge(entity);
                var result = await table.ExecuteAsync(mergeOperation);
            }
            catch (Exception ex)
            {
                var message = $"Unable to get insert link";
                logger.LogError(ex, message);
                throw;
            }
        }

        /// <summary>
        /// Get a given link from the table strorage by it's ShortUrl.
        /// Otherwise throw an expcetion to allow the caller to redirect to https://evancjohnson.com
        /// </summary>
        /// <param name="table">The table holding urls</param>
        /// <param name="shortUrl">The short url to retrive</param>
        /// <param name="logger">The log instance</param>
        /// <returns>
        /// Returns a link entity
        /// </returns>
        public static async Task<LinkEntity> GetLink(CloudTable table, string shortUrl, ILogger logger)
        {
            try
            {
                await table.CreateIfNotExistsAsync();

                var findOperation = TableOperation.Retrieve<LinkEntity>("ShortUrl", shortUrl);
                TableResult result = await table.ExecuteAsync(findOperation);
                LinkEntity linkEntity = result.Result as LinkEntity;

                return linkEntity;
            }
            catch (Exception ex)
            {
                var message = $"Unable to get link for given shortUrl: {shortUrl}";
                logger.LogError(ex, message);
                throw;
            }
        }
    }
}
