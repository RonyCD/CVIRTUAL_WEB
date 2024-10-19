using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Domain.Entities.Categoria
{
    public class CategoriaEntity
    {
        public int IdCategoria { get; set; }
        public int IdCartaVirtual { get; set; }
        public string NombreCategoria { get; set; }
        public string DescCategoria { get; set; }
    }
}
