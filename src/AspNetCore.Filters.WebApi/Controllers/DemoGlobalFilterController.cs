using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.Filters.WebApi.Models;
using AspNetCore.Filters.WebApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;

namespace AspNetCore.Filters.WebApi.Controllers
{
    [FeatureGate(FeatureFlags.DemoGlobalFilter)]
    [Route("api/[controller]")]
    [ApiController]
    public class DemoGlobalController : ControllerBase
    {
        private readonly ILogger<DemoGlobalController> logger = null;

        public DemoGlobalController(ILogger<DemoGlobalController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("MyAction1")]
        public async Task<IActionResult> MyAction1() => Ok();

        [HttpGet("MyAction2")]
        public async Task<IActionResult> MyAction2() => Ok();

        [HttpGet("MyAction3")]
        public async Task<IActionResult> MyAction3() => Ok();
    }
}
