using ERP_API.Models;
using ERP_API.Service.Parceiros.Interface;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros
{
    public class NexxeraService : INexxeraService
    {
        private readonly HttpClient _httpCliente;

        public NexxeraService(IHttpClientFactory httpClientFactory)
        {
            _httpCliente = httpClientFactory.CreateClient();
        }

        async Task<ArquivoListagemNexxeraResponse> INexxeraService.ListagemArquivos(ListagemArquivosNexxeraRequest model)
        {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api-sandbox.nexxera.com/skyline/api/v1/files?{model.InitialDate}&{model.FinalDate}");

                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("service-token", "••••••");
                var content = new StringContent(string.Empty);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = content;
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro ao exibir listagem de arquivo: {response.StatusCode}, Detalhes:{errorResponse}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ArquivoListagemNexxeraResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result;

        }

        async Task<RedisponibilizarArquivoNexxeraResponse> INexxeraService.RedisponibilizarArquivo(RedisponibilizarArquivoNexxeraRequest model)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, "https://api-sandbox.nexxera.com/skyline/api/v1/files/unread");

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("service-token", "••••••");

            var content = JsonContent.Create(model.Filenames);
            
            request.Content = content;
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao exibir listagem de arquivo: {response.StatusCode}, Detalhes:{errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RedisponibilizarArquivoNexxeraResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;

        }

        async Task<DownloadNexxeraResponse> INexxeraService.Download(DownloadNexxeraRequest model)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api-sandbox.nexxera.com/skyline/api/v1/requests/download");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("service-token", "••••••");


            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };

            var content = JsonSerializer.Serialize(model, options);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao exibir listagem de arquivo: {response.StatusCode}, Detalhes:{errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DownloadNexxeraResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;

        }

        async Task<UploadNexxeraResponse> INexxeraService.Upload(UploadNexxeraRequest model)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api-sandbox.nexxera.com/skyline/api/v1/requests/upload");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("service-token", "••••••");

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
            };

            var content = JsonSerializer.Serialize(model, options);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao exibir listagem de arquivo: {response.StatusCode}, Detalhes:{errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UploadNexxeraResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }
    }
}

