using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVirtual.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseCVirtualController : ControllerBase
    {
        protected BadRequestObjectResult badRequest(string errorMensaje)
        {
            return BadRequest(new { ErrorMessage = errorMensaje });
        }
    }
}
