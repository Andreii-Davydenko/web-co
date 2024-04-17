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
    public class TabelarFunction
    {
        private TabelarHandler _tabelarHandler;
        private readonly ILogger<TabelarFunction> _logger;

        public TabelarFunction(TabelarHandler tabelarHandler, ILogger<TabelarFunction> logger)
        {
            _tabelarHandler = tabelarHandler;
            _logger = logger;
        }
#if DEBUG
        [Function("TabelarFunction")]
#else
        [Function("TabelarFunction")]
#endif
        [ServiceBusOutput(Scraper.CustomerStartup.CUSTOMER_PREFIX + "-" + QueueNames.STORE, Connection = "ServiceBus")]
        public async Task<string> Run([ServiceBusTrigger(Scraper.CustomerStartup.CUSTOMER_PREFIX + "-" + QueueNames.SMELLY, Connection = "ServiceBus")] SmellyRequest smell)
        {
            var storeRequest = await _tabelarHandler.GenerateXmlAsync(smell);

            _logger.LogInformation($"Tabelar processed message: {smell.Url}");

            if (storeRequest == null)
                return null;
            return storeRequest.ConvertToString();
        }
    }
}
