using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Messaging.Messages;
using ScrapeIT.Framework.Messaging.Queues;
using ScrapeIT.Framework.Scraper.Contexts;
using ScrapeIT.FunctionApp.Handlers;

namespace Example.FunctionApp
{
    public class ScraperFunction
    {
        private ScrapeHandler<ScrapeContext> _scrapeHandler;
        private readonly ILogger<ScraperFunction> _logger;

        public ScraperFunction(ScrapeHandler<ScrapeContext> scrapeHandler, ILogger<ScraperFunction> logger)
        {
            _scrapeHandler = scrapeHandler;
            _logger = logger;
        }
#if DEBUG
        [Function("ScraperFunction")]
#else
       [Function("ScraperFunction")]
#endif
        [ServiceBusOutput(Scraper.CustomerStartup.CUSTOMER_PREFIX + "-" + QueueNames.ENRICH, Connection = "ServiceBus", EntityType = ServiceBusEntityType.Topic)]
        public async Task<string> Run([ServiceBusTrigger(Scraper.CustomerStartup.CUSTOMER_PREFIX + "-" + QueueNames.SCRAPER, Connection = "ServiceBus")] ScrapeRequest request)
        {
            var next = await _scrapeHandler.ProcessMessageAsync(request);

            _logger.LogInformation($"Scraped {request.Url}, action {next.action}, message {next.message}");
            if (next.action == NextAction.Next)
                return next.request.ConvertToString();
            else if (next.action == NextAction.Retry)
                throw new System.Exception("Retry:" + next.message);
            else
                return null;
        }
    }
}
