using CVirtual.Application.IServices.Categoria;
using CVirtual.Application.Services.Categoria;
using CVirtual.Domain.Entities.Categoria;
using CVirtual.Dto.Categoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVirtual.Api.Controllers.V1.Categoria
{
    [Route("api/categoria")]
    [ApiController]
    public class CategoriaController : BaseCVirtualController
    {
        private readonly ICategoriaService _ICategoriaService;

        public CategoriaController(ICategoriaService iCategoriaService)
        {
            _ICategoriaService = iCategoriaService;
        }

        [HttpPost]
        [Route("crear")]
        [Produces("application/json")]
        public async Task<IActionResult> CrearCategoria([FromBody] CategoriaRequest _Request)
        {
            var _Result = await _ICategoriaService.CrearCategoria(_Request);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(new { _Result.Message, _Result.Data });
        }


        [HttpGet]
        [Route("obtener-idcvirtual")]
        [Produces("application/json")]
        public async Task<IActionResult> ObtenerPorIdCVirtual( int _IdCartaVirtual)
        {
            var _Result = await _ICategoriaService.ObtenerPorIdCVirtual(_IdCartaVirtual);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(new { _Result.Message, _Result.Data });
        }


        [HttpPost]
        [Route("editar")]
        [Produces("application/json")]
        public async Task<IActionResult> EditarCategoria([FromBody] CategoriaEditarRequest _Request)
        {
            var _Result = await _ICategoriaService.EditarCategoria(_Request);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(new { _Result.Message, _Result.Data });
        }


        



    }
}
