using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ERP.Domain
{
    
    public class Transporte
    {
        [XmlElement("modFrete")]
        public string modFrete { get; set; }

        [XmlElement("transporta")]
        public Transportadora Transportadora { get; set; }

        [XmlElement("vol")]
        public Volume Volume { get; set; }

    }

    public class Transportadora
    {
        public string CNPJ { get; set; }
        public string xNome { get; set; }
        public string IE { get; set; }
        public string xMun { get; set; }
        public string UF { get; set; }
    }

    public class Volume
    {
        public decimal qVol { get; set; }
        public string esp { get; set; }
        public double pesoL { get; set; }
        public double pesoB { get; set; }
    }
}
