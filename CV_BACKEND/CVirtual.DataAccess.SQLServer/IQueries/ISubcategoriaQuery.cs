using CVirtual.Domain.Entities.Categoria;
using CVirtual.Domain.Entities.Subcategoria;
using CVirtual.Dto.Subcategoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.IQueries
{
    public interface ISubcategoriaQuery
    {
        Task<SubcategoriaEntity> CrearSubcategoria(SubcategoriaRequest _Request);
        Task<ICollection<SubcategoriaEntity>> ObtenerByIdCategoria(int _IdCategoria);


    }
}
