﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Dto.JsonWebToken
{
        public class JwtInformacionResponse
        {
            public int Id { get; set; }
            public int IdRol { get; set; }
            public string Nombres { get; set; }
            public string ApPaterno { get; set; }
            public string ApMaterno { get; set; }
            public string Correo { get; set; }
            public string Username { get; set; }
        }
}
