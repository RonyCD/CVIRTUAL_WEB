﻿using CVirtual.DataAccess.SQLServer.Commands;
using CVirtual.DataAccess.SQLServer.IQueries;
using CVirtual.Domain.Contract;
using CVirtual.Domain.Entities.Categoria;
using CVirtual.Domain.Entities.Subcategoria;
using CVirtual.Dto.Subcategoria;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.Queries
{
    public class SubcategoriaQuery : BaseUnitOfWork, ISubcategoriaQuery
    {
        public SubcategoriaQuery(ISeguridadDbContext context) : base(context, true)
        {

        }

        /* MODELO
        public async Task<SubcategoriaEntity> CrearSubcategoria(SubcategoriaRequest _Request)
        {
            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_SUBCATEGORIA_CREAR", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdCategoria", _Request.IdCategoria);
                    command.Parameters.AddWithValue("@NombreSubcategoria", _Request.NombreSubcategoria);
                    command.Parameters.AddWithValue("@DescripcionSubcategoria", _Request.DescripcionSubcategoria);
                    command.Parameters.AddWithValue("@Precio", _Request.Precio);
                    command.Parameters.AddWithValue("@Imagen", _Request.Imagen);

                    await command.ExecuteNonQueryAsync();
                }
            }

            var nuevaCategoria = new SubcategoriaEntity
            {
                IdCategoria = _Request.IdCategoria,
                NombreSubcategoria = _Request.NombreSubcategoria,
                DescripcionSubcategoria = _Request.DescripcionSubcategoria,
                Precio = _Request.Precio,
                Imagen = _Request.Imagen
            };

            return nuevaCategoria;
        }
        */

        public async Task<SubcategoriaEntity> CrearSubcategoria(SubcategoriaRequest _Request)
        {
            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_SUBCATEGORIA_CREAR", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdCategoria", _Request.IdCategoria);
                    command.Parameters.AddWithValue("@NombreSubcategoria", _Request.NombreSubcategoria);
                    command.Parameters.AddWithValue("@DescripcionSubcategoria", _Request.DescripcionSubcategoria);
                    command.Parameters.AddWithValue("@Precio", _Request.Precio);
                    command.Parameters.AddWithValue("@Imagen", _Request.Imagen);

                    //ID_SUBCATEGORIA
                    var newIdSubcategoria = (int)await command.ExecuteScalarAsync();

                    var nuevaSubcategoria = new SubcategoriaEntity
                    {
                        IdSubcategoria = newIdSubcategoria,
                        IdCategoria = _Request.IdCategoria,
                        NombreSubcategoria = _Request.NombreSubcategoria,
                        DescripcionSubcategoria = _Request.DescripcionSubcategoria,
                        Precio = _Request.Precio,
                        Imagen = _Request.Imagen
                    };

                    return nuevaSubcategoria;
                }
            }
        }

        public async Task<ICollection<SubcategoriaEntity>> ObtenerByIdCategoria(int _IdCategoria)
        {
            var subCategorias = new List<SubcategoriaEntity>();


            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_SUBCATEGORIA_OBTENER", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdCategoria", _IdCategoria);

                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {

                            byte[] imagenBytes = null;

                            // Obtener tamaño de los bytes
                            long length = reader.GetBytes(reader.GetOrdinal("IMAGEN"), 0, null, 0, 0);

                            if (length > 0)
                            {
                                imagenBytes = new byte[length];

                                // Leer bytes en el arreglo
                                reader.GetBytes(reader.GetOrdinal("IMAGEN"), 0, imagenBytes, 0, (int)length);
                            }

                            var subcategoria = new SubcategoriaEntity
                            {

                                IdSubcategoria = reader.GetInt32(reader.GetOrdinal("ID_SUB_CATE")),
                                IdCategoria = reader.GetInt32(reader.GetOrdinal("ID_CATEGORIA")),
                                NombreSubcategoria = reader.GetString(reader.GetOrdinal("NOMBRE_SUB_CATE")),
                                DescripcionSubcategoria = reader.GetString(reader.GetOrdinal("DESC_SUB_CATE")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("PRECIO")),
                                Imagen = imagenBytes
                            };

                            subCategorias.Add(subcategoria);
                        }
                    }
                }
            }

            return subCategorias;
        }

    }
}
