using ScrapeIT.Framework.Scraper.Paths;
using Example.Scraper.Sites;
using System.Collections.Generic;
using Example.Scraper.Games;

namespace Example.Scraper.Paths
{
    public class Descriptor : ScrapePathDescriptor
    {
        public override bool MappingPerHost => false;

        protected override IEnumerable<ScrapePath> InitializePaths()
        {
            yield return new ScrapePath()
               .Start<PostExtractor>(@"https://brandcenter.wabco-auto.com/portals/wroxleu6/CustomerPortal")
               .Next<JsonExtractor, ImageTabelar>($"https://brandcenter.wabco-auto.com/portals/api/assets/search/public/section/01c3c98e-54a3-49bb-ad6e-0d70be51b794", containsPaging: true);
        }
    }
}
