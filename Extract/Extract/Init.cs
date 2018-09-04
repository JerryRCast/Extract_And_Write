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
        {
            ConfigOps.ReadConfig();
            Console.WriteLine("Iniciando Procedimiento de Extracción...");
            List<ExtractedData> result1 = TSQL.Extract(FileType.Empresa001);

            if (result1.Count > 0)
            {
                TSQL.UpdateSelectedRegs(FileType.Empresa001);
                string path = Writer.CreateFile(result1, FileType.Empresa001);
                Console.WriteLine("Comenzando escritura de datos");
                foreach (var item in result1)
                {
                    Writer.WriteFile(item, path);
                }
                Console.WriteLine("Escritura de datos finalizada");
            }
            else Console.WriteLine("No hay datos de empresa 1 a escribir...");

            List<ExtractedData> result2 = TSQL.Extract(FileType.Empresa004);
            if (result2.Count > 0)
            {
                TSQL.UpdateSelectedRegs(FileType.Empresa004);
                string path = Writer.CreateFile(result2, FileType.Empresa004);
                Console.WriteLine("Comenzando escritura de datos");
                foreach (var item in result2)
                {
                    Writer.WriteFile(item, path);
                }
                Console.WriteLine("Escritura de datos finalizada");
            }
            else Console.WriteLine("No hay datos de empresa 2 a escribir...");

            if (result1.Count > 0 || result2.Count > 0)
            {
                ConfigOps.UpdateConfig();
            }
        }
    }
}
