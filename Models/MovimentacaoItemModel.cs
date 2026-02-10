using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using ERP.Domain.Entidades;
using Microsoft.Identity.Client;

namespace ERP.Models
{

    public class MovimentacaoItemResponse
    {
        public int IdMovimentacaoItem { get; set; }
        public int IdMovimentacao { get; set; }
        public Movimentacao Movimentacao { get; set; }
        public string CodigoProd { get; set; }
        public string CodigoEAN { get; set; }
        public string NomeProduto { get; set; }
        public string NCM { get; set; }
        public string CFOP { get; set; }
        public ICollection<MovimentacaoItem> Itens { get; set; }

        public string Unidade { get; set; }
        public double Quantidade { get; set; }
        public double ValorUnitario { get; set; }

        public double SubTotal { get; set; }

    }

    public class MoivmentacaoItemRequest
    {
        public int IdMovimentacaoItem { get; set; }
        public Movimentacao Movimentacao { get; set; }
        public int IdMovimentacao { get; set; }
        public string CodigoProd { get; set; }
        public string CodigoEAN { get; set; }
        public string NomeProduto { get; set; }
        public string NCM { get; set; }
        public string CFOP { get; set; }
        public ICollection<MovimentacaoItem> Itens { get; set; }
     

        public string Unidade { get; set; }
        public double Quantidade { get; set; }
        public double ValorUnitario { get; set; }

        public double SubTotal { get; set; }
    }
}
