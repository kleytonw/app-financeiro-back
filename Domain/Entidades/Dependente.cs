using ERP.Domain;
using ERP.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Dependente : BaseModel
    {

        public int IdDependente { get; private set; }
        public string Nome { get; private set; }


        public Dependente () { }

        public Dependente(string nome, string usuarioInclusao)
        {
            Nome = nome;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, string usuarioAlteracao)
        {
            Nome = nome;
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
