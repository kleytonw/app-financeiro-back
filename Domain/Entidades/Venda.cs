using ERP.Domain.Entidades;
using ERP.Models;
using Org.BouncyCastle.Security;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Venda: BaseModel
    {
        public int IdVenda { get; private set; }
        public DateTime? DataVenda { get; private set; }
        public DateTime? DataPrevPagamento { get; private set; }
        public DateTime? DataPagamento { get; private set; }
        public string Cliente { get; private set; }
        public decimal? ValorBruto { get; private set; }
        public decimal? ValorDespesa {  get; private set; }
        public decimal? ValorLiquido { get; private set; }
        public decimal? ValorPagamento { get; private set; }
        public decimal? Taxa { get; private set; }
        public string MeioPagamento { get; private set; }
        public string NomeBandeira { get; private set; }
        public Bandeira Bandeira { get; private set; }
        public int? IdBandeira { get; private set; }
        public Operadora Operadora { get; private set; }
        public int? IdOperadora { get; private set; }
        public ContaBancaria ContaRecebimento { get; private set; }
        public int? IdContaRecebimento { get; private set; }
        public ContaBancaria ContaGravame { get; private set; }
        public int? IdContaGravame { get; private set; }
        public string NomeOperadora { get; private set; }
        public string Gravame { get; private set; }
        public string StatusConciliacao { get; private set; }
        public string StatusVenda { get; private set; }
        public int? IdUnidade { get; private set; }
        public Unidade Unidade { get; private set; }
        public Empresa Empresa { get; private set; }
        public int? IdEmpresa { get; private set; }
        public string Identificador { get; private set; }
        public string Produto { get; private set; }
        public string ProdutoCliente {  get; private set; }
        public string Modalidade { get; private set; }
        public string Autorizacao { get; private set; }
        public string Parcela {  get; private set; }
        public string Terminal {  get; private set; }
        public string Observacao { get; private set; }

        public bool? Diagnostico { get; private set; }

        public Venda() { }

        public Venda(DateTime? dataVenda,
                     DateTime? dataPrevPagamento,
                     DateTime? dataPagamento,
                     string cliente,
                     decimal? valorBruto,
                     decimal? valorDespesa,
                     decimal? valorLiquido,
                     decimal? valorPagamento,
                     decimal? taxa,
                     string meioPagamento,
                     Bandeira bandeira,
                     string nomeBandeira,
                     Operadora operadora,
                     ContaBancaria contaRecebimento,
                     ContaBancaria contaGravame,
                     string nomeOperadora,
                     string gravame,
                     string statusConciliacao,
                     string statusVenda,
                     Unidade unidade,
                     Empresa empresa,
                     string identificador, string produto, string produtoCliente, string modalidade, string autorizacao, string parcela, string terminal, string usuarioInclusao)
        {
            DataVenda = dataVenda;
            DataPrevPagamento = dataPrevPagamento;
            DataPagamento = dataPagamento;
            Cliente = cliente;
            ValorBruto = valorBruto;
            ValorDespesa = valorDespesa;
            ValorLiquido = valorLiquido;
            ValorPagamento = valorPagamento;
            Taxa = taxa;
            MeioPagamento = meioPagamento;
            Bandeira = bandeira;
            NomeBandeira = nomeBandeira;
            Operadora = operadora;
            ContaRecebimento = contaRecebimento;
            ContaGravame = contaGravame;
            NomeOperadora = nomeOperadora;
            Gravame = gravame;
            StatusConciliacao = statusConciliacao;
            StatusVenda = statusVenda;
            Unidade = unidade;
            Empresa = empresa;
            Identificador = identificador;
            Produto = produto;
            ProdutoCliente = produtoCliente;
            Modalidade = modalidade;
            Autorizacao = autorizacao;
            Parcela = parcela;
            Terminal = terminal;
            Diagnostico = false;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(DateTime? dataVenda,
                            DateTime? dataPrevPagamento,
                            DateTime? dataPagamento,
                            string cliente,
                            decimal? valorBruto,
                            decimal? valorDespesa,
                            decimal? valorLiquido,
                            decimal? valorPagamento,
                            decimal? taxa,
                            string meioPagamento,
                            Bandeira bandeira,
                            string nomeBandeira,
                            Operadora operadora,
                            ContaBancaria contaRecebimento,
                            ContaBancaria contaGravame,
                            string nomeOperadora,
                            string gravame,
                            string statusConciliacao,
                            string statusVenda,
                            Unidade unidade,
                            Empresa empresa,
                            string identificador, string produto, string produtoCliente, string modalidade, string autorizacao, string parcela, string terminal, string usuarioAlteracao)
        {
            DataVenda = dataVenda;
            DataPrevPagamento = dataPrevPagamento;
            DataPagamento = dataPagamento;
            Cliente = cliente;
            ValorBruto = valorBruto;
            ValorDespesa = valorDespesa;
            ValorLiquido = valorLiquido;
            ValorPagamento = valorPagamento;
            Taxa = taxa;
            MeioPagamento = meioPagamento;
            Bandeira = bandeira;
            NomeBandeira = nomeBandeira;
            Operadora = operadora;
            ContaRecebimento = contaRecebimento;
            ContaGravame = contaGravame;
            NomeOperadora = nomeOperadora;
            Gravame = gravame;
            StatusConciliacao = statusConciliacao;
            StatusVenda = statusVenda;
            Unidade = unidade;
            Empresa = empresa;
            Identificador = identificador;
            Produto = produto;
            ProdutoCliente = produtoCliente;
            Modalidade = modalidade;
            Autorizacao = autorizacao;
            Parcela = parcela;
            Terminal = terminal;
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
            if (DataVenda == null || DataVenda == DateTime.MinValue)
                throw new Exception("A data da venda é obrigatória!");
            if (string.IsNullOrEmpty(Cliente))
                throw new Exception("Cliente é obrigatório");
            if (ValorBruto == null)
                throw new Exception("O valor bruto é obrigatório!");
            if (string.IsNullOrEmpty(MeioPagamento))
                throw new Exception("O meio de pagamento é obrigatório");
            if (Bandeira == null)
                throw new Exception("A bandeira é obrigatória");
            if (string.IsNullOrEmpty(NomeBandeira))
                throw new Exception("O nome da bandeira é obrigatória");
            if (string.IsNullOrEmpty(Identificador))
                throw new Exception("O identificador é obrigatório!");
            if (Operadora == null)
                throw new Exception("A operadora é obrigatória");
            if (string.IsNullOrEmpty(NomeOperadora))
                throw new Exception("O nome da operadora é obrigatório!");
            if (string.IsNullOrEmpty(StatusConciliacao))
                throw new Exception("O status da conciliação é obrigatório!");
            if (string.IsNullOrEmpty(StatusVenda))
                throw new Exception("O status da venda é obrigatório");
            if (Unidade == null)
                throw new Exception("A unidade é obrigatoria");
            if (Empresa == null)
                throw new Exception("Empresa é obrigatória");
        }
    }
}


