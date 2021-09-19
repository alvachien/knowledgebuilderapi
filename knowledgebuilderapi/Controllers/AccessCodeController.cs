using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace knowledgebuilderapi.Controllers
{
    public sealed class AccessCode
    {
        public string InputtedCode { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AccessCodeController : ControllerBase
    {
        private IConfiguration _configuration;
        public AccessCodeController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        [HttpPost]
        public IActionResult Post([FromQuery]String accessCode )
        {
            var userName = _configuration[$"UserMapping:{accessCode}"];
            if (!String.IsNullOrEmpty(userName))
            {
                return Ok(userName);
            }
            return BadRequest();
        }
    }
}
