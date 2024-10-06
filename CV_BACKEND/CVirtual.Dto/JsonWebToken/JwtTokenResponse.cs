using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Dto.JsonWebToken
{
    public class JwtTokenResponse
    {
        public string Token { get; set; }
        public DateTime? ExpiraEn { get; set; }
        public string Tipo { get; set; }
    }
}
