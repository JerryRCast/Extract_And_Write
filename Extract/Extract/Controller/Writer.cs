using Extract.Modell;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Extract.Controller
{
    class Writer
    {
        public static int consecutive { get; set; } 
        private static void MakeFile(string path, string date, string hour, int regs)
        {
            StreamWriter sw;

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
                sw.WriteLine(date + "|" + hour + "|" + regs + "|8185|" + consecutive);
                sw.Close();
            }
        }
        public static string CreateFile(List<ExtractedData> data, FileType op)
        {
            string path = "";
            int regs = data.Count;
            string date = DateTime.Now.ToString("yyyyMMdd");
            string hour = DateTime.Now.ToLongTimeString().Remove(8);

            switch (op)
            {
                case FileType.Empresa001:
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" +
                        date.Replace("/", "") + "_" + hour.Replace(":", "") + "_REPORTE_REPUVE_VEH.txt";
                    MakeFile(path, date, hour, regs);
                    break;
                case FileType.Empresa004:
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" +
                        date.Replace("/", "") + "_" + hour.Replace(":", "") + "_REPORTE_REPUVE_VAN.txt";
                    MakeFile(path, date, hour, regs);
                    break;
                default:
                    break;
            }
            return path;  
        } 

        public static void WriteFile(ExtractedData item, string path)
        {
            try
            {
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(item.VIN + "|" + item.Engine + "||" + item.Customer.Remove(0,3) + "|" + item.Colour + "|8185|" + item.Folio + "|" + 
                    item.BillingDate.ToShortDateString() + "|" + item.Pedimento.Remove(0,34) + "|" + 
                    TreatDate(item.PedimentoDate.Remove(0, 3)) + "||");
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\nPila de llamadas: " + ex.StackTrace);
            }
        }

        private static string TreatDate(string date)
        {
            // ^([12]\d{3}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01]))$ => "yyyyMMdd" => 20180904  'Este Regex Pattern permite buscar una cadena con formato para DateTime 
            DateTime finalDate = new DateTime();
            DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out finalDate);
            return finalDate.ToString("dd/MM/yyyy");
        }
    }
}
