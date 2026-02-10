using CsvHelper;
using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class PlanoComissao : BaseModel
    {
        public int IdPlanoComissao { get; set; }
        public int Nivel { get; set; }
        public decimal Percentual { get; set; }

        public PlanoComissao(int nivel, decimal percentual, string usuarioInclusao)
        {
            Nivel = nivel;
            Percentual = percentual;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(int nivel, decimal percentual, string usuarioAlteracao)
        {
            Nivel = nivel;
            Percentual = percentual;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Nivel == 0)
                throw new Exception("O nível é obrigatório");
            if (Percentual == 0)
                throw new Exception("O percentual é obrigatório");
        }

    }
}
