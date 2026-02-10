using ERP.Models;
using Org.BouncyCastle.Asn1.Mozilla;
using System;

namespace ERP_API.Domain.Entidades
{
    public class VendasConciliadas_bkp : BaseModel
    {
        public int IdVendasConciliadas { get; private set; }
        public string IdentificadorConciliadora { get; private set; }
        public DateTime DataInicial { get; private set; }
        public DateTime DataFinal { get; private set; }
        public string Versao { get; private set; }
        public int? Lote { get; private set; }
        public string NomeSistema { get; private set; }
        public int Produto { get; private set; }
        public string DescricaoTipoProduto { get; private set; }
        public string CodigoAutorizacao { get; private set; }
        public string IdentificadorPagamento { get; private set; }
        public DateTime DataVenda { get; private set; }
        public DateTime? DataVencimento { get; private set; }
        public decimal ValorVendaParcela { get; private set; }
        public decimal ValorLiquidoParcela { get; private set; }
        public decimal TotalVenda { get; private set; }
        public decimal? Taxa { get; private set; }
        public int Parcela { get; private set; }
        public int TotalParcelas { get; private set; }
        public decimal? ValorBrutoMoeda { get; private set; }
        public decimal? ValorLiquidoMoeda { get; private set; }
        public decimal? CotacaoMoeda { get; private set; }
        public string Moeda { get; private set; }
        public string NSU { get; private set; }
        public string TID { get; private set; }
        public string Terminal { get; private set; }
        public string MeioCaptura { get; private set; }
        public int Operadora { get; private set; }
        public string Modalidade { get; private set; }
        public string Status { get; private set; }
        public string Observacao { get; private set; }
        public decimal? ValorBrutoConciliadora { get; private set; }

        public VendasConciliadas_bkp()
        {
        }

        public VendasConciliadas_bkp(string identificaodorConciliadora,
                                 DateTime dataInicial,
                                 DateTime dataFinal,
                                 string versao,
                                 int? lote,
                                 string nomeSistema,
                                 int produto,
                                 string descricaoTipoProduto,
                                 string codigoAutorizacao,
                                 string identificadorPagamento,
                                 DateTime dataVenda,
                                 DateTime? dataVencimento,
                                 decimal valorVendaParcela,
                                 decimal valorLiquidoParcela,
                                 decimal totalVenda,
                                 decimal? taxa,
                                 int parcela,
                                 int totalParcelas,
                                 decimal? valorBrutoMoeda,
                                 decimal? valorLiquidoMoeda,
                                 decimal? cotacaoMoeda,
                                 string moeda,
                                 string nSU,
                                 string tID,
                                 string terminal,
                                 string meioCaptura,
                                 int operadora,
                                 string modalidade,
                                 string status,
                                 string usuarioInclusao)
        {
            IdentificadorConciliadora = identificaodorConciliadora;
            DataInicial = dataInicial;
            DataFinal = dataFinal;
            Versao = versao;
            Lote = lote;
            NomeSistema = nomeSistema;
            Produto = produto;
            DescricaoTipoProduto = descricaoTipoProduto;
            CodigoAutorizacao = codigoAutorizacao;
            IdentificadorPagamento = identificadorPagamento;
            DataVenda = dataVenda;
            DataVencimento = dataVencimento;
            ValorVendaParcela = valorVendaParcela;
            ValorLiquidoParcela = valorLiquidoParcela;
            TotalVenda = totalVenda;
            Taxa = taxa;
            Parcela = parcela;
            TotalParcelas = totalParcelas;
            ValorBrutoMoeda = valorBrutoMoeda;
            ValorLiquidoMoeda = valorLiquidoMoeda;
            CotacaoMoeda = cotacaoMoeda;
            Moeda = moeda;
            NSU = nSU;
            TID = tID;
            Terminal = terminal;
            MeioCaptura = meioCaptura;
            Operadora = operadora;
            Modalidade = modalidade;
            Status = status;
            SetUsuarioInclusao(usuarioInclusao);
            this.Status = "Pendente";
            Validar();
        }

        public void Validar()
        {
            if(string.IsNullOrEmpty(IdentificadorConciliadora))
                throw new Exception("Identificador da Conciliadora é obrigatório.");
            if (DataInicial == default)
                throw new Exception("Data Inicial é obrigatória.");
            if (DataFinal == default)
                throw new Exception("Data Final é obrigatória.");
            if (string.IsNullOrEmpty(Versao))
                throw new Exception("Versão é obrigatória.");
            if (string.IsNullOrEmpty(NomeSistema))
                throw new Exception("Nome do Sistema é obrigatório.");
            if (Produto <= 0)
                throw new Exception("Produto é obrigatório.");
            if (string.IsNullOrEmpty(CodigoAutorizacao))
                throw new Exception("Código de Autorização é obrigatória.");
            if (DataVenda == default)
                throw new Exception("Data da Venda é obrigatória.");
            if (ValorVendaParcela <= 0)
                throw new Exception("Valor da Venda por Parcela é obrigatório.");
            if (TotalVenda <= 0)
                throw new Exception("Total da Venda é obrigatório.");
            if (Parcela < 0)
                throw new Exception("Número da Parcela é obrigatório.");
            if (TotalParcelas < 0)
                throw new Exception("Total de Parcelas é obrigatório.");
            if (string.IsNullOrEmpty(NSU))
                throw new Exception("NSU é obrigatório.");
            if (string.IsNullOrEmpty(MeioCaptura))
                throw new Exception("Meio de Captura é obrigatório.");
        }

        public void Conciliar(string nsu, decimal valor, int adquirente)
        {
            this.ValorBrutoConciliadora = valor;
            if(this.ValorVendaParcela != valor)
            {
                this.Observacao = "Valor diferente";
                this.Status = "Não Conciliada";
            }
            if(this.Operadora != adquirente)
            {
                this.Observacao += "Adquirente diferente";
                this.Status = "Não Conciliada";
            }
            if(ValorVendaParcela == valor && Operadora == adquirente)
            {
                this.DataAlteracao = DateTime.Now;
                this.Status = "Conciliada";
            }
        }

    }
}
