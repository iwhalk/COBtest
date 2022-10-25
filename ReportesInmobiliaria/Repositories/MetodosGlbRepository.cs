using Oracle.ManagedDataAccess.Client;
using ReportesInmobiliaria.Services;
using System.Data;

namespace ReportesInmobiliaria.Repositories
{
    public class MetodosGlbRepository
    {
        //private static readonly string ConnectString = "User Id=GEADBA;Password=fgeuorjvne;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.1.1.148)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=GEAPROD)))";
        private static readonly OracleConnection Conexion = new();
        private readonly IConfiguration _configuration;
        //private readonly CreationLogger _logger = new();
        private ILogger<MetodosGlbRepository> _logger;

        public MetodosGlbRepository(ILogger<MetodosGlbRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public DataSet Ds = new();
        public DataSet Ds1 = new();
        public DataSet Ds2 = new();
        public DataSet Ds3 = new();
        public DataSet Ds4 = new();
        public DataSet Ds5 = new();
        public DataSet Ds6 = new();
        public DataSet Ds7 = new();
        public DataSet Ds8 = new();
        public DataSet DsTarifa = new();
        public DataRow oDataRow;
        public DataRow oDataRow1;
        public DataRow oDataRow2;
        public DataRow oDataRow3;
        public DataRow oDataRow4;
        public DataRow oDataRow5;
        public DataRow oDataRow6;
        public DataRow oDataRow7;
        public DataRow oDataRow8;
        public DataRow oDataRowTarifa;


        /// <summary>
        /// Abre la conexión Oracle y retorna un OracleConnection.
        /// </summary>
        /// <returns></returns>
        public OracleConnection GetConnectionOracle(string NameConString)
        {
            try
            {
                if (Conexion.State == ConnectionState.Closed)
                {
                    string ConnectString = _configuration.GetConnectionString("Oracle");
                    Conexion.ConnectionString = ConnectString;
                    Conexion.Open();

                }

                return Conexion;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Cierra la conexión Oracle.
        /// </summary>
        /// <returns></returns>
        public void CloseConnectionOracle()
        {
            if (Conexion.State == ConnectionState.Open)
                Conexion.Close();
        }

        /// <summary>
        /// Devuelve una cadena que contiene un número especificado de caracteres a partir del lado izquierdo de una cadena.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(string param, int length)
        {
            string result = param.Substring(0, length);

            return result;
        }

        /// <summary>
        /// Funcion creada con base a VB para los if comprimidos. 
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="TruePart"></param>
        /// <param name="FalsePart"></param>
        /// <returns></returns>
        public string IIf(bool Expression, string TruePart, string FalsePart)
        {
            string ReturnValue = Expression == true ? TruePart : FalsePart;

            return ReturnValue;
        }

        /// <summary>
        /// Función creada con base a VB para retornar un booleano si la expresión se puede evaluar en númerico.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public Boolean IsNumeric(string valor)
        {
            return int.TryParse(valor, out int result);
        }

        /// <summary>
        /// Convierte una cadena a un formato dd/MM/yyyy. 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string Fecha(string fecha)
        {
            string _fecha = fecha.Substring(6, 2) + "/" + fecha.Substring(4, 2) + "/" + fecha.Substring(0, 4) + " " + fecha.Substring(8, 2) + ":" + fecha.Substring(10, 2) + ":" + fecha.Substring(12, 2);

            return _fecha;
        }

        /// <summary>
        /// Método 0 para ejecutrar un query y adaptarlo a un DataSet especificando la nombre de la tabla.
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Nombre_tabla"></param>
        /// <returns></returns>
        public bool QueryDataSet(string Query, string Nombre_tabla)
        {
            bool _return = false;

            if (Ds.Tables.Count != 0)
                Ds.Clear();

            using (OracleCommand Cmd = new(Query, Conexion))
            {
                using OracleDataAdapter Da = new(Cmd);
                Da.Fill(Ds, Nombre_tabla);
                try
                {
                    if (Ds.Tables[Nombre_tabla].Rows.Count > 0)
                    {
                        _return = true;
                        oDataRow = Ds.Tables[Nombre_tabla].Rows[0];
                    }
                    else
                        _return = false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _return = false;
                }

                finally
                {
                    Cmd.Dispose();
                }
            }

            return _return;
        }

        /// <summary>
        /// Método 1 para ejecutrar un query y adaptarlo a un DataSet especificando la nombre de la tabla.
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Nombre_tabla"></param>
        /// <returns></returns>
        public bool QueryDataSet1(string Query, string Nombre_tabla)
        {
            bool _return = false;

            if (Ds1.Tables.Count != 0)
                Ds1.Clear();

            using (OracleCommand Cmd = new(Query, Conexion))
            {
                using OracleDataAdapter Da = new(Cmd);
                Da.Fill(Ds1, Nombre_tabla);
                try
                {
                    if (Ds1.Tables[Nombre_tabla].Rows.Count > 0)
                    {
                        _return = true;
                        oDataRow1 = Ds1.Tables[Nombre_tabla].Rows[0];
                    }
                    else
                        _return = false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _return = false;
                }

                finally
                {
                    Cmd.Dispose();
                }
            }

            return _return;
        }

        /// <summary>
        /// Método 2 para ejecutrar un query y adaptarlo a un DataSet especificando la nombre de la tabla.
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Nombre_tabla"></param>
        /// <returns></returns>
        public bool QueryDataSet2(string Query, string Nombre_tabla)
        {
            bool _return = false;

            if (Ds2.Tables.Count != 0)
                Ds2.Clear();

            using (OracleCommand Cmd = new(Query, Conexion))
            {
                using OracleDataAdapter Da = new(Cmd);
                Da.Fill(Ds2, Nombre_tabla);
                try
                {
                    if (Ds2.Tables[Nombre_tabla].Rows.Count > 0)
                    {
                        _return = true;
                        oDataRow2 = Ds2.Tables[Nombre_tabla].Rows[0];
                    }
                    else
                        _return = false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _return = false;
                }
                finally
                {
                    Cmd.Dispose();
                }
            }

            return _return;
        }

        /// <summary>
        /// Método 3 para ejecutrar un query y adaptarlo a un DataSet especificando la nombre de la tabla.
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Nombre_tabla"></param>
        /// <returns></returns>
        public bool QueryDataSet3(string Query, string Nombre_tabla)
        {
            bool _return = false;

            if (Ds3.Tables.Count != 0)
                Ds3.Clear();

            using (OracleCommand Cmd = new(Query, Conexion))
            {
                using OracleDataAdapter Da = new(Cmd);
                Da.Fill(Ds3, Nombre_tabla);
                try
                {
                    if (Ds3.Tables[Nombre_tabla].Rows.Count > 0)
                    {
                        _return = true;
                        oDataRow3 = Ds3.Tables[Nombre_tabla].Rows[0];
                    }
                    else
                        _return = false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _return = false;
                }
                finally
                {
                    Cmd.Dispose();
                }
            }

            return _return;
        }

        /// <summary>
        /// Método 4 para ejecutrar un query y adaptarlo a un DataSet especificando la nombre de la tabla.
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Nombre_tabla"></param>
        /// <returns></returns>

        public bool QueryDataSet4(string Query, string Nombre_tabla)
        {
            bool _return = false;

            if (Ds4.Tables.Count != 0)
                Ds4.Clear();

            using (OracleCommand Cmd = new(Query, Conexion))
            {
                using OracleDataAdapter Da = new(Cmd);
                Da.Fill(Ds4, Nombre_tabla);
                try
                {
                    if (Ds4.Tables[Nombre_tabla].Rows.Count > 0)
                    {
                        _return = true;
                        oDataRow4 = Ds4.Tables[Nombre_tabla].Rows[0];
                    }
                    else
                        _return = false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _return = false;
                }
                finally
                {
                    Cmd.Dispose();
                }
            }

            return _return;
        }

        /// <summary>
        /// Método 5 para ejecutrar un query y adaptarlo a un DataSet especificando la nombre de la tabla.
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Nombre_tabla"></param>
        /// <returns></returns>
        public bool QueryDataSet5(string Query, string Nombre_tabla)
        {
            bool _return = false;

            if (Ds5.Tables.Count != 0)
                Ds5.Clear();

            using (OracleCommand Cmd = new(Query, Conexion))
            {
                using OracleDataAdapter Da = new(Cmd);
                Da.Fill(Ds5, Nombre_tabla);
                try
                {
                    if (Ds5.Tables[Nombre_tabla].Rows.Count > 0)
                    {
                        _return = true;
                        oDataRow5 = Ds5.Tables[Nombre_tabla].Rows[0];
                    }
                    else
                        _return = false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _return = false;
                }
                finally
                {
                    Cmd.Dispose();
                }
            }

            return _return;
        }


        /// <summary>
        /// Método 6 para ejecutrar un query y adaptarlo a un DataSet especificando la nombre de la tabla.
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Nombre_tabla"></param>
        /// <returns></returns>
        public bool QueryDataSet6(string Query, string Nombre_tabla)
        {
            bool _return = false;

            if (Ds6.Tables.Count != 0)
                Ds6.Clear();

            using (OracleCommand Cmd = new(Query, Conexion))
            {
                using OracleDataAdapter Da = new(Cmd);
                Da.Fill(Ds6, Nombre_tabla);
                try
                {
                    if (Ds6.Tables[Nombre_tabla].Rows.Count > 0)
                    {
                        _return = true;
                        oDataRow6 = Ds6.Tables[Nombre_tabla].Rows[0];
                    }
                    else
                        _return = false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _return = false;
                }
                finally
                {
                    Cmd.Dispose();
                }
            }

            return _return;
        }

        /// <summary>
        /// Método 7 para ejecutrar un query y adaptarlo a un DataSet especificando la nombre de la tabla.
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Nombre_tabla"></param>
        /// <returns></returns>
        public bool QueryDataSet7(string Query, string Nombre_tabla)
        {
            bool _return = false;

            if (Ds7.Tables.Count != 0)
                Ds7.Clear();

            using (OracleCommand Cmd = new(Query, Conexion))
            {
                using OracleDataAdapter Da = new(Cmd);
                Da.Fill(Ds7, Nombre_tabla);
                try
                {
                    if (Ds7.Tables[Nombre_tabla].Rows.Count > 0)
                    {
                        _return = true;
                        oDataRow7 = Ds7.Tables[Nombre_tabla].Rows[0];
                    }
                    else
                        _return = false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _return = false;
                }
                finally
                {
                    Cmd.Dispose();
                }
            }

            return _return;
        }

        /// <summary>
        /// Método 8 para ejecutrar un query y adaptarlo a un DataSet especificando la nombre de la tabla.
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Nombre_tabla"></param>
        /// <returns></returns>
        public bool QueryDataSet8(string Query, string Nombre_tabla)
        {
            bool _return = false;

            if (Ds8.Tables.Count != 0)
                Ds8.Clear();

            using (OracleCommand Cmd = new(Query, Conexion))
            {
                using OracleDataAdapter Da = new(Cmd);
                Da.Fill(Ds8, Nombre_tabla);
                try
                {
                    if (Ds8.Tables[Nombre_tabla].Rows.Count > 0)
                    {
                        _return = true;
                        oDataRow8 = Ds8.Tables[Nombre_tabla].Rows[0];
                    }
                    else
                        _return = false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _return = false;
                }
                finally
                {
                    Cmd.Dispose();
                }
            }

            return _return;
        }

        /// <summary>
        /// Método Tarifa para ejecutrar un query y adaptarlo a un DataSet especificando la nombre de la tabla.
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Nombre_tabla"></param>
        /// <returns></returns>
        public bool QueryDataSetTarifa(string Query, string Nombre_tabla)
        {
            bool _return = false;

            if (DsTarifa.Tables.Count != 0)
                DsTarifa.Clear();

            using (OracleCommand Cmd = new(Query, Conexion))
            {
                using OracleDataAdapter Da = new(Cmd);
                Da.Fill(DsTarifa, Nombre_tabla);
                try
                {
                    if (DsTarifa.Tables[Nombre_tabla].Rows.Count > 0)
                    {
                        _return = true;
                        oDataRowTarifa = DsTarifa.Tables[Nombre_tabla].Rows[0];
                    }
                    else
                        _return = false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _return = false;
                }
                finally
                {
                    Cmd.Dispose();
                }
            }

            return _return;
        }

        /// <summary>
        /// Agregar columna a un DataTable, dando formato 
        /// </summary>
        /// <param name="columna"></param>
        /// <param name="int_tipo"></param>
        /// <param name="Nombre_colum"></param>
        public DataColumn Agregar_datacolum(int int_tipo, string Nombre_colum)
        {
            DataColumn columna = new();

            switch (int_tipo)
            {
                case 1: //double
                    columna = new DataColumn
                    {
                        DataType = System.Type.GetType("System.Double"),
                        ColumnName = Nombre_colum,
                        ReadOnly = true,
                        Unique = false
                    };
                    break;
                case 2: //string
                    columna = new DataColumn
                    {
                        DataType = System.Type.GetType("System.String"),
                        ColumnName = Nombre_colum,
                        ReadOnly = true,
                        Unique = false
                    };
                    break;
                case 3: //date
                    columna = new DataColumn
                    {
                        DataType = System.Type.GetType("System.DateTime"),
                        ColumnName = Nombre_colum,
                        ReadOnly = true,
                        Unique = false
                    };
                    break;
                default:
                    break;
            }

            return columna;
        }

        /// <summary>
        /// asignacion_excedente_clase (Código viejo)
        /// </summary>
        /// <param name="id_clase"></param>
        /// <returns></returns>
        public string Asignacion_excedente_clase(int id_clase)
        {
            string rpt = string.Empty;
            switch (id_clase)
            {
                //10, 11, 17
                case int n when (n == 10 || n == 11 || n == 17):
                    rpt = "01";
                    break;
                //16, 18
                case int n when (n == 16 || n == 18):
                    rpt = "09";
                    break;
                default:
                    break;
            }

            return rpt;
        }
    }
}