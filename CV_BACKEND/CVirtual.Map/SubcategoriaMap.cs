using AutoMapper;
using CVirtual.Domain.Entities.Categoria;
using CVirtual.Domain.Entities.Subcategoria;
using CVirtual.Dto.Categoria;
using CVirtual.Dto.Subcategoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Map
{
    public class SubcategoriaMap : Profile
    {
        public SubcategoriaMap() {

            CreateMap<SubcategoriaEntity, SubcategoriaResponse>()
                    .ForMember(des => des.IdSubcategoria, opt => opt.MapFrom(src => src.IdSubcategoria))
                    .ForMember(des => des.IdCategoria, opt => opt.MapFrom(src => src.IdCategoria))
                    .ForMember(des => des.NombreSubcategoria, opt => opt.MapFrom(src => src.NombreSubcategoria))
                    .ForMember(des => des.DescripcionSubcategoria, opt => opt.MapFrom(src => src.DescripcionSubcategoria))
                    .ForMember(des => des.Precio, opt => opt.MapFrom(src => src.Precio))
                    .ForMember(des => des.Imagen, opt => opt.MapFrom(src => src.Imagen))
                 ;

            CreateMap<SubcategoriaEditarRequest, SubcategoriaEditarEntity>()
                    .ForMember(des => des.IdSubcategoria, opt => opt.MapFrom(src => src.IdSubcategoria))
                    .ForMember(des => des.NombreSubcategoria, opt => opt.MapFrom(src => src.NombreSubcategoria))
                    .ForMember(des => des.DescripcionSubcategoria, opt => opt.MapFrom(src => src.DescripcionSubcategoria))
                    .ForMember(des => des.Precio, opt => opt.MapFrom(src => src.Precio))
                    .ForMember(des => des.Imagen, opt => opt.MapFrom(src => src.Imagen))
                 ;
        }
        
    }
}
