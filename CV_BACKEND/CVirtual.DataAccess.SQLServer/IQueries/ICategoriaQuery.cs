﻿using CVirtual.Domain.Entities.Categoria;
using CVirtual.Dto.Base;
using CVirtual.Dto.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.IQueries
{
    public interface ICategoriaQuery
    {
        Task<CategoriaEntity> CrearCategoria(CategoriaRequest _Request);

        Task<ICollection<CategoriaEntity>> ObtenerPorIdCVirtual(int _IdCartaVirtual);

        Task<bool> EditarCategoria(CategoriaEditarEntity _Request);
       
        Task<bool> EliminarById(int idCategoria);




    }
}
