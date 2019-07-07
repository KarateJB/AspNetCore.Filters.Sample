using System;
using System.Diagnostics;
using System.Linq;
using AspNetCore.Filters.WebApi.Controllers;
using AspNetCore.Filters.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Filters.WebApi.Utils
{
    public class LogParamFilter : Attribute, IActionFilter
    {
        //private readonly ILogger<LogFilter> _logger = null;

        public EnumAction Action { get; set; }

        public LogParamFilter()
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            string msg = $"[OnActionExecuted] Request for {this.Action.ToString()}";

            // 1. Use injected ILogger<LogParamFilter>
            //var logger = (ILogger<LogParamFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<LogParamFilter>));
            //logger.LogInformation(msg);

            // 2. Get injected logger from context
            Type generic = typeof(ILogger<>);
            Type constructed = generic.MakeGenericType(context.Controller.GetType());
            var serviceObj = context.HttpContext.RequestServices.GetServices(constructed).FirstOrDefault();
            if (serviceObj.GetType().Equals(typeof(Logger<DemoController>)))
            {
                var logger = (ILogger<DemoController>)serviceObj;
                logger.LogInformation(msg);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string msg = $"[OnActionExecuting] Request for {this.Action.ToString()}";
            var logger = (ILogger<LogParamFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<LogParamFilter>));
            logger.LogInformation(msg);
        }
    }
}
