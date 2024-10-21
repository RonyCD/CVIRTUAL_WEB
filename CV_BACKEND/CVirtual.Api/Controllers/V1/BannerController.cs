using CVirtual.Application.IServices;
using CVirtual.Application.Services;
using CVirtual.Dto.Banner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVirtual.Api.Controllers.V1
{
    [Route("api/v1/banner")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _IBannerService;

        public BannerController(IBannerService iBannerService)
        {
            _IBannerService = iBannerService;
        }

        [HttpPost]
        [Route("agregar")]
        [Produces("application/json")]
        public async Task<IActionResult> AgregarBanner([FromBody] BannerRequest _Request)
        {
            var _Result = await _IBannerService.AgregarBanner(_Request);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(_Result);
        }


        [HttpGet]
        [Route("obtener-idcvirtual")]
        [Produces("application/json")]
        public async Task<IActionResult> ObtenerPorIdCVirtual(int _IdCartaVirtual)
        {
            var _Result = await _IBannerService.ObtenerPorIdCVirtual(_IdCartaVirtual);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(_Result);
        }


        [HttpDelete]
        [Route("eliminar")]
        [Produces("application/json")]
        public async Task<IActionResult> EliminarBanner(int _IdBanner)
        {
            var _Result = await _IBannerService.EliminarBanner(_IdBanner);

            if (!_Result.Success)
                return BadRequest(new { _Result.Message });

            return Ok(_Result);
        }
    }
}
