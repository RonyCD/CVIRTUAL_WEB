using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.Configurations
{
    public class TokenConfigurations
    {
        public string SecretKey { get; set; }        
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int NotBeforeMinutes { get; set; }
        public int AccessTokenExpiration { get; set; }
    }
}
