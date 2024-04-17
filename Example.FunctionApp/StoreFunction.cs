using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Messaging.Messages;
using ScrapeIT.Framework.Messaging.Queues;
using ScrapeIT.Framework.Scraper.Mapper;
using ScrapeIT.FunctionApp.Handlers;

namespace Example.FunctionApp
{
    public class StoreFunction
    {
        private StoreHandler _storeHandler;
        private readonly QueueFactory _queueFactory;
        private readonly ILogger<StoreFunction> _logger;

        public StoreFunction(StoreHandler storeHandler, QueueFactory queueFactory, ILogger<StoreFunction> logger)
        {
            _storeHandler = storeHandler;
            _queueFactory = queueFactory;
            _logger = logger;
        }

#if DEBUG
        [Function("StoreFunction")]
#else
        [Function("StoreFunction")]
#endif
        [ServiceBusOutput(Scraper.CustomerStartup.CUSTOMER_PREFIX + "-" + QueueNames.DATASTORED, Connection = "ServiceBus")]
        public async Task<string> Run([ServiceBusTrigger(Scraper.CustomerStartup.CUSTOMER_PREFIX + "-" + QueueNames.STORE, Connection = "ServiceBus")] StoreRequest storeRequest)
        {
            var result = await _storeHandler.StoreRequestAsync(storeRequest);

            switch (result.Status)
            {
                case MapperStatus.MappingAvailable:
                    _logger.LogInformation($"Store message processed: {storeRequest.Url}");

                    return storeRequest.ConvertToString();
                case MapperStatus.NoMappingsAvailable:
                    _logger.LogInformation($"Moved to mapping queue{storeRequest.Url}");

                    var manualMappingQueue = _queueFactory.ResolveQueue(QueueNames.MANUALMAPPING);
                    await manualMappingQueue.SendAsync(new MapManualRequest(storeRequest).ConvertToMessage());
                    return null;
                case MapperStatus.MoveToSmellyQueue:
                    _logger.LogInformation($"Moved to smelly queue{storeRequest.Url}");
                    SmellyRequest smellyRequest = _storeHandler.ConvertToSmellyRequest(storeRequest);
                    var smellyQueue = _queueFactory.ResolveQueue(QueueNames.SMELLY);
                    await smellyQueue.SendAsync(smellyRequest.ConvertToMessage());
                    return null;
            }

            throw new NotSupportedException("What to do?");
        }
    }
}
