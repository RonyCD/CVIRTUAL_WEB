using AutoMapper;
using CVirtual.Application.IServices;
using CVirtual.Application.IServices.JsonWebToken;
using CVirtual.DataAccess.SQLServer.IQueries;
using CVirtual.Domain.Entities.Comun;
using CVirtual.Domain.Entities.CuentaUsuario;
using CVirtual.Dto.Base;
using CVirtual.Dto.CuentaUsuario;
using CVirtual.Dto.JsonWebToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioQuery _ICuentaUsuarioQuery;
        private readonly IJsonWebTokenService _JsonWebTokenService;
        private readonly IMapper _IMapper;

        public UsuarioService(IUsuarioQuery iCuentaUsuarioQuery, IJsonWebTokenService iJsonWebTokenService, IMapper iMapper)
        {
            _ICuentaUsuarioQuery = iCuentaUsuarioQuery;
            _JsonWebTokenService = iJsonWebTokenService;
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

                JwtInformacionResponse _JwtInformacionResponse = new JwtInformacionResponse();
                _JwtInformacionResponse.Id = usuario.Id;

                // Genera y asigna TOKEN
                usuario.JwtToken = await _JsonWebTokenService.CrearToken(_JwtInformacionResponse);

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