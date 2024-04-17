using ScrapeIT.Framework.Messaging.Interfaces;
using ScrapeIT.Framework.Messaging.Interfaces.Scraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Scraper.Sites
{
    internal class MyScraper : ScrapeIT.Framework.Scraper.Scrapers.IE11Scraper
    {
        public MyScraper(IAppSettings appSettings) : base(appSettings)
        {
        }

        protected override async Task<IWebResponse> ScrapeGetAsyn(Uri address, Uri referAddress)
        {
            var response = await base.ScrapeGetAsyn(address, referAddress);
            if(response.StatusCode == System.Net.HttpStatusCode.Moved)
            {
                return await ScrapeGetAsyn(response.ResponseUri, referAddress);
            }
            return response;
        }
    }
}
