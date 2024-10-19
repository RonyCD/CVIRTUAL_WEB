using CVirtual.DataAccess.SQLServer.Commands;
using CVirtual.DataAccess.SQLServer.IQueries.Admin;
using CVirtual.Domain.Contract;
using CVirtual.Dto.Admin;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.Queries.Admin
{
    public class AdminQuery : BaseUnitOfWork, IAdminQuery
    {

        public AdminQuery(ISeguridadDbContext context) : base(context, true)
        {

        }

        public async Task<int?> CrearUsuario(RegistrarUsuarioRequest request)
        {
            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_USUARIO_CREAR", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdRol", request.IdRol);
                    command.Parameters.AddWithValue("@Nombres", request.Nombres);
                    command.Parameters.AddWithValue("@ApPaterno", request.ApPaterno);
                    command.Parameters.AddWithValue("@ApMaterno", request.ApMaterno);
                    command.Parameters.AddWithValue("@Username", request.Username);
                    command.Parameters.AddWithValue("@Correo", request.Correo);
                    command.Parameters.AddWithValue("@Contrasenia", request.Contrasenia);

                    // Parametro de salida
                    var idUsuarioParam = new SqlParameter("@IdUsuario", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(idUsuarioParam);

                    await command.ExecuteNonQueryAsync();

                    // Retornar el ID generado
                    return (int?)idUsuarioParam.Value;
                }
            }
        }

        public async Task<bool> CrearCliente(int idUsuario, RegistrarUsuarioRequest request)
        {
            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_CLIENTE_CREAR", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    command.Parameters.AddWithValue("@RUC", request.RUC);
                    command.Parameters.AddWithValue("@Telefono", request.Telefono);
                    command.Parameters.AddWithValue("@Celular1", request.Celular1);
                    command.Parameters.AddWithValue("@Celular2", request.Celular2);
                    command.Parameters.AddWithValue("@NombreComercial", request.NombreComercial);
                    command.Parameters.AddWithValue("@Direccion", request.Direccion);
                    command.Parameters.AddWithValue("@Logo", request.Logo ?? (object)DBNull.Value); // Manejo de valores null

                    // Parametro de salida para el ID_CLIENTE
                    var idClienteParam = new SqlParameter("@IdCliente", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(idClienteParam);

                    await command.ExecuteNonQueryAsync();

                    // Retornar true si el cliente fue creado correctamente
                    return idClienteParam.Value != null;
                }
            }
        }


    }
}
