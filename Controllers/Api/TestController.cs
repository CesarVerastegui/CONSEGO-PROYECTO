using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CONSEGO.Controllers.Api
{

    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("FUNCIONA");
        }
    }
}
