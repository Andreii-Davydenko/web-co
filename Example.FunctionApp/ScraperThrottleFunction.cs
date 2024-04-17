using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Messaging.Queues;
using ScrapeIT.FunctionApp.Handlers;

namespace Example.FunctionApp
{
    public class ScraperThrottleFunction
    {

        private ScrapePlannerHandler _scrapePlannerHandler;
        private readonly QueueFactory _queueFactory;
        private readonly ILogger<ScraperThrottleFunction> _logger;
        private ICustomerContext _customerContext;


        public ScraperThrottleFunction(ScrapePlannerHandler scrapePlannerHandler, QueueFactory queueFactory, ICustomerContext customerContext, ILogger<ScraperThrottleFunction> logger)
        {
            _scrapePlannerHandler = scrapePlannerHandler;
            _queueFactory = queueFactory;
            _logger = logger;
            _customerContext = customerContext;

        }
#if DEBUG
        [Function("ScraperThrottleFunction")]
#else
        [Function("ScraperThrottleFunction")]
#endif

        public Task Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer)
        {
            var scrapeQueue = _queueFactory.ResolveQueue(QueueNames.SCRAPER);

            return _scrapePlannerHandler.CopyToScrapeQueueAsync(scrapeQueue, 5);
        }
    }
}
