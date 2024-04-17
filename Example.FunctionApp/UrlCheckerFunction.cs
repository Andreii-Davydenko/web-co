using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.Messaging.Messages;
using ScrapeIT.Framework.Messaging.Queues;
using ScrapeIT.Framework.Scraper.UrlChecker;

namespace Example
{
    public class UrlCheckerFunction
    {
        private Compute _compute;
        private readonly ILogger<UrlCheckerFunction> _logger;

        public UrlCheckerFunction(Compute compute, ILogger<UrlCheckerFunction> logger)
        {
            _compute = compute;
            _logger = logger;
        }
#if DEBUG
        [Function("UrlCheckerFunction")]
#else
        [Function("UrlCheckerFunction")]
#endif
        public async Task Run([ServiceBusTrigger(Scraper.CustomerStartup.CUSTOMER_PREFIX + "-" + QueueNames.URLCHECKER, Connection = "ServiceBus")] UrlCheckerRequest request)
        {
            var result = await _compute.ComputeRequestAsync(request, CancellationToken.None);

            switch (result)
            {
                case ComputeResult.QueueOnHold:
                    throw new Exception("Queue on hold");
            }

            _logger.LogInformation($"UrlCheckerFunction processed message: {result}");
        }
    }
}
