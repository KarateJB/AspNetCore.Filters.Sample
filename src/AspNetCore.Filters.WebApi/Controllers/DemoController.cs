using System.Net;
using System.Threading.Tasks;
using AspNetCore.Filters.WebApi.Models;
using AspNetCore.Filters.WebApi.Utils;
using CyberSoft.ServiceSwitching.Utils.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace AspNetCore.Filters.WebApi.Controllers
{
    [FeatureGate(FeatureFlags.Demo)]
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly ILogger<DemoController> logger = null;
        private readonly IFeatureManager featureManager = null;

        public DemoController(
            ILogger<DemoController> logger,
            IFeatureManager featureManager)
        {
            this.logger = logger;
            this.featureManager = featureManager;
        }

        [HttpGet("MyAction1")]
        //[TypeFilter(typeof(LogFilter), Arguments = new object[] { EnumAction.Action1 })]
        [LogParamFilter(Action = EnumAction.Action1)]
        public async Task<IActionResult> MyAction1()
        {
            return Ok();
        }

        [HttpGet("MyAction2")]
        [TypeFilter(typeof(LogFilter), Arguments = new object[] { EnumAction.Action2 })]
        public async Task<IActionResult> MyAction2()
        {
            return Ok();
        }

        [HttpGet("MyAction3")]
        [TypeFilter(typeof(LogFilter), Arguments = new object[] { EnumAction.Action3 })]
        public async Task<IActionResult> MyAction3()
        {
            return Ok();
        }

        [HttpPost("SignIn")]
        [ServiceFilter(typeof(HybridFilter))]
        public async Task<IActionResult> SignIn([FromBody] User user)
        {
            return Ok();
        }

        [HttpGet("TestDisableApiFilter")]
        [ApiExplorerSettings(IgnoreApi = true)] // Optional, if you dont want expose this API to something like Swagger.
        [TypeFilter(typeof(DisableApiFilter))]
        //[TypeFilter(typeof(DisableApiFilter), Arguments = new object[] { "^(.*)[Pp]roduction(.*)$" })]
        public async Task<IActionResult> TestDisableApiFilter()
        {
            if (await featureManager.IsEnabledAsync(nameof(FeatureFlags.Tests)))
            {
                return this.Ok();
            }
            else 
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}
