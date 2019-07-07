using System;
using AspNetCore.Filters.WebApi.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Filters.WebApi.Utils
{
    public class LogFilter: Attribute, IActionFilter
    {
        private readonly ILogger<LogFilter> _logger = null;
        private readonly EnumAction _action;

        public LogFilter(EnumAction action, ILogger<LogFilter> logger)
        {
            this._logger = logger;
            this._action = action;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            string msg = $"[OnActionExecuted] Request for {this._action.ToString()}";
            this._logger.LogInformation(msg);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string msg = $"[OnActionExecuting] Request for {this._action.ToString()}";
            this._logger.LogInformation(msg);
        }
    }
}
