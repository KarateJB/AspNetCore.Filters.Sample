using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNetCore.Filters.WebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace CyberSoft.ServiceSwitching.Utils.Attributes
{
    /// <summary>
    /// Disable API filter
    /// </summary>
    public class DisableApiFilter : Attribute, IAsyncResourceFilter
    {
        private readonly string env = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hostEnvironment">IWebHostEnvironment</param>
        /// <param name="options">IOptions</param>
        /// <param name="onEnv">Regex for checking ASPNETCORE_ENVIRONMENT</param>
        public DisableApiFilter(
           IWebHostEnvironment hostEnvironment,
            IOptions<AppSettings> options,
            string onEnv = "")
        {
            this.env = hostEnvironment.EnvironmentName;
            this.OnEnv = string.IsNullOrEmpty(onEnv) ? options.Value?.EnvForDisableApiFilter : onEnv;
        }

        /// <summary>
        /// Regression expression of ASPNETCORE_ENVIRONMENT for disabling API
        /// </summary>
        public string OnEnv { get; set; }

        /// <summary>
        /// OnResourceExecutionAsync
        /// </summary>
        /// <param name="context">ResourceExecutingContext</param>
        /// <param name="next">ResourceExecutionDelegate</param>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (string.IsNullOrEmpty(this.OnEnv))
            {
                await next();
            }
            else
            {
                var regex = new Regex(this.OnEnv);

                if (regex.IsMatch(this.env))
                {
                    context.Result = new EmptyResult();
                    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                }
                else
                {
                    await next();
                }
            }
        }
    }
}
