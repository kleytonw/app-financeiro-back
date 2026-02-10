using ERP.Models;
using System;
using System.Collections.Generic;

namespace ERP_API.Domain.Entidades
{
    public class Cliente : BaseModel
    {
        public int IdPessoa { get; private set; }
        public Pessoa Pessoa { get; private set; }
        public Afiliado Afiliado { get; private set; }
        public int? IdAfiliado { get; private set; }
        public string? IdentificadorConciliadora { get; private set; }
        public string? Senha {  get; private set; }
        public string? SenhaConciliadora { get; private set; }
        public string? ApiKeyConciliadora { get; private set; }
        public string? NomeResponsavel {  get; private set; }
        public string? CelularResponsavel { get; private set; }
        public string? EmailResponsavel { get; private set; }
        public string? NomeContratante { get; private set; }
        public string? CelularContratante { get; private set; }
        public string? EmailContratante { get; private set; }
        public int? IdColaborador { get; private set; }
        public Colaborador Colaborador { get; private set; }
        public string Status { get; private set; }

        public int? IdSacadoUnique { get; private set; }


        public List<ClienteERP> ERPs { get; set; }

        public Cliente() { }

        public Cliente(Pessoa pessoa, string senha, string nomeResponsavel, string celularResponsavel, string emailResponsavel,
            string nomeContratante, string celularContratante, string emailContratante, Colaborador colaborador, Afiliado afiliado, string usuarioInclusao)
        {
            Pessoa = pessoa;
            Senha = senha;
            NomeResponsavel = nomeResponsavel;
            CelularResponsavel = celularResponsavel;
            EmailResponsavel = emailResponsavel;
            NomeContratante = nomeContratante;
            CelularContratante = celularContratante;
            EmailContratante = emailContratante;
            Colaborador = colaborador;
            Afiliado = afiliado;
            SetUsuarioInclusao(usuarioInclusao);
        }

        public Cliente(Pessoa pessoa, Afiliado afiliado)
        {
            Pessoa = pessoa;
            Status = "Em análise";
            Afiliado = afiliado;
            SetUsuarioInclusao("Anônimo");
            ValidaClienteAnonimo();
        }

        public void Alterar(Pessoa pessoa, string senha, string nomeResponsavel, string celularResponsavel, string emailResponsavel,
            string nomeContratante, string celularContratante, string emailContratante, Colaborador colaborador, Afiliado afiliado, string usuarioAlteracao)
        {
            Pessoa = pessoa;
            Senha = senha;
            NomeResponsavel = nomeResponsavel;
            CelularResponsavel = celularResponsavel;
            EmailResponsavel = emailResponsavel;
            NomeContratante = nomeContratante;
            CelularContratante = celularContratante;
            EmailContratante = emailContratante;
            Colaborador = colaborador;
            Afiliado = afiliado;
            SetUsuarioInclusao(usuarioAlteracao);
            Valida();
        }

        public void AdicionarDadosConciliadora(string? identificadorConciliadora, string? senhaConciliadora, string? apiKeyConciliadora, string usuarioAlteracao)
        {
            IdentificadorConciliadora = identificadorConciliadora;
            SenhaConciliadora = senhaConciliadora;
            ApiKeyConciliadora = apiKeyConciliadora;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void SalvarSenha(string senha, string usuarioAlteraca)
        {
            Senha = senha;
            SetUsuarioAlteracao(usuarioAlteraca);
        }

        public void Ativar(string usuarioAlteracao)
        {
            Status = "Ativo";
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Inativar(string usuarioAlteracao)
        {
            Status = "Inativo";
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Valida()
        {
            if (Pessoa == null)
                throw new Exception("Pessoa é obrigatório");
            if(string.IsNullOrEmpty(Senha))
                throw new Exception("Senha é obrigatório");
        }

        public void ValidaClienteAnonimo()
        {
            if (Pessoa == null)
                throw new Exception("Pessoa é obrigatória");
        }
    }
}
