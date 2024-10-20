using CVirtual.Domain.Entities.Banner;
using CVirtual.Dto.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.IQueries
{
    public interface IBannerQuery
    {
        Task<BannerEntity> AgregarBanner(BannerRequest _Request);
        Task<ICollection<BannerEntity>> ObtenerPorIdCVirtual(int _IdCartaVirtual);
    }
}
