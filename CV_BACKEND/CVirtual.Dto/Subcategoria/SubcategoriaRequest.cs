﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Dto.Subcategoria
{
    public class SubcategoriaRequest
    {
        public int IdCategoria { get; set; }
        public string NombreSubcategoria { get; set; }
        public string DescripcionSubcategoria { get; set; }
        public decimal Precio { get; set; }
        public byte[] Imagen { get; set; }
    }
}
