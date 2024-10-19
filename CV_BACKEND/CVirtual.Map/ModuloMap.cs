using AutoMapper;
using CVirtual.Domain.Entities.Modulo;
using CVirtual.Dto.Modulo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Map
{
    public class ModuloMap : Profile
    {
        public ModuloMap()
        {
            CreateMap<ModuloEntity, ModuloResponse>()
                    .ForMember(des => des.IdModulo, opt => opt.MapFrom(src => src.Id))
                    .ForMember(des => des.NombreModulo, opt => opt.MapFrom(src => src.Nombre))
                    .ForMember(des => des.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                    .ForMember(des => des.Estado, opt => opt.MapFrom(src => src.Estado))                    
                 ;
        }
    }
}
