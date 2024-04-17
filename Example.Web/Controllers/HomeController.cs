using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;

namespace Example.Web.Controllers
{
    public class HomeController : ScrapeIT.Web.Mvc.Controllers.HomeControllerBase
    {
        public HomeController(ICustomerContext customrContext, ScrapeIT.Framework.CloudUtilities.Startup startup) : base(customrContext, startup)
        {
        }
    }
}