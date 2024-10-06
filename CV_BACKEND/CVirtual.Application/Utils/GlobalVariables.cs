using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.Utils
{
    public sealed class GlobalVariables
    {
        // Atributo estático para almacenar la única instancia de la clase.
        private static volatile GlobalVariables? instancia = null;

        // Objeto de bloqueo para garantizar la concurrencia segura.
        private static readonly object syncLock = new object();

        private static List<ConstantesSingleton>? Elementos { get; set; }

        // Constructor privado para evitar la creación de instancias fuera de la clase.
        private GlobalVariables()
        {
            Elementos = new List<ConstantesSingleton>();
        }
        public GlobalVariables(List<ConstantesSingleton>? elementos)
        {
            Elementos = elementos;
        }

        // Propiedad para acceder a la instancia del servicio.
        public static GlobalVariables Instancia
        {
            get
            {
                // Verificar si la instancia ya existe.
                if (instancia == null)
                {
                    // Si no existe, bloquear el hilo para evitar concurrencia.
                    lock (syncLock)
                    {
                        // Verificar nuevamente dentro del bloque para evitar la creación de instancias duplicadas.
                        if (instancia == null)
                        {
                            instancia = new GlobalVariables();
                        }
                    }
                }

                return instancia;
            }
        }

        public static GlobalVariables GetInstance()
        {
            return Instancia;
        }

        public static int SetVariable(string key, string value)
        {
            ConstantesSingleton? Elemento = (from Variables in Elementos
                                             where Variables.Key == key
                                             select Variables).SingleOrDefault();
            int index = 0;
            if (Elemento is null)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ConstantesSingleton elemento = new ConstantesSingleton();
                    index = Elementos.Count + 1;
                    elemento.Index = index;
                    elemento.Key = key;
                    elemento.Value = value;
                    Elementos.Add(elemento);
                }
            }
            return index;
        }

        public static string GetVariable(string key)
        {
            ConstantesSingleton? Elemento = (from Variables in Elementos
                                             where Variables.Key == key
                                             select Variables).SingleOrDefault();
            string? value = string.Empty;

            if (Elemento is not null)
            {
                value = Elemento.Value;
            }
            return value;
        }


        public static List<ConstantesSingleton>? GetVariables()
        {
            return Elementos;
        }

        //public static int SetCodigoSistema(string value)
        //{
        //    return SetVariable(Configuraciones.CODIGO_SISTEMA, value);
        //}

        //public static string GetCodigoSistema()
        //{
        //    return GetVariable(Configuraciones.CODIGO_SISTEMA);
        //}

        //public static void LoadVariables()
        //{


        //    string? urlApiSucV2 = GetVariable(Configuraciones.UrlAPISucV2);
        //    string? CodigoSistema = GetCodigoSistema();
        //    string? CodigoConfiguracion = string.Empty;

        //    try
        //    {
        //        SucV2ApiClient sucClient = new SucV2ApiClient(urlApiSucV2, CodigoSistema);

        //        ConfiguracionObtenerConfiguracionData? _Data = null;


        //        // RUTA_LOG
        //        CodigoConfiguracion = Configuraciones.RUTA_LOG;

        //        _Data = sucClient.ConfiguracionObtenerConfiguracion(CodigoConfiguracion);

        //        if (_Data is not null)
        //        {
        //            SetVariable(CodigoConfiguracion, _Data.ValConf1);
        //            SetVariable(Configuraciones.NOMBRE_SISTEMA, _Data.NombreSistema);
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        throw new Exception("Exception iniciada en GlobalVariables.LoadVariables", exception);
        //    }
        //}
    }
}
