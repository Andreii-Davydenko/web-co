using Microsoft.Extensions.Logging;
using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Interface.Repository;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.Business.Transformations;
using ScrapeIT.Framework.CloudUtilities;
using ScrapeIT.Framework.CloudUtilities.MessageBus;
using ScrapeIT.Framework.Messaging.Interfaces;
using ScrapeIT.Framework.Scraper;
using ScrapeIT.Framework.Scraper.Mapper;
using ScrapeIT.Framework.Scraper.Tabelar;

namespace Example.Web.Controllers
{
    public class MappingsController : ScrapeIT.Web.Mvc.Controllers.MappingsControllerBase
    {
        public MappingsController(ICustomerStartup initialize, TypeConverterProcessor typeConverterProcessor, MessageTransformator messageTransformator, TopicFactory topicFactory, PageRepository pageRepository, ITypeFactory typeFactory, QueueFactory queueFactory, IDomainRepository domainRepository, IDataRepositoryFactory dataRepositoryFactory, MappingRepository mappingRepository, MappingElementRepository mappingElementRepository, ScrapeRepository scrapeRepository, TabelarFactory tabelarFactory, ICustomerContext customerContext, IBlobStorage blobStorage, ILogger<MappingsController> logger, LastUsedMappingsCache lastUsedMappingsCache) : base(initialize, typeConverterProcessor, messageTransformator, topicFactory, pageRepository, typeFactory, queueFactory, domainRepository, dataRepositoryFactory, mappingRepository, mappingElementRepository, scrapeRepository, tabelarFactory, customerContext, blobStorage, logger, lastUsedMappingsCache)
        {
        }
    }
}