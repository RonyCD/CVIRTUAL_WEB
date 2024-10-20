using CVirtual.Domain.Entities.Categoria;
using CVirtual.Domain.Entities.Subcategoria;
using CVirtual.Dto.Base;
using CVirtual.Dto.Categoria;
using CVirtual.Dto.Modulo;
using CVirtual.Dto.Subcategoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.IServices
{
    public interface ISubcategoriaService
    {
        Task<BaseResponse<SubcategoriaResponse>> CrearSubcategoria(SubcategoriaRequest _Request);    
        Task<BaseResponse<ICollection<SubcategoriaResponse>>> ObtenerByIdCategoria(int _IdCartaVirtual);
        Task<BaseResponse<bool>> EditarSubcategoria(SubcategoriaEditarRequest _Request);
        Task<BaseResponse<ICollection<SubcategoriaResponse>>> EliminarById(int _IdSubcategoria);
        BaseResponse<ICollection<SubcategoriaResponse>> ListarSubcategorias( int _IdCategoria);




    }
}
