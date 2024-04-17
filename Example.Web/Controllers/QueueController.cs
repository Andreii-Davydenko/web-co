using Microsoft.AspNetCore.Mvc;
using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Scraper.Tabelar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Web.Controllers
{
    public class QueueController : ScrapeIT.Web.Mvc.Controllers.QueueControllerBase
    {
        public QueueController(QueueFactory queueFactory, TopicFactory topicFactory, ScrapeIT.Framework.CloudUtilities.Startup startup, ScrapeRepository scrapeRepository, TabelarFactory tabelarFactory, ICustomerContext customerContext) : base(queueFactory, topicFactory, startup, scrapeRepository, tabelarFactory, customerContext)
        {
        }
    }
}
