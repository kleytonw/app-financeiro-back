using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ERPs: BaseModel
    {
        public int IdERPs { get; private set; }
        public string Nome { get; private set; }
        public Fornecedor Fornecedor { get; private set; }
        public int IdFornecedor { get; private set; }

        public ERPs() { }

        public ERPs(string nome, Fornecedor fornecedor, string usuarioInclusao)
        {
            Nome = nome;
            Fornecedor = fornecedor;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, Fornecedor fornecedor, string usuarioAlteracao)
        {
            Nome = nome;
            Fornecedor = fornecedor;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("O nome do ERP não pode ser vazio.");
            if (Fornecedor == null)
                throw new Exception("O fornecedor é obrigatório.");
        }
    }
}
