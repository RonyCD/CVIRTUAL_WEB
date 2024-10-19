using CVirtual.Domain.Entities.Admin;
using CVirtual.Dto.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.IQueries
{
    public interface IAdminQuery
    {
        Task<int?> CrearUsuario(RegistrarUsuarioRequest request);
        Task<bool> CrearCliente(int idUsuario, RegistrarUsuarioRequest request);

    }
}
