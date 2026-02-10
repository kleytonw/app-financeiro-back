using ERP.Domain.Entidades;
using ERP.Models;
using System;
using System.Drawing;
using System.Security.Policy;

namespace ERP_API.Domain.Entidades
{
    public class ParceiroParametro : BaseModel
    { 
        public int IdParceiroParametro {  get; set; }
        public string Chave { get; private set; }
        public string Valor {  get; private set; }
        public int? IdParceiroSistema { get; private set; }
        public ParceiroSistema ParceiroSistema { get; private set; }

        public ParceiroParametro() { }
     
        public ParceiroParametro( string chave, string valor, ParceiroSistema parceiroSistema, string usuarioInclusao)
            {
                Chave = chave;
                Valor = valor;
                ParceiroSistema = parceiroSistema;
                SetUsuarioInclusao(usuarioInclusao);
                Valida();
            }
        public void Alterar(string chave, string valor, ParceiroSistema parceiroSistema, string usuarioAlteracao)
            {
                Chave = chave;
                Valor = valor;
                ParceiroSistema = parceiroSistema;
            SetUsuarioAlteracao(usuarioAlteracao);
                Valida();
            }

        public void Excluir(string usuarioExclusao)
            {
                SetUsuarioExclusao(usuarioExclusao);
            }

        public void Valida()
            {
                if (Chave == null)
                    throw new Exception("A chave é obrigatória!");
                if (Valor == null)
                    throw new Exception("O valor é obrigatório!");
                if (ParceiroSistema == null)
                    throw new Exception("O parceiro do sistema é obrigatório!");
            }

    }
}
