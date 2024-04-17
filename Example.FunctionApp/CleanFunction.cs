using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.Data.TableStorage.Graph;
using ScrapeIT.FunctionApp.Clean;

namespace Example.FunctionApp
{
    public class CleanFunction
    {
        private readonly CleanBlobs _cleanBlobs;
        private readonly CleanScrapes _cleanScrapes;
        private readonly CleanQueues _cleanQueues;
        private readonly ILogger<CleanFunction> _logger;

        public CleanFunction(CleanBlobs cleanBlobs, CleanScrapes cleanScrapes, CleanQueues cleanQueues, ILogger<CleanFunction> logger)
        {
            _cleanBlobs = cleanBlobs;
            _cleanScrapes = cleanScrapes;
            _cleanQueues = cleanQueues;
            _logger = logger;
        }

#if DEBUG
        [Function("CleanFunction")]
#else
        [Function("CleanFunction")]
#endif
        public Task Run([TimerTrigger("0 0 1 * * *")] TimerInfo myTimer)
        {
            return Task.WhenAll(_cleanBlobs.CleanAsync(), _cleanScrapes.CleanAsync(), _cleanQueues.CleanScrapeQueuesAsync());
        }
    }
}
