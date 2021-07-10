using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
        private static readonly string[] AccessCodes = new[]
        {
            "Angel", "Dora", "Esther", "Alva", 
        };

        [HttpPost]
        public IActionResult Post([FromQuery]String accessCode )
        {
            if (AccessCodes.Contains(accessCode))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
