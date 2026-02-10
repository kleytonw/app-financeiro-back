using System;

namespace ERP.Models.Relatorio
{
    public class FiltroExcelTodoProdutosMonkey
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
    }

    public class ExcelTodosProdutos
    {
        public string NomeCliente { get; set; }
        public string CelularCliente { get; set; }
        public string Telefone { get; set; }
        public string EmailCliente { get; set; }
        public string CPFCliente { get; set; }
        public string CEPCliente { get; set; }
        public string EstadoCliente { get; set; }
        public string CidadeCliente { get; set; }
        public string BairroCliente { get; set; }
        public string LogradouroCliente { get; set; }
        public string NumeroCliente { get; set; }
        public string ComplementoCliente { get; set; }
        public string ReferenciaCliente { get; set; }
        public string Mae { get; set; }
        public string Pai { get; set; }
        public string Rg { get; set; }
        public string OrgaoExpeditor { get; set; }
        public string Sexo { get; set; }

        public int NumeroDoPedido { get; set; }
        public Decimal QuantidadeItensNoPedido { get; set; }
        public Decimal ValorTotalFreteDoPedido { get; set; }
        public Decimal TotalProdutosNoPedido { get; set; }

        public int QuantidadeItens { get; set; }
        public Decimal ValorItem { get; set; }
        public Decimal ValorFreteItem { get; set; }
        public bool? CombinarVendedor { get; set; }
        public bool? FreteCorreios { get; set; }
        public bool? RetirarEmMaos { get; set; }

        public int CodigoProduto { get; set; }
        public string NomeProduto { get; set; }
        public Decimal ValorProtuto { get; set; }
        public int CodigoEmpresa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string PesoProduto { get; set; }
        public Decimal ComprimentoProtudo { get; set; }
        public Decimal AlturaProduto { get; set; }
        public Decimal DiametroProduto { get; set; }

        public string NomeEmpresa { get; set; }
        public string CNPJ { get; set; }
        public string CepEmpresa { get; set; }
        public string EstadoEmpresa { get; set; }
        public string CidadeEmpresa { get; set; }
        public string BairroEmpresa { get; set; }
        public string LogradouroEmpresa { get; set; }
        public string NumeroEmpresa { get; set; }
        public string ComplementoEmpresa { get; set; }
        public string ReferenciaEmpresa { get; set; }
        public string TelefoneEmpresa { get; set; }
        public string TelefoneCelularEmpresa { get; set; }
        public string EmailEmpresa { get; set; }

        public DateTime DataInclusao { get; set; }
    }
}
