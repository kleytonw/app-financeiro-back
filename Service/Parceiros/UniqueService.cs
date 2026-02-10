using Azure;
using ERP_API.Domain.Entidades;
using ERP_API.Models.NotaFiscal;
using ERP_API.Service.Parceiros.Interface;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros
{
    public class UniqueService : IUniqueService
    {
        private readonly HttpClient _httpClient;
        private string _url { get; set; }

        public UniqueService(HttpClient httpClient)
        {
            _httpClient = httpClient;
             _url = "https://unique-prod-b0b2ecbybcc7frc3.brazilsouth-01.azurewebsites.net/api";
            // _url = "https://localhost:5001/api";

        }

        public async Task<string> GerarAccessTokenAsync(string login, string senha, string url)
        {
            try
            {
                // Criar o objeto de requisição
                var loginRequest = new
                {
                    login = login,
                    senha = senha
                };

                // Serializar para JSON
                var content = new StringContent(
                    JsonConvert.SerializeObject(loginRequest),
                    Encoding.UTF8,
                    "application/json"
                );

                // Fazer a requisição
                var response = await _httpClient.PostAsync(url + "/api/Usuario/login", content);

                // Verificar se foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserializar a resposta (baseado na sua imagem da API)
                    var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // Extrair o token baseado na estrutura real da resposta
                    if (result?.data?.token != null)
                    {
                        return result.data.token.ToString();
                    }

                    throw new Exception("Token não encontrado na resposta da API");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro na autenticação: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao gerar token de acesso: {ex.Message}", ex);
            }
        }
        public async Task<CriarCobrancaResponse> CriarCobrancaAsync(CobrancaRequest request, string token, string url)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);

                // Serializar o request para JSON
                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                // Criar uma requisição HTTP com o token no cabeçalho
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url + "/api/Transacao/criar-boleto"))
                {
                    requestMessage.Content = content;

                    // ADICIONAR O TOKEN NO CABEÇALHO AUTHORIZATION
                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    // Fazer a requisição POST
                    var response = await _httpClient.SendAsync(requestMessage);

                    // Verificar se foi bem-sucedida
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserializar a resposta
                        var result = JsonConvert.DeserializeObject<CriarCobrancaResponse>(responseContent);
                        return result;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Erro ao criar cobrança: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar cobrança: {ex.Message}", ex);
            }
        }

        public async Task<HttpResponseMessage> ConsultarCobrancaAsync(int idTransacao, string token, string url)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { idTransacao, token }), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url + "/api/Transacao/obterPorId", content);
        }

        public async Task<HttpResponseMessage> CancelarCobrancaAsync(int? idTransacao, string token, string url)
        {
            try
            {

                var requestBody = new { idTransacao = idTransacao };


                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url + "/api/Transacao/cancelar"))
                {
                    requestMessage.Content = content;

                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var response = await _httpClient.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<HttpResponseMessage>(responseContent);
                        return result;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        //throw new Exception($"Erro ao criar cobrança: {response.StatusCode} - {errorContent}");
                        var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        errorResponse.Content = new StringContent("Erro ao cancelar o boleto.");
                        return errorResponse;
                    }


                }
            }

            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar cobrança: {ex.Message}", ex);
            }
        }

        public async Task<HttpResponseMessage> ConsultarCobrancasAsync(FiltroCobrancaRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("/api/Transacao/listar", content);
        }

        public async Task<HttpResponseMessage> DepositarAsync(DepositoRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("/api/Deposito/criar", content);
        }

        public async Task<NotaFiscalResponseModel> CriarNfeAsync(CriarNotaFiscalRequestModel request, string token, string usuarioInclusao)
        {
            try
            {

                var requestBody = new CriarServicoUniqueRequestModel
                {
                    IdSacado = request.IdSacadoUnique,
                    IdCliente = 25, // concicard fixo por enquanto
                    NomeCliente = request.NomeCliente,
                    ChaveAcesso = "-",
                    TotalServico = request.Valor,
                    Serie = 1,
                    DataHoraInclusao = DateTime.UtcNow,
                    Observacao = "",
                    Itens = new List<ItemServicoUniqueRequestModel>
                    {
                        new ItemServicoUniqueRequestModel
                        {
                            IdNotaFiscalItem = 1,
                            NomeServico = "Exploracao",
                            CodigoServico = "03.03",
                            ValorServico = request.Valor,
                            ValorISS = 3.01m,
                            Aliquota = 3.01m
                        }
                    }
                };


                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, _url + "/notafiscal/criar"))
                {
                    requestMessage.Content = content;

                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var response = await _httpClient.SendAsync(requestMessage);

                    var responseContent = await response.Content.ReadAsStringAsync();


                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Erro ao criar NFe: {response.StatusCode} - {responseContent}");
                    }

                    var result = JsonConvert.DeserializeObject<NotaFiscalResponseModel>(responseContent);

                    return result;
                }
            }

            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar cobrança: {ex.Message}", ex);
            }
        }

        public async Task<NotaFiscalResponseModel> ConsultaNfeAsync(int idNotaFiscal, string token, string usuarioLogado)
        {
            try
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Delete,
                    $"{_url}/notafiscal/?idNotaFiscal={idNotaFiscal}");

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(
                        $"Erro ao excluir NFe: {(int)response.StatusCode} - {responseContent}");
                }

                return JsonConvert.DeserializeObject<NotaFiscalResponseModel>(responseContent)
                       ?? throw new Exception("Resposta inválida da API.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir nota fiscal: {ex.Message}", ex);
            }
        }

        public async Task<NotaFiscalResponseModel> ExcluirNfeAsync(int idNotaFiscal, string token, string usuarioExclusao)
        {
            try
            { 
                using (var requestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"{_url}/notafiscal/excluir/?idNotaFiscal={idNotaFiscal}"))
                            {
                    requestMessage.Headers.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var response = await _httpClient.SendAsync(requestMessage);

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(
                            $"Erro ao excluir NFe: {response.StatusCode} - {responseContent}");
                    }

                    var result = JsonConvert.DeserializeObject<NotaFiscalResponseModel>(responseContent);

                    return result;
                }
            }

            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar cobrança: {ex.Message}", ex);
            }
        }
    }
}
