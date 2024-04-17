using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.Scraper.Tabelar;
using ScrapeIT.Web.Mvc.Controllers;

namespace Example.Web.Controllers
{
    public class TabelarController : TabelarControllerBase
    {
        public TabelarController(ScrapeRepository scrapeRepository, TabelarFactory tabelarFactory, ICustomerContext customerContext) : base(scrapeRepository, tabelarFactory, customerContext)
        {
        }
    }
}