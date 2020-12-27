namespace AspNetCore.Filters.WebApi.Models
{
    /// <summary>
    /// AppSettings
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// The regex for checking ASPNETCORE_ENVIRONMENT value for DisableApiFilter
        /// </summary>
        /// <remarks>When matches, the API with DisableApiFilter will return 404</remarks>
        public string EnvForDisableApiFilter { get; set; }
    }
}
