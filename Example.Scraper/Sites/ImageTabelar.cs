using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.CloudUtilities;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Scraper.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Example.Scraper.Sites
{
    public class ImageTabelar : ScrapeIT.Framework.Scraper.Json.JsonTabelarBase
    {
        private readonly IBlobStorage _blobstorage;
        public ImageTabelar(ScrapeRepository scrapeRepository, IBlobStorage blobStorage, ILogger<JsonTabelarBase> logger, PageRepository pageRepository, QueueFactory queueFactory, ICustomerContext customerContext) : base(scrapeRepository, blobStorage, logger, pageRepository, queueFactory, customerContext)
        {
            _blobstorage = blobStorage;
        }


        protected override async Task ConvertJsonToDataSetAsync(JObject jObject)
        {
            var items = jObject["items"];
            foreach(var item in items)
            {
                var external_id = item["external_id"].ToString();
                var fileName = item["filename"].ToString();
                var id = item["id"].ToString();
                var photoUrl = $"https://wabco.widen.net/content/{external_id}/png/{fileName}";
                var newImageUrl = $"https://wabco.widen.net/content/{external_id}/png/{fileName}";

                AddToTable("Images", "Id", id);
                AddToTable("Images", "ImageOldFileName", fileName);
                AddToTable("Images", "ImageNewFileName", fileName);
                AddToTable("Images", "ImageOldUrl", photoUrl);
                AddToTable("Images", "ImageNewUrl", newImageUrl);
            }
        }
    }
}
