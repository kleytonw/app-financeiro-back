using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class UsuarioCliente : BaseModel
    {
        public int IdUsuarioCliente {  get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }

        public UsuarioCliente() { }

        public UsuarioCliente(Cliente cliente, Usuario usuario, string usuarioInclusao)
        {
            Cliente = cliente;
            Usuario = usuario;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Cliente cliente, Usuario usuario, string usuarioAlteracao)
        {
            Cliente = cliente;
            Usuario = usuario;
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
                throw new Exception("O cliente é obrigatório!");
            if (Usuario == null)
                throw new Exception("O usuário é obrigatório!");
        }
    }
}
