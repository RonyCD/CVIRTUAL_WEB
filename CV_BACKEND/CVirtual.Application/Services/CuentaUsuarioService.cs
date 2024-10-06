using AutoMapper;
using CVirtual.Application.IServices;
using CVirtual.DataAccess.SQLServer.IQueries;
using CVirtual.Domain.Entities.Comun;
using CVirtual.Domain.Entities.CuentaUsuario;
using CVirtual.Dto.Base;
using CVirtual.Dto.CuentaUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.Services
{
    public class CuentaUsuarioService : ICuentaUsuarioService
    {
        private readonly ICuentaUsuarioQuery _ICuentaUsuarioQuery;
        private readonly IMapper _IMapper;

        public CuentaUsuarioService(ICuentaUsuarioQuery iCuentaUsuarioQuery, IMapper iMapper)
        {
            _ICuentaUsuarioQuery = iCuentaUsuarioQuery;
            _IMapper = iMapper;
        }

        
        public async Task<BaseResponse<IniciarSesionResponse>> IniciarSesion(IniciarSesionRequest _Request)
        {
            try
            {
                var usuario = await _ICuentaUsuarioQuery.ValidarUsuario(_Request);

                if (usuario == null)
                {
                    return new BaseResponse<IniciarSesionResponse>
                    {
                        Success = false,
                        Message = "Usuario no encontrado o credenciales incorrectas"
                    };
                }

                return new BaseResponse<IniciarSesionResponse>
                {
                    Success = true,
                    Message = "Login exitoso",
                    Data = usuario
                };
            }
            catch (UnauthorizedAccessException)
            {
                return new BaseResponse<IniciarSesionResponse>
                {
                    Success = false,
                    Message = "Credenciales incorrectas"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IniciarSesionResponse>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
