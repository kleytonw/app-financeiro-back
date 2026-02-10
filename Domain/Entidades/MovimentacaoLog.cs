using ERP.Domain.Entidades;
using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class MovimentacaoLog : BaseModel
    {
        public int IdMovimentacaoLog { get; private set; }
        public string ChaveAcesso { get; private set; }
        public int IdEmpresa { get; private set; }
        public Empresa Empresa { get; private set; }
        public DateTime  DataMovimentacaoLog { get; private set; }

        public MovimentacaoLog() { }

        public MovimentacaoLog(string chaveAcesso, Empresa empresa, string usuarioInclusao)
        {

            ChaveAcesso = chaveAcesso;
            Empresa = empresa;
            SetUsuarioInclusao(usuarioInclusao);
        }

        public void Alterar(string chaveAcesso, Empresa empresa, string usuarioAlteracao)
        {

            ChaveAcesso = chaveAcesso;
            Empresa = empresa;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);

        }

        public void Valida()
        {
            if (ChaveAcesso == null)
                throw new Exception("ChaveAcesso é obrigatório");
            if (Empresa == null)
                throw new Exception("Empresa é obrigatório");
        }
    }
}
