using CVirtual.Dto.Base;
using CVirtual.Dto.Categoria;
using CVirtual.Dto.Modulo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.IServices.Modulo
{
    public interface IModuloService
    {    
        BaseResponse<ICollection<ModuloResponse>> ListarModulos();

    }
}
