using AutoMapper;
using CVirtual.Domain.Entities.Banner;
using CVirtual.Dto.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Map
{
    public class BannerMap : Profile
    {
        public BannerMap()
        {
            CreateMap<BannerEntity, BannerResponse>()
                    .ForMember(des => des.IdBanner, opt => opt.MapFrom(src => src.IdBanner))
                    .ForMember(des => des.IdCartaVirtual, opt => opt.MapFrom(src => src.IdCartaVirtual))
                    .ForMember(des => des.ImagenBanner, opt => opt.MapFrom(src => src.ImagenBanner))
                 ;

        }
    }
}
