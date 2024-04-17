using Microsoft.AspNetCore.Mvc;
using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.CloudUtilities;
using ScrapeIT.Framework.Scraper.Tabelar;

namespace Example.Web.Controllers
{
    public class BlobController : ScrapeIT.Web.Mvc.Controllers.BlobControllerBase
    {
        public BlobController(IBlobStorage blobStorage, ScrapeRepository scrapeRepository, TabelarFactory tabelarFactory, ICustomerContext customerContext) : base(blobStorage, scrapeRepository, tabelarFactory, customerContext)
        {
        }


        // GET: Blob
        public ActionResult Index()
        {
            return View();
        }
    }
}