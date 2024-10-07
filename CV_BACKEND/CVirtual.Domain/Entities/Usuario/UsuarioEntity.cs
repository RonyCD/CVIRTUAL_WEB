using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Domain.Entities.CuentaUsuario
{
    public class UsuarioEntity
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public string Nombres { get; set; }
        public string Ap_Paterno { get; set; }
        public string Ap_Materno { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha_Registro { get; set; }

    }
}
