using System;

namespace ERP.Models
{
    public class BaseModel
    {
        public string UsuarioInclusao { get; set; }
        public string UsuarioAlteracao { get; set; }
        public string UsuarioExclusao { get; set; }

        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataExclusao { get; set; }

        public string Situacao { get; set; }

        public void SetUsuarioInclusao(string usuarioInclusao)
        {
            this.Situacao = "Ativo";
            this.UsuarioInclusao = usuarioInclusao;
            this.DataInclusao = DateTime.Now;
        }
        public void SetUsuarioAlteracao(string usuarioAlteracao)
        {
            this.UsuarioAlteracao = usuarioAlteracao;
            this.DataAlteracao = DateTime.Now;
        }
        public void SetUsuarioExclusao(string usuarioExclusao)
        {
            this.Situacao = "Excluido";
            this.UsuarioExclusao = usuarioExclusao;
            this.DataExclusao = DateTime.Now;
        }
    }
}
