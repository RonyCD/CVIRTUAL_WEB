using CVirtual.Application.IServices;
using CVirtual.Dto.Admin;
using CVirtual.Dto.CuentaUsuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVirtual.Api.Controllers.V1
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

            return Ok(_Result);
        }


    }
}
