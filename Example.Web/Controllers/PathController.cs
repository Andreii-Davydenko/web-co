using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.Scraper.Contexts;
using ScrapeIT.Framework.Scraper.Processors;
using ScrapeIT.Framework.Scraper.Tabelar;
using ScrapeIT.FunctionApp.Handlers;

namespace Example.Web.Controllers
{
    public class PathController : ScrapeIT.Web.Mvc.Controllers.PathControllerBase
    {
        public PathController(ScrapeHandler<ScrapeContext> scrapeHandler, ReProcessor reProcessor, PageRepository pageRepository, ScrapeRepository scrapeRepository, TabelarFactory tabelarFactory, ICustomerContext customerContext) : base(scrapeHandler, reProcessor, pageRepository, scrapeRepository, tabelarFactory, customerContext)
        {
        }

    }
}