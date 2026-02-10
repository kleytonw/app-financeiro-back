using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ERP.Domain.ModelSerialization
{
    public class ProtNFe
    {
        [XmlElement(ElementName = "infProt")]
        public infProt InfProt { get; set; }

        public class infProt
        {
            [XmlElement("chNFe")]
            public string ChaveAcesso { get; set; }



        }
    }
}
