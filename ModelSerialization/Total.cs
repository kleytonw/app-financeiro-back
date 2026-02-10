using System.Xml.Serialization;

namespace ERP.Domain.ModelSerialization
{
    public class Total
    {
        [XmlElement("ICMSTot")]
        public ICMSTot ICMSTot { get; set; }
    }

    public class ICMSTot
    {
        /// <summary>
        ///  informar o somatório da BC do ICMS (vBC) informado nos itens
        /// </summary>
        public decimal vBC { get; set; }

        /// <summary>
        /// informar o somatório de ICMS (vICMS) informado nos itens
        /// </summary>
        public decimal vICMS { get; set; }

        /// <summary>
        /// informar o somatório da BC ST (vBCST) informado nos itens
        /// </summary>
        public decimal vBCST { get; set; }

        /// <summary>
        /// informar o somatório do ICMS ST (vICMSST)informado nos itens
        /// </summary>
        public decimal vST { get; set; }

        /// <summary>
       /// informar o somatório de valor dos produtos(vProd) dos itens que tenham indicador de totalização = 1 (indTot)  Os valores dos itens sujeitos ao ISSQN não devem ser acumulados neste campo.
        /// </summary>
        public decimal vProd { get; set; }

        /// <summary>
        /// informar o somatório de valor do Frete (vFrete) informado nos itens
        /// </summary>
        public decimal vFrete { get; set; }

        /// <summary>
        /// informar o somatório valor do Seguro (vSeg) informado nos itens
        /// </summary>
        public decimal vSeg { get; set; }

        /// <summary>
        /// informar o somatório do Desconto (vDesc) informado nos itens
        /// </summary>
        public decimal vDesc { get; set; }

        /// <summary>
        /// informar o somatório de II (vII) informado nos itens
        /// </summary>
        public decimal vII { get; set; }

        /// <summary>
        /// informar o somatório de IPI (vIPI) informado nos itens
        /// </summary>
        public decimal vIPI { get; set; }

        /// <summary>
        /// informar o somatório de PIS (vPIS) informado nos itens sujeitos ao ICMS
        /// </summary>
        public decimal vPIS { get; set; }

        /// <summary>
        /// informar o somatório de COFINS (vCOFINS) informado nos itens sujeitos ao ICMS
        /// </summary>
        public decimal vCOFINS { get; set; }

        /// <summary>
        /// informar o somatório de vOutro (vOutro) informado nos itens
        /// </summary>
        public decimal vOutro { get; set; }

        /// <summary>
        /// informar o valor total a NF Acrescentar o valor dos Serviços informados no grupo do ISSQN
        /// </summary>
        public decimal vNF { get; set; }

        /// <summary>
        /// informar o somatório do valor total aproximado dos tributos (vTotTrib) informado nos itens, deve considerar valor de itens sujeitos ao ISSQN também.
        /// </summary>
        public decimal? vTotTrib { get; set; }

        /// <summary>
        /// informar o somatório do Valor do ICMS desonerado (vICMSDeson) informado nos itens.
        /// </summary>
        public decimal? vICMSDeson { get; set; }

        /// <summary>
        /// informar o somatório do Valor do ICMS Interestadual para a UF de destino (vICMSUFDest) informado nos itens.
        /// </summary>
        public decimal? vICMSUFDest_Opc { get; set; }

        /// <summary>
        /// informar o somatório do Valor total do ICMS Interestadual para a UF do remetente vICMSUFRemet) informado nos itens.  Nota: A partir de 2019, este valor será zero.
        /// </summary>
        public decimal? vICMSUFRemet_Opc { get; set; }

        /// <summary>
        /// informar o somatório do Valor do ICMS relativo ao Fundo de Combate à Pobreza (FCP) da UF de destino. Corresponde ao total da soma dos campos vFCP informado nos itens.
        /// </summary>
        public decimal? vFCPUFDest_Opc { get; set; }

        /// <summary>
        /// informar o somatório do Valor do FCP (Fundo de Combate à Pobreza) (vFCP) informado nos itens.
        /// </summary>
        public decimal? vFCP { get; set; }

        /// <summary>
        /// informar o somatório do Valor do FCP retido anteriormente por Substituição. Corresponde ao total da soma dos campos vFCPST informado nos itens.
        /// </summary>
        public decimal? vFCPST { get; set; }

        /// <summary>
        /// informar o somatório do Valor do FCP retido anteriormente por Substituição. Corresponde ao total da soma dos campos vFCPSTRet informado nos itens.
        /// </summary>
        public decimal? vFCPSTRet { get; set; }

        /// <summary>
        /// informar o somatório do Valor do IPI devolvido. Deve ser informado quando preenchido o Grupo Tributos Devolvidos na emissão de nota finNFe = 4(devolução) nas operações com não contribuintes do IPI.        Corresponde ao total da soma dos campos vIIPIDevol do item.
        /// </summary>
        public decimal? vIPIDevol { get; set; }
    }
}