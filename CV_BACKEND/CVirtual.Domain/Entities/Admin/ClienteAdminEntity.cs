using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Domain.Entities.Admin
{
    public class ClienteAdminEntity
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public string RUC { get; set; }
        public string Telefono { get; set; }
        public string Celular1 { get; set; }
        public string Celular2 { get; set; }
        public string NombreComercial { get; set; }
        public string Direccion { get; set; }
        public string Logo { get; set; }
    }
}
