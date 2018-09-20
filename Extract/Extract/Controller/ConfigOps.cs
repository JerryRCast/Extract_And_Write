using Extract.Modell;
using System;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Xml.XPath;
using System.Collections.Specialized;

namespace Extract.Controller
{
    class ConfigOps
    {
        public static ConfigurationAttribs.StandardSecurity config { get; set; }

        public static void ReadConfig()
        {
            string[] attribs = new string[8];
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
                //consecutive = attribs[4],
                logPath = attribs[4],
                filePath = attribs[5],
                status = attribs[6]
            };
            ConfigOps.config = config;
        }
    }
}
