using Azure.Core;
using CVirtual.DataAccess.SQLServer.Commands;
using CVirtual.DataAccess.SQLServer.IQueries.Categoria;
using CVirtual.Domain.Contract;
using CVirtual.Domain.Entities.Categoria;
using CVirtual.Dto.Admin;
using CVirtual.Dto.Base;
using CVirtual.Dto.Categoria;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.Queries.Categoria
{
    public class CategoriaQuery : BaseUnitOfWork, ICategoriaQuery
    {

        public CategoriaQuery(ISeguridadDbContext context) : base(context, true)
        {

        }

        public async Task<CategoriaEntity> CrearCategoria(CategoriaRequest request)
        {
            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_CATEGORIA_CREAR", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@IdCartaVirtual", request.IdCartaVirtual);
                    command.Parameters.AddWithValue("@NombreCategoria", request.NombreCategoria);
                    command.Parameters.AddWithValue("@DescCategoria", request.DescCategoria);
             
                    await command.ExecuteNonQueryAsync();
                }
            }
            
            var nuevaCategoria = new CategoriaEntity
            {   
                IdCartaVirtual = request.IdCartaVirtual,
                NombreCategoria = request.NombreCategoria,
                DescCategoria = request.DescCategoria
            };

            return nuevaCategoria;
        }

    }
}
