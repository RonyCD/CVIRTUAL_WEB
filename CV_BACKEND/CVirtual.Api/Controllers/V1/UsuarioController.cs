using CVirtual.Application.IServices;
using CVirtual.Application.Services;
using CVirtual.Dto.CuentaUsuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVirtual.Api.Controllers.V1
{
    [Route("api/v1/usuario")]
    [ApiController]
    public class UsuarioController : BaseCVirtualController
    {
        private readonly IUsuarioService _ICuentaUsuarioService;

        public UsuarioController(IUsuarioService iUsuarioService)
        {
            _ICuentaUsuarioService = iUsuarioService;
        }

        [HttpPost]
        [Route("iniciar-sesion")]
        [Produces("application/json")]
        public async Task<IActionResult> IniciarSesion([FromBody] IniciarSesionRequest _Request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos");

            var _Result = await _ICuentaUsuarioService.IniciarSesion(_Request);

            if (!_Result.Success)
                return Unauthorized(new { _Result.Message });

            return Ok(_Result);
        }
    }
}
