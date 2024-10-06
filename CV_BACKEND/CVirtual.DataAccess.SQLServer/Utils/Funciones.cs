using Itenso.TimePeriod;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.Utils
{
    public class Funciones
    {
        public static string EliminarDominioUsuario(string strUserName)
        {
            int hasDomain = strUserName.IndexOf(@"\");
            if (hasDomain > 0)
            {
                strUserName = strUserName.Remove(0, hasDomain + 1);
            }
            return strUserName;
        }

        public static string ToUpperFirstLetter(string strCadena)
        {
            if (string.IsNullOrEmpty(strCadena))
                return string.Empty;

            char[] chrLetras = strCadena.ToCharArray();
            chrLetras[0] = char.ToUpper(chrLetras[0]);

            return new string(chrLetras);
        }

        public static IEnumerable<KeyValuePair<string, string>> ListarMeses()
        {
            for (int i = 1; i <= 12; i++)
            {
                yield return new KeyValuePair<string, string>(i.ToString().Trim().PadLeft(2, '0'), DateTimeFormatInfo.CurrentInfo.GetMonthName(i).ToUpper());
            }
        }

        public static IEnumerable<string> ListarAnios(int intAnioInicio)
        {
            List<string> lstAnios = new List<string>();

            for (int i = DateTime.Now.Year; i >= intAnioInicio; i--)
            {
                lstAnios.Add(i.ToString());
            }

            return lstAnios;
        }

        public static bool IsNumber(Object oCadena)
        {
            try
            {
                Double.Parse(oCadena.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool IsInt(string oCadena)
        {
            try
            {
                int.Parse(oCadena.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidarRUC(string strRUC)
        {
            int intSumDig = 0;
            string strModCal;
            string strCadena = "5432765432";
            if (strRUC.Trim().Length == 11)
            {
                //Aplico Modulo 11
                int j = 0;
                for (int i = 1; i <= 10; i++)
                {
                    intSumDig = intSumDig + (int.Parse(strRUC.Trim().Substring(j, i - j)) * int.Parse(strCadena.Substring(j, i - j)));
                    j = j + 1;
                }

                //Calculando el modulo once
                string strMod11 = (11 - (intSumDig % 11)).ToString();

                if (strMod11.Length == 2)
                {
                    strModCal = strMod11.Substring(1, 1);
                }
                else
                {
                    strModCal = strMod11.Substring(0, 1);
                }

                //Comparando Resultados
                if (strModCal != strRUC.Substring(10, 1))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public static string CalcularAnteriorLetra(string strLetra)
        {
            string strAnteriorLetra;

            if (string.IsNullOrEmpty(strLetra) || strLetra.Trim().ToUpper() == "A")
            {
                strAnteriorLetra = "A";
            }
            else
            {
                strAnteriorLetra = ExcelColumnFromNumber(NumberFromExcelColumn(strLetra) - 1);
            }

            return strAnteriorLetra;
        }

        public static string CalcularSiguienteLetra(string strLetra)
        {
            string strSiguienteLetra;

            if (string.IsNullOrEmpty(strLetra))
            {
                strSiguienteLetra = "A";
            }
            else
            {
                strSiguienteLetra = ExcelColumnFromNumber(NumberFromExcelColumn(strLetra) + 1);
            }

            return strSiguienteLetra;
        }

        public static string ExcelColumnFromNumber(int value)
        {
            StringBuilder sb = new StringBuilder();

            do
            {
                value--;
                int remainder = 0;
                value = Math.DivRem(value, 26, out remainder);
                sb.Insert(0, Convert.ToChar('A' + remainder));

            } while (value > 0);

            return sb.ToString();
        }

        public static int NumberFromExcelColumn(string column)
        {
            int retVal = 0;
            string col = column.ToUpper();
            for (int iChar = col.Length - 1; iChar >= 0; iChar--)
            {
                char colPiece = col[iChar];
                int colNum = colPiece - 64;
                retVal = retVal + colNum * (int)Math.Pow(26, col.Length - (iChar + 1));
            }
            return retVal;
        }

        public static int CalculateDays(DateTime oldDate, DateTime newDate)
        {
            // Diferencia de fechas
            //LA FECHA MAYOR VA COMO SEGUNDO PARÁMETRO
            TimeSpan ts = newDate - oldDate;

            // Diferencia de días
            return ts.Days;
        }

        // OP = 1; devuelve diferencia en dias
        // OP = 2; devuelve diferencia en meses
        // OP = 3; devuelve diferencia en años
        public static int DateDifference(DateTime pFechaUno, DateTime pFechaDos, int op)
        {
            int vValorReturn = 0;
            try
            {
                int[] vDiasMes = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

                DateTime vFechaOrigen;
                DateTime vFechaDestino;
                int vAnio;
                int vMes;
                int vDia;
                int vIncremento;

                //Determinamos cual es la fecha menor
                if (pFechaUno > pFechaDos)
                {
                    vFechaOrigen = pFechaDos;
                    vFechaDestino = pFechaUno;
                }
                else
                {
                    vFechaOrigen = pFechaUno;
                    vFechaDestino = pFechaDos;
                }

                // Calculamos los dias
                vIncremento = 0;

                if (vFechaOrigen.Day > vFechaDestino.Day)
                {
                    vIncremento = vDiasMes[vFechaOrigen.Month - 1];

                }
                if (vIncremento == -1)
                {
                    if (DateTime.IsLeapYear(vFechaOrigen.Year))
                    {
                        // Para los años bisiestos
                        vIncremento = 29;
                    }
                    else
                    {
                        vIncremento = 28;
                    }
                }
                if (vIncremento != 0)
                {
                    vDia = (vFechaDestino.Day + vIncremento) - vFechaOrigen.Day;
                    vIncremento = 1;
                }
                else
                {
                    vDia = vFechaDestino.Day - vFechaOrigen.Day;
                }

                //Calculamos los meses
                if ((vFechaOrigen.Month + vIncremento) > vFechaDestino.Month)
                {
                    vMes = (vFechaDestino.Month + 12) - (vFechaOrigen.Month + vIncremento);
                    vIncremento = 1;
                }
                else
                {
                    vMes = (vFechaDestino.Month) - (vFechaOrigen.Month + vIncremento);
                    vIncremento = 0;
                }
                //Calculamos los años
                vAnio = vFechaDestino.Year - (vFechaOrigen.Year + vIncremento);

                if (op == 1) { vValorReturn = vDia; }
                if (op == 2) { vValorReturn = vMes; }
                if (op == 3) { vValorReturn = vAnio; }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vValorReturn;
        }

        public static Boolean IsValidEmail(String email)
        {
            Regex oRegExp = new Regex(@"^[A-Za-z0-9_.\-]+@[A-Za-z0-9_\-]+\.([A-Za-z0-9_\-]+\.)*[A-Za-z][A-Za-z]+$", RegexOptions.IgnoreCase);
            return oRegExp.Match(email).Success;
        }

        public static string FormatoCorreo(List<string> lstCorreoAlerta)
        {
            string strCorreo = string.Empty;

            foreach (string strCorreoAlerta in lstCorreoAlerta)
            {
                if (strCorreo != string.Empty)
                {
                    strCorreo += "; ";
                }

                strCorreo += strCorreoAlerta;
            }

            return strCorreo;
        }

        public static decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            return Math.Truncate(step * value) / step;
        }

        public static double RoundUp(double dblNumero, int intDigitos)
        {
            return Math.Ceiling(dblNumero * Math.Pow(10, intDigitos)) / Math.Pow(10, intDigitos);
        }

        public static bool ValidarDecimalPrecisionScale(decimal decValor, int intPrecision, int intScale)
        {
            try
            {
                System.Data.SqlTypes.SqlDecimal.ConvertToPrecScale(new SqlDecimal(decValor), intPrecision, intScale);
            }
            catch (SqlTruncateException ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Permite convertir de String a DateTime con su respectivo formato de fecha
        /// </summary>
        /// <param name="valor">Fecha</param>
        /// <param name="formato">Formato</param>
        /// <returns>Retorna de Fecha</returns>
        public static DateTime? CadenaFormatoFecha(string valor, string formato)
        {
            try
            {
                return string.IsNullOrEmpty(valor) ? (DateTime?)null : DateTime.ParseExact(valor, formato, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        /// <summary>
        /// Permite convertir DateTime a String con su respectivo formato de fecha
        /// </summary>
        /// <param name="valor">Fecha</param>
        /// <param name="formato">Formato</param>
        /// <returns>Retorna la fecha formateada</returns>
        public static string FechaFormatoCadena(DateTime? valor, string formato)
        {
            if (valor.HasValue)
            {
                return Convert.ToDateTime(valor).ToString(formato, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Permite convertir de String a Decimal con su respectivo formato de decimal
        /// </summary>
        /// <param name="value">Decimal</param>
        /// <param name="format">Formato</param>
        /// <param name="numberFormat">Formato de Número</param>
        /// <returns>Retorna de Decimal</returns>
        public static decimal? CadenaFormatoDecimal(string value, string format, string separadorDecimal, int intCantidadDecimal = 2)
        {
            try
            {
                if (!format.Contains("{"))
                {
                    format = "{0:" + format + "}";
                }

                NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
                numberFormatInfo.NumberDecimalSeparator = separadorDecimal;
                //numberFormatInfo.NumberGroupSeparator = string.Empty;

                return string.IsNullOrEmpty(value) ? (decimal?)null : decimal.Round(System.Decimal.Parse(value, numberFormatInfo), intCantidadDecimal, MidpointRounding.AwayFromZero);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        /// <summary>
        /// Permite convertir Decimal a String con su respectivo formato de decimal
        /// </summary>
        /// <param name="value">Decimal</param>
        /// <param name="format">Formato</param>
        /// <param name="numberFormat">Formato de Número</param>
        /// <returns>Retorna el decimal formateado</returns>
        public static string DecimalFormatoCadena(decimal? value, string format, string separadorDecimal)
        {
            if (value.HasValue)
            {
                if (!format.Contains("{"))
                {
                    format = "{0:" + format + "}";
                }
                NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
                numberFormatInfo.NumberDecimalSeparator = separadorDecimal;
                //numberFormatInfo.NumberGroupSeparator = string.Empty;

                format = format.Replace(numberFormatInfo.NumberDecimalSeparator, "@");
                format = format.Replace("@", ".");

                return string.Format(numberFormatInfo, format, value);
            }
            else
            {
                return string.Empty;
            }
        }

        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        public static double GetMonthDifferenceDouble(DateTime startDate, DateTime endDate, bool returnRealMonths = false)
        {
            //if (endDate.Ticks < startDate.Ticks)
            //{
            //    DateTime temp = startDate;
            //    startDate = endDate;
            //    endDate = temp;
            //}

            //double percFrom = (double)startDate.Day / DateTime.DaysInMonth(startDate.Year, startDate.Month);
            //double percTo = (double)endDate.Day / DateTime.DaysInMonth(endDate.Year, endDate.Month);
            //double months = (endDate.Year * 12 + endDate.Month) - (startDate.Year * 12 + startDate.Month);

            //return Math.Ceiling(months - percFrom + percTo);
            //var date = Convert.ToDateTime("10/11/2021");
            var dateDiff = new DateDiff(startDate, endDate);
            var months = dateDiff.Months;
            if (dateDiff.ElapsedDays > 0) months = months + 1;

            return (months > 0 || returnRealMonths) ? months : 1;
        }

        public static double GetMonthDifferenceDoubleCalculate(DateTime startDate, DateTime endDate, bool returnRealMonths = false)
        {
            var dateDiff = new DateDiff(startDate, endDate);
            var months = dateDiff.Months;
            var dtmFecha = dateDiff.ElapsedDays > -1 ? dateDiff.ElapsedDays : 0;
            var mesesTranscurridos = double.Parse(dateDiff.Months + "." + dtmFecha);
            return mesesTranscurridos;
        }

        public static bool GuardarArchivoCarga(string directorioTemporal, string directorioFinal, string nombreArchivo, string strIdArchivo, string strIdArchivoGenerado, string strIdOrigen)
        {
            try
            {
                if (!File.Exists(directorioFinal))
                {
                    Directory.CreateDirectory(directorioFinal);
                }

                string archivoOrigen = Path.Combine(directorioTemporal, nombreArchivo);
                string archivoDestino = Path.Combine(directorioFinal, strIdArchivoGenerado + nombreArchivo);

                if (File.Exists(archivoOrigen))
                {
                    File.Copy(archivoOrigen, archivoDestino);
                    File.Delete(archivoOrigen);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw; //new SCPException(ex.Message);
            }

            // return true;
        }

        public static bool GuardarArchivo(string directorioTemporal, string directorioFinal, string nombreArchivo, string strIdArchivo, string strIdArchivoGenerado, string strIdOrigen)
        {
            try
            {
                if (!File.Exists(directorioFinal))
                {
                    Directory.CreateDirectory(directorioFinal);
                }

                string archivoOrigen = Path.Combine(directorioTemporal, strIdArchivo + strIdOrigen + nombreArchivo);
                string archivoDestino = Path.Combine(directorioFinal, strIdArchivoGenerado + nombreArchivo);

                if (File.Exists(archivoOrigen))
                {
                    File.Move(archivoOrigen, archivoDestino);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false; //new SCPException(ex.Message);
            }

            return true;
        }

        public static bool EliminarArchivo(string directorioTemporal, string strIdArchivo, string nombreArchivo, string directorioFinal, string strIdOrigen)
        {
            string archivoTemporal = Path.Combine(directorioTemporal, strIdArchivo + strIdOrigen + nombreArchivo);
            if (File.Exists(archivoTemporal))
            {
                try
                {
                    File.Delete(archivoTemporal);
                }
                catch (IOException ex)
                {
                    return false; // new SCPException(ex.Message);
                }
            }

            string archivoFinal = Path.Combine(directorioFinal, strIdArchivo + nombreArchivo);

            if (File.Exists(archivoFinal))
            {
                try
                {
                    File.Delete(archivoFinal);
                }
                catch (IOException ex)
                {
                    return false; // new SCPException(ex.Message);
                }
            }

            return true;
        }

        public static void CrearArchivoTexto(string strRutaArchivo, string strNombreArchivo, string strContenido)
        {
            if (!Directory.Exists(strRutaArchivo))
            {
                Directory.CreateDirectory(strRutaArchivo);
            }

            string archivo = Path.Combine(strRutaArchivo, strNombreArchivo);

            using (StreamWriter swFile = new StreamWriter(archivo, true))
            {
                swFile.WriteLine(strContenido);
            }
        }

        public static int GenerarNumeroAleatorio(int intRangoInicio, int intRangoFin)
        {
            Random rnd = new Random();
            return rnd.Next(intRangoInicio, intRangoFin);
        }

        public static string[] SplitPorCantidadCaracteres(string words, int lenght)
        {
            var newSplit = new List<string>();
            var splitted = words.Split();
            string word = null;

            for (int i = 0; i < splitted.Length; i++)
            {
                if (word == null)
                {
                    word = splitted[i];
                }
                else if (splitted[i].Length + 1 + word.Length > lenght)
                {
                    newSplit.Add(word);
                    word = splitted[i];
                }
                else
                {
                    word += " " + splitted[i];
                }
            }

            newSplit.Add(word);

            return newSplit.ToArray();
        }

        public static IEnumerable<string> JustificarTexto(string strTexto, int intTamanioLinea)
        {
            int rowLength = -1;
            var splitted = strTexto.Split(' ');
            var wordList = new List<string>();

            foreach (var x in splitted)
            {
                rowLength += 1 + x.Length;

                if (rowLength <= intTamanioLinea)
                {
                    wordList.Add(x);
                }
                else
                {
                    var row = JustifyRow(wordList, intTamanioLinea);
                    rowLength = x.Length;
                    wordList = new List<String> { x };
                    yield return row;
                }
            }
            yield return string.Join(" ", wordList);
        }

        public static string JustifyRow(List<string> strTexto, int intTamanioLinea)
        {
            if (strTexto.Count == 1) { return strTexto[0]; }
            var rem = intTamanioLinea - strTexto.Sum(x => x.Length);
            var baseSpaces = rem / (strTexto.Count - 1);
            var addSpaces = rem - baseSpaces * (strTexto.Count - 1);

            for (int i = 0; i < strTexto.Count - 1; i += 2)
            {
                strTexto.Insert(i + 1, new String(' ', baseSpaces + (addSpaces-- > 0 ? 1 : 0)));
            }

            return string.Join("", strTexto);
        }

        public static DateTime CalcularPrimerDiaMes(int intAnio, int intMes)
        {
            DateTime dtFechaInicioMes = new DateTime(intAnio, intMes, 1);

            return dtFechaInicioMes;
        }

        public static DateTime CalcularPrimerDiaHabilMes(DateTime dtFechaCalcular, List<string> listaFeriados)
        {
            DateTime dtFechaInicioMes = CalcularPrimerDiaMes(dtFechaCalcular.Year, dtFechaCalcular.Month);
            DateTime dtFechaFinMes = CalcularUltimoDiaMes(dtFechaCalcular.Year, dtFechaCalcular.Month);

            DateTime dtPrimerDiaHabilMes = dtFechaInicioMes;

            for (int i = 0; i < dtFechaFinMes.Day; i++)
            {
                DateTime dtFechaEvaluar = dtFechaInicioMes.AddDays(i);
                string strFechaEvaluar = FechaFormatoCadena(dtFechaEvaluar, Constants.Formato.FormatoFecha);

                if (dtFechaEvaluar.DayOfWeek != DayOfWeek.Saturday && dtFechaEvaluar.DayOfWeek != DayOfWeek.Sunday)
                {
                    bool esFeriadoFechaEvaluar = listaFeriados.Any(x => x == strFechaEvaluar);

                    if (!esFeriadoFechaEvaluar)
                    {
                        dtPrimerDiaHabilMes = dtFechaEvaluar;
                        break;
                    }
                }
            }

            return dtPrimerDiaHabilMes;
        }

        public static DateTime CalcularUltimoDiaMes(int intAnio, int intMes)
        {
            DateTime dtFechaInicioMes = CalcularPrimerDiaMes(intAnio, intMes);

            return dtFechaInicioMes.AddMonths(1).AddDays(-1);
        }

        public static DateTime CalcularUltimoDiaHabilMesDiaSiguiente(DateTime dtFechaCalcular, List<string> listaFeriados, int intDiasSiguiente)
        {
            DateTime dtFechaFinMes = CalcularUltimoDiaMes(dtFechaCalcular.Year, dtFechaCalcular.Month);

            DateTime dtUltimoDiaHabilMes = dtFechaFinMes;

            for (int i = 0; i < dtFechaFinMes.Day; i++)
            {
                DateTime dtFechaEvaluar = dtFechaFinMes.AddDays(-i);
                string strFechaEvaluar = FechaFormatoCadena(dtFechaEvaluar, Constants.Formato.FormatoFecha);

                if (dtFechaEvaluar.DayOfWeek != DayOfWeek.Saturday && dtFechaEvaluar.DayOfWeek != DayOfWeek.Sunday)
                {
                    bool esFeriadoFechaEvaluar = listaFeriados.Any(x => x == strFechaEvaluar);

                    if (!esFeriadoFechaEvaluar)
                    {
                        dtUltimoDiaHabilMes = dtFechaEvaluar;
                        break;
                    }
                }
            }

            return dtUltimoDiaHabilMes.AddDays(intDiasSiguiente);
        }

        public static bool FechaEsDiaHabil(DateTime dtFechaEvaluar, List<string> listaFeriados)
        {
            bool blEsDiaHabil = true;
            string strFechaEvaluar = FechaFormatoCadena(dtFechaEvaluar, Constants.Formato.FormatoFecha);

            if (dtFechaEvaluar.DayOfWeek != DayOfWeek.Saturday && dtFechaEvaluar.DayOfWeek != DayOfWeek.Sunday)
            {
                bool esFeriadoFechaEvaluar = listaFeriados.Any(x => x == strFechaEvaluar);

                if (esFeriadoFechaEvaluar)
                {
                    blEsDiaHabil = false;
                }
            }
            else
            {
                blEsDiaHabil = false;
            }

            return blEsDiaHabil;
        }
        /// <summary>
        /// Mètodo que obtiene la fecha habil siguiente segùn la cantidad de dias a adicionar
        /// </summary>
        /// <param name="fechaProceso">fecha a procesar</param>
        /// <param name="diasHabilesAdicionar">número de días hábiles a adicionar</param>
        /// <param name="listaFeriados">listado de feriados</param>
        /// <returns>fecha hábil procesada</returns>
        public static DateTime ObtenerFechaHabilSiguiente(DateTime fechaProceso,
            int diasHabilesAdicionar, List<string> listaFeriados)
        {
            while (diasHabilesAdicionar > 0)
            {
                fechaProceso = fechaProceso.AddDays(1);
                if (fechaProceso.EsDiaHabil(listaFeriados))
                    diasHabilesAdicionar--;
            }
            return fechaProceso;
        }

        public static string ConvertirDNIaRUC(string strDNI)
        {
            string salida = "";
            int intSumDig = 0;
            string strModCal;
            string strCadena = "5432765432";
            if (strDNI.Trim().Length == 8)
            {
                string tmpCadenaConDNI = "10" + strDNI;
                //Aplico Modulo 11
                int j = 0;
                for (int i = 1; i <= 10; i++)
                {
                    intSumDig = intSumDig + (int.Parse(tmpCadenaConDNI.Trim().Substring(j, i - j)) * int.Parse(strCadena.Substring(j, i - j)));
                    j = j + 1;
                }

                //Calculando el modulo once
                string strMod11 = (11 - (intSumDig % 11)).ToString();

                if (strMod11.Length == 2)
                {
                    strModCal = strMod11.Substring(1, 1);
                }
                else
                {
                    strModCal = strMod11.Substring(0, 1);
                }

                salida = "10" + strDNI + "" + strModCal;
            }
            return salida;
        }

        public static string ObtenerNombreMesNumero(int numeroMes)
        {
            try
            {
                DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes = formatoFecha.GetMonthName(numeroMes);
                return nombreMes;
            }
            catch
            {
                return "Desconocido";
            }
        }

        public static DateTime AgregarDiasHabiles(List<string> listaFeriados, DateTime dtFechaEvaluar, int ndias)
        {
            DateTime dtFechaPivote = dtFechaEvaluar;
            for (int i = 0; i < ndias; i++)
            {
                dtFechaPivote = dtFechaPivote.AddDays(1);
                string strFechaPivote = FechaFormatoCadena(dtFechaPivote, Constants.Formato.FormatoFecha);

                if (dtFechaPivote.DayOfWeek == DayOfWeek.Saturday || dtFechaPivote.DayOfWeek == DayOfWeek.Sunday || listaFeriados.Any(x => x == strFechaPivote))
                {
                    i--;
                }
            }
            return dtFechaPivote;
        }

        public static DateTime AgregarDiasHabiles(List<DateTime> listaFeriados, DateTime dtFechaEvaluar, int ndias)
        {
            DateTime dtFechaPivote = dtFechaEvaluar;
            for (int i = 0; i < ndias; i++)
            {
                dtFechaPivote = dtFechaPivote.AddDays(1);

                if (dtFechaPivote.DayOfWeek == DayOfWeek.Saturday || dtFechaPivote.DayOfWeek == DayOfWeek.Sunday || listaFeriados.Any(x => x == dtFechaPivote))
                {
                    i--;
                }
            }
            return dtFechaPivote;
        }

        public static byte[]? ConvertirUTF8(byte[]? archivoBits)
        {
            byte[]? retorno;
            byte[]? retornoBits;

            if (archivoBits.Length < 6 || archivoBits == null)
            {
                return archivoBits;
            }

            byte[] BOMBytes = new byte[] { archivoBits[0], archivoBits[1], archivoBits[2], archivoBits[3] };
            Encoding? encoding;
            Encoding utf8 = Encoding.UTF8;


            if (BOMBytes[0] == 0xff && BOMBytes[1] == 0xfe && BOMBytes[2] != 0x00 && BOMBytes[3] == 0x00)
            {
                encoding = Encoding.Unicode;
                retorno = archivoBits;
            }
            else
            if (BOMBytes[0] == 0xfe && BOMBytes[1] == 0xff && BOMBytes[2] == 0x00 && BOMBytes[3] != 0x00)
            {
                encoding = Encoding.BigEndianUnicode;
                retorno = archivoBits;
            }
            else
            if (BOMBytes[0] == 0xef && BOMBytes[1] == 0xbb && BOMBytes[2] == 0xbf)
            {
                retorno = new byte[archivoBits.Length - 3];

                Array.Copy(archivoBits, 3, retorno, 0, retorno.Length);
                encoding = Encoding.UTF8;
            }
            else
            {
                retorno = archivoBits;
                encoding = Encoding.UTF8;
            }

            retornoBits = Encoding.Convert(encoding, utf8, retorno);

            if (Encoding.UTF8.GetString(retornoBits).IndexOf(Convert.ToChar(65533)) >= 0)
            {
                retornoBits = Encoding.Convert(Encoding.Latin1, Encoding.UTF8, retorno);
            }

            return retornoBits;
        }
    }

    public static class ExtensionesDateTime
    {
        /// <summary>
        /// Método extendido que balida si la fecha es día habil, no considera sábados ni domingos
        /// </summary>
        /// <param name="dtFechaEvaluar">fecha a evaluar</param>
        /// <param name="listaFeriados">lsitado de feriados</param>
        /// <returns>true si es día habil, false caso contrario</returns>
        public static bool EsDiaHabil(this DateTime dtFechaEvaluar, List<string> listaFeriados)
        {
            string strFechaEvaluar = Funciones.FechaFormatoCadena(
                dtFechaEvaluar, Constants.Formato.FormatoFecha);
            if (dtFechaEvaluar.DayOfWeek != DayOfWeek.Saturday
                && dtFechaEvaluar.DayOfWeek != DayOfWeek.Sunday
                && !listaFeriados.Contains(strFechaEvaluar))
            {
                return true;
            }
            else return false;

        }
    }
}
