using Azure.Core;
using CVirtual.DataAccess.SQLServer.Commands;
using CVirtual.DataAccess.SQLServer.IQueries;
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
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.Queries
{
    public class CategoriaQuery : BaseUnitOfWork, ICategoriaQuery
    {

        public CategoriaQuery(ISeguridadDbContext context) : base(context, true)
        {

        }

        

        public async Task<CategoriaEntity> CrearCategoria(CategoriaRequest _Request)
        {
            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_CATEGORIA_CREAR", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdCartaVirtual", _Request.IdCartaVirtual);
                    command.Parameters.AddWithValue("@NombreCategoria", _Request.NombreCategoria);
                    command.Parameters.AddWithValue("@DescCategoria", _Request.DescCategoria);

                    //ID_CATEGORIA
                    var newIdCategoria = (int)await command.ExecuteScalarAsync();

                    var nuevaCategoria = new CategoriaEntity
                    {
                        IdCategoria = newIdCategoria,
                        IdCartaVirtual = _Request.IdCartaVirtual,
                        NombreCategoria = _Request.NombreCategoria,
                        DescCategoria = _Request.DescCategoria
                    };

                    return nuevaCategoria;
                }
            }
        }



        public async Task<bool> EditarCategoria(CategoriaEditarEntity _Request)
        {
            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_CATEGORIA_EDITAR", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdCategoria", _Request.IdCategoria);
                    command.Parameters.AddWithValue("@NombreCategoria", _Request.NombreCategoria);
                    command.Parameters.AddWithValue("@DescCategoria", _Request.DescCategoria);

                    //var rowsAffected = await command.ExecuteNonQueryAsync();
                    var rowsAffected = (int)await command.ExecuteScalarAsync();

                    return rowsAffected > 0;
                }
            }
        }




        public async Task<ICollection<CategoriaEntity>> ObtenerPorIdCVirtual(int _IdCartaVirtual)
        {
            var categorias = new List<CategoriaEntity>();

            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_CATEGORIA_OBTENER", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdCartaVirtual", _IdCartaVirtual);

                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            var categoria = new CategoriaEntity
                            {

                                IdCategoria = reader.GetInt32(reader.GetOrdinal("ID_CATEGORIA")),
                                IdCartaVirtual = reader.GetInt32(reader.GetOrdinal("ID_CARTA_VIRTUAL")),
                                NombreCategoria = reader.GetString(reader.GetOrdinal("NOMBRE_CATEGORIA")),
                                DescCategoria = reader.GetString(reader.GetOrdinal("DESC_CATEGORIA"))
                            };

                            categorias.Add(categoria);
                        }
                    }
                }
            }

            return categorias;
        }


        public async Task<bool> EliminarById(int _IdCategoria)
        {
            bool isDeleted = false;

            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_CATEGORIA_ELIMINAR", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdCategoria", _IdCategoria);

                    var result = await command.ExecuteScalarAsync();

                    if (result != null && Convert.ToInt32(result) > 0)
                    {
                        isDeleted = true;
                    }
                }
            }

            return isDeleted;
        }


    }
}
