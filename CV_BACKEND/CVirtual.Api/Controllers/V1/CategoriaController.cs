using CVirtual.Application.IServices;
using CVirtual.Domain.Entities.Categoria;
using CVirtual.Dto.Categoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVirtual.Api.Controllers.V1
{
    [Route("api/v1/categoria")]
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

            return Ok(_Result);
        }


        [HttpGet]
        [Route("obtener-idcvirtual")]
        [Produces("application/json")]
        public async Task<IActionResult> ObtenerPorIdCVirtual(int _IdCartaVirtual)
        {
            var _Result = await _ICategoriaService.ObtenerPorIdCVirtual(_IdCartaVirtual);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(_Result);
        }


        [HttpPut]
        [Route("editar")]
        [Produces("application/json")]
        public async Task<IActionResult> EditarCategoria([FromBody] CategoriaEditarRequest _Request)
        {
            var _Result = await _ICategoriaService.EditarCategoria(_Request);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(_Result);
        }


        [HttpDelete]
        [Route("eliminar")]
        [Produces("application/json")]
        public async Task<IActionResult> EliminarById(int _IdCategoria)
        {
            var _Result = await _ICategoriaService.EliminarById(_IdCategoria);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(_Result);
        }






    }
}
