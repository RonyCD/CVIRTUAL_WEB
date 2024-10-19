using CVirtual.Dto.Base;
using CVirtual.Dto.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.IServices.Categoria
{
    public interface ICategoriaService
    {       
        Task<BaseResponse<CategoriaResponse>> CrearCategoria(CategoriaRequest _Request);

        Task<BaseResponse<ICollection<CategoriaResponse>>> ObtenerPorIdCVirtual(int _IdCartaVirtual);
        
        Task<BaseResponse<bool>> EditarCategoria(CategoriaEditarRequest _Request);

    }
}
