using Example.Scraper.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MontfoortIT.Office.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Example.Web.Controllers
{
    public class InputController : Controller
    {
        private readonly DataRepository _dataRepository;

        public InputController(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile excel)
        {
            Application inputApp = new Application();
            inputApp.ReadFile(excel.OpenReadStream());
            List<Image> inputs = new List<Image>();
            var sheets = inputApp.Workbook.Sheets;
          
            inputs = inputs.Distinct().ToList();

            await _dataRepository.RemoveInputsAsync();
            await _dataRepository.SaveDataObjectsAsync(inputs, null, null);

            return View();
        }
    }
}
