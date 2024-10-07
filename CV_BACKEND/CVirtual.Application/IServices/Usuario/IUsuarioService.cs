using CVirtual.Dto.Base;
using CVirtual.Dto.CuentaUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.IServices.CuentaUsuario
{
    public interface IUsuarioService
    {
        Task<BaseResponse<IniciarSesionResponse>> IniciarSesion(IniciarSesionRequest _Request);

    }
}
