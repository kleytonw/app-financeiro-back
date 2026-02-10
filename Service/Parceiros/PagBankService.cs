using ERP_API.Service.Parceiros.Interface;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros
{
    public class PagBankService : IPagBankService
    {
        private readonly HttpClient _httpClient;

        public PagBankService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        async Task<ConsultaTransacaoPagBankResponseModel> IPagBankService.ConsultaTransacaoPagBankAsync(ConsultaPagBankRequestModel request)
        {
            var ediVersion = "v3.00";
            var tipoMovimento = "transactional"; 
            var data = request.DataConsulta.ToString("yyyy-MM-dd");
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            var url = $"https://edi.api.pagbank.com.br/movement/{ediVersion}/{tipoMovimento}/{data}?pageNumber={pageNumber}&pageSize={pageSize}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("user", request.User);     
            _httpClient.DefaultRequestHeaders.Add("token", request.Token);   

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro na chamada PagBank: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            var content = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<ConsultaTransacaoPagBankResponseModel>(content);

            return resultado!;
        }

        async Task<ConsultaPagamentoPagBankResponseModel> IPagBankService.ConsultaPagamentoPagBankAsync(ConsultaPagBankRequestModel request)
        {
            var ediVersion = "v3.00";
            var tipoMovimento = "financial";
            var data = request.DataConsulta.ToString("yyyy-MM-dd");
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            var url = $"https://edi.api.pagbank.com.br/movement/{ediVersion}/{tipoMovimento}/{data}?pageNumber={pageNumber}&pageSize={pageSize}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("user", request.User);
            _httpClient.DefaultRequestHeaders.Add("token", request.Token);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro na chamada PagBank: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            var content = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<ConsultaPagamentoPagBankResponseModel>(content);

            return resultado!;
        }

        async Task<ConsultaCashBackPagBankResponseModel> IPagBankService.ConsultaCashBackPagBankAsync(ConsultaPagBankRequestModel request)
        {
            var ediVersion = "v3.00";
            var tipoMovimento = "cashouts";
            var data = request.DataConsulta.ToString("yyyy-MM-dd");
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            var url = $"https://edi.api.pagbank.com.br/movement/{ediVersion}/{tipoMovimento}/{data}?pageNumber={pageNumber}&pageSize={pageSize}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("user", request.User);
            _httpClient.DefaultRequestHeaders.Add("token", request.Token);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro na chamada PagBank: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            var content = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<ConsultaCashBackPagBankResponseModel>(content);

            return resultado!;
        }
    }
}
