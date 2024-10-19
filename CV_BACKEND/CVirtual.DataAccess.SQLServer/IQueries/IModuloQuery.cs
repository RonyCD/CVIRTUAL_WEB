using CVirtual.Domain.Entities.Modulo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.IQueries
{
    public interface IModuloQuery
    {
        ICollection<ModuloEntity> ListarModulos();
    }
}
