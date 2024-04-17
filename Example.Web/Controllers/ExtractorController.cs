using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Scraper.Tabelar;
using ScrapeIT.Framework.Scraper.UrlExtractor;
using ScrapeIT.Web.Mvc.Controllers;

namespace Example.Web.Controllers
{
    public class ExtractorController : ExtractorControllerBase
    {
        public ExtractorController(QueueFactory queueFactory, ExtractorFactory extractorFactory, ScrapeRepository scrapeRepository, TabelarFactory tabelarFactory, ICustomerContext customerContext) : base(queueFactory, extractorFactory, scrapeRepository, tabelarFactory, customerContext)
        {
        }
    }
}