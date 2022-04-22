using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DauStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {

        [HttpPost]
        public IActionResult addItem([FromQuery] string a)
        {
            return Ok($"Hello {a}");
        }
    }
}
