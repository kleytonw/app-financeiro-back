using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ClienteParametros : BaseModel
    {
        public int IdClienteParametros { get; private set; }
        public Cliente Cliente { get; private set; }
        public int IdCliente { get; private set; }
        public string Chave { get; private set; }
        public string Valor { get; private set; }

        public ClienteParametros() { }

        public ClienteParametros(Cliente cliente, string chave, string valor, string usuarioInclusao)
        {
            Cliente = cliente;
            Chave = chave;
            Valor = valor;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Cliente cliente, string chave, string valor, string usuarioAlteracao)
        {
            Cliente = cliente;
            Chave = chave;
            Valor = valor;
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
            if (string.IsNullOrEmpty(Chave))
                throw new Exception("A chave do parâmetro é obrigatória.");
            if (string.IsNullOrEmpty(Valor))
                throw new Exception("O valor do parâmetro é obrigatório.");
        }
    }
}
