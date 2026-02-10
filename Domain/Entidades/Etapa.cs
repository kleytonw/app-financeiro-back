using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Etapa : BaseModel
    {
        public int IdEtapa { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool EtapaConcluida { get; private set; }

        public Etapa() { }

        public Etapa(string nome, string descricao, bool etapaConcluida, string usuarioInclusao) 
        {
            Nome = nome;
            Descricao = descricao;
            EtapaConcluida = etapaConcluida;
            SetUsuarioInclusao(usuarioInclusao);
        }

        public void Alterar(string nome, string descricao, bool etapaConcluida, string usuarioAlteracao)
        {
            Nome = nome;
            Descricao = descricao;
            EtapaConcluida = etapaConcluida;
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
            if (EtapaConcluida == null)
                throw new Exception("Etapa concluida é obrigatória");
        }


    }
}
