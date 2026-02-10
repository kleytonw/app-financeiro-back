using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ERP.Infra;
using ERP_API.Service;
using System.Linq;
using System.Data.Entity;
using ERP_API.Models;
using ERP_API.Domain.Entidades;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System;
using Azure.Storage.Sas;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ClienteDocumentoController : ControllerBase
    {
        protected Context context;
        protected readonly IBlobStorageService _blobStorageService;

        public ClienteDocumentoController(Context context, IBlobStorageService blobStorageService)
        {
            this.context = context;
            _blobStorageService = blobStorageService;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar(int idCliente)
        {
            var result = context.ClienteDocumento.Include(c => c.Cliente.Pessoa).
                Include(c => c.TipoDocumento)
                .Where(x => x.IdCliente == idCliente)
                .Select(c => new
                {
                    c.IdClienteDocumento,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdTipoDocumento,
                    NomeDocumento = c.TipoDocumento.Nome,
                    c.Situacao,
                }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarAtivos")]
        public IActionResult ListarAtivos()
        {
            var result = context.ClienteDocumento.Include(c => c.Cliente.Pessoa).
                Include(c => c.TipoDocumento)
                .Where(x => x.Situacao == "Ativo")
                .Select(c => new
                {
                    c.IdClienteDocumento,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdTipoDocumento,
                    NomeDocumento = c.TipoDocumento.Nome,
                    c.Situacao,
                }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public async Task<IActionResult> Salvar([FromForm] IFormFile arquivo, [FromForm] int idCliente, [FromForm] int idTipoDocumento, [FromForm] int idClienteDocumento)
        {
            ClienteDocumento clienteDocumento;

                var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == idCliente);
                var tipoDocumento = context.TipoDocumento.FirstOrDefault(x => x.IdTipoDocumento == idTipoDocumento);

                var responseUpload = await _blobStorageService.UploadAsync(arquivo);

                clienteDocumento = new ClienteDocumento(
                    cliente,
                    tipoDocumento,
                    responseUpload,
                    User.Identity.Name);
                context.ClienteDocumento.Add(clienteDocumento);
            
            context.SaveChanges();
            return Ok(clienteDocumento.IdClienteDocumento);
        }

        [HttpGet]
        [Route("baixarArquivo")]
        public async Task<IActionResult> BaixarArquivo(int id)
        {
            try
            {
                // 1. Buscar documento no banco
                var clienteDocumento = context.ClienteDocumento
                    .Include(c => c.Cliente.Pessoa)
                    .Include(c => c.TipoDocumento)
                    .FirstOrDefault(c => c.IdClienteDocumento == id);

                if (clienteDocumento == null)
                {
                    return NotFound("Documento não encontrado.");
                }

                // 2. Baixar arquivo do Blob Storage
                var (stream, contentType) = await _blobStorageService.DownloadAsync(clienteDocumento.Arquivo);

                if (stream == null || stream.Length == 0)
                {
                    return NotFound("Arquivo não encontrado no storage.");
                }

                var extensaoOriginal = Path.GetExtension(clienteDocumento.Arquivo);

                // 4. Definir Content-Type correto
                var mimeType = ObterMimeType(extensaoOriginal) ?? contentType ?? "application/octet-stream";

                // 5. Garantir posição inicial do stream
                stream.Position = 0;

                var nomeArquivo = clienteDocumento.Arquivo; // Nome original do GUID

                // Limpar Response Headers primeiro (para evitar duplicatas)
                Response.Headers.Remove("Content-Disposition");

                // Adicionar header manualmente
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{nomeArquivo}\"");


                // 7. Retornar arquivo
                return File(stream, mimeType, clienteDocumento.Arquivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao baixar arquivo: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("obterUrlVisualizacao")]
        public async Task<IActionResult> ObterUrlVisualizacao(int id, int duracaoMinutos = 60)
        {
            try
            {
                var clienteDocumento = context.ClienteDocumento
                    .Include(c => c.Cliente.Pessoa)
                    .Include(c => c.TipoDocumento)
                    .FirstOrDefault(c => c.IdClienteDocumento == id);

                if (clienteDocumento == null)
                {
                    return NotFound("Documento não encontrado.");
                }

                // Gerar URL SAS para VISUALIZAÇÃO (não download)
                var urlVisualizacao = await _blobStorageService.GenerateViewUrlAsync(
                    clienteDocumento.Arquivo,
                    TimeSpan.FromMinutes(duracaoMinutos)
                );

                if (string.IsNullOrEmpty(urlVisualizacao))
                {
                    return NotFound("Arquivo não encontrado no storage.");
                }

                return Ok(new
                {
                    url = urlVisualizacao,
                    nomeArquivo = clienteDocumento.Arquivo,
                    tipoDocumento = clienteDocumento.TipoDocumento?.Nome,
                    cliente = clienteDocumento.Cliente?.Pessoa?.Nome,
                    expiracaoMinutos = duracaoMinutos,
                    expiracaoEm = DateTime.UtcNow.AddMinutes(duracaoMinutos),
                    // Identificar se o arquivo pode ser visualizado no browser
                    podeVisualizar = PodeVisualizarNoBrowser(clienteDocumento.Arquivo)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao gerar URL de visualização: {ex.Message}");
            }
        }

        private bool PodeVisualizarNoBrowser(string nomeArquivo)
        {
            var extensao = Path.GetExtension(nomeArquivo).ToLower();
            var extensoesVisualizaveis = new[] {
        ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg",
        ".txt", ".html", ".css", ".js", ".json", ".xml"
    };
            return extensoesVisualizaveis.Contains(extensao);
        }

        // Método auxiliar para obter MIME type correto
        private string ObterMimeType(string extensao)
        {
            var mimeTypes = new Dictionary<string, string>
            {
                { ".pdf", "application/pdf" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".gif", "image/gif" },
                { ".doc", "application/msword" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".txt", "text/plain" },
                { ".zip", "application/zip" },
                { ".rar", "application/x-rar-compressed" }
            };

            return mimeTypes.TryGetValue(extensao.ToLower(), out var mimeType) ? mimeType : null;
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var clienteDocumento = context.ClienteDocumento.FirstOrDefault(x => x.IdClienteDocumento == id);
            if (clienteDocumento == null)
                return NotFound("Documento não encontrado.");
            clienteDocumento.Excluir(User.Identity.Name);
            context.ClienteDocumento.Update(clienteDocumento);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var clienteDocumento = context.ClienteDocumento.FirstOrDefault(c => c.IdClienteDocumento == id);
            if (clienteDocumento == null)
                return NotFound("Documento não encontrado.");

            return Ok(new ClienteDocumentoResponse()
            {
                IdClienteDocumento = clienteDocumento.IdClienteDocumento,
                IdCliente = clienteDocumento.IdCliente,
                IdTipoDocumento = clienteDocumento.IdTipoDocumento,
                NomeArquivo = clienteDocumento.Arquivo,
                Situacao = clienteDocumento.Situacao
            });
        }
    }
}
