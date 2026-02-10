using ERP_API.Service.Parceiros;
using ERP_API.Service.Parceiros.Interface;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using RestSharp;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using ERP_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using WhatsappBusiness.CloudApi.Messages.Requests;
using Azure.Core;
using System.Linq;

namespace ERP_API.Service
{
    public class ConciliadoraService : IConciliadoraService
    {
        public async Task<(bool Success, string ErrorMessage, AdquirentesConciliadoraResponse Data)> ListaAdquirenteConciliadoraResponse1(string apiKey, int? top, int? skip)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/Adquirentes";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", apiKey);

                string url = "";

                if ((skip == null || skip == 0) && (top == null || top == 0))
                {
                    url = $"{urlBase}";
                }
                else
                { 
                    url = $"{urlBase}?$top={top}&$skip={skip}&$count=true";
                }


                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<AdquirentesConciliadoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, null, resultado);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (false, $"Erro ao fazer requisição: {response.StatusCode} - {json}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, VendasCanceladasConciladoraResponse Data)> VendasCanceladas(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/CancelamentoVenda";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", apiKey);

                var filtro = $"$filter=DataVenda ge {dataInicio:yyyy-MM-ddTHH:mm:ssZ} and DataVenda le {dataFim:yyyy-MM-ddTHH:mm:ssZ} and RefoId eq {identificadorConciliadora}";
                if (adquirente > 0)
                {
                    filtro += $" and AdqId eq {adquirente}";
                }
                if (produto > 0)
                {
                    filtro += $" and CodigoProduto eq {produto}";
                }
                if (!string.IsNullOrEmpty(nsu))
                {
                    filtro += $" and Nsu eq '{nsu}'";
                }

                var url = $"{urlBase}?{filtro}&$top={top}&$skip={skip}&$count=true";

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<VendasCanceladasConciladoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, null, resultado);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (false, $"Erro ao consultar vendas canceladas: {response.StatusCode} - {json}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, ConciliacaoSistemaConciliadoraResponse Data)> ConciliacaoSistema(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/ConsultaConciliacaoSistema";
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", apiKey);

                var filtro = $"$filter=DataVenda ge {dataInicio:yyyy-MM-ddTHH:mm:ssZ} and DataVenda le {dataFim:yyyy-MM-ddTHH:mm:ssZ} and RefoId eq {identificadorConciliadora}";

                if (adquirente > 0)
                {
                    filtro += $" and AdqId eq {adquirente}";
                }
                if (produto > 0)
                {
                    filtro += $" and CodigoProduto eq {produto}";
                }
                if (!string.IsNullOrEmpty(nsu))
                {
                    filtro += $" and Nsu eq '{nsu}'";
                }

                var url = $"{urlBase}?{filtro}&$top={top}&$skip={skip}&$count=true";

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<ConciliacaoSistemaConciliadoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, null, resultado);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (false, $"Erro ao consultar conciliação sistema: {response.StatusCode} - {json}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, PagamentosConciliadoraResponse Data)> PagamentosRecebidos(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/ConsultaPagamento";
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", apiKey);

                var filtro = $"$filter=DataPagamento ge {dataInicio:yyyy-MM-ddTHH:mm:ssZ} and DataPagamento le {dataFim:yyyy-MM-ddTHH:mm:ssZ} and RefoId eq {identificadorConciliadora}";

                if (adquirente > 0)
                {
                    filtro += $" and AdqId eq {adquirente}";
                }
                if (produto > 0)
                {
                    filtro += $" and CodigoProduto eq {produto}";
                }
                if (!string.IsNullOrEmpty(nsu))
                {
                    filtro += $" and Nsu eq '{nsu}'";
                }

                string url = "";


                if ((skip == null || skip == 0) && (top == null || top == 0))
                {
                    url = $"{urlBase}?{filtro}";
                }
                else
                {
                    url = $"{urlBase}?{filtro}&$top={top}&$skip={skip}&$count=true";
                }

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<PagamentosConciliadoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, null, resultado);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (false, $"Erro ao consultar pagamentos: {response.StatusCode} - {json}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, PrevisaoPagamentoConciliadoraResponse Data)> PrevisaoPagamento(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/ConsultaPrevisaoPagamento";
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", apiKey);

                var filtro = $"$filter=DataVenda ge {dataInicio:yyyy-MM-ddTHH:mm:ssZ} and DataVenda le {dataFim:yyyy-MM-ddTHH:mm:ssZ} and RefoId eq {identificadorConciliadora}";

                if (adquirente > 0)
                {
                    filtro += $" and AdqId eq {adquirente}";
                }
                if (produto > 0)
                {
                    filtro += $" and CodigoProduto eq {produto}";
                }
                if (!string.IsNullOrEmpty(nsu))
                {
                    filtro += $" and Nsu eq '{nsu}'";
                }

                var url = $"{urlBase}?{filtro}&$top={top}&$skip={skip}&$count=true";

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<PrevisaoPagamentoConciliadoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, null, resultado);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (false, $"Erro ao consultar previsão de pagamento: {response.StatusCode} - {json}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, List<StatusProcessamentoResponse> Data)> StatusProcessamento(StatusProcessamentoConciladoraRequest model)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/ConsultaStatusProcessamento";

                int top = 100;
                int skip = 0;
                bool continuar = true;
                var resultadoFinal = new List<StatusProcessamentoResponse>();

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", model.ApiKey);

                while (continuar)
                {
                    var filtro = $"$filter=IdEmpresa eq {model.IdEmpresa} and " +
                                 $"DataInicio eq {model.DataInicio:yyyy-MM-dd} and " +
                                 $"DataFim eq {model.DataFim:yyyy-MM-dd} and " +
                                 $"TipoArquivo eq '{model.TipoArquivo}' and " +
                                 $"Status eq '{model.Status}'";

                    var url = $"{urlBase}?{filtro}&$top={top}&$skip={skip}&$count=true";

                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorJson = await response.Content.ReadAsStringAsync();
                        return (false, $"Erro ao consultar status processamento: {response.StatusCode} - {errorJson}", null);
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var resultado = JsonSerializer.Deserialize<StatusProcessamentoConciliadoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (resultado?.Value != null)
                    {
                        resultadoFinal.AddRange(resultado.Value);
                        if (resultado.Value.Count < top)
                            continuar = false;
                        else
                            skip += top;
                    }
                    else continuar = false;
                }

                return (true, null, resultadoFinal);
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, VendasConciliadoraResponse Data)> Vendas(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu, int? modalidade)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/ConsultaVenda";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", apiKey);

                var filtro = $"$filter=DataVenda ge {dataInicio:yyyy-MM-ddTHH:mm:ssZ} and DataVenda le {dataFim:yyyy-MM-ddTHH:mm:ssZ} and RefoId eq {identificadorConciliadora}";

                if (adquirente > 0)
                {
                    filtro += $" and AdqId eq {adquirente}";
                }
                if (produto > 0)
                {
                    filtro += $" and CodigoProduto eq {produto}";
                }
                if (!string.IsNullOrEmpty(nsu))
                {
                    filtro += $" and Nsu eq '{nsu}'";
                }

                if (modalidade > 0)
                {
                    filtro += $" and CodigoModalidade eq {modalidade}";
                }


                string url = "";


                if ((skip == null || skip == 0) && (top == null || top == 0))
                {
                    url = $"{urlBase}?{filtro}"; ;
                }
                else
                {
                    url = $"{urlBase}?{filtro}&$top={top}&$skip={skip}&$count=true"; ;
                }

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<VendasConciliadoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, null, resultado);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (false, $"Erro ao consultar vendas: {response.StatusCode} - {json}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, VendasConciliadasConciliadoraResponse Data)> VendasConciliadas(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu, int? modalidade)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/ConsultaVendasConciliadas";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", apiKey);

                var filtro = $"$filter=DataVenda ge {dataInicio:yyyy-MM-ddTHH:mm:ssZ} and DataVenda le {dataFim:yyyy-MM-ddTHH:mm:ssZ} and RefoId eq {identificadorConciliadora}";

                if (adquirente > 0)
                {
                    filtro += $" and AdqId eq {adquirente}";
                }
                if (produto > 0)
                {
                    filtro += $" and CodigoProduto eq {produto}";
                }
                if (!string.IsNullOrEmpty(nsu))
                {
                    filtro += $" and Nsu eq '{nsu}'";
                }

                if (modalidade != null)
                {
                    filtro += $" and CodigoModalidade eq {modalidade}";
                }

                var url = $"{urlBase}?{filtro}&$top={top}&$skip={skip}&$count=true";

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<VendasConciliadasConciliadoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, null, resultado);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (false, $"Erro ao consultar vendas conciliadas: {response.StatusCode} - {json}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, VendasNaoEnviadasConciliadoraResponse Data)> VendasNaoEnviadas(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string apiKey, int? top, int? skip, int? adquirente, int? produto, string nsu, int? modalidade)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/ConsultaVendaNaoEnviada";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", apiKey);

                var filtro = $"$filter=DataVenda ge {dataInicio:yyyy-MM-ddTHH:mm:ssZ} and DataVenda le {dataFim:yyyy-MM-ddTHH:mm:ssZ} and RefoId eq {identificadorConciliadora}";

                if (adquirente > 0)
                {
                    filtro += $" and AdqId eq {adquirente}";
                }
                if (produto > 0)
                {
                    filtro += $" and CodigoProduto eq {produto}";
                }
                if (!string.IsNullOrEmpty(nsu))
                {
                    filtro += $" and Nsu eq '{nsu}'";
                }

                if (modalidade > 0)
                {
                    filtro += $" and CodigoModalidade eq {modalidade}";
                }

                var url = $"{urlBase}?{filtro}&$top={top}&$skip={skip}&$count=true";

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<VendasNaoEnviadasConciliadoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, null, resultado);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (false, $"Erro ao consultar vendas não enviadas: {response.StatusCode} - {json}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, MeioCapturaConciliadoraResponse Data)> MeioCaptura(string apiKey, int? top, int? skip)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/MeiosCaptura";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", apiKey);

                string url = "";

                if ((skip == null || skip == 0) && (top == null || top == 0))
                {
                    url = $"{urlBase}";
                }
                else
                {

                    url = $"{urlBase}?$top={top}&$skip={skip}&$count=true";
                }

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<MeioCapturaConciliadoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, null, resultado);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (false, $"Erro ao consultar meios de captura: {response.StatusCode} - {json}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, ModalidadeConciliadoraResponse Data)> Modalidade(string apiKey, int? top, int? skip)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/Modalidades";

                using var client = new HttpClient();

                string url = "";

                client.DefaultRequestHeaders.Add("Authorization", apiKey);

                if ((skip == null || skip == 0) && (top == null || top == 0))
                {
                    url = $"{urlBase}";
                }
                else
                {

                    url = $"{urlBase}?$top={top}&$skip={skip}&$count=true";
                }

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<ModalidadeConciliadoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, null, resultado);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (false, $"Erro ao consultar modalidades: {response.StatusCode} - {json}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, ProdutoConciliadoraResponse Data)> Produto(string apiKey, int? top, int? skip)
        {
            try
            {
                var urlBase = "https://api.conciliadora.com.br/api/Produtos";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", apiKey);

                string url = "";

                if ((skip == null || skip == 0) && (top == null || top == 0))
                {
                    url = $"{urlBase}";
                }
                else
                {

                    url = $"{urlBase}?$top={top}&$skip={skip}&$count=true";
                }

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<ProdutoConciliadoraResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return (true, null, resultado);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (false, $"Erro ao consultar produtos: {response.StatusCode} - {json}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string ErrorMessage, ErroXmlResponse Data)> EnviarVendaSistemaAsync(string senha, string idEmpresa, IFormFile arquivoXml)
        {
            try
            {
                if (arquivoXml == null || arquivoXml.Length == 0)
                    return (false, "Arquivo não enviado", null);

                var client = new RestClient("https://api.conciliadora.com.br/api/EnvioVendaSistema");
                client.Timeout = -1;

                var request = new RestRequest(RestSharp.Method.POST);
                request.AlwaysMultipartFormData = true;

                request.AddParameter("senha", senha);
                request.AddParameter("idEmpresa", idEmpresa);

                using var memoryStream = new MemoryStream();
                await arquivoXml.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                request.AddFile("file", fileBytes, arquivoXml.FileName, "application/xml");

                IRestResponse response = await client.ExecuteAsync(request);


                if (response.IsSuccessful)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var vendaResponse = JsonSerializer.Deserialize<ErroXmlResponse>(response.Content, options);
                    return (true, null, vendaResponse);
                }
                else
                {
                    return (false, $"Erro ao enviar venda: {response.StatusCode} - {response.Content}", null);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
            catch (TaskCanceledException ex)
            {
                return (false, "Timeout na requisição", null);
            }
            catch (JsonException ex)
            {
                return (false, $"Erro ao processar JSON: {ex.Message}", null);
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message, null);
            }
            catch (Exception ex)
            {
                return (false, $"Erro interno: {ex.Message}", null);
            }
        }

        public static string GerarXmlVendas(CabecalhoXml cabecalho, List<RegistroVenda> registros)
        {
            var xml = new StringBuilder();
            xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xml.AppendLine("<registros>");

            // Cabeçalho
            xml.AppendLine("  <cabecalho>");
            xml.AppendLine($"    <Empresa>{cabecalho.Empresa}</Empresa>");
            xml.AppendLine($"    <DataInicial>{cabecalho.DataInicial}</DataInicial>");
            xml.AppendLine($"    <DataFinal>{cabecalho.DataFinal}</DataFinal>");
            xml.AppendLine($"    <Versao>{cabecalho.Versao}</Versao>");
            if (!string.IsNullOrEmpty(cabecalho.Lote))
                xml.AppendLine($"    <Lote>{cabecalho.Lote}</Lote>");
            xml.AppendLine($"    <NomeSistema>{cabecalho.NomeSistema}</NomeSistema>");
            xml.AppendLine("  </cabecalho>");

            // Registros
            foreach (var registro in registros)
            {
                xml.AppendLine("  <registro>");
                xml.AppendLine($"    <Produto>{registro.Produto}</Produto>");
                if (!string.IsNullOrEmpty(registro.DescricaoTipoProduto))
                    xml.AppendLine($"    <DescricaoTipoProduto>{registro.DescricaoTipoProduto}</DescricaoTipoProduto>");
                xml.AppendLine($"    <CodigoAutorizacao>{registro.CodigoAutorizacao}</CodigoAutorizacao>");
                if (!string.IsNullOrEmpty(registro.IdentificadorPagamento))
                    xml.AppendLine($"    <IdentificadorPagamento>{registro.IdentificadorPagamento}</IdentificadorPagamento>");
                xml.AppendLine($"    <DataVenda>{registro.DataVenda}</DataVenda>");
                if (!string.IsNullOrEmpty(registro.DataVencimento))
                    xml.AppendLine($"    <DataVencimento>{registro.DataVencimento}</DataVencimento>");
                xml.AppendLine($"    <ValorVendaParcela>{registro.ValorVendaParcela}</ValorVendaParcela>");
                if (!string.IsNullOrEmpty(registro.ValorLiquidoParcela))
                    xml.AppendLine($"    <ValorLiquidoParcela>{registro.ValorLiquidoParcela}</ValorLiquidoParcela>");
                xml.AppendLine($"    <TotalVenda>{registro.TotalVenda}</TotalVenda>");
                if (!string.IsNullOrEmpty(registro.Taxa))
                    xml.AppendLine($"    <Taxa>{registro.Taxa}</Taxa>");
                xml.AppendLine($"    <Parcela>{registro.Parcela}</Parcela>");
                xml.AppendLine($"    <TotalDeParcelas>{registro.TotalDeParcelas}</TotalDeParcelas>");
                xml.AppendLine($"    <NSU>{registro.NSU}</NSU>");
                if (!string.IsNullOrEmpty(registro.TID))
                    xml.AppendLine($"    <TID>{registro.TID}</TID>");
                if (!string.IsNullOrEmpty(registro.Terminal))
                    xml.AppendLine($"    <Terminal>{registro.Terminal}</Terminal>");
                xml.AppendLine($"    <MeioCaptura>{registro.MeioCaptura}</MeioCaptura>");
                if (!string.IsNullOrEmpty(registro.Operadora))
                    xml.AppendLine($"    <Operadora>{registro.Operadora}</Operadora>");
                if (!string.IsNullOrEmpty(registro.Modalidade))
                    xml.AppendLine($"    <Modalidade>{registro.Modalidade}</Modalidade>");
                xml.AppendLine("  </registro>");
            }

            xml.AppendLine("</registros>");
            return xml.ToString();
        }
    }
}