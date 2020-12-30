using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
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
    /// <see cref="https://github.com/microsoft/FeatureManagement-Dotnet/blob/main/src/Microsoft.FeatureManagement/ConfigurationFeatureDefinitionProvider.cs"/>
    public class RemoteFeatureProvider : IFeatureDefinitionProvider
    {
        private const string FEATURE_MANAGEMENT_SECTION = "FeatureManagement";
        private const string FFEATURE_FILTER_SECTION = "EnabledFor";
        private const int CACHE_TIME = 180; // Cache time in seconds
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
            this.definitions = new ConcurrentDictionary<string, FeatureDefinition>();
        }

        public async IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
        {
            await this.reloadConfiguration();

            foreach (var featureSection in this.getFeatureDefinitionSections())
            {
                yield return this.definitions.GetOrAdd(featureSection.Key, _ => this.readFeatureDefinition(featureSection));
            }
        }

        public async Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
        {
            if (featureName == null)
            {
                throw new ArgumentNullException(nameof(featureName));
            }

            await this.reloadConfiguration();
            var definition = this.definitions.GetOrAdd(featureName, name => this.readFeatureDefinition(name));
            return definition;
        }

        private async Task reloadConfiguration()
        {
            if (!this.memoryCache.TryGetValue(CacheKeys.FeatureConfig, out IConfiguration featureConfig))
            {
                this.definitions.Clear();

                using var httpClient = this.httpClientFactory.CreateClient();
                var streamResponse = await httpClient.GetStreamAsync("http://localhost:3000/feature-configuration"); // Use json-server to run the json file at /AspNetCore.Filters.WebApi/Assets/feature_management.json
                this.configuration = new ConfigurationBuilder().AddJsonStream(streamResponse).Build();

                this.memoryCache.Set(CacheKeys.FeatureConfig, this.configuration, TimeSpan.FromSeconds(CACHE_TIME));
            }
            else
            {
                this.configuration = featureConfig;
            }
        }

        private FeatureDefinition readFeatureDefinition(string featureName)
        {
            var configurationSection = this.getFeatureDefinitionSections().FirstOrDefault(section => section.Key.Equals(featureName, StringComparison.OrdinalIgnoreCase));

            if (configurationSection == null)
            {
                return null;
            }

            return this.readFeatureDefinition(configurationSection);
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

        private IEnumerable<IConfigurationSection> getFeatureDefinitionSections()
        {
            bool isExist = this.configuration.GetChildren().Any(x => x.Key.Equals(FEATURE_MANAGEMENT_SECTION));
            if (isExist)
                return this.configuration.GetSection(FEATURE_MANAGEMENT_SECTION).GetChildren();
            else
                return this.configuration.GetChildren();
        }

    }
}
