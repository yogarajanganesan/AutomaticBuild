using Microsoft.AspNetCore.Mvc;

namespace AutomaticBuild.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Product list");
        }
    }
}
