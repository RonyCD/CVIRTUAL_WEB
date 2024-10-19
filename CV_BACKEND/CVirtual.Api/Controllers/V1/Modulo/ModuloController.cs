using CVirtual.Application.IServices.CuentaUsuario;
using CVirtual.Application.IServices.Modulo;
using CVirtual.Dto.CuentaUsuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVirtual.Api.Controllers.V1.Modulo
{
    [Route("api/modulo")]
    [ApiController]
    public class ModuloController : BaseCVirtualController
    {
        private readonly IModuloService _IModuloService;

        public ModuloController(IModuloService iModuloService)
        {
            _IModuloService = iModuloService;
        }

        [HttpPost]
        [Route("listar")]
        [Produces("application/json")]
        public IActionResult ListarModulos()
        {          

            var _Result = _IModuloService.ListarModulos();

            if (!_Result.Success)
                return Unauthorized(new { _Result.Message });

            return Ok(new { _Result.Message, _Result.Data });
        }

    }
}
