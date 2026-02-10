using ERP.Models;
using System;

namespace ERP.Domain.Entidades
{
    public class MovimentacaoFatura : BaseModel
    {
        public MovimentacaoFatura() { }
        
        public int IdMovimentacaoFatura { get; set; }
        public int IdMovimentacao { get; set; }
        public Movimentacao Movimentacao { get; set; }

        public decimal? ValorOriginal { get; set; }
        public decimal? ValorDesconto { get; set; }
        public decimal? ValorLiquido { get; set; }

        public MovimentacaoFatura(decimal? valorOriginal, decimal? valorDesconto, decimal? valorLiquido)
        {
            ValorOriginal = valorOriginal;
            ValorDesconto = valorDesconto;
            ValorLiquido = valorLiquido;

            SetUsuarioInclusao("admin");
        }
    }

    public class MovimentacaoDuplicata : BaseModel
    {
        public MovimentacaoDuplicata() { }
        public MovimentacaoDuplicata(string numeroDuplicata, DateTime? dataVencimento, double valorDuplicata)
        {
            NumeroDuplicata = numeroDuplicata;

            if(dataVencimento!= null)
            {
                DataVencimento = dataVencimento;
            }
            ValorDuplicata = valorDuplicata;

            SetUsuarioInclusao("admin");
        }

        public int IdMovimentacaoDuplicata { get; set; }
        public int IdMovimentacao { get; set; }
        public Movimentacao Movimentacao { get; set; }

        public string NumeroDuplicata { get; set; }
        public DateTime? DataVencimento { get; set; }
        public double ValorDuplicata { get; set; }

    }
}
