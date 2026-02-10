using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;
using Azure.Storage.Sas;
using System.Linq;
using System.Web;

namespace ERP_API.Service
{
        public class BlobStorageService : IBlobStorageService
        {
            private readonly BlobServiceClient _blobServiceClient;
            private readonly BlobContainerClient _containerClient;
            private const string ContainerName = "arquivoserp";

            public BlobStorageService(BlobServiceClient blobServiceClient)
            {
                _blobServiceClient = blobServiceClient;
                _containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
                _containerClient.CreateIfNotExists();
            }

            public async Task<string> UploadAsync(IFormFile file)
            {
                try
                {
                    // Validar extensão
                    var ext = Path.GetExtension(file.FileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".pdf", ".jpeg", ".png", ".webp", ".bmp", ".xml" };

                    if (!Array.Exists(allowedExtensions, e => e == ext))
                    {
                        throw new ArgumentException("Tipo de arquivo não suportado");
                    }

                    // Gerar nome único
                    string nomeArquivo = Guid.NewGuid() + Path.GetExtension(file.FileName);

                    // Upload
                    var blobClient = _containerClient.GetBlobClient(nomeArquivo);
                    await blobClient.UploadAsync(file.OpenReadStream(), true);

                    return nomeArquivo;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao fazer upload: {ex.Message}", ex);
                }
            }

            public async Task<(MemoryStream stream, string contentType)> DownloadAsync(string fileName)
            {
                try
                {
                    var blobClient = _containerClient.GetBlobClient(fileName);
                    var memoryStream = new MemoryStream();
                    await blobClient.DownloadToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var contentType = blobClient.GetProperties().Value.ContentType;
                    return (memoryStream, contentType);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao baixar arquivo: {ex.Message}", ex);
                }
            }

        public async Task<string> GenerateViewUrlAsync(string blobName, TimeSpan expiration)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(blobName);

                if (!await blobClient.ExistsAsync())
                    return null;

                if (blobClient.CanGenerateSasUri)
                {
                    var sasBuilder = new BlobSasBuilder
                    {
                        BlobContainerName = _containerClient.Name,
                        BlobName = blobName,
                        Resource = "b",
                        ExpiresOn = DateTimeOffset.UtcNow.Add(expiration)
                    };

                    sasBuilder.SetPermissions(BlobSasPermissions.Read);

                    // IMPORTANTE: Adicionar parâmetros para forçar visualização
                    var sasUri = blobClient.GenerateSasUri(sasBuilder);

                    // Adicionar parâmetro para evitar download
                    var urlBuilder = new UriBuilder(sasUri);
                    var query = HttpUtility.ParseQueryString(urlBuilder.Query);
                    query["response-content-disposition"] = "inline"; // Força visualização
                    urlBuilder.Query = query.ToString();

                    return urlBuilder.ToString();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao gerar URL de visualização: {ex.Message}", ex);
            }
        }



        public async Task<bool> DeleteAsync(string fileName)
            {
                try
                {
                    var blobClient = _containerClient.GetBlobClient(fileName);
                    var result = await blobClient.DeleteIfExistsAsync();
                    return result.Value;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao deletar arquivo: {ex.Message}", ex);
                }
            }

            public string GetFileUrlAsync(string fileName)
            {
                try
                {
                    var blobClient = _containerClient.GetBlobClient(fileName);
                    return blobClient.Uri.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao obter URL: {ex.Message}", ex);
                }
            }
        }
    }
