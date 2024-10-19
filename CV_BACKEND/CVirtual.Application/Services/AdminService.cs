using AutoMapper;
using CVirtual.Application.IServices;
using CVirtual.DataAccess.SQLServer.IQueries;
using CVirtual.Domain.Entities.Admin;
using CVirtual.Dto.Admin;
using CVirtual.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminQuery _IAdminQuery;
        private readonly IMapper _IMapper;

        public AdminService(IAdminQuery iAdminQuery, IMapper iMapper)
        {
            _IAdminQuery = iAdminQuery;
            _IMapper = iMapper;
        }



        public async Task<BaseResponse<RegistrarUsuarioResponse>> CrearUsuarioCliente(RegistrarUsuarioRequest _RequestUsuarioCliente)
        {
            var _BaseResponse = new BaseResponse<RegistrarUsuarioResponse>
            {
                Code = "200",
                Success = true,
                Data = null,
                Validations = new List<MessageResponse>()
            };

            // Crear el usuario y obtener el ID generado
            var idUsuario = await _IAdminQuery.CrearUsuario(_RequestUsuarioCliente);

            if (!idUsuario.HasValue)
            {
                _BaseResponse.Success = false;
                _BaseResponse.Data = new RegistrarUsuarioResponse
                {
                    Exito = false,
                    Mensaje = "No se pudo crear el usuario."
                };
                return _BaseResponse;
            }

            // Crear el cliente usando el ID del usuario creado
            var clienteCreado = await _IAdminQuery.CrearCliente(idUsuario.Value, _RequestUsuarioCliente);

            _BaseResponse.Data = new RegistrarUsuarioResponse
            {
                Exito = clienteCreado,
                Mensaje = clienteCreado
                    ? "Usuario y cliente creados exitosamente."
                    : "Usuario creado, pero falló la creación del cliente."
            };

            if (!clienteCreado)
            {
                _BaseResponse.Success = false;
                _BaseResponse.Code = "500";
            }

            return _BaseResponse;
        }

    }
}
