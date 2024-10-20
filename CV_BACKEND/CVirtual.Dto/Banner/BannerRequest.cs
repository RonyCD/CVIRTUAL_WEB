using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Dto.Banner
{
    public class BannerRequest
    {       
        public int IdCartaVirtual { get; set; }
        public DateTime FechaSubida { get; set; }
        public byte[] ImagenBanner { get; set; }
    }
}

