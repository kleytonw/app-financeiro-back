using ERP.Domain.Entidades;
using ERP.Models;
using Org.BouncyCastle.Security;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Transacao : BaseModel
    {
        public int IdTransacao { get; private set; }
        public int? IdUnidade { get; private set; }
        public Unidade Unidade { get; private set; }
        public string Cliente { get; private set; }
        public Empresa Empresa { get; private set; }
        public int? IdEmpresa { get; private set; }
        public decimal? ValorBruto { get; private set; }
        public decimal? Taxa { get; private set; }
        public decimal? Despesa { get; private set; }
        public decimal? ValorLiquido { get; private set; }
        public DateTime? DataVenda { get; private set; }
        public DateTime? DataMovimentacao { get; private set; }
        public string MeioPagamento { get; private set; }
        public string Bandeira { get; private set; }
        public Operadora Operadora { get; private set; }
        public int? IdOperadora { get; private set; }
        public string NomeOperadora { get; private set; }
        public string StatusConciliacao { get; private set; }
        public decimal? ValorPagoConciliacao { get; private set; }
        public decimal? ValorTarifaConciliacao { get; private set; }
        public string Observacao { get; private set; }
        public string Identificador { get; private set; }
        public int? QuantidadeParcela { get; private set; }
        public long? NumeroVenda { get; private set; }
        public string StatusTransacao {  get; private set; }
        public string Terminal {  get; private set; }
        public string ChargebackStatus { get; private set; }
        public int? TipoCaptura { get; private set; }
        public bool? Flex { get; private set; }
        public decimal? FlexAmount { get; private set; }
        public decimal? ValorTotalTaxa { get; private set; }
        public decimal? ValorEmbarqueTransacao { get; private set; }
        public bool? Tokenizado { get; private set; }
        public string Tid {  get; private set; }
        public string NumeroDoPedido { get; private set; }
        public string NumeroCartao { get; private set; }


        public string Produto { get; set; }
        public string DescricaoProduto { get; set; }

        public string TipoProduto { get; set;  }
        public string CodigoProduto { get; set; }

        public bool? Diagnostico { get; set;  }

        public Transacao() { }


        public Transacao(string cliente,
                         Unidade unidade,
                         Empresa empresa,
                         decimal? valorBruto,
                         decimal? taxa,
                         decimal? despesa,
                         decimal? valorLiquido,
                         DateTime? dataVenda,
                         DateTime? dataMovimentacao,
                         string meioPagamento,
                         string bandeira,
                         Operadora operadora,
                         string nomeOperadaora,
                         string statusConciliacao,
                         string identificador,
                         int? quantidadeParcela,
                         long? numeroVenda,
                         string stastusTransacao,
                         string terminal,
                         string chargebackStatus,
                         int? tipoCaptura,
                         bool? flex,
                         decimal? flexAmount,
                         decimal? valorTotalTaxa,
                         decimal? valorEmbarqueTransacao,
                         bool? tokenizado,
                         string tid,
                         string numerodoDoPedido,
                         string numeroCartao,
                         string produto,
                         string tipoProduto,
                         string codigoProduto,
                         string usuarioInclusao, string descricaoProduto)
        {
            Cliente = cliente;
            Unidade = unidade;
            Empresa = empresa;
            ValorBruto = valorBruto;
            Taxa = taxa;
            Despesa = despesa;
            ValorLiquido = valorLiquido;
            DataVenda = dataVenda;
            DataMovimentacao = dataMovimentacao;
            MeioPagamento = meioPagamento;
            Bandeira = bandeira;
            Operadora = operadora;
            NomeOperadora = nomeOperadaora;
            StatusConciliacao = statusConciliacao;
            Identificador = identificador;
            QuantidadeParcela = quantidadeParcela;
            NumeroVenda = numeroVenda;
            StatusTransacao = stastusTransacao;
            Terminal = terminal;
            ChargebackStatus = chargebackStatus;
            TipoCaptura = tipoCaptura;
            Flex = flex;
            FlexAmount = flexAmount;
            ValorTotalTaxa = valorTotalTaxa;
            ValorEmbarqueTransacao = valorEmbarqueTransacao;
            Tokenizado = tokenizado;
            Tid = tid;
            NumeroDoPedido = numerodoDoPedido;
            NumeroCartao = numeroCartao;
            SetUsuarioInclusao(usuarioInclusao);

            this.Produto = produto;
            this.CodigoProduto = codigoProduto;
            this.DescricaoProduto = descricaoProduto;
            Diagnostico = false;

            Valida();
        }



        // construtor importação REDE 
        public Transacao(string cliente,
                         Unidade unidade, 
                         Empresa empresa, 
                         decimal? valorBruto, 
                         decimal? taxa, 
                         decimal? despesa, 
                         decimal? valorLiquido, 
                         DateTime? dataVenda, 
                         DateTime? dataMovimentacao, 
                         string meioPagamento, 
                         string bandeira, 
                         Operadora operadora, 
                         string nomeOperadaora,
                         string statusConciliacao, 
                         string identificador,
                         int? quantidadeParcela, 
                         long? numeroVenda, 
                         string stastusTransacao, 
                         string terminal, 
                         string chargebackStatus, 
                         int? tipoCaptura, 
                         bool? flex, 
                         decimal? flexAmount, 
                         decimal? valorTotalTaxa,
                         decimal? valorEmbarqueTransacao,
                         bool? tokenizado, 
                         string tid, 
                         string numerodoDoPedido,
                         string numeroCartao, 
                         string produto,
                         string tipoProduto,
                         string codigoProduto,
                         string usuarioInclusao)
        {
            Cliente = cliente;
            Unidade = unidade;
            Empresa = empresa;
            ValorBruto = valorBruto;
            Taxa = taxa;
            Despesa = despesa;
            ValorLiquido = valorLiquido;
            DataVenda = dataVenda;
            DataMovimentacao = dataMovimentacao;
            MeioPagamento = meioPagamento;
            Bandeira = bandeira;
            Operadora = operadora;
            NomeOperadora = nomeOperadaora;
            StatusConciliacao = statusConciliacao;
            Identificador = identificador;
            QuantidadeParcela = quantidadeParcela;
            NumeroVenda = numeroVenda;
            StatusTransacao = stastusTransacao;
            Terminal = terminal;
            ChargebackStatus = chargebackStatus;
            TipoCaptura = tipoCaptura;
            Flex = flex;
            FlexAmount = flexAmount;
            ValorTotalTaxa = valorTotalTaxa;
            ValorEmbarqueTransacao = valorEmbarqueTransacao;
            Tokenizado = tokenizado;
            Tid = tid;
            NumeroDoPedido = numerodoDoPedido;
            NumeroCartao = numeroCartao;
            SetUsuarioInclusao(usuarioInclusao);

            this.Produto = produto;
            this.CodigoProduto = codigoProduto;
            this.DescricaoProduto = GetPaymentDescriptionRede(produto);
            Diagnostico = false;

            Valida();
        }

        public static string GetPaymentDescriptionRede(string paymentType)
        {
            return paymentType switch
            {
                "NO_INSTALLMENTS_DEBIT" => "DÉBITO À VISTA",
                "NO_INSTALLMENTS" => "CRÉDITO À VISTA",
                "IN_INSTALLMENTS_WITH_INTEREST" => "PARCELADO COM JUROS",
                "IN_INSTALLMENTS_NO_INTEREST" => "PARCELADO SEM JUROS",
                "CREDIT_PLAN" => "CREDIÁRIO",
                "PRE_DATED" => "PRE DATADO",
                "FUEL" => "COMBUSTÍVEL",
                "FOOD" => "ALIMENTAÇÃO",
                "AWARD" => "PREMIAÇÃO",
                "PRIVATE_LABEL" => "PRIVATE LABEL",
                "COVENANT" => "CONVÊNIO",
                "MEAL" => "REFEIÇÃO",
                "PREMIUM" => "PREMIUM",
                "MULT_BENEFITS" => "MULTI BENEFÍCIO",
                "DRUGSTORE" => "FARMÁCIA",
                "FLEET" => "GESTÃO DE FROTA",
                "CULTURE" => "CULTURA",
                "AUTOMOBILE" => "AUTO",
                "GIFT" => "GIFT",
                "VOUCHER" => "VOUCHER",
                "NO_INSTALLMENTS_PIX" => "PIX NÃO PARCELADO",
                "OTHERS" => "OUTROS",
                "TOTAL_HATES" => "Taxas cobradas MDR + FLEX",
                "TOTALS" => "Totais",
                "TOTAL_RECEIVABLES" => "Valores totalizados de recebimentos",
                "TOTAL_ADJUSTMENTS_AND_CHARGES" => "Valores totalizados de ajustes e cobranças",
                "IN_INSTALLMENTS_2_6" => "Valores totalizados de crédito de 2 a 6 parcelas",
                "IN_INSTALLMENTS_7_12" => "Valores totalizados de crédito de 7 a 12 parcelas",
                "IN_INSTALLMENTS_13_21" => "Valores totalizados de crédito de 13 a 21 parcelas",
                "RATES_NO_INSTALLMENTS_DEBIT" => "Taxas totalizadas para débito à vista",
                "RATES_NO_INSTALLMENTS" => "Taxas totalizadas para crédito à vista",
                "RATES_IN_INSTALLMENTS_2_6" => "Taxas totalizadas para crédito parcelado de 2 a 6",
                "RATES_IN_INSTALLMENTS_7_12" => "Taxas totalizadas para crédito parcelado de 7 a 12",
                "RATES_IN_INSTALLMENTS_13_21" => "Taxas totalizadas para crédito parcelado de 13 a 21",
                "RATES_IN_INSTALLMENTS_WITH_INTEREST" => "Taxas totalizadas parcelado com juros",
                "RATES_CREDIT_PLAN" => "Taxas totalizadas para crediário",
                "RATES_VOUCHER" => "Taxas totalizadas para vouchers",
                "RATES_OTHERS" => "Taxas totalizadas para outras modalidades",
                _ => "Tipo desconhecido"
            };
        }

        public void Alterar(string cliente, 
                            Unidade unidade, 
                            Empresa empresa, 
                            decimal? valorBruto, 
                            decimal? taxa, 
                            decimal? despesa, 
                            decimal? valorLiquido,
                            DateTime? dataVenda,
                            DateTime? dataMovimentacao,
                            string meioPagamento, 
                            string bandeira, 
                            Operadora operadora, 
                            string nomeOperadora, 
                            string statusConciliacao, 
                            decimal? valorPagoConciliacao, 
                            decimal? valorTarifaConciliacao, 
                            string observacao, 
                            string identificador,
                             int? quantidadeParcela,
                             long? numeroVenda,
                             string stastusTransacao,
                             string terminal,
                             string chargebackStatus,
                             int? tipoCaptura,
                             bool? flex,
                             decimal? flexAmount,
                             decimal? valorTotalTaxa,
                             decimal? valorEmbarqueTransacao,
                             bool? tokenizado,
                             string tid,
                             string numerodoDoPedido,
                             string numeroCartao, string usuarioAlteracao)
        {
            Cliente = cliente;
            Unidade = unidade;
            Empresa = empresa;
            ValorBruto = valorBruto;
            Taxa = taxa;
            Despesa = despesa;
            ValorLiquido = valorLiquido;
            DataVenda = dataVenda;
            DataMovimentacao = dataMovimentacao;
            MeioPagamento = meioPagamento;
            Bandeira = bandeira;
            Operadora = operadora;
            NomeOperadora = nomeOperadora;
            StatusConciliacao = statusConciliacao;
            ValorPagoConciliacao = valorPagoConciliacao;
            ValorTarifaConciliacao = valorTarifaConciliacao;
            Observacao = observacao;
            Identificador = identificador;
            QuantidadeParcela = quantidadeParcela;
            NumeroVenda = numeroVenda;
            StatusTransacao = stastusTransacao;
            Terminal = terminal;
            ChargebackStatus = chargebackStatus;
            TipoCaptura = tipoCaptura;
            Flex = flex;
            FlexAmount = flexAmount;
            ValorTotalTaxa = valorTotalTaxa;
            ValorEmbarqueTransacao = valorEmbarqueTransacao;
            Tokenizado = tokenizado;
            Tid = tid;
            NumeroDoPedido = numerodoDoPedido;
            NumeroCartao = numeroCartao;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void SetConciliacao(string statusConciliacao, string observacao)
        {
            StatusConciliacao = statusConciliacao;
            Observacao = observacao;
        }

        public void Valida()
        {
            if (Unidade == null)
                throw new Exception("A unidade é obrigatoria");
            if (string.IsNullOrEmpty(Cliente))
                throw new Exception("Cliente é obrigatório");
            if (Empresa == null)
                throw new Exception("Empresa é obrigatória");
            if (string.IsNullOrEmpty(MeioPagamento))
                throw new Exception("O meio de pagamento é obrigatória");
            if (string.IsNullOrEmpty(Bandeira))
                throw new Exception("A bandeira é obrigatória");
            if (string.IsNullOrEmpty(StatusConciliacao))
                throw new Exception("O status da conciliação é obrigatória");
            if (string.IsNullOrEmpty(Identificador))
                throw new Exception("O identificador é obrigatória");
            if (Operadora == null)
                throw new Exception("A operadora é obrigatória");
            if (string.IsNullOrEmpty(NomeOperadora))
                throw new Exception("O nome da operadora é obrigatório");
        }
    }
}


