using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_API.Service.Parceiros
{
    public class ConciliadoraAuthResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public string DataString { get; set; }

        // Propriedade calculada para acessar os dados deserializados
        [JsonIgnore]
        public ConciliadoraAuthData Data
        {
            get
            {
                if (string.IsNullOrEmpty(DataString))
                    return null;

                try
                {
                    // O campo "data" vem como string JSON escapada, precisa ser deserializada
                    return JsonConvert.DeserializeObject<ConciliadoraAuthData>(DataString);
                }
                catch
                {
                    return null;
                }
            }
        }
    }

    public class ConciliadoraAuthData
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("initialPage")]
        public string InitialPage { get; set; }

        [JsonProperty(".issued")]
        public string Issued { get; set; }

        [JsonProperty(".expires")]
        public string Expires { get; set; }

        [JsonProperty(".refresh")]
        public string Refresh { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        // Propriedades calculadas para facilitar o uso
        [JsonIgnore]
        public DateTime IssuedDateTime
        {
            get
            {
                if (DateTime.TryParse(Issued, out var result))
                    return result;
                return DateTime.MinValue;
            }
        }

        [JsonIgnore]
        public DateTime ExpiresDateTime
        {
            get
            {
                if (DateTime.TryParse(Expires, out var result))
                    return result;
                return DateTime.MinValue;
            }
        }

        [JsonIgnore]
        public bool IsExpired => ExpiresDateTime <= DateTime.UtcNow;

        [JsonIgnore]
        public bool CanRefresh => !string.IsNullOrEmpty(RefreshToken) &&
                                 Refresh?.ToLowerInvariant() == "true";
    }

    /// <summary>
    /// Classe de compatibilidade com o código existente (DEPRECATED)
    /// Mantenha esta classe apenas para compatibilidade com código legado
    /// </summary>
    [Obsolete("Use ConciliadoraAuthData em vez desta classe")]
    public class AuthData
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

    public class PreviaEmailRequest
    {
        public ConciliadoraDashboardRequest Data { get; set; }
        public string Email { get; set; }
    }

    public class ConciliadoraDashboardRequest
    {
        public ConciliadoraDashBoardVendasRequest Body { get; set; }
        public string Token { get; set; }
    }

    public class ConciliadoraDashBoardVendasRequest
    {
        public Filter filter { get; set; }
    }

    public class Filter
    {
        public List<int> RefOIds { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Custom Custom { get; set; }
    }

    public class Custom
    {
        public string DashboardGroupType { get; set; }
    }

    public sealed class ConciliadoraDashboardVendaResponseModel
    {
        [JsonProperty("gridData")]
        public List<TransacaoFinanceira> GridData { get; set; } = new();

        [JsonProperty("chartSeries")]
        public List<SerieGrafico> ChartSeries { get; set; } = new();

        [JsonProperty("chartData")]
        public List<DadosGrafico> ChartData { get; set; } = new();

        /// <summary>
        /// Valida se os dados estão utilizáveis
        /// </summary>
        [JsonIgnore]
        public bool DadosValidos => GridData?.Any() == true && ChartData?.Any() == true;

        /// <summary>
        /// Obtém o valor total das transações (soma de GridData)
        /// </summary>
        [JsonIgnore]
        public decimal ValorTotal => GridData?.Sum(t => t.Valor) ?? 0m;
    }

    /// <summary>
    /// Representa uma transação financeira agregada com tratamento para datas inválidas
    /// </summary>
    public sealed class TransacaoFinanceira
    {
        [JsonProperty("Descricao")]
        public string Descricao { get; set; }

        [JsonProperty("Data")]
        public string Data { get; set; }

        [JsonProperty("Valor")]
        public decimal Valor { get; set; }

        [JsonProperty("Estabelecimento")]
        public string? Estabelecimento { get; set; }

        [JsonProperty("Terminal")]
        public string? Terminal { get; set; }

        [JsonProperty("Adquirente")]
        public string? Adquirente { get; set; }

        [JsonProperty("Banco")]
        public string? Banco { get; set; }

        [JsonProperty("Agencia")]
        public string? Agencia { get; set; }

        [JsonProperty("ContaCorrente")]
        public string? ContaCorrente { get; set; }

        [JsonProperty("Modalidade")]
        public int Modalidade { get; set; }

        [JsonProperty("NumParcelas")]
        public int NumParcelas { get; set; }

        [JsonProperty("Porcentagem")]
        public decimal Porcentagem { get; set; }

        // Propriedades calculadas
        [JsonIgnore]
        public decimal ValorLiquido => Valor * Porcentagem;

        [JsonIgnore]
        public bool EhParcelado => NumParcelas > 1;

        [JsonIgnore]
        public string DescricaoModalidade => Modalidade switch
        {
            0 => "Débito",
            1 => "Crédito à Vista",
            2 => "Crédito Parcelado",
            3 => "PIX",
            4 => "Dinheiro",
            _ => "Não Especificado"
        };

    }

    /// <summary>
    /// Configuração das séries do gráfico
    /// </summary>
    public sealed class SerieGrafico
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("valueField")]
        public string ValueField { get; set; }

        [JsonProperty("argumentField")]
        public string ArgumentField { get; set; }

        [JsonProperty("minBarSize")]
        public int MinBarSize { get; set; }

        // Helpers para tipo de gráfico
        [JsonIgnore]
        public bool EhGraficoBarras => Type?.IndexOf("bar", StringComparison.OrdinalIgnoreCase) >= 0;

        [JsonIgnore]
        public bool EhGraficoLinha => Type?.IndexOf("line", StringComparison.OrdinalIgnoreCase) >= 0;

        [JsonIgnore]
        public bool EhGraficoEmpilhado => Type?.IndexOf("stacked", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    /// <summary>
    /// Pontos de dados do gráfico com tratamento robusto para datas
    /// </summary>
    public sealed class DadosGrafico
    {
        [JsonProperty("Data")]
        public string Data { get; set; }

        // Captura propriedades dinâmicas (valores das séries)
        [JsonExtensionData]
        public IDictionary<string, JToken> ValoresDinamicos { get; set; } = new Dictionary<string, JToken>();

        /// <summary>
        /// Acesso seguro aos valores por série
        /// </summary>
        [JsonIgnore]
        public IReadOnlyDictionary<string, decimal> ValoresPorSerie
            => new Dictionary<string, decimal>(
                ValoresDinamicos
                    .Where(kv => !string.Equals(kv.Key, "Data", StringComparison.OrdinalIgnoreCase))
                    .ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.Type is JTokenType.Integer or JTokenType.Float
                            ? kv.Value.Value<decimal>()
                            : 0m));

        /// <summary>
        /// Obtém valor de uma série específica
        /// </summary>
        public decimal Valor(string serie)
            => ValoresDinamicos.TryGetValue(serie, out var tok) ? tok.Value<decimal>() : 0m;

        /// <summary>
        /// Define valor para uma série
        /// </summary>
        public void SetValor(string serie, decimal valor)
        {
            if (string.Equals(serie, "Data", StringComparison.OrdinalIgnoreCase)) return;
            ValoresDinamicos[serie] = JToken.FromObject(valor);
        }

        [JsonIgnore]
        public bool TemVenda => ValoresPorSerie.Values.Any(v => v > 0m);

        /// <summary>
        /// Verifica se o ponto tem dados válidos (não é apenas zero)
        /// </summary>
        [JsonIgnore]
        public bool PontoValido => ValoresPorSerie.Values.Any(v => v != 0m);
    }

    /// <summary>
    /// Model principal contendo os dados do dashboard
    /// </summary>
    public class ConciliadoraDashboardDebitosResponseModel
    {
        /// <summary>
        /// Lista de dados para o gráfico de linhas
        /// </summary>
        [JsonProperty("cpDadosGraficoLinhas")]
        public List<DadosGraficoLinhas> DadosGraficoLinhas { get; set; }

        /// <summary>
        /// Configurações das séries do gráfico
        /// </summary>
        [JsonProperty("cpSeriesGraficoLinhas")]
        public List<SerieGraficoLinhas> SeriesGraficoLinhas { get; set; }

        /// <summary>
        /// Formato de exibição da data no gráfico
        /// </summary>
        [JsonProperty("cpGraficoFormatoData")]
        public string GraficoFormatoData { get; set; }

        public ConciliadoraDashboardDebitosResponseModel()
        {
            DadosGraficoLinhas = new List<DadosGraficoLinhas>();
            SeriesGraficoLinhas = new List<SerieGraficoLinhas>();
        }
    }

    /// <summary>
    /// Model representando os dados de cada ponto no gráfico
    /// </summary>
    public class DadosGraficoLinhas
    {
        /// <summary>
        /// Data do pagamento
        /// </summary>
        [JsonProperty("DataPagamento")]
        [DataType(DataType.DateTime)]
        public DateTime DataPagamento { get; set; }

        /// <summary>
        /// Valor de cancelamento
        /// </summary>
        [JsonProperty("Cancelamento")]
        public decimal Cancelamento { get; set; }

        /// <summary>
        /// Valor de serviços
        /// </summary>
        [JsonProperty("Servicos")]
        public decimal Servicos { get; set; }

        /// <summary>
        /// Tipo de débito (opcional)
        /// </summary>
        [JsonProperty("TipoDebito")]
        public string TipoDebito { get; set; }

        // Propriedades calculadas úteis
        [JsonIgnore]
        public decimal Total => Servicos + Cancelamento;

        [JsonIgnore]
        public string MesAno => DataPagamento.ToString("MM/yyyy");

        [JsonIgnore]
        public string MesAnoFormatado => DataPagamento.ToString("MMM/yyyy");
    }

    /// <summary>
    /// Model representando a configuração de cada série no gráfico
    /// </summary>
    public class SerieGraficoLinhas
    {
        /// <summary>
        /// Nome da série exibido na legenda
        /// </summary>
        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Tipo de gráfico (line, bar, area, etc.)
        /// </summary>
        [JsonProperty("type")]
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Campo de valor a ser plotado no eixo Y
        /// </summary>
        [JsonProperty("valueField")]
        [Required]
        public string ValueField { get; set; }

        /// <summary>
        /// Campo de argumento a ser plotado no eixo X
        /// </summary>
        [JsonProperty("argumentField")]
        [Required]
        public string ArgumentField { get; set; }

        /// <summary>
        /// Cor da linha/série
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }

        // Propriedades adicionais que podem ser úteis
        [JsonIgnore]
        public bool IsLineChart => Type?.ToLower() == "line";

        [JsonIgnore]
        public bool IsBarChart => Type?.ToLower() == "bar";

        [JsonIgnore]
        public bool IsAreaChart => Type?.ToLower() == "area";
    }

    /// <summary>
    /// Enum para tipos de gráfico (opcional, para type safety)
    /// </summary>
    public enum TipoGrafico
    {
        Line,
        Bar,
        Area,
        Scatter,
        Pie
    }

    /// <summary>
    /// Enum para cores predefinidas (opcional, para type safety)
    /// </summary>
    public enum CorGrafico
    {
        DarkBlue,
        DarkRed,
        DarkGreen,
        DarkOrange,
        DarkPurple,
        DarkGray
    }


    /// <summary>
    /// Model principal do dashboard de economia por bandeira
    /// </summary>
    public class ConciliadoraDashboardTaxaResponse
    {
        /// <summary>
        /// Indica se deve mostrar o grid de taxas de vendas
        /// </summary>
        [JsonProperty("cpMostrarGridTaxasVendas")]
        public bool MostrarGridTaxasVendas { get; set; }

        /// <summary>
        /// Lista analítica de economia por bandeira
        /// </summary>
        [JsonProperty("P021_EconomiaBandeiraAnalitico")]
        public List<EconomiaBandeiraItem> EconomiaBandeiraAnalitico { get; set; }

        /// <summary>
        /// Lista resumida de economia por bandeira
        /// </summary>
        [JsonProperty("P021_EconomiaBandeira")]
        public List<EconomiaBandeiraItem> EconomiaBandeira { get; set; }

        /// <summary>
        /// Lista de taxas de vendas
        /// </summary>
        [JsonProperty("P021_TaxasVendas")]
        public List<TaxaVendaItem> TaxasVendas { get; set; }

        /// <summary>
        /// Indica se a economia deve ser visível
        /// </summary>
        [JsonProperty("cpEconomiaVisivel")]
        public bool EconomiaVisivel { get; set; }

        /// <summary>
        /// Valor total de economia
        /// </summary>
        [JsonProperty("cpEconomiaTotal")]
        public decimal EconomiaTotal { get; set; }

        public ConciliadoraDashboardTaxaResponse()
        {
            EconomiaBandeiraAnalitico = new List<EconomiaBandeiraItem>();
            EconomiaBandeira = new List<EconomiaBandeiraItem>();
            TaxasVendas = new List<TaxaVendaItem>();
        }
    }

    /// <summary>
    /// Model representando item de economia por bandeira
    /// </summary>
    public class EconomiaBandeiraItem
    {
        /// <summary>
        /// Identificador único
        /// </summary>
        [JsonProperty("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Código JSON serializado com informações adicionais
        /// </summary>
        [JsonProperty("Codigo")]
        public string Codigo { get; set; }

        /// <summary>
        /// Taxa média aplicada
        /// </summary>
        [JsonProperty("TaxaMedia")]
        public decimal TaxaMedia { get; set; }

        /// <summary>
        /// Valor bruto das transações
        /// </summary>
        [JsonProperty("ValorBruto")]
        public decimal ValorBruto { get; set; }

        /// <summary>
        /// Valor líquido após descontos
        /// </summary>
        [JsonProperty("ValorLiquido")]
        public decimal ValorLiquido { get; set; }

        /// <summary>
        /// Porcentagem das vendas totais
        /// </summary>
        [JsonProperty("PorcentagemVendas")]
        public decimal PorcentagemVendas { get; set; }

        /// <summary>
        /// Bandeira associada (nullable)
        /// </summary>
        [JsonProperty("EobtpBandeira")]
        public string EobtpBandeira { get; set; }

        /// <summary>
        /// Nome da bandeira
        /// </summary>
        [JsonProperty("Bandeira")]
        [Required]
        public string Bandeira { get; set; }

        /// <summary>
        /// Nome da adquirente
        /// </summary>
        [JsonProperty("Adquirente")]
        [Required]
        public string Adquirente { get; set; }

        /// <summary>
        /// Modalidade de pagamento
        /// </summary>
        [JsonProperty("Modalidade")]
        [Required]
        public string Modalidade { get; set; }

        /// <summary>
        /// Nome da empresa
        /// </summary>
        [JsonProperty("Empresa")]
        public string Empresa { get; set; }

        /// <summary>
        /// Total de parcelas
        /// </summary>
        [JsonProperty("TotalParcelas")]
        public int TotalParcelas { get; set; }

        /// <summary>
        /// ID do tipo de bandeira
        /// </summary>
        [JsonProperty("EobtpId")]
        public int EobtpId { get; set; }

        /// <summary>
        /// ID de referência
        /// </summary>
        [JsonProperty("RefoId")]
        public int RefoId { get; set; }

        /// <summary>
        /// ID da companhia
        /// </summary>
        [JsonProperty("CoId")]
        public int CoId { get; set; }

        /// <summary>
        /// Valor de economia
        /// </summary>
        [JsonProperty("Economia")]
        public decimal Economia { get; set; }

        /// <summary>
        /// Menor taxa disponível
        /// </summary>
        [JsonProperty("MenorTaxa")]
        public decimal MenorTaxa { get; set; }

        /// <summary>
        /// Adquirente com menor taxa
        /// </summary>
        [JsonProperty("AdquirenteMenorTaxa")]
        public string AdquirenteMenorTaxa { get; set; }

        /// <summary>
        /// Valor bruto do produto
        /// </summary>
        [JsonProperty("ValorBrutoProduto")]
        public decimal ValorBrutoProduto { get; set; }

        /// <summary>
        /// Economia por produto
        /// </summary>
        [JsonProperty("EconomiaProduto")]
        public decimal EconomiaProduto { get; set; }

        /// <summary>
        /// Economia total
        /// </summary>
        [JsonProperty("EconomiaTotal")]
        public decimal EconomiaTotal { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        [JsonProperty("Produto")]
        public string Produto { get; set; }

        /// <summary>
        /// Percentual de vendas do produto
        /// </summary>
        [JsonProperty("Per_Vendas_Produto")]
        public decimal PercentualVendasProduto { get; set; }

        // Propriedades calculadas úteis
        [JsonIgnore]
        public decimal ValorTaxa => ValorBruto * (TaxaMedia / 100);

        [JsonIgnore]
        public bool TemEconomia => Economia > 0;

        [JsonIgnore]
        public bool EhDebito => Modalidade?.Contains("Débito") ?? false;

        [JsonIgnore]
        public bool EhCredito => Modalidade?.Contains("Crédito") ?? false;

        [JsonIgnore]
        public bool EhBeneficio => Modalidade?.Contains("Benefício") ?? false;

        [JsonIgnore]
        public bool EhParcelado => Modalidade?.Contains("Parcelado") ?? false;

        [JsonIgnore]
        public CodigoInfo CodigoDeserializado =>
            string.IsNullOrEmpty(Codigo) ? null : JsonConvert.DeserializeObject<CodigoInfo>(Codigo);
    }

    /// <summary>
    /// Model para deserializar o campo Codigo
    /// </summary>
    public class CodigoInfo
    {
        [JsonProperty("EobtpId")]
        public int EobtpId { get; set; }

        [JsonProperty("Modalidade")]
        public string Modalidade { get; set; }
    }

    /// <summary>
    /// Model para item de taxa de venda
    /// </summary>
    public class TaxaVendaItem
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Codigo")]
        public string Codigo { get; set; }

        [JsonProperty("TaxaMedia")]
        public float TaxaMedia { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal ValorBruto { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal ValorLiquido { get; set; }

        [Column(TypeName = "decimal(38,28)")]
        [DisplayFormat(DataFormatString = "{0:P4}")]
        public decimal PorcentagemVendas { get; set; }

        public string? EobtpBandeira { get; set; }

        public string Modalidade { get; set; }
        public string? Empresa { get; set; }
        public int TotalParcelas { get; set; }
        public int EobtpId { get; set; }
        public int RefoId { get; set; }
        public int CoId { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Economia { get; set; }

        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal MenorTaxa { get; set; }
        public string? AdquirenteMenorTaxa { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal ValorBrutoProduto { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal EconomiaProduto { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal EconomiaTotal { get; set; }

        public string? Produto { get; set; }

        [DisplayFormat(DataFormatString = "{0:P2}")]
        [JsonProperty("Per_Vendas_Produto")]
        public decimal PercentualVendasProduto { get; set; }

        [JsonProperty("Bandeira")]
        public string Bandeira { get; set; }

        [JsonProperty("Taxa")]
        public decimal Taxa { get; set; }

        [JsonProperty("ValorVenda")]
        public decimal ValorVenda { get; set; }

        [JsonProperty("Adquirente")]
        public string Adquirente { get; set; }
    }

    public class ConciliadoraDashboardInformacoesComplementaresResponse
    {
        [JsonProperty("cpTotalTransacoesVendas")]
        public int CpTotalTransacoesVendas { get; set; }

        [JsonProperty("cpQuantidadeVendas")]
        public int CpQuantidadeVendas { get; set; }

        [JsonProperty("cpTotalTransacoesPagamentos")]
        public int CpTotalTransacoesPagamentos { get; set; }

        [JsonProperty("cpTotalTransacoes")]
        public int CpTotalTransacoes { get; set; }

        [JsonProperty("cpValorMedioVendas")]
        public decimal CpValorMedioVendas { get; set; }
    }

}
