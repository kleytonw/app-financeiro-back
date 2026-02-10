using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class Banco: BaseModel
    {

        public int IdBanco { get; private set; }
        public string NomeBanco { get; private set; }
        public string CodigoBancoTecnoSpeed { get; private set; }
        public bool PossuiIntegracaoTecnospeed {  get; private set; }

        public int? IdOpenFinance { get; private set; }
        public Banco() { }

        public Banco(string nomeBanco, string codigoBancoTecnoSpeed, string usuarioInclusao)
        {
            NomeBanco = nomeBanco;
            CodigoBancoTecnoSpeed = codigoBancoTecnoSpeed;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nomeBanco, string codigoBancoTecnoSpeed, string usuarioAlteracao)
        {
            NomeBanco = nomeBanco;
            CodigoBancoTecnoSpeed = codigoBancoTecnoSpeed;
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
            if (string.IsNullOrEmpty(NomeBanco))
                throw new Exception("O nome é obrigatório");
            if (string.IsNullOrEmpty(CodigoBancoTecnoSpeed))
                throw new Exception("O códgio do Banco na TecnoSpeed é obrigatório");
        }
    }
}


