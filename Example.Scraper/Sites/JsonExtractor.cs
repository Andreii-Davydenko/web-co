using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.CloudUtilities;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Core.Extensions;
using ScrapeIT.Framework.Scraper.Html;
using ScrapeIT.Framework.Scraper.UrlExtractor;
using System;
using System.Collections.Generic;

namespace Example.Scraper.Sites
{
    internal class JsonExtractor : ScrapeIT.Framework.Scraper.Json.ExtractorBase
    {
        public JsonExtractor(IBlobStorage blobStorage, ILogger<ScrapeIT.Framework.Scraper.Json.ExtractorBase> logger, PageRepository pageRepository, QueueFactory queueFactory, ICustomerContext customerContext) : base(blobStorage, logger, pageRepository, queueFactory, customerContext)
        {
        }
        protected override IEnumerable<ExtractorRequest> RetrievePostRequests()
        {
            var query = RequestUrl.ParseQueryString();
            var total_count = int.Parse(JObject["total_count"].ToString());
            if (query.ContainsKey("filters"))
            {
                var filter = query["filters"];
                Uri searchUri = new Uri("https://brandcenter.wabco-auto.com/portals/api/assets/search/public/section/01c3c98e-54a3-49bb-ad6e-0d70be51b794");
                for (int i = 0; i < total_count; i++)
                {
                    string jsonData = "{\n" +
                    "    \"expands\": \"file_properties,spinset,metadata,metadata_info,metadata_vocabulary\",\n" +
                    "    \"filters\": {},\n" +
                    "    \"limit\": 1,\n" +
                    "    \"offset\": " + i + ",\n" +
                    "    \"query\": \"" + filter + "\",\n" +
                    "    \"sortField\": \"asset-filename\",\n" +
                    "    \"sortOrder\": \"desc\"\n" +
                    "}";
                    yield return ExtractorRequest.PostRequest(searchUri, jsonData, true);

                }
            }
        }
    }
}
