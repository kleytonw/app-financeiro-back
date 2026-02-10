using System.Collections.Generic;
using System;

namespace ERP_API.Service.Parceiros
{
    public class CobrancaRequest
    {
        public string nomeSacado { get; set; }
        public string bairroSacado { get; set; }
        public string boletoMensagem1 { get; set; }
        public string boletoMensagem2 { get; set; }
        public string boletoMensagem3 { get; set; }
        public string boletoMensagem4 { get; set; }
        public string cepSacado { get; set; }
        public string cidadeSacado { get; set; }
        public string cpfCnpjSacado { get; set; }
        public DateTime dataVencimento { get; set; }
        public string emailSacado { get; set; }
        public string enderecoSacado { get; set; }
        public string identificador { get; set; }
        public string numeroPedido { get; set; }
        public string numeroSacado { get; set; }
        public List<SplitValor> splitsValores { get; set; }
        public string telefoneSacado { get; set; }
        public decimal valor { get; set; }
        public decimal valorDesconto { get; set; }
        public decimal valorJuros { get; set; }
        public decimal valorMulta { get; set; }
    }

    public class SplitValor
    {
        public bool cobrarTarifa { get; set; }
        public string agencia { get; set; }
        public string conta { get; set; }
        public decimal valor { get; set; }
    }
}
