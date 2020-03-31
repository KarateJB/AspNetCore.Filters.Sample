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
    public class DemoGlobalController : ControllerBase
    {
        private readonly ILogger<DemoGlobalController> _logger = null;

        public DemoGlobalController(ILogger<DemoGlobalController> logger)
        {
            this._logger = logger;
        }

        [HttpGet("MyAction1")]
        public async Task<IActionResult> MyAction1()
        {
            return Ok();
        }

        [HttpGet("MyAction2")]
        public async Task<IActionResult> MyAction2()
        {
            return Ok();
        }

        [HttpGet("MyAction3")]
        public async Task<IActionResult> MyAction3()
        {
            return Ok();
        }
    }
}
