using ERP.Domain.Entidades;
using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ClienteContato : BaseModel
    {
        public int IdClienteContato { get; private set; }
        public Cliente Cliente { get; private set; }
        public int IdCliente { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public string Cargo { get; private set; }
        public string Observacao { get; private set; }
        public ClienteContato() { }
        public ClienteContato(Cliente cliente, string nome, DateTime dataNascimento, string email, string telefone, string cargo, string observacao, string usuarioInclusao)
        {
            Cliente = cliente;
            Nome = nome;
            DataNascimento = dataNascimento;
            Email = email;
            Telefone = telefone;
            Cargo = cargo;
            Observacao = observacao;
            SetUsuarioInclusao(usuarioInclusao);
        }

        public void Alterar(Cliente cliente, string nome, DateTime dataNascimento, string email, string telefone, string cargo, string observacao, string usuarioAlteracao)
        {
            Cliente = cliente;
            Nome = nome;
            DataNascimento = dataNascimento;
            Email = email;
            Telefone = telefone;
            Cargo = cargo;
            Observacao = observacao;
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
                throw new Exception("Cliente é obrigatório");
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("Nome é obrigatório");
            if (DataNascimento == DateTime.MinValue)
                throw new Exception("Data de Nascimento é obrigatório");
            if (string.IsNullOrEmpty(Email))
                throw new Exception("Email é obrigatório");
            if (string.IsNullOrEmpty(Telefone))
                throw new Exception("Telefone é obrigatório");
            if (string.IsNullOrEmpty(Cargo))
                throw new Exception("Cargo é obrigatório");
        }
    }

}

