using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.CloudUtilities;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Scraper.Html;
using ScrapeIT.Framework.Scraper.Paths;
using System;
using System.Threading.Tasks;

namespace Example.Scraper.Games
{
    public class GameTabelar : HtmlTabelarBase
    {
        public GameTabelar(ScrapeRepository scrapeRepository, IBlobStorage blobStorage, ILogger<HtmlTabelarBase> logger, PageRepository pageRepository, QueueFactory queueFactory, ICustomerContext customerContext) : base(scrapeRepository, blobStorage, logger, pageRepository, queueFactory, customerContext)
        {
        }

        protected override Task ProcessHtmlDocumentAsync()
        {
            var gameName = HtmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'title_link')]/h1")?.InnerText;
            //AddToTable("Game", "GameName", gameName);

            var description = HtmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'des-box')]/div[@class='description']")?.InnerText;
            //AddToTable("Game", "Description", description);


            return Task.CompletedTask;
        }

    }
}
