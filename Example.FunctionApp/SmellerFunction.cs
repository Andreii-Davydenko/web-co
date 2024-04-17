using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Messaging.Messages;
using ScrapeIT.Framework.Messaging.Queues;
using ScrapeIT.FunctionApp.Handlers;

namespace Example.FunctionApp
{
    public class SmellerFunction
    {
        private SmellerHandler _smellerHandler;
        private readonly ILogger<SmellerFunction> _logger;

        public SmellerFunction(SmellerHandler smellerHandler, ILogger<SmellerFunction> logger)
        {
            _smellerHandler = smellerHandler;
            _logger = logger;
        }
#if DEBUG
        [Function("SmellerFunction")]
#else
        [Function("SmellerFunction")]
#endif
        [ServiceBusOutput(Scraper.CustomerStartup.CUSTOMER_PREFIX + "-" + QueueNames.SMELLY, Connection = "ServiceBus")]
        public async Task<string> Run([ServiceBusTrigger(Scraper.CustomerStartup.CUSTOMER_PREFIX + "-" + QueueNames.ENRICH, TopicSubscriptionNames.ENRICH_SMELLER, Connection = "ServiceBus")] EnrichRequest request)
        {
            bool pageIsSmelly = await _smellerHandler.IsSmellyAsync(request);

            _logger.LogInformation($"smelly processed message: {request.Url}, Smelly:{pageIsSmelly}");

            if (pageIsSmelly)
                return _smellerHandler.ConvertEnrichToSmellyRequest(request).ConvertToString();

            return null;
        }
    }
}
