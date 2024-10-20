using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Dto.Banner
{
    public class BannerResponse
    {
        public int IdBanner { get; set; }
        public int IdCartaVirtual { get; set; }
        public DateTime FechaSubida { get; set; }
        public byte[] ImagenBanner { get; set; }
    }
}
