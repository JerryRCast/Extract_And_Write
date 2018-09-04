using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extract.Modell
{
    class ExtractedData
    {
        public string VIN { get; set; }
        public string Engine { get; set; }
        public string Customer { get; set; }
        public string Colour { get; set; }
        public string Folio { get; set; }
        public DateTime BillingDate { get; set; }
        public string Pedimento { get; set; }
        public string PedimentoDate { get; set; }
        public string ControlError { get; set; }
    }
}
