using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Org.BouncyCastle.Bcpg;
using System;

namespace ERP_API.Models
{
    public class PesquisarRequestModel
    {
        public string Chave { get;  set; }
        public string Valor { get;  set; }
        public int IdUnidade { get; set; }
        public int IdOperadora { get; set; }
        public int IdMeioPagamento { get; set; }
        public int? IdMensagem { get; set; }
        public int? IdEmpresa { get; set; }
        public string? Nome { get; set; }
        public string? ChaveAcesso { get; set; }
        public DateTime DataMovimentacaoLog { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataTermino { get; set; }

    }
}
