using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.Filters.WebApi.Models;
using AspNetCore.Filters.WebApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Filters.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly ILogger<DemoController> _logger = null;

        public DemoController(ILogger<DemoController> logger)
        {
            this._logger = logger;
        }

        [HttpGet("MyAction1")]
        //[TypeFilter(typeof(LogFilter), Arguments = new object[] { EnumAction.Action1 })]
        [LogParamFilter(Action = EnumAction.Action1)]
        public async Task<IActionResult> MyAction1()
        {
            return Ok();
        }

        [HttpGet("MyAction2")]
        [TypeFilter(typeof(LogFilter), Arguments = new object[] { EnumAction.Action2})]
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
    }
}
