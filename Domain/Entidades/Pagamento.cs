using ERP.Domain.Entidades;
using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Pagamento : BaseModel
    {
        public int IdPagamento { get; private set; }
        public Empresa Empresa { get; private set; }
        public int IdEmpresa { get; private set; }
        public Operadora Operadora { get; private set; }
        public int IdOperadora { get; private set; }
        public Unidade Unidade { get; private set; }
        public int IdUnidade { get; private set; }
        public Diagnostico Diagnostico { get; private set; }
        public int? IdDiagnostico { get; private set; }
        public DateTime? DataPagamento { get; private set; }
        public int? IdBanco { get; private set; }
        public Banco Banco { get; private set; }
        public string NomeBanco { get; private set; }
        public string CodigoBanco { get; private set; }
        public string Agencia { get; private set; }
        public string Conta { get; private set; }
        public int IdBandeira { get; private set; }
        public Bandeira Bandeira { get; private set; }
        public string NomeBandeira { get; private set; }
        public string CodigoBandeira { get; private set; }
        public string RazaoSocial { get; private set; } 
        public decimal? ValorPagamento { get; private set; }
        public string StatusPagamento { get; private set; }
        public bool StatusConciliado { get; private set; }
        public string TipoPagamento { get; private set; }

        public Pagamento() { }

        public Pagamento(Operadora operadora,
                         Empresa empresa,
                         Unidade unidade,  
                         DateTime? dataPagamento, 
                         Banco banco,
                         string codigoBanco,
                         string nomeBanco,
                         string agencia, 
                         string conta, 
                         Bandeira bandeira,
                         string codigoBandeira,
                         string nomeBandeira,
                         string razaoSocial, decimal? valorPagamento, string statusPagamento, string tipoPagamento, string usuarioInclusao)
        {
            Operadora = operadora;
            Empresa = empresa;
            Unidade = unidade;
            DataPagamento = dataPagamento;
            Banco = banco;
            CodigoBanco = codigoBanco;
            NomeBanco = nomeBanco;
            Agencia = agencia;
            Conta = conta;
            Bandeira = bandeira;
            CodigoBandeira = codigoBandeira;
            NomeBandeira = nomeBandeira;
            RazaoSocial = razaoSocial;
            ValorPagamento = valorPagamento;
            StatusPagamento = statusPagamento;
            TipoPagamento = tipoPagamento;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Operadora operadora,
                         Empresa empresa,
                         Unidade unidade,
                         DateTime? dataPagamento,
                         Banco banco,
                         string codigoBanco,
                         string nomeBanco,
                         string agencia,
                         string conta,
                         Bandeira bandeira,
                         string codigoBandeira,
                         string nomeBandeira,
                         string razaoSocial, decimal? valorPagamento, string statusPagamento, string tipoPagamento, string usuarioAlteracao)
        {
            Operadora = operadora;
            Empresa = empresa;
            Unidade = unidade;
            DataPagamento = dataPagamento;
            Banco = banco;
            CodigoBanco = codigoBanco;
            NomeBanco = nomeBanco;
            Agencia = agencia;
            Conta = conta;
            Bandeira = bandeira;
            CodigoBandeira = codigoBandeira;
            NomeBandeira = nomeBandeira;
            RazaoSocial = razaoSocial;
            ValorPagamento = valorPagamento;
            StatusPagamento = statusPagamento;
            TipoPagamento = tipoPagamento;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)

        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Conciliar(bool concilar )
        {
            StatusConciliado = concilar;
        }

        public void SetIdDiagnostico(int idDiagnostico)
        {

            IdDiagnostico = idDiagnostico;
        }

        public void Valida()
        {
            if (Operadora == null)
                throw new Exception("Operadora é obrigatória");
            if (Unidade == null)
                throw new Exception("Unidade é obrigatória");
            if (DataPagamento == default(DateTime) || DataPagamento == DateTime.MinValue)
                throw new Exception("Data do pagamento é obrigatória");
            if (Banco == null)
                throw new Exception("Banco é obrigatório");
            if (string.IsNullOrEmpty(CodigoBanco))
                throw new Exception("Código do banco é obrigatório");
            if (string.IsNullOrEmpty(NomeBanco))
                throw new Exception("Nome do banco é obrigatório");
            if (string.IsNullOrEmpty(Agencia))
                throw new Exception("Agência é obrigatória");
            if (string.IsNullOrEmpty(Conta))
                throw new Exception("Conta é obrigatória");
            if (Bandeira == null)
                throw new Exception("Bandeira é obrigatória");
            if(string.IsNullOrEmpty(CodigoBandeira))
                throw new Exception("Código da bandeira é obrigatório");
            if (string.IsNullOrEmpty(NomeBandeira))
                throw new Exception("Nome da bandeira é obrigatório");
            if (string.IsNullOrEmpty(RazaoSocial))
                throw new Exception("Razão Social é obrigatória");
            if (ValorPagamento == null || ValorPagamento == 0)
                throw new Exception("Valor do pagamento é obrigatório");
            if (string.IsNullOrEmpty(StatusPagamento))
                throw new Exception("Status do pagamento é obrigatório");
            if (string.IsNullOrEmpty(TipoPagamento))
                throw new Exception("Tipo do pagamento é obrigatório");
        }
    }
}
