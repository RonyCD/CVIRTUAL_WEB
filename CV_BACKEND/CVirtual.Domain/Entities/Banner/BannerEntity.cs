using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Domain.Entities.Banner
{
    public class BannerEntity
    {
        public int IdBanner { get; set; }
        public int IdCartaVirtual { get; set; }
        public DateTime FechaSubida { get; set; }
        public byte[] ImagenBanner { get; set; }
    }
}

