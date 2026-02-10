using ERP.Domain.ModelSerialization;
using ERP.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace ERP.Domain.Entidades
{
    public class Movimentacao : BaseModel
    {
        public Movimentacao() {
            this.Itens = new Collection<MovimentacaoItem>();
            this.Duplicatas = new Collection<MovimentacaoDuplicata>();
                  }


        public string VersaoNfe { get; set;  }
        public int IdMovimentacao { get; set; }

        public int IdEmpresa { get; set; }

        public string TipoMovimentacao { get; set;  }

        #region Identificação 
        public int CodigoUF { get; set; }
        public string CodigoNF { get; set; }
        public string NaturezaOperacao { get; set; }
        public int IndicadorFormaPagamento { get; set; }
        public string Modelo { get; set; }
        public int Serie { get; set; }
        public string NumeroNF { get; set; }
        public string ChaveAcesso { get; set; }
        public DateTime? DataHoraEmissao { get; set; }
        public DateTime? DataHoraSaiEntrada { get; set; } 
        #endregion

        #region Emitente 
        public string EmitenteCNPJ { get; set; }
        public string EmitenteNome { get; set; }
        public string EmitenteFantasia { get; set; }
        public string EmitenteIE { get; set; }
        public string EmitenteIEST { get; set; }
        public int EmitenteCRT { get; set; }
        #endregion

        #region Endereço Emitente
        public string EmitenteLogradouro { get; set; }
        public string EmitenteNumero { get; set; }
        public string EmitenteBairro { get; set; }
        public string EmitenteCodigoMunicipio { get; set; }
        public string EmitenteMunicipio { get; set; }
        public string EmitenteUF { get; set; }
        public string EmitenteCEP { get; set; }
        public int EmitenteCodigoPais { get; set; }
        public string EmitentePais { get; set; }
        #endregion

        #region Destinatário 

        public string DestinatarioCNPJ { get; set; }
        public string DestinatarioCPF { get; set; }
        public string DestinatarioNome { get; set; }
        public string DestinatarioEmail { get; set; }

        #endregion

        #region Endereço Destinatario
        public string DestinatarioLogradouro { get; set; }
        public string DestinatarioNumero { get; set; }
        public string DestinatarioBairro { get; set; }
        public string DestinatarioCodigoMunicipio { get; set; }
        public string DestinatarioMunicipio { get; set; }
        public string DestinatarioUF { get; set; }
        public string DestinatarioCEP { get; set; }
        public int DestinatarioCodigoPais { get; set; }
        public string DestinatarioPais { get; set; }
        #endregion

        public ICollection<MovimentacaoItem> Itens { get; set; }
 
        public decimal Total { get; set; }
        public decimal ValorICMS { get; set; }
        public decimal ValorBC { get; set; }
        public decimal ValorBCST { get; set; }
        public decimal ValorST { get; set; }
        public decimal ValorProdutos { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorVLL { get; set; }
        public decimal ValorIPI { get; set; }
        public decimal ValorPIS { get; set; }
        public decimal ValorCofins { get; set; }
        public decimal ValorOutro { get; set; }
        public decimal ValorNF { get; set; }
        public decimal? ValorTotalTributos { get; set; }

        // public MovimentacaoFatura Fatura { get; set; }

        public ICollection<MovimentacaoDuplicata> Duplicatas { get; set; } 
        
        public void SetIdentificacao( int codigoUF, string codigoNF, string naturezaOperacao, int indicadorFormaPagamento, string modelo, int serie, string numeroNF, DateTime? dataHoraEmissao, DateTime? dataHoraSaiEntrada, string chaveAcesso,string tipoMovimentacao,int idEmpresa)
        {
            CodigoUF = codigoUF;
            CodigoNF = codigoNF;
            NaturezaOperacao = naturezaOperacao;
            IndicadorFormaPagamento = indicadorFormaPagamento;
            Modelo = modelo;
            Serie = serie;
            NumeroNF = numeroNF;

            if (dataHoraEmissao != null)
                DataHoraEmissao = dataHoraEmissao;

            if(dataHoraSaiEntrada != null)
                DataHoraSaiEntrada = dataHoraSaiEntrada;
            
            ChaveAcesso = chaveAcesso;
            TipoMovimentacao = tipoMovimentacao;
            IdEmpresa = idEmpresa;
        }

        public void SetDadosEmitente(string emitenteCNPJ, string emitenteNome, string emitenteFantasia, string emitenteIE, string emitenteIEST, int emitenteCRT)
        {
            EmitenteCNPJ = emitenteCNPJ;
            EmitenteNome = emitenteNome;
            EmitenteFantasia = emitenteFantasia;
            EmitenteIE = emitenteIE;
            EmitenteIEST = emitenteIEST;
            EmitenteCRT = emitenteCRT;
        }

        public void SetEnderecoEmitente(string emitenteLogradouro, string emitenteNumero, string emitenteBairro, string emitenteCodigoMunicipio, string emitenteMunicipio, string emitenteUF, string emitenteCEP, int emitenteCodigoPais, string emitentePais)
        {
            EmitenteLogradouro = emitenteLogradouro;
            EmitenteNumero = emitenteNumero;
            EmitenteBairro = emitenteBairro;
            EmitenteCodigoMunicipio = emitenteCodigoMunicipio;
            EmitenteMunicipio = emitenteMunicipio;
            EmitenteUF = emitenteUF;
            EmitenteCEP = emitenteCEP;
            EmitenteCodigoPais = emitenteCodigoPais;
            EmitentePais = emitentePais;
        }

        public void SetDadosDestinatario(string destinatarioCNPJ, string destinatarioCPF, string destinatarioNome, string destinatarioEmail)
        {
            DestinatarioCNPJ = destinatarioCNPJ;
            DestinatarioCPF = destinatarioCPF;
            DestinatarioNome = destinatarioNome;
            DestinatarioEmail = destinatarioEmail;
        }

        public void SetEnderecoDestinatario(string destinatarioLogradouro, string destinatarioNumero, string destinatarioBairro, string destinatarioCodigoMunicipio, string destinatarioMunicipio, string destinatarioUF, string destinatarioCEP, int destinatarioCodigoPais, string destinatarioPais)
        {
            DestinatarioLogradouro = destinatarioLogradouro;
            DestinatarioNumero = destinatarioNumero;
            DestinatarioBairro = destinatarioBairro;
            DestinatarioCodigoMunicipio = destinatarioCodigoMunicipio;
            DestinatarioMunicipio = destinatarioMunicipio;
            DestinatarioUF = destinatarioUF;
            DestinatarioCEP = destinatarioCEP;
            DestinatarioCodigoPais = destinatarioCodigoPais;
            DestinatarioPais = destinatarioPais;
        }
        public void SetTotal(decimal total, decimal valorICMS, decimal valorBC, decimal valorBCST, decimal valorST, decimal valorProdutos, decimal valorFrete, decimal valorSeguro, decimal valorDesconto, decimal valorvII, decimal valorIPI, decimal valorPIS, decimal valorCofins, decimal valorOutro, decimal valorNF, decimal? valorTotalTributos)
        {
            Total = total;
            ValorICMS = valorICMS;
            ValorBC = valorBC;
            ValorBCST = valorBCST;
            ValorST = valorST;
            ValorProdutos = valorProdutos;
            ValorFrete = valorFrete;
            ValorSeguro = valorSeguro;
            ValorDesconto = valorDesconto;
            ValorVLL = valorvII;
            ValorIPI = valorIPI;
            ValorPIS = valorPIS;
            ValorCofins = valorCofins;
            ValorOutro = valorOutro;
            ValorNF = valorNF;
            ValorTotalTributos = valorTotalTributos;
        }


        public void AddItem(MovimentacaoItem item)
        {
            if (this.Itens == null)
                this.Itens = new List<MovimentacaoItem>();

            this.Itens.Add(item);
        }
        public void AddDuplicada(MovimentacaoDuplicata item)
        {
            if (this.Duplicatas == null)
                this.Duplicatas = new List<MovimentacaoDuplicata>();

            this.Duplicatas.Add(item);
        }


    }
}
