using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class Bandeira : BaseModel
    {

        public int IdBandeira { get; private set; }
        public string NomeBandeira { get; private set; }
        public string CodigoBandeiraCartao { get; private set; }
        public string CodigoBandeiraCartaoRede { get; private set; }


        public Bandeira() { }

        public Bandeira(string nomeBandeira, string codigoBandeiraCartao, string codigoBandeiraCartaoRede, string usuarioInclusao)
        {
            NomeBandeira = nomeBandeira;
            CodigoBandeiraCartao = codigoBandeiraCartao;
            CodigoBandeiraCartaoRede = codigoBandeiraCartaoRede;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nomeBandeira, string codigoBandeiraCartao, string codigoBandeiraCartaoRede, string usuarioAlteracao)
        {
            NomeBandeira = nomeBandeira;
            CodigoBandeiraCartao = codigoBandeiraCartao;
            CodigoBandeiraCartaoRede = codigoBandeiraCartaoRede;
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
            if (string.IsNullOrEmpty(NomeBandeira))
                throw new Exception("O nome da bandeira é obrigatório");
            if (string.IsNullOrEmpty(CodigoBandeiraCartao))
                throw new Exception("O código da bandeira do cartão é obrigatório");
            if (string.IsNullOrEmpty(CodigoBandeiraCartaoRede))
                throw new Exception("O código da bandeira do cartão da Rede é obrigatório");
        }
    }
}


