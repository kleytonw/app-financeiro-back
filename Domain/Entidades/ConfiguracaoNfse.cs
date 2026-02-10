using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ConfiguracaoNfse : BaseModel
    {
        public int IdConfiguracaoNfse { get; private set; }
        public int NumeroRPS { get; private set; }

        public ConfiguracaoNfse() { }

        public ConfiguracaoNfse(int numeroRPS, string usuarioInclusao)
        {
            NumeroRPS = numeroRPS;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(int numeroRPS, string usuarioAlteracao)
        {
            NumeroRPS = numeroRPS;
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
            if (NumeroRPS == 0) 
                throw new Exception("O número do RPS deve ser informado.");
        }
    }
}
