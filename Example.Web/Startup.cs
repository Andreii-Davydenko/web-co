using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ScrapeIT.Framework.Scraper;
using Example.Scraper;
using Microsoft.Extensions.Hosting;

namespace Example.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            ScrapeIT.Web.Mvc.DependencyInjection.Startup.Configure(services, false);

            services.AddTransient<ICustomerStartup, CustomerStartup>();
            services.AddTransient<Scraper.Data.DataRepository>();
            services.AddTransient<CustomerStartup>();
            CustomerStartup customerStartup = CustomerStartup.InitializeForRegistration();

            customerStartup.StartRegistrationAsync(services);

            services.AddTransient<ScrapeIT.Framework.CloudUtilities.Startup>();
            services.AddTransient<ScrapeIT.Framework.Scraper.Html.DefaultHtmlExtractor>();

            services.AddControllersWithViews(options => { options.MaxModelBindingCollectionSize = 1024 * 8; });

            services.AddMvc(options =>
            {
                options.MaxModelBindingCollectionSize = 102400;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // new for .net core 3.1
            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
