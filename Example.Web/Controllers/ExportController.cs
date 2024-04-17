using ScrapeIT.Framework.Export.Excel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ScrapeIT.Framework.Business.Interface;
using Microsoft.AspNetCore.Mvc;
using Example.Scraper.Data;
using ScrapeIT.Framework.Core.Extensions;
using System.Net.Http;
using System;
using MontfoortIT.Office.Excel;
using MontfoortIT.Office.Excel.Templates;

namespace Example.Web.Controllers
{
    public class ExportController : ScrapeIT.Web.Mvc.Base.Controller
    {
        private readonly DataRepository _dataRepository;
        public ExportController(DataRepository dataRepository, ICustomerContext customerContext) : base(customerContext)
        {
            _dataRepository = dataRepository;
        }

        private string ChangeToCode(string input)
        {
            string numericPart = input.Substring(0, Math.Min(10, input.Length));
            bool isNumeric = true;
            foreach (char c in numericPart)
            {
                if (!Char.IsDigit(c))
                {
                    isNumeric = false;
                    break;
                }
            }

            // If all characters are numeric, format the string
            if (isNumeric)
            {
                return $"WES{numericPart.Substring(0, 3)}.{numericPart.Substring(3, 3)}.{numericPart.Substring(6, 3)}.{numericPart.Substring(9)}";
            }
            else
            {
                // If not all characters are numeric, return original string
                return "";
            }
        }
        public async Task<IActionResult> ExportImages()
        {

            var images = await _dataRepository.GetAllImages().ToListAsync();
            MemoryStream memStream = new MemoryStream();
            List<ColumnTemplate> templates = new List<ColumnTemplate>();

            templates.Add(new FuncColumnTemplate<Image>("Id", c => c.Id));
            templates.Add(new FuncColumnTemplate<Image>("FielName", c => c.ImageOldFileName));
            templates.Add(new FuncColumnTemplate<Image>("WES Code", c => ChangeToCode(c.ImageOldFileName)));
            //templates.Add(new FuncColumnTemplate<Image>("NewUrl", c => c.ImageNewUrl));

            var customerName = "Images";
            using (ScrapeITFormat app = new ScrapeITFormat())
            {


                app.Initialize(customerName, "Images");
                app.FillFromObjects(templates, images);
                app.Write(memStream);
            }

            memStream.Position = 0;

            return File(memStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", customerName + ".xlsx");
        }
        public async Task<IActionResult> DownloadBlobsAsZip()
        {
            var images = await _dataRepository.GetAllImages().ToListAsync();
            string directoryPath = @"D:\Images";
            foreach (var image in images)
            {
                string localFilePath = Path.Combine(directoryPath, image.ImageOldFileName);

                // Ensure the directory exists
                Directory.CreateDirectory(directoryPath);
                if (System.IO.File.Exists(localFilePath))
                {
                    // Skip the current iteration if the file exists
                    continue;
                }
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(image.ImageOldUrl);

                    // Ensure we got a successful response
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Error: {response.StatusCode}");
                    }

                    // Read the content as byte array
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                    // Write the downloaded image bytes to a local file
                    await System.IO.File.WriteAllBytesAsync(localFilePath, imageBytes);

                }
            }
            return Ok(true);
        }
    }
}