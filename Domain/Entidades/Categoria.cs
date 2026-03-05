using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Categoria : BaseModel
    {
        public int IdCategoria { get; private set; }
        public string Nome { get; private set; }
        public string Cor { get; private set; }
        public Categoria() { }
        public Categoria(string nome, string cor, string usuarioInclusao)
        {
            Nome = nome;
            Cor = cor;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }
        public void Alterar(string nome, string cor, string usuarioAlteracao)
        {
            Nome = nome;
            Cor = cor;
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
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("Nome é obrigatório");
        }
    }
}
