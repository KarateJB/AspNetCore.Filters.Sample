using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CyberSoft.ServiceSwitching.Utils.Attributes
{
    /// <summary>
    /// Custom Http header filter
    /// </summary>
    public class CustomHeaderFilter : Attribute, IAsyncActionFilter
    {
        private readonly string env;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hostEnvironment">IWebHostEnvironment</param>
        public CustomHeaderFilter(IWebHostEnvironment hostEnvironment)
        {
            this.env = hostEnvironment.EnvironmentName;
        }

        /// <summary>
        /// OnActionExecutionAsyn
        /// </summary>
        /// <param name="context">ActionExecutingContext</param>
        /// <param name="next">ActionExecutionDelegate</param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            await next();

            string header = "Server-Env";
            context.HttpContext.Response.Headers.Remove(header);
            context.HttpContext.Response.Headers.Add(header, this.env);
        }
    }
}
