using ERP.Domain;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class Usuario : BaseModel
    {
        public int IdUsuario { get; set; }
        public int? IdEmpresa { get; set; }
        public Cliente Cliente { get; set; }
        public int? IdPessoa { get; set; }
        public Consultor Consultor { get; set; }
        public Pessoa Pessoa { get; set; }
        public Afiliado? Afiliado { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; } 

        public string TipoUsuario { get; set; }

        public string Status { get; set; }

        public string PrimeiroAcesso { get; set; }

        public ERPs ERP { get; set; }
        public int? IdERPs { get; set; }

        public Usuario() { }

        public Usuario(
            string login,
            Cliente? cliente,
            Consultor? consultor,
            Afiliado? afiliado,
            string nome,
            string email,
            string tipoUsuario,
            string senha,
            string usuarioInclusao
            )
        {
            this.Login = ValidaDomain.ValidaCampoNulo("Login",login);
            this.Cliente = cliente;
            this.Consultor = consultor;
            this.Afiliado = afiliado;
            this.Nome = ValidaDomain.ValidaCampoNulo("Nome", nome);
            this.Email = ValidaDomain.ValidaEmail(email);
            this.TipoUsuario = ValidaDomain.ValidaCampoNulo("Tipo Usuário", tipoUsuario);
            SetUsuarioInclusao(usuarioInclusao);
            
            this.Senha = senha;
            this.PrimeiroAcesso = "S";
        }

        public void CriarUsuarioEmpresaAprovada(
            string login,
            string nome,
            string email,
            string tipoUsuario,
            Empresa empresa,
            string usuarioInclusao)
        {
            this.Login = ValidaDomain.ValidaCampoNulo("Login", login);
            this.Email = ValidaDomain.ValidaEmail(email);
            this.TipoUsuario = ValidaDomain.ValidaCampoNulo("Tipo Usuário", tipoUsuario);
            SetUsuarioInclusao(usuarioInclusao);

            this.Senha = "123456";
            this.PrimeiroAcesso = "S";
        } 
        
        public void Alterar(string login, Cliente? cliente, Consultor? consultor, Afiliado afiliado, string nome, string email, string tipoUsuario, string usuarioAlteracao)
        {
            Nome = nome;
            Cliente = cliente;
            Consultor = consultor; 
            TipoUsuario = tipoUsuario;
            Afiliado = afiliado;
            Login = login;
            Email = email;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void AlterarSenha(string senha)
        {
            this.Senha = senha;

            if (string.IsNullOrEmpty(senha))
                throw new Exception("Senha obrigatório ");

            /* HashPassword passwordHasher = new HashPassword(SHA512.Create());
            string hashSenha = passwordHasher.CriptografarSenha(senha);
            this.Senha = hashSenha; */
        }

        public void ResetarSenha(string usuarioAlteracao)
        {
            this.Senha = "123456";
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void GerarTokenRecuperacaoSenha()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";  // abcdefghijklmnopqrstuvwxyz
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = chars[random.Next(chars.Length)];

            // this.TokenRecuperacaoSenha = new String(stringChars);
        }

        public void GerarTokenCriacaoSenha()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";  // abcdefghijklmnopqrstuvwxyz
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = chars[random.Next(chars.Length)];

            // this.TokenRecuperacaoSenha = new String(stringChars);
        }

        public void Inativar(string usuarioAlteracao)
        {
            this.Situacao = "Inativo";
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Ativar(string usuarioAlteracao)
        {
            this.Situacao = "Ativo";
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Reativar(string usuarioAlteracao)
        {
            this.Situacao = "Ativo";
            this.PrimeiroAcesso = "N";
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        internal void Excluir(string usuarioExclusao)
        {
            this.Situacao = "Excluir";
            SetUsuarioExclusao(usuarioExclusao);
        }

         public void Valida()
        {
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("Nome é obrigatório");
            if (string.IsNullOrEmpty(TipoUsuario))
                throw new Exception("Tipo de Usuário é obrigatório");
            if (string.IsNullOrEmpty(Login))
                throw new Exception("Login é obrigatório");
            if (string.IsNullOrEmpty(Email))
                throw new Exception("Email é obrigatório");
        } 
    }
}
