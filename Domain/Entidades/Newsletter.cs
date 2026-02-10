using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class Newsletter : BaseModel
    {
        public int IdNewsletter { get; private set; }
        public Empresa Empresa { get; private set; }
        public int IdEmpresa { get; private set; }
        public string Email { get; private set; }
        public DateTime? Data { get; private set; }

        protected Newsletter() { }

        public Newsletter(Empresa empresa, string email, DateTime? data, string usuarioInclusao)
        {
            Empresa = empresa;
            Email = email;
            Data = data;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }
        public void Alterar(Empresa empresa, string email, DateTime? data, string usuarioAlteracao)
        {
            Empresa = empresa;
            IdEmpresa = empresa.IdEmpresa;
            Email = email;
            Data = data;

            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        private void Valida()
        {
            if (Empresa == null)
                throw new Exception("A empresa é obrigatória.");

            if (string.IsNullOrWhiteSpace(Email))
                throw new Exception("O email é obrigatório.");

            if (Data == default(DateTime))
                throw new Exception("A data é obrigatória.");
        }
    }
}
