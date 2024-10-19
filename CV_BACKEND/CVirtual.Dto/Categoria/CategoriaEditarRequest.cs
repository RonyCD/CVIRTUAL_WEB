using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Dto.Categoria
{
    public class CategoriaEditarRequest
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public string DescCategoria { get; set; }
    }
}
