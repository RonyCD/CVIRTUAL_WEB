using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Domain.Entities.Admin
{
    public class UsuarioAdminEntity
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string Nombres { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string Username { get; set; }
        public string Contrasenia { get; set; }
        public string Correo { get; set; }
    }
}
