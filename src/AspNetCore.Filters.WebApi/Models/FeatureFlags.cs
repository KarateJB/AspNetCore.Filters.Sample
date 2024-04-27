namespace AspNetCore.Filters.WebApi.Models
{
    /// <summary>
    /// Feature flags
    /// </summary>
    public enum FeatureFlags
    {
        /// <summary>
        /// API Doc
        /// </summary>
        ApiDoc = 1,

        /// <summary>
        /// Demo
        /// </summary>
        Demo,

        /// <summary>
        /// DemoGlobalFilter
        /// </summary>
        DemoGlobalFilter,

        /// <summary>
        /// Tests
        /// </summary>
        Tests,

        /// <summary>
        /// Http header: Server environment
        /// </summary>
        ServerEnvHeader
    }
}
