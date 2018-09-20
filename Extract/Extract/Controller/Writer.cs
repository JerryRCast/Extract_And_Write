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
        private static long modNum = 0; 
        private static void MakeFile(string path, string date, string hour, int regs)
        {
            modNum = modNum % 499;
            StreamWriter sw;

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
                sw.WriteLine(date + "|" + hour + "|" + regs + "|8185|" + modNum);
                sw.Close();
            }
            modNum = 0;
        }
        public static string CreateFile(List<ExtractedData> data, FileType op)
        {
            string path = "";
            int regs = data.Count;
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            string hour = DateTime.Now.TimeOfDay.ToString().Remove(8);

            switch (op)
            {
                case FileType.Empresa001:
                    path = ConfigOps.config.filePath + "\\" +
                        date.Replace("/", "") + "_" + hour.Replace(":", "") + "_REPORTE_REPUVE_VEH.txt";
                    MakeFile(path, date, hour, regs);
                    break;
                case FileType.Empresa004:
                    path = ConfigOps.config.filePath + "\\" +
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
                    item.PedimentoDate + "||");
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\nPila de llamadas: " + ex.StackTrace);
            }
        }

        public static List<ExtractedData> TreatSpecialData(List<ExtractedData> elements)
        {
            List<int> numData = new List<int>();
            DateTime finalDate = new DateTime();
            // ^([12]\d{3}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01]))$ => "yyyyMMdd" => 20180904  'Este Regex Pattern permite buscar una cadena con formato para DateTime 

            foreach (var item in elements)
            {
                // Tratamos el formato y longitud de la fecha
                DateTime.TryParseExact(item.PedimentoDate.Remove(0,3), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out finalDate);
                item.PedimentoDate = finalDate.ToString("dd/MM/yyyy");

                // Tratamos longitud de la descripción (COLOR)
                if (item.Colour.Length > 20)
                {
                    item.Colour = item.Colour.Substring(0, 20);
                }

                // Tratamos el dato VIN para obtener el 'pseudo-consecutivo'
                byte[] vinData = Encoding.ASCII.GetBytes(item.VIN);
                int localSum = 0;
                foreach (var vin in vinData)
                {
                    localSum = localSum + vin;
                }
                numData.Add(localSum);
            }

            foreach (var num in numData)
            {
                modNum = modNum + num;
            }
            return elements;
        }
    }
}
