using CVirtual.Application.IServices.Admin;
using CVirtual.Application.IServices.CuentaUsuario;
using CVirtual.Dto.Admin;
using CVirtual.Dto.CuentaUsuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVirtual.Api.Controllers.V1.Admin
{
    [Route("api/v1/admin")]
    [ApiController]
    public class AdminController : BaseCVirtualController
    {
        private readonly IAdminService _IAdminService;

        public AdminController(IAdminService iAdminService)
        {
            _IAdminService = iAdminService;
        }


        [HttpPost]
        [Route("crear-usuario")]
        [Produces("application/json")]
        public async Task<IActionResult> CrearUsuario([FromBody] RegistrarUsuarioRequest _Request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos");

            var _Result = await _IAdminService.CrearUsuarioCliente(_Request);

            if (!_Result.Success)
                return Unauthorized(new { _Result.Message });

            return Ok(new { _Result.Message, _Result.Data });
        }

        
    }
}
