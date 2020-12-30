using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.Filters.WebApi.Utils.Factory;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace AspNetCore.Filters.WebApi.Utils.Provider
{
    /// <summary>
    /// Remote Feature Provider
    /// </summary>
    public class RemoteFeatureProvider : IFeatureDefinitionProvider
    {
        private const string FEATURE_MANAGEMENT_SECTION = "FeatureManagement";
        private const string FFEATURE_FILTER_SECTION = "EnabledFor";
        private const int CACHE_TIME = 30; // Cache time in seconds
        private readonly ILogger<RemoteFeatureProvider> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMemoryCache memoryCache;
        private IConfiguration configuration;
        private ConcurrentDictionary<string, FeatureDefinition> definitions;

        /// <summary>
        /// Custructor
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="httpClientFactory">HttpClientFactory</param>
        /// <param name="memoryCache">Memory cache</param>
        public RemoteFeatureProvider(
            ILogger<RemoteFeatureProvider> logger,
            IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.memoryCache = memoryCache;
        }

        public async IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
        {
            await this.reloadFeatureConfigsAsync();

            foreach (var featureSection in await this.getSectionAsync(FEATURE_MANAGEMENT_SECTION))
            {
                yield return this.definitions.GetOrAdd(featureSection.Key, _ => this.readFeatureDefinition(featureSection));
            }
        }

        public async Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
        {
            throw new NotImplementedException();
        }

        private async Task reloadFeatureConfigsAsync()
        {
            if (!this.memoryCache.TryGetValue(CacheKeys.FeatureConfig, out IConfiguration featureConfig))
            {
                using var httpClient = this.httpClientFactory.CreateClient();
                var streamResponse = await httpClient.GetStreamAsync("/");
                this.configuration = new ConfigurationBuilder().AddJsonStream(streamResponse).Build();

                this.memoryCache.Set(CacheKeys.FeatureConfig, this.configuration, TimeSpan.FromSeconds(CACHE_TIME));
            }
        }

        private async Task<IEnumerable<IConfigurationSection>> getSectionAsync([Required] string section)
        {
            bool isExist = this.configuration.GetChildren().Any(x => x.Key.Equals(section));
            if (isExist)
                return await Task.FromResult(this.configuration.GetSection(section).GetChildren());
            else
                return await Task.FromResult(this.configuration.GetChildren());
        }

        private FeatureDefinition readFeatureDefinition(IConfigurationSection configurationSection)
        {
            var enabledFor = new List<FeatureFilterConfiguration>();

            var val = configurationSection.Value;

            if (string.IsNullOrEmpty(val))
            {
                val = configurationSection[FFEATURE_FILTER_SECTION];
            }

            if (!string.IsNullOrEmpty(val) && bool.TryParse(val, out bool result) && result)
            {
                enabledFor.Add(new FeatureFilterConfiguration { Name = "AlwaysOn" });
            }
            else
            {
                var filterSections = configurationSection.GetSection(FFEATURE_FILTER_SECTION).GetChildren();

                foreach (var filterSection in filterSections)
                {
                    if (int.TryParse(filterSection.Key, out int i) && !string.IsNullOrEmpty(filterSection[nameof(FeatureFilterConfiguration.Name)]))
                    {
                        enabledFor.Add(
                            new FeatureFilterConfiguration
                            {
                                Name = filterSection[nameof(FeatureFilterConfiguration.Name)],
                                Parameters = filterSection.GetSection(nameof(FeatureFilterConfiguration.Parameters))
                            });
                    }
                }
            }

            return new FeatureDefinition { Name = configurationSection.Key, EnabledFor = enabledFor };
        }
    }
}
