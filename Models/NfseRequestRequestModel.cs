using System.Collections.Generic;

namespace ERP_API.Models
{
    public class NfseRequestRequestModel
    {
        public string Numero { get; set; }
        public string Serie { get; set; }
        public string Status { get; set; }
        public string DataEmissao { get; set; } // ISO 8601
        public NfseRequestTomador Tomador { get; set; }
        public NfseRequestItemServico Servico { get; set; }
        public string InformacoesComplementares { get; set; }
    }

    public class NfseRequestItemServico
    {
        public string CodigoMunicipio { get; set; }

        public string CodigoMunicipioPrestacao { get; set; }
        public string Codigo { get; set; }
        public string CodigoTributacaoMunicipio { get; set; }
        public string Discriminacao { get; set; }
        public string UnidadeNome { get; set; }
        public string UnidadeQuantidade { get; set; }
        public decimal UnidadeValor { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal ValorLiquido { get; set; }
        public decimal ValorServicos { get; set; }
        public decimal ValorBaseCalculoIss { get; set; }
        public decimal AliquotaIss { get; set; }
        public decimal ValorIss { get; set; }
    }

    public class NfseRequestTomador
    {
        public string? Cnpj { get; set; }
        public string? Cpf { get; set; }
        public string RazaoSocial { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public NfseRequestEndereco Endereco { get; set; }
    }

    public class NfseRequestEndereco
    {
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string CodigoMunicipio { get; set; }
        public string NomeMunicipio { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
    }

    public class NfsListaErrorResponseModel
    {
        public List<NfsErrorResponseModel> Erros { get; set; } = new List<NfsErrorResponseModel>();
    }
    public class NfsErrorResponseModel
    {
        public string Campo { get; set; }
        public string ErroDescricao { get; set; } // "erro" renomeado para evitar confusão
        public string Descricao { get; set; }
        public string Detalhes { get; set; }
    }
}
