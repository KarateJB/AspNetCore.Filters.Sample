using System;
using System.Diagnostics;
using AspNetCore.Filters.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Filters.WebApi.Utils
{
    public class LogParamFilter: Attribute, IActionFilter
    {
        //private readonly ILogger<LogFilter> _logger = null;

        public EnumAction Action { get; set; }

        public LogParamFilter()
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            string msg = $"[OnActionExecuted] Request for {this.Action.ToString()}";
            ILogger<LogParamFilter> logger = (ILogger<LogParamFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<LogParamFilter>));
            logger.LogInformation(msg);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string msg = $"[OnActionExecuting] Request for {this.Action.ToString()}";
            ILogger<LogParamFilter> logger = (ILogger<LogParamFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<LogParamFilter>));
            logger.LogInformation(msg);
        }
    }
}
