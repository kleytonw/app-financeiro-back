using ERP.Models;
using Hangfire.Storage.Monitoring;
using System;
using System.Drawing;

namespace ERP_API.Domain.Entidades
{
    public class ControleCartaVan: BaseModel
    {
        public int IdControleCartaVan { get; private set; }
        public int IdCliente { get; private set; }
        public Cliente Cliente { get; private set; }
        public int IdClienteContaBancaria { get; private set; }
        public ClienteContaBancaria ClienteContaBancaria { get; private set; }
        public int IdEtapa {  get; private set; }
        public Etapa Etapa { get; private set; }
        public string Status {  get; private set; }
        public string TicketFornecedor { get; private set; }
        public string Descricao { get; private set; }

        ControleCartaVan() { }

        public ControleCartaVan(Cliente cliente, ClienteContaBancaria clienteContaBancaria, Etapa etapa, string ticketFornecedor, string descricao, string usuarioInclusao)
        {
            Cliente = cliente;
            ClienteContaBancaria = clienteContaBancaria;
            Etapa = etapa;
            TicketFornecedor = ticketFornecedor;
            Descricao = descricao;

            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Cliente cliente, ClienteContaBancaria clienteContaBancaria, Etapa etapa, string ticketFornecedor, string descricao, string usuarioAlteracao)
        {
            Cliente = cliente;
            ClienteContaBancaria = clienteContaBancaria;
            Etapa = etapa;
            TicketFornecedor = ticketFornecedor;
            Descricao = descricao;

            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void AlterarEtapa(Etapa etapa)
        {
            Etapa = etapa;
            if (Etapa.EtapaConcluida == true)
                Status = "Concluido";
            else
                Status = "Em andamento";

        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida() 
        {
            if (Cliente == null)
                throw new Exception("Cliente é obrigatório!");
            if (ClienteContaBancaria == null)
                throw new Exception("Conta Bancaria é obrigatória!");


        }


    }
}
