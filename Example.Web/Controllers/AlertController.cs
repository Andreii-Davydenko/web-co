using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScrapeIT.Framework.Business.Checks;
using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.Scraper.Tabelar;
using ScrapeIT.Web.Mvc.Controllers;

namespace Example.Web.Controllers
{
    public class AlertController : AlertControllerBase
    {
        public AlertController(ScrapeRepository scrapeRepository, TabelarFactory tabelarFactory, ICustomerContext customerContext, ScheduleRepository scheduleRepository) : base(scrapeRepository, tabelarFactory, customerContext, scheduleRepository)
        {
        }

        public async Task<IActionResult> Test()
        {            
            await ScheduleRepository.AddNewAsync(DateTime.Now.AddMinutes(5), "TestAlert", "Deze test is OK!");
            return RedirectToAction("Index");
        }
    }
}