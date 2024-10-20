using CVirtual.Dto.Banner;
using CVirtual.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.IServices
{
    public interface IBannerService
    {
        Task<BaseResponse<BannerResponse>> AgregarBanner(BannerRequest _Request);
        Task<BaseResponse<ICollection<BannerResponse>>> ObtenerPorIdCVirtual(int _IdCartaVirtual);

    }
}
