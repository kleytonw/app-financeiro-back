using ERP_API.Service.Parceiros.Interface;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace ERP_API.Service.Parceiros
{
    public class TecnospeeedService : ITecnospeedService
    {
        private readonly HttpClient _httpClient;

        public TecnospeeedService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        async Task<CadastroPagadorResponseModel> ITecnospeedService.CadastroPagadorTecnospeed(CadastroPagadorRequestModel request, string cnpjsh, string tokensh, string url)
        {
            

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url + "/api/v1/payer");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };

            var jsonRequest = JsonSerializer.Serialize(request, options);
            Console.WriteLine(jsonRequest);
            requestMessage.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao cadastrar pagador: {response.StatusCode}, Detalhes:{errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CadastroPagadorResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        async Task<CadastroPagadorResponseModel> ITecnospeedService.BuscarPagadorTecnospeed(string cnpjPagador, string cnpjsh, string tokensh, string url)
        {


            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url + "/api/v1/payer");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);
            requestMessage.Headers.Add("payercpfcnpj", cnpjPagador);

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao cadastrar pagador: {response.StatusCode}, Detalhes:{errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CadastroPagadorResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        async Task<AtualizarPagadorResponseModel> ITecnospeedService.AtualizarPagadorTecnospeed(AtualizarPagadorRequestModel request, Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url + "/api/v1/payer");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);
            requestMessage.Headers.Add("payercpfcnpj", unidade.CpfCnpj);

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };

            var jsonRequest = JsonSerializer.Serialize(request, options);
            Console.WriteLine(jsonRequest);
            requestMessage.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao cadastrar pagador: {response.StatusCode}, Detalhes:{errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AtualizarPagadorResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }


        async Task<CadastrarContaListResponseModel> ITecnospeedService.CadastrarContaTecnospeed(CadastrarContaRequestListModel request, Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url + "/api/v1/account");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);
            requestMessage.Headers.Add("payercpfcnpj", unidade.CpfCnpj);

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };

            var jsonRequest = JsonSerializer.Serialize(request.Accounts, options);
            requestMessage.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao cadastrar pagador: {response.StatusCode}, Detalhes:{errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CadastrarContaListResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        async Task<AtualizarContaResponseModel> ITecnospeedService.AtualizarContaTecnospeed(AtualizarContaRequestModel request, Unidade unidade, string hashDaConta, string cnpjsh, string tokensh, string url)
        {

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url + $"/api/v1/account/{hashDaConta}");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);
            requestMessage.Headers.Add("payercpfcnpj", unidade.CpfCnpj);

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };

            var jsonRequest = JsonSerializer.Serialize(request, options);
            Console.WriteLine(jsonRequest);
            requestMessage.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao cadastrar pagador: {response.StatusCode}, Detalhes:{errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AtualizarContaResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        async Task<DeletarContaResponseModel> ITecnospeedService.DeletarContaTecnospeed(DeletarContaRequestModel request, Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url + "/api/v1/account");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);
            requestMessage.Headers.Add("payercpfcnpj", unidade.CpfCnpj);

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };

            var jsonRequest = JsonSerializer.Serialize(request, options);
            Console.WriteLine(jsonRequest);
            requestMessage.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao cadastrar pagador: {response.StatusCode}, Detalhes:{errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DeletarContaResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        async Task<BilhetagemTecnospeedResponseModel> ITecnospeedService.BilhetagemTecnospeed(BilhetagemTecnospeedRequestModel request, string cnpjsh, string tokensh, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url + "/api/v1/report");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };

            var jsonRequest = JsonSerializer.Serialize(request, options);
            requestMessage.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao consultar bilhetagem: {response.StatusCode}, Detalhes: {errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BilhetagemTecnospeedResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        async Task<CriarNotificacaoTecnospeedResponseModel> ITecnospeedService.CriarNotificacaoTecnospeed(CriarNotificacaoTecnospeedRequestModel request, Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url + "/api/v1/notification");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);
            requestMessage.Headers.Add("payercpfcnpj", unidade.CpfCnpj);

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };

            var jsonRequest = JsonSerializer.Serialize(request, options);
            requestMessage.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao consultar bilhetagem: {response.StatusCode}, Detalhes: {errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CriarNotificacaoTecnospeedResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        async Task<ListarNotificacaoTecnospeedResponseModel> ITecnospeedService.ListarNotificacaoTecnospeed(Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url + "/api/v1/notification");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);
            requestMessage.Headers.Add("payercpfcnpj", unidade.CpfCnpj);

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao consultar Notificação: {response.StatusCode}, Detalhes: {errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ListarNotificacaoTecnospeedResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        async Task<DeletarNotificacaoTecnospeedResponseModel> ITecnospeedService.DeletarNotificacaoTecnospeed(Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url + "/api/v1/notification");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);
            requestMessage.Headers.Add("payercpfcnpj", unidade.CpfCnpj);

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };

            var requestBody = new
            {
                uniqueId = new List<string> { unidade.UniqueId }
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody, options);
            requestMessage.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao consultar bilhetagem: {response.StatusCode}, Detalhes: {errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DeletarNotificacaoTecnospeedResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        public async Task<EnviarExtratoTecnospeedResponseModel> EnviarExtratoTecnospeed(EnviarExtratoTecnospeedRequestModel request, Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            using var formData = new MultipartFormDataContent();

            formData.Headers.Add("cnpjsh", cnpjsh);
            formData.Headers.Add("tokensh", tokensh);
            formData.Headers.Add("payercpfcnpj", unidade.CpfCnpj);

            if (request.File != null)
            {
                var streamContent = new StreamContent(request.File.OpenReadStream());
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                formData.Add(streamContent, "file", request.File.FileName);
            }
            else
            {
                throw new ArgumentException("O arquivo é obrigatório para enviar o extrato.");
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url + "/api/v1/statement/parser")
            {
                Content = formData
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao enviar extrato. Status: {response.StatusCode}, Detalhes: {errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EnviarExtratoTecnospeedResponseModel>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }

        async Task<ConsultarExtratoTecnospeedResponseModel> ITecnospeedService.ConsultarExtratoTecnospeed(Unidade unidade, Extrato extrato, string cnpjsh, string tokensh, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url + $"/api/v1/statement/parser/{extrato.UniqueId}");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);
            requestMessage.Headers.Add("payercpfcnpj", unidade.CpfCnpj);
            //requestMessage.Headers.Add("Content-Type", "application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao consultar bilhetagem: {response.StatusCode}, Detalhes: {errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ConsultarExtratoTecnospeedResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        async Task<ConsultarExtratoPorPeriodoTecnospeedResponseModel> ITecnospeedService.ConsultarExtratoPorPeriodoTecnospeed(ConsultarExtratoPorPeriodoTecnospeedRequestModel request, Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "dateStart", request.DateStart.ToString("yyyy-MM-dd") }, 
                { "dateEnd", request.DateEnd.ToString("yyyy-MM-dd") }
            };

            if (!string.IsNullOrEmpty(request.BankCode))
                queryParams.Add("bankCode", request.BankCode);

            if (!string.IsNullOrEmpty(request.Type))
                queryParams.Add("type", request.Type);

            if (!string.IsNullOrEmpty(request.AccountHash))
                queryParams.Add("accountHash", request.AccountHash);

            if (request.Page.HasValue)
                queryParams.Add("page", request.Page.Value.ToString());

            if (request.Limit.HasValue)
                queryParams.Add("limit", request.Limit.Value.ToString());

            var requestUrl = QueryHelpers.AddQueryString(url + "/api/v1/statement", queryParams);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);
            requestMessage.Headers.Add("payercpfcnpj", unidade.CpfCnpj);
            requestMessage.Headers.Add("Content-Type", "application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao consultar extrato: {response.StatusCode}, Detalhes: {errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ConsultarExtratoPorPeriodoTecnospeedResponseModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        async Task<byte[]> ITecnospeedService.BaixarExtratoTecnospeed(Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url + $"/api/v1/statement/{unidade.UniqueId}/download");
            requestMessage.Headers.Add("cnpjsh", cnpjsh);
            requestMessage.Headers.Add("tokensh", tokensh);
            requestMessage.Headers.Add("payercpfcnpj", unidade.CpfCnpj);
            requestMessage.Headers.Add("Content-Type", "application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao consultar bilhetagem: {response.StatusCode}, Detalhes: {errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            byte[] fileBytes = Convert.FromBase64String(responseContent);

            return fileBytes;
        }


    }
}
