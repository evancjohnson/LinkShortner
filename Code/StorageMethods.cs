using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LinkShortner.Code
{
    public static class StorageMethods
    {
        /// <summary>
        /// Insert a link into the table storage
        /// </summary>
        /// <returns></returns>
        public static async Task InsertLink(CloudTable table, ITableEntity entity)
        {
            try
            {
                //merge the op
                var mergeOperation = TableOperation.InsertOrMerge(entity);

                var result = await table.ExecuteAsync(mergeOperation);
            }
            catch (Exception ex)
            {

                throw;
            }
        }    
    }
}
