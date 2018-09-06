using Extract.Modell;
using System;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Xml.XPath;

namespace Extract.Controller
{
    class ConfigOps
    {
        public static ConfigurationAttribs.StandardSecurity config { get; set; }
        public static void UpdateConfig()
        {
            try
            {
                int newConsecutive = Writer.consecutive;
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Consecutive"].Value = Convert.ToString(newConsecutive);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar App.Config: " + ex.Message);
                Logger.WriteLog("Error al actualizar App.Config: " + ex.Message);
            }
            
        }

        public static void ReadConfig()
        {
            string[] attribs = new string[5];
            var appSettings = ConfigurationManager.AppSettings;
            int counter = 0;

            foreach (var key in appSettings.AllKeys)
            {
                attribs[counter] = appSettings[key];
                counter += 1;
            }

            ConfigurationAttribs.StandardSecurity config = new ConfigurationAttribs.StandardSecurity()
            {
                server = attribs[0],
                database = attribs[1],
                userName = attribs[2],
                password = attribs[3],
                consecutive = attribs[4]
            };

            Writer.consecutive = Convert.ToInt32(config.consecutive) + 1;
            ConfigOps.config = config;
        }
    }
}
