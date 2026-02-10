using ERP.Domain.Entidades;
using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class RegiaoConsultor : BaseModel
    {
        public int IdRegiaoConsultor { get; private set; }
        public Regiao Regiao { get; private set; }
        public int? IdRegiao { get; private set; }
        public Consultor Consultor { get; private set; }
        public int? IdPessoa { get; private set; }

        public RegiaoConsultor() { }

        public RegiaoConsultor(Regiao regiao, Consultor consultor, string usuarioInclusao)

        {

            Regiao = regiao;
            Consultor = consultor;
            SetUsuarioInclusao(usuarioInclusao);
        }

        public void Alterar(Regiao regiao, Consultor consultor, string usuarioAlteracao)
        {

            Regiao = regiao;
            Consultor = consultor;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);

        }

        public void Valida()
        {
            if (IdRegiao == null)
                throw new Exception("IdRegiao é obrigatório");
            if (IdPessoa == null)
                throw new Exception("IdConsultor é obrigatório");
        }
    }
}

