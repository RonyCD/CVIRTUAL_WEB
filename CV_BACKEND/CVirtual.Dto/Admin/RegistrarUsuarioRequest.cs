using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Dto.Admin
{
    public class RegistrarUsuarioRequest
    {
        //Datos Usuario
        public int IdRol { get; set; }
        public string Nombres { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string Username { get; set; }
        public string Contrasenia { get; set; }
        public string Correo { get; set; }


        // Datos Cliente
        public string RUC { get; set; }
        public string Telefono { get; set; }
        public string Celular1 { get; set; }
        public string Celular2 { get; set; }
        public string NombreComercial { get; set; }
        public string Direccion { get; set; }
        public string Logo { get; set; }
    }
}
