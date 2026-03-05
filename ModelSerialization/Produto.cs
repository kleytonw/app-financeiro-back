using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Domain.ModelSerialization
{
    public class Produto
    {
        public string cProd { get; set; }
        public string cEAN { get; set; }
        public string xProd { get; set; }
        public string NCM { get; set; }
        public string CFOP { get; set; }
        public string uCom { get; set; }
        public decimal qCom { get; set; }
        public decimal vUnCom { get; set; }
    }
}
