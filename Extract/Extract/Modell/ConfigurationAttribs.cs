using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Extract.Modell
{
    public class ConfigurationAttribs
    {
        public class StandardSecurity
        {
            public string server { get; set; }
            public string database { get; set; }
            public string userName { get; set; }
            public string password { get; set; }
            public string consecutive { get; set; }
        }
    }
}
