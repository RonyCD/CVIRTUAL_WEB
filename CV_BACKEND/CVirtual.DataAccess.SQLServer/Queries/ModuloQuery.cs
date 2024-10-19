using CVirtual.DataAccess.SQLServer.Commands;
using CVirtual.DataAccess.SQLServer.IQueries;
using CVirtual.Domain.Contract;
using CVirtual.Domain.Entities.Modulo;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.Queries
{
    public class ModuloQuery : BaseUnitOfWork, IModuloQuery
    {

        public ModuloQuery(ISeguridadDbContext context) : base(context, true)
        {

        }

        public ICollection<ModuloEntity> ListarModulos()
        {
            var listaModulos = new List<ModuloEntity>();

            using (SqlConnection cnn = new SqlConnection(_ctx.SQLCnn()))
            {
                cnn.Open();

                using (var command = new SqlCommand("SP_CV_API_MODULO_LISTAR", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var modulo = new ModuloEntity
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ID_MODULO")),
                                Nombre = reader.GetString(reader.GetOrdinal("NOMBRE")),
                                Descripcion = reader.GetString(reader.GetOrdinal("DESCRIPCION")),
                                Estado = reader.GetString(reader.GetOrdinal("ESTADO"))
                            };

                            listaModulos.Add(modulo);
                        }
                    }
                }
            }

            return listaModulos;
        }

    }
}
