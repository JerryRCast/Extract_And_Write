using Extract.Controller;
using Extract.Modell;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Extract
{
    class Init
    {
        static void Main(string[] args)
        {/*
            DateTime finalDate = new DateTime();
            string a = "DEL20180920";
            DateTime.TryParseExact(a.Remove(0, 3), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out finalDate);
            a = finalDate.ToString("dd/MM/yyyy");
            Console.WriteLine(a.Remove(0,3));
            Console.Read();
            */
            ConfigOps.ReadConfig();
            Console.WriteLine("Iniciando Procedimiento de Extracción...");
            Logger.WriteLog("\n\r" + DateTime.Now + "\n\rIniciando Procedimiento de Extracción...");

            try
            {
                List<ExtractedData> result1 = TSQL.Extract(FileType.Empresa001);
                /*List<ExtractedData> result1 = new List<ExtractedData>()
                {
                    new ExtractedData()
                    {
                        VIN = "TEST",Colour = "TEST", ControlError = "TEST", Customer = "TEST", Engine = "TEST",
                        Folio = "TEST", Pedimento = "TEST", PedimentoDate = "TEST", BillingDate = DateTime.Now 
                    },
                    new ExtractedData()
                    {
                        VIN = "TEST",Colour = "TEST", ControlError = "TEST", Customer = "TEST", Engine = "TEST",
                        Folio = "TEST", Pedimento = "TEST", PedimentoDate = "TEST", BillingDate = DateTime.Now
                    },
                    new ExtractedData()
                    {
                        VIN = "TEST",Colour = "TEST", ControlError = "TEST", Customer = "TEST", Engine = "TEST",
                        Folio = "TEST", Pedimento = "TEST", PedimentoDate = "TEST", BillingDate = DateTime.Now
                    }
                };*/
                result1 = Writer.TreatSpecialData(result1);
                if (result1.Count > 0)
                {
                    TSQL.UpdateSelectedRegs(FileType.Empresa001);
                    string path = Writer.CreateFile(result1, FileType.Empresa001);
                    Console.WriteLine("\n\rComenzando escritura de datos--------------->>>");
                    Logger.WriteLog("\n\rComenzando escritura de datos--------------->>>");
                    Writer.count = 0;
                    foreach (var item in result1)
                    {
                        Writer.WriteFile(item, path);
                    }
                    Logger.WriteLog("Se escribieron " + Writer.count + " registros en este documento");
                    Console.WriteLine("Escritura de datos finalizada");
                    Logger.WriteLog("Escritura de datos finalizada");
                }
                else
                {
                    Console.WriteLine("No hay datos de empresa 1 a escribir...");
                    Logger.WriteLog("No hay datos de empresa 1 a escribir...");
                }

                List<ExtractedData> result2 = TSQL.Extract(FileType.Empresa004);
                /*List<ExtractedData> result2 = new List<ExtractedData>()
                {
                    new ExtractedData()
                    {
                        VIN = "TEST",Colour = "TEST", ControlError = "TEST", Customer = "TEST", Engine = "TEST",
                        Folio = "TEST", Pedimento = "TEST", PedimentoDate = "TEST", BillingDate = DateTime.Now
                    },
                    new ExtractedData()
                    {
                        VIN = "TEST",Colour = "TEST", ControlError = "TEST", Customer = "TEST", Engine = "TEST",
                        Folio = "TEST", Pedimento = "TEST", PedimentoDate = "TEST", BillingDate = DateTime.Now
                    },
                    new ExtractedData()
                    {
                        VIN = "TEST",Colour = "TEST", ControlError = "TEST", Customer = "TEST", Engine = "TEST",
                        Folio = "TEST", Pedimento = "TEST", PedimentoDate = "TEST", BillingDate = DateTime.Now
                    }
                };*/
                result2 = Writer.TreatSpecialData(result2);
                if (result2.Count > 0)
                {
                    TSQL.UpdateSelectedRegs(FileType.Empresa004);
                    string path = Writer.CreateFile(result2, FileType.Empresa004);
                    Console.WriteLine("\n\rComenzando escritura de datos--------------->>>");
                    Logger.WriteLog("\n\rComenzando escritura de datos--------------->>>");
                    Writer.count = 0;
                    foreach (var item in result2)
                    {
                        Writer.WriteFile(item, path);
                    }
                    Logger.WriteLog("Se escribieron " + Writer.count + " registros en este documento");
                    Console.WriteLine("Escritura de datos finalizada");
                    Logger.WriteLog("Escritura de datos finalizada");
                }
                else
                {
                    Console.WriteLine("No hay datos de empresa 4 a escribir...");
                    Logger.WriteLog("No hay datos de empresa 4 a escribir...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error de ejecución: " + ex.Message);
            }
        }
    }
}
