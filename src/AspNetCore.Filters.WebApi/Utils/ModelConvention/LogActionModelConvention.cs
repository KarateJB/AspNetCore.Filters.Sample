using AspNetCore.Filters.WebApi.Controllers;
using AspNetCore.Filters.WebApi.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AspNetCore.Filters.WebApi.Utils.ModelConvention
{
    public class LogActionModelConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            if (action.Controller.ControllerType.Equals(typeof(DemoGlobalController)) &&
                action.ActionName.Equals("MyAction1"))
            {
                // Add Filter
                action.Filters.Add(new LogParamFilter { Action = EnumAction.DontCare });

                // Add Attribute
                //if (action.Attributes is List<object> attributes)
                //{
                //    attributes.Add(new MyCustomAttribute());
                //}
            }
        }
    }
}
