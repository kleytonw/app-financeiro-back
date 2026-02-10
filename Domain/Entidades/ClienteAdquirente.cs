using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ClienteAdquirente : BaseModel
    {
        public int IdClienteAdquirente { get; private set; }
        public Operadora Operadora { get; private set; }
        public int IdOperadora { get; private set; }
        public Cliente Cliente { get; private set; }
        public int IdCliente { get; private set; }

        public ClienteAdquirente() { }

        public ClienteAdquirente(Operadora operadora, Cliente cliente, string usuarioInclusao)
        {
            Operadora = operadora;
            Cliente = cliente;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Operadora operadora, Cliente cliente, string usuarioAlteracao)
        {
            Operadora = operadora;
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
            if (Operadora == null)
                throw new Exception("A operadora é obrigatório.");
            if (Cliente == null)
                throw new Exception("O cliente é obrigatório.");
        }
    }
}
