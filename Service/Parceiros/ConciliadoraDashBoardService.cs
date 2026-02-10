using ERP.Models;
using ERP_API.Service.Parceiros.Interface;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros
{
    public class ConciliadoraDashBoardService : IConciliadoraDashBoardService
    {
        private readonly HttpClient _httpCliente;

        public ConciliadoraDashBoardService(IHttpClientFactory httpClientFactory)
        {
            _httpCliente = httpClientFactory.CreateClient();
        }

        public async Task<ConciliadoraAuthResponse> LoginAsync(string username, string password)
        {
            var url = "https://concicard.conciliadora.com.br/Login/Login";

            var client = new RestClient(url);
            client.Timeout = -1;

            var request = new RestRequest(url, RestSharp.Method.POST);

            // adiciona os parâmetros do x-www-form-urlencoded
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            // executa a requisição
            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Erro no login: {response.StatusCode} - {response.ErrorMessage}");
            }

            var result = JsonConvert.DeserializeObject<ConciliadoraAuthResponse>(response.Content);

            return result;
        }

        public async Task<ConciliadoraDashboardVendaResponseModel> GetVendasAsync(string token, ConciliadoraDashBoardVendasRequest requestModel)
        {
            var url = "https://app-api-wl.conciliadora.com.br/api/Dashboard/Gerencial/GetVendas";

            var client = new RestClient(url);
            var request = new RestRequest(url, RestSharp.Method.POST);

            // Adiciona o Authorization Bearer Token
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            // Serializa o body JSON
            request.AddJsonBody(requestModel);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Erro ao buscar vendas: {response.StatusCode} - {response.ErrorMessage}");
            }

            // CORREÇÃO AQUI - Lê o conteúdo como string primeiro
            var jsonContent = response.Content;

            // Se o JSON vier com problema de dupla serialização
            if (!string.IsNullOrEmpty(jsonContent) && jsonContent.StartsWith("\"") && jsonContent.EndsWith("\""))
            {
                // Remove dupla serialização
                jsonContent = JsonConvert.DeserializeObject<string>(jsonContent);
            }

            // Agora deserializa para o objeto
            var result = JsonConvert.DeserializeObject<ConciliadoraDashboardVendaResponseModel>(jsonContent);

            return result;
        }

        public async Task<ConciliadoraDashboardVendaResponseModel> GetPagamentosAsync(string token, ConciliadoraDashBoardVendasRequest requestModel)
        {
            var url = "https://app-api-wl.conciliadora.com.br/api/Dashboard/Gerencial/GetPagamentos";

            var client = new RestClient(url);
            var request = new RestRequest(url, RestSharp.Method.POST);

            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            request.AddJsonBody(requestModel);


            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Erro ao buscar vendas: {response.StatusCode} - {response.ErrorMessage}");
            }

            // CORREÇÃO AQUI - Lê o conteúdo como string primeiro
            var jsonContent = response.Content;

            // Se o JSON vier com problema de dupla serialização
            if (!string.IsNullOrEmpty(jsonContent) && jsonContent.StartsWith("\"") && jsonContent.EndsWith("\""))
            {
                // Remove dupla serialização
                jsonContent = JsonConvert.DeserializeObject<string>(jsonContent);
            }

            // Agora deserializa para o objeto
            var result = JsonConvert.DeserializeObject<ConciliadoraDashboardVendaResponseModel>(jsonContent);

            return result;
        }

        public async Task<ConciliadoraDashboardDebitosResponseModel> GetDebitosAsync(string token, ConciliadoraDashBoardVendasRequest requestModel)
        {
            var url = "https://app-api-wl.conciliadora.com.br/api/Dashboard/Gerencial/GetDebitos";

            var client = new RestClient(url);
            var request = new RestRequest(url, RestSharp.Method.POST);

            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            request.AddJsonBody(requestModel);


            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Erro ao buscar vendas: {response.StatusCode} - {response.ErrorMessage}");
            }

            // CORREÇÃO AQUI - Lê o conteúdo como string primeiro
            var jsonContent = response.Content;

            // Se o JSON vier com problema de dupla serialização
            if (!string.IsNullOrEmpty(jsonContent) && jsonContent.StartsWith("\"") && jsonContent.EndsWith("\""))
            {
                // Remove dupla serialização
                jsonContent = JsonConvert.DeserializeObject<string>(jsonContent);
            }

            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                Error = (s, e) => { e.ErrorContext.Handled = true; } // ignora erro na propriedade
            };

            // Agora deserializa para o objeto
            var result = JsonConvert.DeserializeObject<ConciliadoraDashboardDebitosResponseModel>(jsonContent, settings);

            return result;
        }

        public async Task<ConciliadoraDashboardTaxaResponse> GetTaxasAsync(string token, ConciliadoraDashBoardVendasRequest requestModel)
        {
            var url = "https://app-api-wl.conciliadora.com.br/api/Dashboard/Gerencial/GetTaxas";

            var client = new RestClient(url);
            var request = new RestRequest(url, RestSharp.Method.POST);

            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            request.AddJsonBody(requestModel);


            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Erro ao buscar vendas: {response.StatusCode} - {response.ErrorMessage}");
            }

            // CORREÇÃO AQUI - Lê o conteúdo como string primeiro
            var jsonContent = response.Content;

            // Se o JSON vier com problema de dupla serialização
            if (!string.IsNullOrEmpty(jsonContent) && jsonContent.StartsWith("\"") && jsonContent.EndsWith("\""))
            {
                // Remove dupla serialização
                jsonContent = JsonConvert.DeserializeObject<string>(jsonContent);
            }

            // Agora deserializa para o objeto
            var result = JsonConvert.DeserializeObject<ConciliadoraDashboardTaxaResponse>(jsonContent);

            return result;
        }

        public async Task<ConciliadoraDashboardInformacoesComplementaresResponse> GetInformacoesComplementaresAsync(string token, ConciliadoraDashBoardVendasRequest requestModel)
        {
            var url = "https://app-api-wl.conciliadora.com.br/api/Dashboard/Gerencial/GetInfoComplementar";

            var client = new RestClient(url);
            var request = new RestRequest(url, RestSharp.Method.POST);

            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            request.AddJsonBody(requestModel);


            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Erro ao buscar vendas: {response.StatusCode} - {response.ErrorMessage}");
            }

            // CORREÇÃO AQUI - Lê o conteúdo como string primeiro
            var jsonContent = response.Content;

            // Se o JSON vier com problema de dupla serialização
            if (!string.IsNullOrEmpty(jsonContent) && jsonContent.StartsWith("\"") && jsonContent.EndsWith("\""))
            {
                // Remove dupla serialização
                jsonContent = JsonConvert.DeserializeObject<string>(jsonContent);
            }

            // Agora deserializa para o objeto
            var result = JsonConvert.DeserializeObject<ConciliadoraDashboardInformacoesComplementaresResponse>(jsonContent);

            return result;
        }
    }
    
}
