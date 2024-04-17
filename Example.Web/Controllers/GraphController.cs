using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.Scraper.Tabelar;

namespace Example.Web.Web.Controllers
{
    public class GraphController : ScrapeIT.Web.Mvc.Controllers.GraphControllerBase
    {
        public GraphController(PageRepository pageRepository, ScrapeRepository scrapeRepository, TabelarFactory tabelarFactory, ICustomerContext customerContext) : base(pageRepository, scrapeRepository, tabelarFactory, customerContext)
        {
        }
    }
}