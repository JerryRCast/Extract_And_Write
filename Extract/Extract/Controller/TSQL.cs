using Extract.Modell;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extract.Controller
{
    class TSQL
    {
        private static string ConnString { get; set; }

        private static SqlConnection CreateConnection()
        {
            ConfigurationAttribs.StandardSecurity config = ConfigOps.config;
            ConnString = "Server="+config.server+ ";Database=" + config.database + ";User Id=" + config.userName + ";Password=" + config.password + ";";
            SqlConnection conn = new SqlConnection(ConnString);
            return conn;
        }

        private static bool ValConn()
        {
            Console.WriteLine("Iniciando prueba de conexión a la BD.\n");
            Logger.WriteLog("Iniciando prueba de conexión a la BD.\n");
            SqlConnection conn = CreateConnection();
            bool flag = false;
            try
            {
                Console.WriteLine("Probando conexión...");
                Logger.WriteLog("Probando conexión...");
                conn.Open();
                Console.WriteLine("Conexión exitosa!!!");
                Logger.WriteLog("Conexión exitosa!!!");
                flag = true;
                conn.Close();
                Console.WriteLine("Cerrando conexión...");
                Logger.WriteLog("Cerrando conexión...");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error SQL: " + ex.Message + "\n" + ex.StackTrace);
                Logger.WriteLog("Error SQL: " + ex.Message + "\n" + ex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\n" + ex.StackTrace);
                Logger.WriteLog("Error: " + ex.Message + "\n" + ex.StackTrace);
            }
            return flag;
        }
        
        public static List<ExtractedData> Extract(FileType op)
        {
            List<ExtractedData> data = new List<ExtractedData>();
            bool flag = ValConn();

            if (flag)
            {
                try
                {
                    Console.WriteLine("Extrayendo datos...");
                    Logger.WriteLog("Extrayendo datos...");

                    if (op == FileType.Empresa001)
                    {
                        SqlCommand cmd = new SqlCommand("select a.referencia1, d.customfield14, b.cliente, d.customfield22, a.serie + a.folio, c.fechaTimbrado, " +
                        "d.customfield08, d.customfield09 from cfdicomprobante as a, CFDIReceptor as b, CFDiTimbre as c, CFDICustomFields as d " +
                        "where a.empresaId = '0000000001' and a.referencia4 <> 1 and a.empresaId = b.empresaId and a.empresaId = c.empresaId " +
                        "and a.empresaId = d.empresaId and a.noDocumento = b.noDocumento and a.noDocumento = c.noDocumento and " +
                        "a.noDocumento = d.noDocumento and facturaStatus in(" + ConfigOps.config.status + ")", CreateConnection());
                        data = ExecuteSQL(cmd);
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("select a.referencia1, d.customfield14, b.cliente, d.customfield22, a.serie+a.folio, c.fechaTimbrado, " +
                        "d.customfield08, d.customfield09 from cfdicomprobante as a, CFDIReceptor as b, CFDiTimbre as c, CFDICustomFields as d " +
                        "where a.empresaId = '0000000004' and a.referencia4 <> 1 and a.empresaId = b.empresaId and a.empresaId = c.empresaId " +
                        "and a.empresaId = d.empresaId and a.noDocumento = b.noDocumento and a.noDocumento = c.noDocumento and " +
                        "a.noDocumento = d.noDocumento and facturaStatus in(" + ConfigOps.config.status + ");", CreateConnection());
                        data = ExecuteSQL(cmd);
                    }  
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error SQL: " + ex.Message + "\n" + ex.StackTrace);
                    Logger.WriteLog("Error SQL: " + ex.Message + "\n" + ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + "\n" + ex.StackTrace);
                    Logger.WriteLog("Error: " + ex.Message + "\n" + ex.StackTrace);
                }
            }
            else data = null;
            return data;
        }

        private static List<ExtractedData> ExecuteSQL(SqlCommand cmd)
        {
            List<ExtractedData> data = new List<ExtractedData>();
            SqlDataReader reader;

            cmd.Connection.Open();
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    data.Add(new ExtractedData
                    {
                        VIN = (string)reader[0],
                        Engine = (string)reader[1],
                        Customer = (string)reader[2],
                        Colour = (string)reader[3],
                        Folio = (string)reader[4],
                        BillingDate = (DateTime)reader[5],
                        Pedimento = (string)reader[6],
                        PedimentoDate = (string)reader[7]
                    });
                }
            }
            else data.DefaultIfEmpty(new ExtractedData()
            {
                ControlError = "Empty" // El contenido del objeto puede omitirse
            });
            cmd.Connection.Close();
            return data;
        }

        public static void UpdateSelectedRegs(FileType op)
        {
            if (op == FileType.Empresa001)
            {
                SqlCommand cmd = new SqlCommand("update cfdicomprobante set referencia4 = '1' " +
                        "where referencia1 in(select distinct referencia1 from cfdicomprobante where empresaId like '0000000001' " +
                        "and referencia4 <> 1) and facturaStatus in(" + ConfigOps.config.status + ");", CreateConnection());
                ExecuteUpdate(cmd);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("update cfdicomprobante set referencia4 = '1' " +
                       "where referencia1 in(select distinct referencia1 from cfdicomprobante where empresaId like '0000000004' " +
                       "and referencia4 <> 1) and facturaStatus in(" + ConfigOps.config.status + ");", CreateConnection());
                ExecuteUpdate(cmd);
            }
        }

        private static void ExecuteUpdate(SqlCommand cmd)
        {
            try
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error SQL: " + ex.Message + "\n" + ex.StackTrace);
                Logger.WriteLog("Error SQL: " + ex.Message + "\n" + ex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\n" + ex.StackTrace);
                Logger.WriteLog("Error: " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
