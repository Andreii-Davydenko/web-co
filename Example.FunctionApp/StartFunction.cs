using System;
using Example.Scraper.Data;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;

using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Messaging.Interfaces;
using ScrapeIT.Framework.Messaging.Messages;
using ScrapeIT.Framework.Messaging.Queues;

namespace Example
{
    public class StartFunction
    {


        private readonly QueueFactory _queueFactory;
        private readonly ILogger<StartFunction> _logger;
        private readonly DataRepository _dataRepository;

        public StartFunction(IAppSettings appSettings, ILogger<StartFunction> logger, QueueFactory queueFactory, DataRepository dataRepository)
        {
            _queueFactory = queueFactory;

            var configuration = ScrapeIT.Framework.Scraper.Configuration.ScrapeITConfigurationReader.GetConfig();

            _logger = logger;
            _dataRepository = dataRepository;
        }
#if DEBUG
        //[Function("StartFunction")]
#else
   // [Function("StartFunction")]
#endif

        public async Task Run([TimerTrigger("0 0 11 * * *", RunOnStartup = true)] TimerInfo myTimer)
        {
            _logger.LogInformation($"StartFunction Timer trigger function executed at: {DateTime.Now}");

            var correlationId = Guid.NewGuid();


        }
    }
}
