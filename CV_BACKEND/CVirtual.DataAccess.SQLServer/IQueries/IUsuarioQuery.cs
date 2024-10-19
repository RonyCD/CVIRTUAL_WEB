using CVirtual.Domain.Entities.Comun;
using CVirtual.Dto.Base;
using CVirtual.Dto.CuentaUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.IQueries
{
    public interface IUsuarioQuery
    {

        Task<IniciarSesionResponse> ValidarUsuario(IniciarSesionRequest _Request);

    }
}
