using ERP_API.Service.Pluggy.Interface;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

using ERP_API.Models.Pluggy;
using ERP_API.Models;
using ERP.Infra; 
using System.Data.Entity;
using System.Linq;


namespace ERP_API.Service
{
    public class PluggyService : IPluggyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private string? _accessToken;
        private string? _apiKey;

        protected Context context;
        public PluggyService(HttpClient httpClient, Context context)
        {
            _httpClient = httpClient;
            _clientId = "b5100a6d-6146-4be9-98e4-c6dc0840a621";
            _clientSecret = "4edddcc2-ef98-4c50-96a8-f7e089826966";
            _httpClient.BaseAddress = new Uri("https://api.pluggy.ai/");
            this.context = context;
        }

        public async Task<string> AuthenticateAsync()
        {
            var body = new { clientId = _clientId, clientSecret = _clientSecret };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("auth", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            _apiKey = doc.RootElement.GetProperty("apiKey").GetString();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("X-API-KEY", _apiKey);
            return _apiKey!;
        }

        public async Task<string> CreateConnectTokenAsync()
        {
            if (string.IsNullOrEmpty(_apiKey))
                await AuthenticateAsync();

            // string clienteUserId, string itemId


            using var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pluggy.ai/connect_token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("X-API-KEY", _apiKey);

            // Corpo da requisição com parâmetros
            var payload = new
            {
                itemId = 1,
                options = new
                {
                    webhookUrl = "https://api-sovarejo-desenv-bahjcfhzfbghe9h8.brazilsouth-01.azurewebsites.net/api/WebhookPluggy/criar-log-webhook-pluggy",
                    clientUserId = "2"
                }
            };

            var json = JsonSerializer.Serialize(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseBody);
            if (doc.RootElement.TryGetProperty("accessToken", out var tokenElement))
            {
                var connectToken = tokenElement.GetString();
                return connectToken;
            }

            return "";

        }

        public async Task<GetItemResponseModel> GetItemAsync(string itemId)
        {
            if (string.IsNullOrEmpty(_accessToken))
                await AuthenticateAsync();

            // Limpa e adiciona os headers necessários
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            string url = $"https://api.pluggy.ai/items/{itemId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            
            var result = JsonSerializer.Deserialize<GetItemResponseModel>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return result;
        }

        public async Task<ListaContasResponse> GetAccountsAsync(string itemId)
        {
            if (string.IsNullOrEmpty(_accessToken))
                await AuthenticateAsync();


            // Limpa e adiciona os headers necessários
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");


            var response = await _httpClient.GetAsync($"accounts?itemId={itemId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ListaContasResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );


            return result;
        }

        public async Task<PluggyAccountResponse?> GetAccountByIdAsync(string accountId)
        {
            if (string.IsNullOrEmpty(_accessToken))
                await AuthenticateAsync();


            // Limpa e adiciona os headers necessários
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _httpClient.GetAsync($"https://api.pluggy.ai/accounts/{accountId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<PluggyAccountResponse>(json, options);
        }

        public async Task<List<TransactionDto>> GetAllTransactionsAsync(
        Guid? accountId = null,
        DateTime? from = null,
        DateTime? to = null,
        string? ids = null,
        int pageSize = 500)
        {
            // Lista acumuladora
            var transacoes = new List<TransactionDto>();

            // Garante API KEY
            if (string.IsNullOrEmpty(_apiKey))
                await AuthenticateAsync();

            if (string.IsNullOrEmpty(_apiKey))
                throw new Exception("API Key Pluggy não configurada");

            int paginaAtual = 1;
            int totalPages = 1;

            do
            {
                var url = BuildTransactionUrl(
                    paginaAtual, pageSize, accountId, from, to, ids);

                SetDefaultHeaders();

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro Pluggy: {response.StatusCode} - {error}");
                }

                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<TransactionListResponse>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    });

                if (result?.Results != null)
                    transacoes.AddRange(result.Results);

                totalPages = result?.TotalPages ?? 1;
                paginaAtual++;

            } while (paginaAtual <= totalPages);

            return transacoes;
        }

        private string BuildTransactionUrl(
            int page, int pageSize,
            Guid? accountId, DateTime? from, DateTime? to, string? ids)
        {
            var url = $"https://api.pluggy.ai/transactions?page={page}&pageSize={pageSize}";

            if (accountId.HasValue)
                url += $"&accountId={accountId.Value}";

            if (!string.IsNullOrEmpty(ids))
                url += $"&ids={ids}";

            if (from.HasValue)
                url += $"&from={Uri.EscapeDataString(from.Value.ToString("yyyy-MM-dd"))}";

            if (to.HasValue)
                url += $"&to={Uri.EscapeDataString(to.Value.ToString("yyyy-MM-dd"))}";

            return url;
        }

        private void SetDefaultHeaders()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<string> UpdateItemAsync(string itemId)
        {
            if (string.IsNullOrEmpty(_accessToken))
                await AuthenticateAsync();

            var url = $"https://api.pluggy.ai/items/{itemId}";

            // Limpa e adiciona os headers necessários
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Corpo JSON
            var body = new
            {
                parameters = new
                {
                    user = "user",
                    password = "password"
                },
                webhookUrl = "https://api-sovarejo-desenv-bahjcfhzfbghe9h8.brazilsouth-01.azurewebsites.net/api/WebhookPluggy/criar-log-webhook-pluggy"
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Criar request PATCH manual
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Patch,
                RequestUri = new Uri(url),
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro Pluggy: {response.StatusCode} - {error}");
            }
        }

        public async Task<PluggyConnectResponse> CreateItemPessoalAsync(CreateItemPessoalPluggyRequestModel request)
        {
            if (string.IsNullOrEmpty(_apiKey))
                await AuthenticateAsync();
              
            var h = new HttpRequestMessage(HttpMethod.Post, "https://api.pluggy.ai/items");
            h.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            h.Headers.Add("X-API-KEY", _apiKey);
             

            var json = JsonSerializer.Serialize(request);
            h.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(h);
            var responseBody = await response.Content.ReadAsStringAsync();

            var pluggyResponse = JsonSerializer.Deserialize<PluggyConnectResponse>(
                    responseBody,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return pluggyResponse; // .Parameter.Data
        }

        public async Task<PluggyConnectResponse> CreateItemEmpresarialAsync(CreateItemEmpresarialPluggyRequestModel request)
        {
            if (string.IsNullOrEmpty(_apiKey))
                await AuthenticateAsync();

            var h = new HttpRequestMessage(HttpMethod.Post, "https://api.pluggy.ai/items");
            h.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            h.Headers.Add("X-API-KEY", _apiKey);


            var json = JsonSerializer.Serialize(request);
            h.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(h);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseBody.ToString()); 

            var pluggyResponse = JsonSerializer.Deserialize<PluggyConnectResponse>(
                    responseBody,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return pluggyResponse;
        }

        public async Task<List<ConectorOpenFinanceResponseModel>> GetAllConnectors()
        {
            List<ConectorOpenFinanceResponseModel> lista = new List<ConectorOpenFinanceResponseModel>();
            var consulta = context.ConectorOpenFinance.OrderBy(x=>x.Nome).ToList();
            foreach(var item in consulta)
            {
                lista.Add(new ConectorOpenFinanceResponseModel()
                {
                    CorPrimaria = item.CorPrimaria,
                    Nome = item.Nome,
                    DataAtualizacao = item.DataAtualizacao, 
                    ImageUrl = item.ImageUrl,
                    IdConector = item.IdConector
                });
            }
            return lista;
        }
    }
    public class TransactionListResponse
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public List<TransactionDto> Results { get; set; } = new();
    }

    public class TransactionDto
    {
        public string Id { get; set; }
        public string? Description { get; set; }
        public string? DescriptionRaw { get; set; }
        public string? CurrencyCode { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountInAccountCurrency { get; set; }
        public DateTime? Date { get; set; }
        public string? Category { get; set; }
        public string? CategoryId { get; set; }
        public decimal? Balance { get; set; }
        public string? AccountId { get; set; }
        public string? ProviderCode { get; set; }
        public string? Status { get; set; }
        public PaymentDataDto? PaymentData { get; set; }
        public string? Type { get; set; }
        public string? OperationType { get; set; }
        public CreditCardMetadataDto? CreditCardMetadata { get; set; }
        public AcquirerDataDto? AcquirerData { get; set; }
        public MerchantDto? Merchant { get; set; }
        public string? ProviderId { get; set; }
        public int? Order { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class PaymentDataDto
    {
        public PayerReceiverDto? Payer { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Reason { get; set; }
        public PayerReceiverDto? Receiver { get; set; }
        public string? ReceiverReferenceId { get; set; }
        public string? ReferenceNumber { get; set; }
        public object? BoletoMetadata { get; set; }
    }

    public class PayerReceiverDto
    {
        public string? AccountNumber { get; set; }
        public string? BranchNumber { get; set; }
        public DocumentNumberDto? DocumentNumber { get; set; }
        public string? Name { get; set; }
        public string? RoutingNumber { get; set; }
        public string? RoutingNumberISPB { get; set; }
    }

    public class DocumentNumberDto
    {
        public string? Type { get; set; }
        public string? Value { get; set; }
    }

    public class MerchantDto
    {
        public string? Cnae { get; set; }
        public string? Cnpj { get; set; }
        public string? Category { get; set; }
        public string? BusinessName { get; set; }
    }

    public class CreditCardMetadataDto
    {
        // Campos futuros (Pluggy pode retornar este objeto vazio)
    }

    public class AcquirerDataDto
    {
        // Campos futuros (Pluggy pode retornar este objeto vazio)
    }
     
    public class PluggyAccountResponse
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Subtype { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string CurrencyCode { get; set; }
        public string ItemId { get; set; }
        public string Number { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? MarketingName { get; set; }
        public string? TaxNumber { get; set; }
        public string? Owner { get; set; }
        public BankData? BankData { get; set; }
        public object? CreditData { get; set; }
    }

    public class BankData
    {
        public string TransferNumber { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal AutomaticallyInvestedBalance { get; set; }
        public decimal OverdraftContractedLimit { get; set; }
        public decimal OverdraftUsedLimit { get; set; }
        public decimal UnarrangedOverdraftAmount { get; set; }
    }
}