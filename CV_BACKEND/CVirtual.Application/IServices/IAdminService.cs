using CVirtual.Dto.Admin;
using CVirtual.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.IServices
{
    public interface IAdminService
    {
        Task<BaseResponse<RegistrarUsuarioResponse>> CrearUsuarioCliente(RegistrarUsuarioRequest _RequestUsuarioCliente);
    }
}
