using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extract.Controller
{
    class Logger
    {
        public static void WriteLog(string strn)
        {
            string path = ConfigOps.config.logPath + "\\Log_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
            StreamWriter sw;

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
                sw.WriteLine("Log de Ejecución: \r\n");
                sw.Close();
            }

            sw = File.AppendText(path);
            sw.WriteLine(strn);
            sw.Close();
        }
    }
}
