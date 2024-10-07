using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Dto.CuentaUsuario
{
    public class IniciarSesionRequest
    {       
        public string Username { get; set; }
        public string Contrasenia { get; set; }        
    }
}
