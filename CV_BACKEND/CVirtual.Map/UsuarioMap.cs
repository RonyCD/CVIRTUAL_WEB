using AutoMapper;
using CVirtual.Domain.Entities.Comun;
using CVirtual.Domain.Entities.CuentaUsuario;
using CVirtual.Dto.CuentaUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Map
{
    public class UsuarioMap : Profile
    {
        public UsuarioMap()
        {
            CreateMap<CVirtualResult, IniciarSesionResponse>();
        }
    }
}
