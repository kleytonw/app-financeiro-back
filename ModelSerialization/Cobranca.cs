using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ERP.Domain.ModelSerialization
{
    public class Cobranca
    {
        [XmlElement("fat")]
        public Fatura Fatura { get; set;  }

        [XmlElement("dup")]
        public List<Duplicata> Duplicata { get; set; }
    }

    public class Fatura
    {
        public string nFat { get; set; }
        public double vOrig { get; set; }
        public double vDesc { get; set; }
        public double vLiq { get; set; }
    }

    public class Duplicata
    {
        public string nDup   { get; set; }
        public DateTime dVenc { get; set; }
        public double vDup { get; set; }
    }
}
