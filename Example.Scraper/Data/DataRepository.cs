using ScrapeIT.Framework.Core.Extensions;
using ScrapeIT.Framework.Data.TableStorage;
using ScrapeIT.Framework.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.FSharp.Core.ByRefKinds;

namespace Example.Scraper.Data
{
    public class DataRepository : GenericDataRepository<Image>
    {
        public DataRepository(TableHelper tableHelper) : base(tableHelper)
        {
        }

        public override string TableName => "Images";
        public IAsyncEnumerable<Image> GetAllImages()
        {
            return GetByQueryAsync<Image>("PartitionKey ne ''");
        }
        public async Task RemoveInputsAsync()
        {
            var inputs = await GetByPartitionKeyAsync<Image>("Image").ToListAsync();
            await DeleteAsync(inputs);
        }

    }
}
