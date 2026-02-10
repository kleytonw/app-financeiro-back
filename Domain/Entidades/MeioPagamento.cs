using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class MeioPagamento : BaseModel
    {

        public int IdMeioPagamento { get; private set; }
        public string NomeMeioPagamento { get; private set; }


        public MeioPagamento() { }

        public MeioPagamento(string nomeMeioPagamento, string usuarioInclusao)
        {
            NomeMeioPagamento = nomeMeioPagamento;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nomeMeioPagamento, string usuarioAlteracao)
        {
            NomeMeioPagamento = nomeMeioPagamento;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
            Valida();
        }

        public void Valida()
        {
            if (string.IsNullOrEmpty(NomeMeioPagamento))
                throw new Exception("O nome é obrigatório");
        }
    }
}



