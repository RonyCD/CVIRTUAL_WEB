using CVirtual.Application.IServices;
using CVirtual.Application.Services;
using CVirtual.Dto.Categoria;
using CVirtual.Dto.Subcategoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVirtual.Api.Controllers.V1
{
    [Route("api/v1/subcategoria")]
    [ApiController]
    public class SubcategoriaController : BaseCVirtualController
    {
        private readonly ISubcategoriaService _ISubcategoriaService;

        public SubcategoriaController(ISubcategoriaService iSubcategoriaService)
        {
            _ISubcategoriaService = iSubcategoriaService;
        }

        [HttpPost]
        [Route("crear")]
        [Produces("application/json")]
        public async Task<IActionResult> CrearSubcategoria([FromBody] SubcategoriaRequest _Request)
        {
            var _Result = await _ISubcategoriaService.CrearSubcategoria(_Request);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(_Result);
        }


        [HttpGet]
        [Route("obtener-idcategoria")]
        [Produces("application/json")]
        public async Task<IActionResult> ObtenerByIdCategoria(int _IdCategoria)
        {
            var _Result = await _ISubcategoriaService.ObtenerByIdCategoria(_IdCategoria);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(_Result);
        }


        [HttpPut]
        [Route("editar")]
        [Produces("application/json")]
        public async Task<IActionResult> EditarSubcategoria([FromBody] SubcategoriaEditarRequest _Request)
        {
            var _Result = await _ISubcategoriaService.EditarSubcategoria(_Request);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(_Result);
        }


        [HttpDelete]
        [Route("eliminar")]
        [Produces("application/json")]
        public async Task<IActionResult> EliminarById(int _IdSubcategoria)
        {
            var _Result = await _ISubcategoriaService.EliminarById(_IdSubcategoria);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(_Result);
        }

        [HttpGet]
        [Route("listar")]
        [Produces("application/json")]
        public IActionResult ListarSubcategorias(int _IdCategoria)
        {

            var _Result = _ISubcategoriaService.ListarSubcategorias(_IdCategoria);

            return Ok(_Result);
        }
    }
}
