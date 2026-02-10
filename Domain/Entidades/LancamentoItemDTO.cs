using System;

namespace ERP_API.Domain.Entidades
{

public class LancamentoItemDTO
{
        public int? IdLancamentoItem { get; set; }
        public int IdProduto { get; set; }
        public string NomeProduto { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public decimal Subtotal { get; set; }
    }
}
