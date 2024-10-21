using CVirtual.DataAccess.SQLServer.Commands;
using CVirtual.DataAccess.SQLServer.IQueries;
using CVirtual.Domain.Contract;
using CVirtual.Domain.Entities.Banner;
using CVirtual.Domain.Entities.Categoria;
using CVirtual.Dto.Banner;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.Queries
{
    public class BannerQuery : BaseUnitOfWork, IBannerQuery
    {
        public BannerQuery(ISeguridadDbContext context) : base(context, true)
        {

        }

        
        public async Task<BannerEntity> AgregarBanner(BannerRequest _Request)
        {
            BannerEntity bannerEntity = null;

            using (SqlConnection connection = new SqlConnection(_ctx.SQLCnn()))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SP_CV_API_BANNER_AGREGAR", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdCartaVirtual", _Request.IdCartaVirtual);
                    command.Parameters.AddWithValue("@ImagenBanner", _Request.ImagenBanner);

                    try
                    {
                        // ID_BANNER
                        var newIdBanner = await command.ExecuteScalarAsync();
                        if (newIdBanner != null)
                        {
                            bannerEntity = new BannerEntity
                            {
                                IdBanner = Convert.ToInt32(newIdBanner),
                                IdCartaVirtual = _Request.IdCartaVirtual,
                                ImagenBanner = _Request.ImagenBanner
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al agregar el banner: " + ex.Message);
                    }
                }
            }

            return bannerEntity;
        }

        public async Task<bool> EliminarBanner(int _IdBanner)
        {
            bool isDeleted = false;

            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_BANNER_ELIMINAR", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdBanner", _IdBanner);

                    var result = await command.ExecuteScalarAsync();

                    if (result != null && Convert.ToInt32(result) > 0)
                    {
                        isDeleted = true;
                    }
                }
            }

            return isDeleted;
        }



        public async Task<ICollection<BannerEntity>> ObtenerPorIdCVirtual(int _IdCartaVirtual)
        {
            var banners = new List<BannerEntity>();

            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_BANNER_OBTENER", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdCartaVirtual", _IdCartaVirtual);

                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            byte[] imagenBytes = null;

                            // Obtener tamaño de los bytes
                            long length = reader.GetBytes(reader.GetOrdinal("IMAGEN_BANNER"), 0, null, 0, 0);

                            if (length > 0)
                            {
                                imagenBytes = new byte[length];

                                // Leer bytes en el arreglo
                                reader.GetBytes(reader.GetOrdinal("IMAGEN_BANNER"), 0, imagenBytes, 0, (int)length);
                            }

                            var banner = new BannerEntity
                            {

                                IdBanner = reader.GetInt32(reader.GetOrdinal("ID_BANNER")),
                                IdCartaVirtual = reader.GetInt32(reader.GetOrdinal("ID_CARTA_VIRTUAL")),
                                ImagenBanner = imagenBytes
                            };

                            banners.Add(banner);
                        }
                    }
                }
            }

            return banners;
        }

    }
}
