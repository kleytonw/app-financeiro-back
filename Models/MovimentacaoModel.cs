using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using ERP.Domain.Entidades;
using Microsoft.Identity.Client;

namespace ERP.Models
{

    public class MovimentacaoResponse
    {
        
        public string VersaoNfe { get; set; }
        public int IdMovimentacao { get; set; }

        public int IdEmpresa { get; set; }

        public string TipoMovimentacao { get; set; }

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
    }

    public class MoivmentacaoRequest
    {
        public string VersaoNfe { get; set; }
        public int IdMovimentacao { get; set; }

        public int IdEmpresa { get; set; }

        public string TipoMovimentacao { get; set; }
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
    }
}
