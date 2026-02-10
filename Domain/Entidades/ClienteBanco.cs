using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{

    public class ClienteBanco : BaseModel
    {
        public int IdClienteBanco { get; private set; }
        public Cliente Cliente { get; private set; }
        public int IdCliente { get; private set; }
        public Banco Banco { get; private set; }
        public int IdBanco { get; private set; }

        public ClienteBanco() { }

        public ClienteBanco(Banco banco, Cliente cliente, string usuarioInclusao)
        {
            Banco = banco;
            Cliente = cliente;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Banco banco, Cliente cliente, string usuarioAlteracao)
        {
            Banco = banco;
            Cliente = cliente;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Banco == null)
                throw new Exception("O banco é obrigatório.");
            if (Cliente == null)
                throw new Exception("O cliente é obrigatório.");
        }
    }
}
