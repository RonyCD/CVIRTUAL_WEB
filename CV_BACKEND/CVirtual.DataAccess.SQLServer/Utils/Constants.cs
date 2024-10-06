using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.Utils
{
    public class Constants
    {
        public class Formato
        {
            /// <summary>
            /// Formato de Fecha
            /// </summary>
            public static readonly string FormatoFecha = "dd/MM/yyyy";

            public static readonly string FormatoFechaAnioMesDia = "yyyy-MM-dd";

            /// <summary>
            /// Formato de Fecha de Selector
            /// </summary>
            public static readonly string FormatoFechaSelector = "dd/mm/yy";

            /// <summary>
            /// Formato de Fecha de Mascara
            /// </summary>
            public static readonly string FormatoFechaMascara = "00/00/0000";

            /// <summary>
            /// Formato de Hora
            /// </summary>
            public static readonly string FormatoHora = "hh:mm tt";

            /// <summary>
            /// Formato de 24 Horas
            /// </summary>
            public static readonly string Formato24Horas = "HH:mm";

            /// <summary>
            /// Formato de 24 Horas
            /// </summary>
            public static readonly string Formato24HorasYSegundos = "HH:mm:ss";

            /// <summary>
            /// Formato de Número Entero
            /// </summary>
            public static readonly string FormatoNumeroEntero = "#,##0";

            /// <summary>
            /// Formato de Número Decimal
            /// </summary>
            public static readonly string FormatoNumeroDecimal = "#,##0.00";

            /// <summary>
            /// Formato de número de contrato
            /// </summary>
            public static readonly string FormatoNumeroContrato = "{0}.{1}.{2}.{3}.{4}";

            /// <summary>
            /// Formato de número de contrato
            /// </summary>
            public static readonly string SeparadorFormatoNumeroContrato = ".";

            /// <summary>
            /// Separador de Número Decimal
            /// </summary>
            public static readonly string SeparadorNumeroDecimal = ".";

            /// <summary>
            /// Formato de número de adenda
            /// </summary>
            public static readonly string FormatoNumeroAdenda = "{0}-A{1}";


            public static readonly string FormatoFechaSubguion = "dd_MM_yyyy";
        }
    }
}
