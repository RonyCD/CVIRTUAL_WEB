using CVirtual.Domain.Entities.Categoria;
using CVirtual.Dto.Base;
using CVirtual.Dto.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.IQueries.Categoria
{
    public interface ICategoriaQuery
    {
        Task<CategoriaEntity> CrearCategoria(CategoriaRequest _Request);
    }
}
