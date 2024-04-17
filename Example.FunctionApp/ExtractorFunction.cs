using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Messaging.Messages;
using ScrapeIT.Framework.Messaging.Queues;
using ScrapeIT.FunctionApp.Handlers;

namespace Example.FunctionApp
{
    public class ExtractorFunction
    {
        private ExtractorHandler _urlExtractorHandler;
        private readonly ILogger<ExtractorFunction> _logger;

        public ExtractorFunction(ExtractorHandler urlExtractorHandler, ILogger<ExtractorFunction> logger)
        {
            _urlExtractorHandler = urlExtractorHandler;
            _logger = logger;
        }
#if DEBUG
        [Function("UrlExtractorFunction")]
#else
       [Function("UrlExtractorFunction")]
#endif
        public async Task Run([ServiceBusTrigger(Scraper.CustomerStartup.CUSTOMER_PREFIX + "-" + QueueNames.ENRICH, TopicSubscriptionNames.ENRICH_URLEXTRACTOR, Connection = "ServiceBus")] EnrichRequest request)
        {
            var messageCount = await _urlExtractorHandler.ProcessAsync(request);
            _logger.LogInformation($"UrlExtractor processed message (amount:{messageCount}): {request.Url}");
        }
    }
}
