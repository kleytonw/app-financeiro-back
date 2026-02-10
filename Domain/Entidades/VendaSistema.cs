using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class VendaSistema : BaseModel
    {
        public int IdVendaSistema { get; private set; }
        public DateTime Data { get; private set; }
        public int IdCliente { get; private set; }
        public Cliente Cliente { get; private set; }
        public int IdERPs { get; private set; }
        public ERPs ERPs { get; private set; }
        public string Arquivo { get; private set; }
        public string NomeArquivo { get; private set; }
        public int CodigoResposta { get; set; }
        public string MensagemResposta { get; set; }
        public string XMLErrosResposta { get; set; }    
        public bool SucessoResposta { get; set; }

        public VendaSistema() { }

        public VendaSistema(
            DateTime data,
            Cliente cliente,
            ERPs erps,
            string arquivo,
            string nomeArquivo,
            string usuarioInclusao)
        {
            Data = data;
            Cliente = cliente;
            ERPs = erps;
            Arquivo = arquivo;
            NomeArquivo = nomeArquivo;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(
            DateTime data,
            Cliente cliente,
            ERPs erps,
            string arquivo,
            string nomeArquivo,
            string usuarioAlteracao)
        {
            Data = data;
            Cliente = cliente;
            ERPs = erps;
            Arquivo = arquivo;
            NomeArquivo = nomeArquivo;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void SetDadosRespostaEnvio(
            int codigoResposta,
            string mensagemResposta,
            string xmlErrosResposta,
            bool sucessoResposta)
        {
            CodigoResposta = codigoResposta;
            MensagemResposta = mensagemResposta;
            XMLErrosResposta = xmlErrosResposta;
            SucessoResposta = sucessoResposta;
        }

        public void Valida()
        {
            if (DateTime.MinValue == Data)
                throw new Exception("A data da venda é obrigatória.");
            if (Cliente == null)
                throw new Exception("O cliente é obrigatório.");
            if (ERPs == null)
                throw new Exception("O ERP é obrigatório.");
            if (string.IsNullOrEmpty(Arquivo))
                throw new Exception("O arquivo da venda é obrigatório.");
            if (string.IsNullOrEmpty(NomeArquivo))
                throw new Exception("O nome do arquivo é obnrigatório!");
        }
    }
}
