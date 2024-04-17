using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.Scraper.Smeller;
using ScrapeIT.Framework.Scraper.Tabelar;

namespace Example.Web.Controllers
{
    public class SmellerController : ScrapeIT.Web.Mvc.Controllers.SmellerControlerBase
    {
        public SmellerController(SmellerFactory smellerFactory, PageRepository pageRepository, ScrapeRepository scrapeRepository, TabelarFactory tabelarFactory, ICustomerContext customerContext) : base(smellerFactory, pageRepository, scrapeRepository, tabelarFactory, customerContext)
        {
        }
    }
}