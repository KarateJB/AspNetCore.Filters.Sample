using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Filters.WebApi.Utils
{
    public class LogFilter: Attribute, IActionFilter
    {
        private readonly ILogger<LogFilter> _logger = null;

        public LogFilter(ILogger<LogFilter> logger)
        {
            this._logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
