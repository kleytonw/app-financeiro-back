using Newtonsoft.Json;
using System.Collections.Generic;

namespace ERP_API.Service.Parceiros
{
    public class ConsultaPagamentoPagBankResponseModel
    {
        public List<VendaDetalheModel> Detalhes { get; set; }

        public PaginacaoModel Pagination { get; set; }
    }

    public class VendaDetalheModel
    {
        public string Movimento_api_codigo { get; set; }
        public string Tipo_registro { get; set; }
        public string Estabelecimento { get; set; }
        public string Data_inicial_transacao { get; set; }
        public string Hora_inicial_transacao { get; set; }
        public string Data_venda_ajuste { get; set; }
        public string Hora_venda_ajuste { get; set; }
        public string Tipo_evento { get; set; }
        public string Tipo_transacao { get; set; }
        public string Codigo_transacao { get; set; }
        public string Codigo_venda { get; set; }
        public decimal Valor_total_transacao { get; set; }
        public decimal Valor_parcela { get; set; }
        public string Pagamento_prazo { get; set; }
        public string Plano { get; set; }
        public string Parcela { get; set; }
        public string Quantidade_parcelas { get; set; }
        public string Data_movimentacao { get; set; }
        public decimal Taxa_parcela_comprador { get; set; }
        public decimal Valor_original_transacao { get; set; }
        public decimal Taxa_intermediacao { get; set; }
        public decimal Tarifa_intermediacao { get; set; }
        public decimal Valor_liquido_transacao { get; set; }
        public decimal Taxa_antecipacao { get; set; }
        public decimal Valor_liquido_antecipacao { get; set; }
        public string Status_pagamento { get; set; }
        public string Meio_pagamento { get; set; }
        public string Instituicao_financeira { get; set; }
        public string Canal_entrada { get; set; }
        public string Leitor { get; set; }
        public string Meio_captura { get; set; }
        public string Nsu { get; set; }
        public string Cartao_bin { get; set; }
        public string Cartao_holder { get; set; }
        public string Codigo_autorizacao { get; set; }
        public string Data_prevista_pagamento { get; set; }
        public string Tid { get; set; }
        public string Codigo_ur { get; set; }
        public string Arranjo_ur { get; set; }
    }

    public class PaginacaoModel
    {
        public int Elements { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int TotalElements { get; set; }
    }

}
