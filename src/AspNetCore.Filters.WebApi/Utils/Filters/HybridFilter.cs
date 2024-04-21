using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.Filters.WebApi.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Filters.WebApi.Utils
{
    public class HybridFilter: IActionFilter, IResultFilter
    {
        private readonly ILogger<LogFilter> _logger = null;
        private ICollection<object> payloads = null;

        public HybridFilter(ILogger<LogFilter> logger)
        {
            this._logger = logger;
            this._logger.LogInformation("Constructor");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            this._logger.LogDebug("OnActionExecuting");
            this.payloads = context.ActionArguments == null ? null : context.ActionArguments.Values;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            this._logger.LogDebug("OnActionExecuted");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            this._logger.LogDebug("OnResultExecuting");
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            this._logger.LogDebug("OnResultExecuted");
            var user = (User)this.payloads.FirstOrDefault();
            int httpStatusCode = context.HttpContext.Response.StatusCode;

            string msg = $"({httpStatusCode.ToString()}) {user.Name} signed in";
            this._logger.LogInformation(msg);
        }

    }
}
