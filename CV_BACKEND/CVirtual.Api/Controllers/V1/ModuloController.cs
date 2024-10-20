﻿using CVirtual.Application.IServices;
using CVirtual.Dto.CuentaUsuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVirtual.Api.Controllers.V1
{
    [Route("api/v1/modulo")]
    [ApiController]
    public class ModuloController : BaseCVirtualController
    {
        private readonly IModuloService _IModuloService;

        public ModuloController(IModuloService iModuloService)
        {
            _IModuloService = iModuloService;
        }

        [HttpGet]
        [Route("listar")]
        [Produces("application/json")]
        public IActionResult ListarModulos()
        {

            var _Result = _IModuloService.ListarModulos();

            return Ok(_Result);
        }

    }
}
