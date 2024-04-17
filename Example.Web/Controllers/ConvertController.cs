using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MontfoortIT.Office.Excel;
using MontfoortIT.Office.Excel.Standard.Templates;
using ScrapeIT.Framework.Export.Excel;

namespace Example.Web.Controllers
{
    public class ConvertController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CsvToXlsx(IFormFile file)
        {
            MontfoortIT.Library.Streams.FileConvertors.SemiColonSeperatorToXml commaSeperatedToXml = new MontfoortIT.Library.Streams.FileConvertors.SemiColonSeperatorToXml();
            XDocument doc;
            
            using (MemoryStream memoryStream = new MemoryStream())
            using (var str = file.OpenReadStream())
            {
                XmlWriter writer = XmlWriter.Create(memoryStream);
                await commaSeperatedToXml.ConvertAsync(str, writer);
                writer.Flush();

                memoryStream.Position = 0;

                doc = XDocument.Load(memoryStream);
            }


            var rows = doc.Element("table").Elements("row");
            var header = rows.First();
            FuncTemplateList<XElement> template = new();
            int cols = 0;
            foreach (var col in header.Elements("column"))
            {
                int colNum = cols;
                template.Add(col.Value.Trim('\"'), c=> GetValue(c,colNum));
                cols++;
            }

            MemoryStream outputFile = new MemoryStream();
            using (ScrapeITFormat format = new ScrapeITFormat())
            {
                format.Initialize("Customer");
                format.FillFromObjects(template, rows.Skip(1));
                format.Write(outputFile);
            }
            outputFile.Position = 0;
            return File(outputFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customer.xlsx");
        }

        private string GetValue(XElement arg, int column)
        {
            string value = arg.Elements("column").ToArray()[column].Value;
            value = value.Replace((char)65533, ' ').Replace("\"\"","\"");
            return value.Trim('\"').Trim();
        }
    }
}
