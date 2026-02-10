using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ConciliacaoBancaria: BaseModel
    {
        public int IdConciliacaoBancaria { get; private set; }
        public Cliente Cliente { get; set; }
        public int IdCliente { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal? ValorConciliacao { get; set; }
        public decimal Valor { get; set; }
        public string Adquirente { get; set; }
        public bool? ConciliadoManual { get; private set; }
        public string Status { get; set; } 

        public ConciliacaoBancaria() { }

        public ConciliacaoBancaria(Cliente cliente, DateTime dataPagamento, decimal valor, string adquirente, string status, string usuarioInclusao)
        {
            Cliente = cliente;
            IdCliente = cliente.IdPessoa;
            DataPagamento = dataPagamento;
            Valor = valor;
            Adquirente = adquirente;
            Status = status;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Cliente cliente, DateTime dataPagamento, decimal valor, string adquirente, string status, string usuarioAlteracao)
        {
            Cliente = cliente;
            IdCliente = cliente.IdPessoa;
            DataPagamento = dataPagamento;
            Valor = valor;
            Adquirente = adquirente;
            Status = status;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void SetStatus(string status, string usuarioAlteracao)
        {
                
             Status = status;

            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void SettConciliadoManual(bool conciliado, string usuarioAlteracao)
        {
            if (conciliado)
            {
                Status = "Conciliado";
                ConciliadoManual = true;
            }
            else
            {
                Status = "Pendente";
                ConciliadoManual = false;
            }

            SetUsuarioAlteracao(usuarioAlteracao);
        }


        public void SetValorConciliacao(decimal valorConciliacao, string usuarioAlteracao)
        {
            this.ValorConciliacao = valorConciliacao;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Cliente == null)
                throw new Exception("Cliente é obrigatório");
            if (DateTime.MinValue.Equals(DataPagamento))
                throw new Exception("DataPagamento é obrigatório");
            if (Valor < 0)
                throw new Exception("Valor deve ser maior que zero");
            if (string.IsNullOrEmpty(Adquirente))
                throw new Exception("Adquirente é obrigatório");
            if (string.IsNullOrEmpty(Status))
                throw new Exception("Status é obrigatório");
        }
    }
}
