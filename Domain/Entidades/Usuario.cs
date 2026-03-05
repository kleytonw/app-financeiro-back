using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Usuario : BaseModel
    {
        public int IdUsuario { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public string Telefone { get; private set; }
        public string Foto { get; private set; }
        public string GoogleId { get; private set; }
        public bool EmailConfirmado { get; private set; }

        public Usuario() { }

        public Usuario(string nome, string email, string senha, string telefone, string usuarioInclusao)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            Telefone = telefone;
            EmailConfirmado = false;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public static Usuario CriarComGoogle(string nome, string email, string googleId, string foto)
        {
            var usuario = new Usuario
            {
                Nome = nome,
                Email = email,
                GoogleId = googleId,
                Foto = foto,
                EmailConfirmado = true
            };
            usuario.SetUsuarioInclusao(email);
            return usuario;
        }

        public void Alterar(string nome, string telefone, string foto, string usuarioAlteracao)
        {
            Nome = nome;
            Telefone = telefone;
            Foto = foto;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void AlterarSenha(string novaSenha, string usuarioAlteracao)
        {
            Senha = novaSenha;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void VincularGoogle(string googleId, string usuarioAlteracao)
        {
            GoogleId = googleId;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void ConfirmarEmail(string usuarioAlteracao)
        {
            EmailConfirmado = true;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("Nome é obrigatório");

            if (string.IsNullOrEmpty(Email))
                throw new Exception("Email é obrigatório");

            if (string.IsNullOrEmpty(GoogleId) && string.IsNullOrEmpty(Senha))
                throw new Exception("Senha é obrigatória para cadastro sem Google");
        }
    }
}
