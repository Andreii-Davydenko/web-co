using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.CloudUtilities;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Scraper.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Scraper.Games
{
    internal class SearchGameExtractor : ScrapeIT.Framework.Scraper.Html.DefaultHtmlExtractor
    {
        public SearchGameExtractor(IBlobStorage blobStorage, ILogger<DefaultHtmlExtractor> logger, PageRepository pageRepository, QueueFactory queueFactory, ICustomerContext customerContext) : base(blobStorage, logger, pageRepository, queueFactory, customerContext)
        {
        }

        protected override IEnumerable<Uri> RetrieveGetUrls()
        {

            var games = HtmlDocument.DocumentNode.SelectNodes("//a[@class='grid-item-title']");
            if (games != null)
            {
                foreach (var game in games)
                {
                    yield return new Uri(game?.Attributes["href"]?.Value);
                }
            }

            var nextNode = HtmlDocument.DocumentNode.SelectSingleNode("//a[@class='show-more']");
            if (nextNode != null)
            {
                var nextUrl = "https://apkpure.net" + nextNode.Attributes["href"]?.Value;
                yield return new Uri(nextUrl);
            }
        }
    }
}
