using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScrapeIT.Framework.Scraper;
using ScrapeIT.FunctionApp.Handlers;

var builder = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults();

builder = builder.ConfigureServices(services =>
{
    services.AddTransient<ICustomerStartup, Example.Scraper.CustomerStartup>();
    services.AddTransient<Example.Scraper.CustomerStartup>();

    Example.Scraper.CustomerStartup customerStartup = Example.Scraper.CustomerStartup.InitializeForRegistration();
    customerStartup.StartRegistrationAsync(services).Wait();

    ScrapeIT.FunctionApp.Startup.Configure(services);
});


var host = builder
    .Build();

host.Run();
