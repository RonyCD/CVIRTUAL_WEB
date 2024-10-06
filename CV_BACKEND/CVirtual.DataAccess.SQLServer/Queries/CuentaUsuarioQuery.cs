using CVirtual.DataAccess.SQLServer.Commands;
using CVirtual.DataAccess.SQLServer.IQueries;
using CVirtual.Domain.Contract;
using CVirtual.Domain.Entities.Comun;
using CVirtual.Dto.Base;
using CVirtual.Dto.CuentaUsuario;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CVirtual.DataAccess.SQLServer.Queries
{
    public class CuentaUsuarioQuery : BaseUnitOfWork, ICuentaUsuarioQuery
    {
        public CuentaUsuarioQuery(ISeguridadDbContext context) : base(context, true)
        {

        }             


        public async Task<IniciarSesionResponse> ValidarUsuario(IniciarSesionRequest _Request)
        {
            using (SqlConnection cnn = new SqlConnection(this._ctx.SQLCnn()))
            {
                await cnn.OpenAsync();

                using (var command = new SqlCommand("SP_CV_API_VALIDAR_USUARIO", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@username", _Request.Username);
                    command.Parameters.AddWithValue("@contrasenia", _Request.Contrasenia);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            int result = reader.GetInt32(reader.GetOrdinal("Result"));

                            if (result == -1)
                                return null; // Usuario no encontrado

                            if (result == 0)
                                throw new UnauthorizedAccessException("Contraseña incorrecta");

                            // Solo se accede a los demás datos si result == 1 (login exitoso)
                            return new IniciarSesionResponse
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ID_USUARIO")),
                                IdRol = reader.GetInt32(reader.GetOrdinal("ID_ROL")),
                                Nombres = reader.GetString(reader.GetOrdinal("NOMBRES")),
                                ApPaterno = reader.GetString(reader.GetOrdinal("AP_PATERNO")),
                                ApMaterno = reader.GetString(reader.GetOrdinal("AP_MATERNO")),
                                Correo = reader.GetString(reader.GetOrdinal("CORREO"))
                            };
                        }
                    }
                }
            }

            return null;
        }







    }
}

