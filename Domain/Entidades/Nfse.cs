using ERP.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Nfse : BaseModel
    {
        public int IdNfse { get; private set; }
        public int IdCliente { get; private set; }
        public Cliente Cliente { get; private set; }
        public string NomeCliente { get; private set; }
        public string ChaveAcesso { get; private set; }
        public string NumeroRPS { get; private set; }
        public string Serie { get; private set; }
        public DateTime DataHoraInclusao { get; private set; }
        public DateTime? DataHoraCancelamento { get; private set; }
        public string StatusNotaFiscal { get; private set; }
        public int IdServicoNfse { get; private set; }
        public ServicoNfse ServicoNfse { get; private set; }
        public decimal Valor  { get; private set; }
        public string CodigoServico { get; private set; }
        public string CodigoNBS { get; private set; }
        public string ObservacoesNotaFiscal { get; private set; }

        public Nfse() { }
        public Nfse(Cliente cliente, string nomeCliente, string serie, DateTime dataHoraInclusao, 
            string statusNotaFiscal, ServicoNfse serviconfse, decimal valor, string codigoServico, string codigoNBS, string observacoesNotaFiscal, string usuarioInclusao)
        {
            Cliente = cliente;
            NomeCliente = nomeCliente;
            Serie = serie;
            DataHoraInclusao = dataHoraInclusao;
            StatusNotaFiscal = statusNotaFiscal;
            ServicoNfse = serviconfse;
            Valor = valor;
            CodigoServico = codigoServico;
            CodigoNBS = codigoNBS;
            ObservacoesNotaFiscal = observacoesNotaFiscal;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }
        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }
        public void Valida()
        {
           if (Cliente  == null) 
                throw new Exception("O cliente é obrigatório");
            if (string.IsNullOrWhiteSpace(NomeCliente))
                throw new Exception("O nome do cliente é obrigatório");
            if (string.IsNullOrWhiteSpace(Serie))
                throw new Exception("A série é obrigatória");
            if (DataHoraInclusao == DateTime.MinValue)
                throw new Exception("A data e hora de inclusão é obrigatória");
            if (string.IsNullOrWhiteSpace(StatusNotaFiscal))
                throw new Exception("O status da nota fiscal é obrigatório");
            if (ServicoNfse == null)
                throw new Exception("O serviço Nfse é obrigatório");
            if (Valor <= 0)
                throw new Exception("O valor deve ser maior ou igual a zero");
            if (string.IsNullOrWhiteSpace(CodigoServico))
                throw new Exception("O código do serviço é obrigatório");
            if (string.IsNullOrWhiteSpace(CodigoNBS))
                throw new Exception("O código NBS é obrigatório");
        }
    }
}
