using ScrapeIT.Framework.Data.TableStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Scraper.Data
{
    public class Image : TableEntityBase
    {
        [Description]
        public string Id { get; set; }
        [Description]
        public string ImageOldFileName { get; set; }
        [Description]
        public string ImageNewFileName { get; set; }
        [Description]
        public string ImageOldUrl { get; set; }
        [Description]
        public string ImageNewUrl { get; set; }

        protected override void PreSave()
        {
            PartitionKey = CleanKey(Id);
            RowKey = CleanKey(ImageNewFileName);
        }
    }
}
