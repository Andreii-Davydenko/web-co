//using Microsoft.Azure.Functions.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection;
//using Example;
//using ScrapeIT.Framework.Scraper;

//[assembly: FunctionsStartup(typeof(Startup))]
//namespace Example
//{
//    public class Startup : ScrapeIT.FunctionApp.Startup
//    {
//        public override void Configure(IFunctionsHostBuilder builder)
//        {
//            base.Configure(builder);

//            builder.Services.AddTransient<ICustomerStartup, Scraper.CustomerStartup>();
//            builder.Services.AddTransient<Scraper.CustomerStartup>();
//            builder.Services.AddSingleton<Scraper.Data.DataRepository>(); // Internally uses a table storage connection, should not create to much of these

//            Scraper.CustomerStartup customerStartup = Scraper.CustomerStartup.InitializeForRegistration();
//            customerStartup.StartRegistration(builder.Services);
//        }
//    }
//}
