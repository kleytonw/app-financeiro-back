using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ClasseRecebimento: BaseModel
    {
        public int IdClasseRecebimento { get; set; }    
        public string Descricao { get; set; }

        public ClasseRecebimento() { }

        public ClasseRecebimento(string descricao, string usuarioInclusao)
        {
            Descricao = descricao;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string descricao, string usuarioAlteracao)
        {
            Descricao = descricao;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioAlteracao)
        {
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Valida()
        {
            if (string.IsNullOrEmpty(Descricao))
                throw new Exception("Descrição é obrigatório");
        }
    }
}
