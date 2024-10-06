using CVirtual.Dto.CuentaUsuario;
using CVirtual.Dto.JsonWebToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.IServices.JsonWebToken
{
    public interface IJsonWebTokenService
    {
        Task<JwtTokenResponse> CrearToken(JwtInformacionResponse info);
    }
}
