using ERP.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ERP.Domain.Entidades
{
    public class MovimentacaoItem : BaseModel
    {

        public int IdMovimentacaoItem { get; set; }
        public Movimentacao Movimentacao { get; set; }
        public int IdMovimentacao { get; set; }
        public string CodigoProd { get; set; }
        public string CodigoEAN { get; set; }
        public string NomeProduto { get; set; }
        public string NCM { get; set; }
        public string CFOP { get; set; }

        public string Unidade { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public decimal SubTotal { get;set; }

        public MovimentacaoItem()
        {

        }

        public MovimentacaoItem( string codigoProd, string codigoEAN, string nomeProduto, 
            string nCM, string cFOP, string unidade, decimal quantidade, decimal valorUnitario)
        {
            CodigoProd = codigoProd;
            CodigoEAN = codigoEAN;
            NomeProduto = nomeProduto;
            NCM = nCM;
            CFOP = cFOP;
            Unidade = unidade;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            SubTotal = Quantidade * ValorUnitario;

            SetUsuarioInclusao("nfe");
        }
    }
}
