using AspNetCore.Filters.WebApi.Models;
using AspNetCore.Filters.WebApi.Utils;
using AspNetCore.Filters.WebApi.Utils.Extensions;
using AspNetCore.Filters.WebApi.Utils.Provider;
using CyberSoft.ServiceSwitching.Utils.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace AspNetCore.Filters.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddMemoryCache();

            // Add feature mangement services 
            services
                .AddSingleton<IFeatureDefinitionProvider, RemoteFeatureProvider>()
                .AddFeatureManagement();

            services
                .AddControllers(o => o.Filters.AddForFeature<CustomHeaderFilter>(nameof(FeatureFlags.ServerEnvHeader))) // Add the global IAsyncActionFilter by feature toggle
                .AddMvcOptions() // Custom extension to add global filters
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.Configure<AppSettings>(this.Configuration);


            // Inject custom filters
            services.AddScoped<HybridFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use toggle features
            app.UseToggleFeatures();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
