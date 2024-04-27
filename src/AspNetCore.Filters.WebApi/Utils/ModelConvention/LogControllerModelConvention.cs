using AspNetCore.Filters.WebApi.Controllers;
using AspNetCore.Filters.WebApi.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AspNetCore.Filters.WebApi.Utils.ModelConvention
{
    public class LogControllerModelConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.Equals(typeof(DemoGlobalController)))
            {
                // Add Filter
                controller.Filters.Add(new LogParamFilter { Action = EnumAction.DontCare });

                // Add Attribute
                //if (controller.Attributes is List<object> attributes)
                //{
                //    attributes.Add(new MyCustomAttribute());
                //}
            }
        }
    }
}
