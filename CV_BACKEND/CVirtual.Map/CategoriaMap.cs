using AutoMapper;
using CVirtual.Domain.Entities.Categoria;
using CVirtual.Domain.Entities.Modulo;
using CVirtual.Dto.Categoria;
using CVirtual.Dto.Modulo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Map
{
    public class CategoriaMap : Profile
    {
        public CategoriaMap()
        {
            CreateMap<CategoriaEntity, CategoriaResponse>()
                    //.ForMember(des => des.IdCategoria, opt => opt.MapFrom(src => src.IdCategoria))
                    .ForMember(des => des.IdCartaVirtual, opt => opt.MapFrom(src => src.IdCartaVirtual))
                    .ForMember(des => des.NombreCategoria, opt => opt.MapFrom(src => src.NombreCategoria))
                    .ForMember(des => des.DescCategoria, opt => opt.MapFrom(src => src.DescCategoria))
                 ;
        }
    }
}
