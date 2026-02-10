using ERP.Domain.Entidades;
using ERP.Models;
using System;
using System.Text;


namespace ERP_API.Domain.Entidades
{
    public class Contrato : BaseModel
    {

        public int IdContrato { get; private set; }
        public Pessoa Cliente { get; private set; } 
        public int IdFinanceiro { get; private set; }
        public Financeiro Financeiro { get; private set; }
        public int? IdCliente { get; private set; }
        public int? IdVendedor { get; private set; }
        public Vendedor Vendedor { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataTermino { get; private set; }
        public decimal ValorMensalidade { get; private set; }
        public string LinkContrato { get; private set; }
        public DateTime DataPrimeiraMensalidade { get; private set; }
        public string Descricao { get; private set; }
        public int IdPlano { get; private set; }
        public Plano Plano { get; private set; }
        public decimal ValorAdesao { get; private set; }
        public DateTime? DataAdesao { get; private set; }
        public bool? ContratoAdesao { get; private set; }
        public decimal ValorTotal { get; private set; }
        public int? NumeroParcelas { get; private set; }
        public string ResponsavelNome { get; private set; }
        public string ResponsavelCpf { get; private set; }
        public string ResponsavelCargo { get; private set; }
        public string ResponsavelEmail { get; private set; }
        public string ResponsavelTelefone { get; private set; }
        public string ResponsavelCelular { get; private set; }


        public Contrato() { }

        public Contrato(Pessoa cliente, 
                        DateTime dataInicio, 
                        DateTime dataTermino,
                        decimal valorMensalidade, 
                        string linkContrato,
                        string descricao, 
                        Plano plano, 
                        decimal valorAdesao, 
                        DateTime? dataAdesao, 
                        bool? contratoAdesao, 
                        decimal valorTotal, 
                        Financeiro financeiro, 
                        int? numeroParcelas, 
                        DateTime dataPrimeiraMensalidade, 
                        Vendedor vendedor,
                        string responsavelNome,
                        string responsavelCpf,
                        string responsavelCargo,
                        string responsavelEmail,
                        string responsavelTelefone,
                        string responsavelCelular,
                        string usuarioInclusao)
        {
            Cliente = cliente;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
            ValorMensalidade = valorMensalidade;
            Descricao = descricao;
            LinkContrato = linkContrato;
            Plano = plano;
            ValorAdesao = valorAdesao;
            DataAdesao = dataAdesao;
            ContratoAdesao = contratoAdesao;
            ValorTotal = valorTotal;
            Financeiro = financeiro;
            NumeroParcelas = numeroParcelas;
            Vendedor = vendedor;
            DataPrimeiraMensalidade = dataPrimeiraMensalidade;
            ResponsavelNome = responsavelNome;
            ResponsavelCpf = responsavelCpf;
            ResponsavelCargo = responsavelCargo;
            ResponsavelEmail = responsavelEmail;
            ResponsavelTelefone = responsavelTelefone;
            ResponsavelCelular = responsavelCelular;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public Contrato(Pessoa cliente,
                DateTime dataInicio,
                DateTime dataTermino,
                decimal valorMensalidade,
                string descricao,
                Plano plano,
                decimal valorAdesao,
                DateTime? dataAdesao,
                bool? contratoAdesao,
                decimal valorTotal,
                Financeiro financeiro,
                string usuarioInclusao)
        {
            Cliente = cliente;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
            ValorMensalidade = valorMensalidade;
            Descricao = descricao;
            Plano = plano;
            ValorAdesao = valorAdesao;
            DataAdesao = dataAdesao;
            ContratoAdesao = contratoAdesao;
            ValorTotal = valorTotal;
            Financeiro = financeiro;
            DataPrimeiraMensalidade = DateTime.Now.Date.AddMonths(1);
            SetUsuarioInclusao(usuarioInclusao);
            ValidaAnonimo();
        }

        public void Alterar(Pessoa cliente, 
                            DateTime dataInicio, 
                            DateTime dataTermino, 
                            decimal valorMensalidade, 
                            string linkContrato, 
                            string descricao, 
                            Plano plano, 
                            decimal valorAdesao, 
                            DateTime? dataAdesao, 
                            bool? contratoAdesao, 
                            decimal valorTotal, 
                            Financeiro financeiro, 
                            int? numeroParcelas, 
                            DateTime dataPrimeiraMensalidade, 
                            Vendedor vendedor,
                            string responsavelNome,
                            string responsavelCpf,
                            string responsavelCargo,
                            string responsavelEmail,
                            string responsavelTelefone,
                            string responsavelCelular,
                            string usuarioAlteracao)
        {
            Cliente = cliente;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
            ValorMensalidade = valorMensalidade;
            Descricao = descricao;
            LinkContrato = linkContrato;
            Plano = plano;
            ValorAdesao = valorAdesao;
            DataAdesao = dataAdesao;
            ContratoAdesao = contratoAdesao;
            ValorTotal = valorTotal;
            Financeiro = financeiro;
            NumeroParcelas = numeroParcelas;
            Vendedor = vendedor;
            DataPrimeiraMensalidade = dataPrimeiraMensalidade;
            ResponsavelNome = responsavelNome;
            ResponsavelCpf = responsavelCpf;
            ResponsavelCargo = responsavelCargo;
            ResponsavelEmail = responsavelEmail;
            ResponsavelTelefone = responsavelTelefone;
            ResponsavelCelular = responsavelCelular;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            //if (IdEmpresa == null)
            //throw new Exception("Empresa é obrigatório");
            if (Cliente == null)
                throw new Exception("Cliente é obrigatório");
            if (DataInicio == default(DateTime))
                throw new Exception("A Data incorreta!");
            if (DataTermino == default(DateTime))
                throw new Exception("A Data incorreta!");
            if(Plano == null)
                throw new Exception("O plano é obrigatório");
            if (ValorAdesao == 0)
                throw new Exception("Valor de adesão é obrigatório");
            if (ContratoAdesao == null)
                throw new Exception("O contrato de adesão é obrigatório");
            if (NumeroParcelas == null || NumeroParcelas == 0)
                throw new Exception("Número de parcelas é obrigatório");
            if (DataPrimeiraMensalidade == null)
                throw new Exception("A data primeira mensalidade é obrigatória");
            if (string.IsNullOrEmpty(ResponsavelNome))
                throw new Exception("O nome do responsavél do contrato é obrigatório!");
            if (string.IsNullOrEmpty(ResponsavelCpf))
                throw new Exception("O CPF do responsavél do contrato é obrigatório!");
            if (string.IsNullOrEmpty(ResponsavelCargo))
                throw new Exception("O cargo do responsavél do contrato é obrigatório!");
            if (string.IsNullOrEmpty(ResponsavelEmail))
                throw new Exception("O email do responsavél do contrato é obrigatório!");
            if (string.IsNullOrEmpty(ResponsavelCelular))
                throw new Exception("O celular do responsavél do contrato é obrigatório!");

        }

        public void ValidaAnonimo()
        {
            if (Cliente == null)
                throw new Exception("Cliente é obrigatório");
            if (DataInicio == default(DateTime))
                throw new Exception("A Data incorreta!");
            if (DataTermino == default(DateTime))
                throw new Exception("A Data incorreta!");
            if (Plano == null)
                throw new Exception("O plano é obrigatório");
            if (ValorAdesao == 0)
                throw new Exception("Valor de adesão é obrigatório");
            if (ContratoAdesao == null)
                throw new Exception("O contrato de adesão é obrigatório");
            if (string.IsNullOrEmpty(Descricao))
                throw new Exception("Descrição é obrigatário");
        }
    }
}


