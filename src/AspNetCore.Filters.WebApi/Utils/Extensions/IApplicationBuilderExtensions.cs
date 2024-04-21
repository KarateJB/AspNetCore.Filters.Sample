using AspNetCore.Filters.WebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Filters.WebApi.Utils.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Use toggle features
        /// </summary>
        /// <param name="app"></param>
        /// <returns>IApplicationBuilder </returns>
        public static IApplicationBuilder UseToggleFeatures(this IApplicationBuilder app)
        {
            var featureManager = app.ApplicationServices.GetService<Microsoft.FeatureManagement.IFeatureManager>();
            bool isEnableApiDoc = featureManager.IsEnabledAsync(nameof(FeatureFlags.ApiDoc)).Result;

            if (isEnableApiDoc)
            {
                // Enable something like Swagger, etc...
            }

            return app;
        }
    }
}
