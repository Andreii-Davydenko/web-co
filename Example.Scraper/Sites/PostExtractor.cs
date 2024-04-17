using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.CloudUtilities;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Core.Extensions;
using ScrapeIT.Framework.Scraper.Html;
using ScrapeIT.Framework.Scraper.Start;
using ScrapeIT.Framework.Scraper.UrlExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Example.Scraper.Sites
{
    internal class PostExtractor : ScrapeIT.Framework.Scraper.Html.DefaultHtmlExtractor
    {
        public PostExtractor(IBlobStorage blobStorage, ILogger<DefaultHtmlExtractor> logger, PageRepository pageRepository, QueueFactory queueFactory, ICustomerContext customerContext) : base(blobStorage, logger, pageRepository, queueFactory, customerContext)
        {
        }
        protected override IEnumerable<ExtractorRequest> RetrievePostRequests()
        { 
            for (int i = 10; i < 100; i ++)
            {
                string jsonData = "{\n" +
                "    \"expands\": \"file_properties,spinset,metadata,metadata_info,metadata_vocabulary\",\n" +
                "    \"filters\": {},\n" +
                "    \"limit\": 1,\n" +
                "    \"offset\": " + 0 + ",\n" +
                "    \"query\": \"" + i + "\",\n" +
                "    \"sortField\": \"asset-filename\",\n" +
                "    \"sortOrder\": \"desc\"\n" +
                "}";
                Uri searchUri = new Uri("https://brandcenter.wabco-auto.com/portals/api/assets/search/public/section/01c3c98e-54a3-49bb-ad6e-0d70be51b794").AppendMeta("filters", i.ToString());

                yield return ExtractorRequest.PostRequest(searchUri, jsonData, true);

            }
        }
    }
}
