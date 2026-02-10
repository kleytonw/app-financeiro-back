using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ClienteERP : BaseModel
    {
        public int IdClienteERP { get; private set; }
        public  Cliente Cliente { get; private set; }
        public int IdCliente { get; private set; }
        public ERPs ERPs { get; private set; }
        public int IdERPs { get; private set; }

        public ClienteERP() { }

        public ClienteERP(Cliente cliente, ERPs erps, string usuarioInclusao)
        {
            Cliente = cliente;
            ERPs = erps;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Cliente cliente, ERPs erps, string usuarioAlteracao)
        {
            Cliente = cliente;
            ERPs = erps;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Cliente == null)
                throw new Exception("O cliente é obrigatório.");
            if (ERPs == null)
                throw new Exception("O ERP é obrigatório.");
        }
    }
}
