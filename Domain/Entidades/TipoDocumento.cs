using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class TipoDocumento : BaseModel
    {
        public int IdTipoDocumento { get; private set; }
        public string Nome { get; private set; }
        public bool Obrigatorio { get; private set; }

        public TipoDocumento() { }

        public TipoDocumento(string nome, bool obrigatorio, string usuarioInclusao)
        {
            Nome = nome;
            Obrigatorio = obrigatorio;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, bool obrigatorio, string usuarioAlteracao)
        {
            Nome = nome;
            Obrigatorio = obrigatorio;
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
                throw new Exception("O nome do tipo de documento não pode ser vazio.");
            if (Obrigatorio != true && Obrigatorio != false)
                throw new Exception("O campo 'Obrigatório' deve ser verdadeiro ou falso.");
        }
    }
}
