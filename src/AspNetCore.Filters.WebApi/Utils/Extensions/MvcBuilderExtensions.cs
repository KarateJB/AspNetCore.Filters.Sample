using System;
using AspNetCore.Filters.WebApi.Models;
using AspNetCore.Filters.WebApi.Utils.ModelConvention;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Filters.WebApi.Utils.Extensions
{
    /// <summary>
    /// MVCBuilder extensions
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Add MVC options
        /// </summary>
        /// <param name="builder">IMVCBuilder</param>
        /// <returns>IMvcBuilder</returns>
        public static IMvcBuilder AddMvcOptions(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<MvcOptions>(options =>
            {
                // If no parameter
                // options.Filters.Add(typeof(LogParamFilter));

                // Give parameter
                // options.Filters.Add(new LogParamFilter { Action = EnumAction.DontCare });

                // Add IControllerModelConvention instance when discovering actions
                // options.Conventions.Add(new LogControllerModelConvention());

                // Add IActionModelConvention instance when discovering actions
                options.Conventions.Add(new LogActionModelConvention());
            });

            return builder;
        }
    }
}
