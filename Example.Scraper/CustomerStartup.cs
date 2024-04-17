using Microsoft.Extensions.DependencyInjection;
using Example.Scraper.Data;
using ScrapeIT.Framework.Business.Interface;
using ScrapeIT.Framework.Business.Repository;
using ScrapeIT.Framework.Data.TableStorage;
using ScrapeIT.Framework.Messaging.Interfaces.Scraper;
using ScrapeIT.Framework.Scraper;
using ScrapeIT.Framework.Scraper.Paths;
using System.Threading.Tasks;
using Example.Scraper.Sites;

namespace Example.Scraper
{
    public class CustomerStartup : CustomerStartupBase<Data.DataRepository>
    {
        public const string CUSTOMER_PREFIX = "wabco";

        public CustomerStartup(DataRepositoryFactory dataRepositoryFactory, DataRepository dataRepository, Registrations<IScraper> registrations) : base(dataRepositoryFactory, dataRepository, registrations)
        {
        }

        private CustomerStartup()
            : base(null, null,null)
        {
        }

        public override string CustomerName => "wabco";

        public override string CustomerPrefix => CUSTOMER_PREFIX;

        public override string CustomerServiceBusKey => "wabco";

        public override ScrapePathDescriptor ScrapePathDescriptor => new Paths.Descriptor();

        public override Task StartRegistrationAsync(IServiceCollection services, bool onlyPathsAreUsed = true)
        {
            base.StartRegistrationAsync(services, onlyPathsAreUsed);

            services.AddTransient<Sites.ImageTabelar>();
            services.AddTransient<Games.GameTabelar>();

            services.AddTransient<Image>();
            services.AddTransient<DataRepository>();

            ScraperRegistrations.Register<Sites.MyScraper>("brandcenter.wabco-auto.com", services);

            DependencyInjection.ConfigureGraph(services);
            DependencyInjection.ConfigureMappings(services);
            return base.StartRegistrationAsync(services, onlyPathsAreUsed);
        }


        public static CustomerStartup InitializeForRegistration()
        {
            return new CustomerStartup();
        }

    }
}
