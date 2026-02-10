using ERP.Domain.Entidades;
using ERP.Models;
using Org.BouncyCastle.Security;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ContatoSite : BaseModel
    {
        public int IdContatoSite { get; private set; }
        public string NomeContato { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public string Titulo { get; private set; }
        public Empresa Empresa { get; private set; }
        public int? IdEmpresa { get; private set; } 
        public string Mensagem { get; private set; }
        public DateTime Data { get; private set; }

        public ContatoSite() { }

        public ContatoSite(string nomeContato, string telefone, string email, string titulo, Empresa empresa, string mensagem, DateTime data, string usuarioInclusao)
        {
            NomeContato = nomeContato;
            Telefone = telefone;
            Email = email;
            Titulo = titulo;
            Empresa = empresa;
            Mensagem = mensagem;
            Data = data;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nomeContato, string telefone, string email, string titulo, Empresa empresa, string mensagem, DateTime data, string usuarioAlteracao)
        {
            NomeContato = nomeContato;
            Telefone = telefone;
            Email = email;
            Titulo = titulo;
            Mensagem = mensagem;
            Data = data;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (string.IsNullOrEmpty(NomeContato))
                throw new Exception("Nome é obrigatório");
            if (string.IsNullOrEmpty(Telefone))
                throw new Exception("Telefone é obrigatório");
            if (string.IsNullOrEmpty(Email))
                throw new Exception("Email é obrigatório");
            if (string.IsNullOrEmpty(Titulo))
                throw new Exception("Título é obrigatório");
            if (Empresa == null)
                throw new Exception("A Empresa é obrigatório");
            if (string.IsNullOrEmpty(Mensagem))
                throw new Exception("Mensagem é obrigatória");
            if (Data == default(DateTime))
                throw new Exception("Data inválida");
        }
    }
}

