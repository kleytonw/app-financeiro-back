using ERP_API.Service.Parceiros.Interface;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static ERP_API.Service.Parceiros.ConsultaVendaRedeResponseModel;
using System.Web;
using System.Collections.Generic;


namespace ERP_API.Service.Parceiros
{
    public class RedeService : IRedeService
    {
        private readonly HttpClient _httpClient;

        public RedeService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        async Task<LoginResponseModel>  IRedeService.LoginRedeAsync(LoginRequestModel request)
        {

            try
            {
                string url = request.Url;

               
                var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{request.UserName}:{request.Password}"));

                using var requisicao = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {

                    { "grant_type", "client_credentials" } 
                    })
                };

                requisicao.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                requisicao.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using var response = await _httpClient.SendAsync(requisicao);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro ao obter token: {response.StatusCode} - {error}");
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<LoginResponseModel>(responseBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return jsonResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao autenticar: {ex.Message}");
            }


        }

        async Task<ConsultaVendaResponseModel> IRedeService.ConsultaVendaRedeAsync(ConsultarVendaRedeRequestModel request)
        {

            using var requisicao = new HttpRequestMessage(HttpMethod.Get, request.Url);
            requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer", request.Authorization);
            requisicao.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["parentCompanyNumber"] = request.ParentMerchantId;
            query["subsidiaries"] = request.Subsidiaries;
            query["startDate"] = request.StartDate;
            query["endDate"] = request.EndDate;
            query["pageKey"] = request.PageKey;

            requisicao.RequestUri = new Uri($"{request.Url}?{query}");

            using var response = await _httpClient.SendAsync(requisicao);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao obter os dados da venda: {response.StatusCode} - {error}");
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<ConsultaVendaResponseModel>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return jsonResponse;
        }

        async Task<ConsultaPagamentoRedeResponseModel> IRedeService.ConsultaPagamentoRedeAsync(ConsultaPagamentoRedeRequestModel request)
        {
            using var requisicao = new HttpRequestMessage(HttpMethod.Get, request.Url);
            requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer", request.Authorization);
            requisicao.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["parentCompanyNumber"] = request.ParentCompanyNumber;
            query["subsidiaries"] = request.Subsidiaries;
            query["startDate"] = request.StartDate;
            query["endDate"] = request.EndDate;
            query["pageKey"] = request.PageKey;

            if (request.Size.HasValue)
                query["size"] = request.Size.Value.ToString();

            if (!string.IsNullOrEmpty(request.PageKey))
                query["pageKey"] = request.PageKey;

            if (request.Brands.HasValue)
                query["brands"] = request.Brands.Value.ToString();

            if (!string.IsNullOrEmpty(request.Status))
                query["status"] = request.Status;

            if (!string.IsNullOrEmpty(request.Types))
                query["types"] = request.Types;

            requisicao.RequestUri = new Uri($"{request.Url}?{query}");

            using var response = await _httpClient.SendAsync(requisicao);

            if (!response.IsSuccessStatusCode)
            {
                string erro = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao obter os dados do pagamento: {response.StatusCode} - {erro}");
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<ConsultaPagamentoRedeResponseModel>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return jsonResponse;
        }

        async Task<ConsultarPagamentoDiarioRedeResponseModel> IRedeService.ConsultarPagamentoDiarioRedeAsync(ConsultarPagamentoDiarioRedeRequestModel request)
        {
            using var requisicao = new HttpRequestMessage(HttpMethod.Get, request.Url);
            requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer", request.Authorization);
            requisicao.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["parentCompanyNumber"] = request.ParentCompanyNumber.ToString();
            query["startDate"] = request.StartDate;
            query["endDate"] = request.EndDate;
            query["pageKey"] = request.PageKey;

            if(request.StatusCodes.HasValue)
                query["statusCodes"] = request.StatusCodes.Value.ToString();

            if (!string.IsNullOrEmpty(request.BankAccounts))
                query["bankAccounts"] = request.BankAccounts;

            if (!string.IsNullOrEmpty(request.PaymentIds))
                query["paymentIds"] = request.PaymentIds;

            if (request.Size.HasValue)
                query["size"] = request.Size.Value.ToString();

            if (!string.IsNullOrEmpty(request.PageKey))
                query["pageKey"] = request.PageKey;

            if (request.Brands.HasValue)
                query["brands"] = request.Brands.Value.ToString();

            if (!string.IsNullOrEmpty(request.Status))
                query["status"] = request.Status;

            if (!string.IsNullOrEmpty(request.Types))
                query["types"] = request.Types;

            requisicao.RequestUri = new Uri($"{request.Url}?{query}");

            using var response = await _httpClient.SendAsync(requisicao);

            if (!response.IsSuccessStatusCode)
            {
                string erro = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao obter os dados do pagamento: {response.StatusCode} - {erro}");
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<ConsultarPagamentoDiarioRedeResponseModel>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return jsonResponse;
        }


    }
}
