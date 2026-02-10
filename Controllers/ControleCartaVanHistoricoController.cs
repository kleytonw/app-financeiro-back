using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using ERP_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ControleCartaVanHistoricoController : ControllerBase
    {
        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        
        private Context context { get; set; }
        protected readonly IBlobStorageService _blobStorageService;
        private IConfiguration _config;

        public ControleCartaVanHistoricoController(Context context, IBlobStorageService blobStorageService, IConfiguration config)
        {
            this.context = context;
            _blobStorageService = blobStorageService;
            _config = config;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]

        public IActionResult Listar(int idControleCartaVan)
        {
            var result = context.ControleCartaVanHistorico
                .Include(x => x.ControleCartaVan)
                .Include(x => x.Etapa)
                .Where(x => x.IdControleCartaVan == idControleCartaVan && x.Situacao == "Ativo")
                .Select(m => new
                {
                    m.IdControleCartaVanHistorico,
                    m.IdControleCartaVan,
                    m.ControleCartaVan.ClienteContaBancaria.Conta,
                    m.ControleCartaVan.ClienteContaBancaria.Agencia,
                    m.ControleCartaVan.Status,
                    NomeEtapa = m.Etapa.Nome,
                    m.Etapa.EtapaConcluida,
                    m.Data,
                    m.Email,
                    m.Descricao,
                    m.Guid,
                    m.Situacao,
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public async Task<IActionResult> Salvar([FromForm] IFormFile arquivo, [FromForm] int idControleCartaVan, [FromForm] int idEtapa, 
                                                [FromForm] string descricao, [FromForm] string enviarPara, [FromForm] string emCopia, [FromForm] string assunto, [FromForm] bool enviarEmail)
        {
            ControleCartaVan controleCartaVan;
            Etapa etapa;

            controleCartaVan = context.ControleCartaVan.FirstOrDefault(x => x.IdControleCartaVan == idControleCartaVan);
            etapa = context.Etapa.FirstOrDefault(x => x.IdEtapa == idEtapa);

            string responseUpload = null;

            if (arquivo != null)
            {
                responseUpload = await _blobStorageService.UploadAsync(arquivo);
            }

            if (enviarEmail == true)
            {
                string subject = $"{assunto}";
                string templateHtml = $"<p>{descricao}</p>";

                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Contato de Cliente");
                mailMsg.Subject = subject;

                if (arquivo is not null && arquivo.Length > 0)
                {
                    var contentType = string.IsNullOrWhiteSpace(arquivo.ContentType)
                    ? MediaTypeNames.Application.Octet
                    : arquivo.ContentType;

                    // ✅ Copia o stream para um MemoryStream
                    using var fileStream = arquivo.OpenReadStream();
                    var memoryStream = new MemoryStream();
                    await fileStream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reset da posição

                    var attachment = new Attachment(memoryStream, arquivo.FileName, contentType);
                    attachment.NameEncoding = System.Text.Encoding.UTF8;
                    mailMsg.Attachments.Add(attachment);
                }

                mailMsg.To.Add(new MailAddress(enviarPara.Trim()));

                var emails = emCopia.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (emCopia.Any())
                {
                    // Restante dos emails vai para CC
                    foreach (var e in emails)
                    {
                        mailMsg.CC.Add(new MailAddress(e.Trim()));
                    }
                }

                string html = templateHtml;
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // 🔹 Anexa o arquivo, se tiver
                if (arquivo != null)
                {
                    mailMsg.Attachments.Add(new Attachment(arquivo.OpenReadStream(), arquivo.FileName));
                }

                smtpClientSendGrid.Credentials = credentialsSendGrid;

                try
                {
                    smtpClientSendGrid.Send(mailMsg);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            var emailsConcat = $"{enviarPara};{emCopia}";


            var controleCartaVanHistorico = new ControleCartaVanHistorico(controleCartaVan, etapa, DateTime.Now.Date, descricao, responseUpload, enviarEmail, emailsConcat, assunto, User.Identity.Name);
            context.ControleCartaVanHistorico.Add(controleCartaVanHistorico);
            controleCartaVan.AlterarEtapa(etapa);
            context.ControleCartaVan.Update(controleCartaVan);
            

            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("baixarArquivo")]
        public async Task<IActionResult> BaixarArquivo(int id)
        {
            try
            {
                // 1. Buscar documento no banco
                var controleCartaVanHistorico = context.ControleCartaVanHistorico
                    .FirstOrDefault(c => c.IdControleCartaVanHistorico == id);

                if (controleCartaVanHistorico == null)
                {
                    return NotFound("Documento não encontrado.");
                }

                // 2. Baixar arquivo do Blob Storage
                var (stream, contentType) = await _blobStorageService.DownloadAsync(controleCartaVanHistorico.Guid);

                if (stream == null || stream.Length == 0)
                {
                    return NotFound("Arquivo não encontrado no storage.");
                }

                var extensaoOriginal = Path.GetExtension(controleCartaVanHistorico.Guid);

                // 4. Definir Content-Type correto
                var mimeType = ObterMimeType(extensaoOriginal) ?? contentType ?? "application/octet-stream";

                // 5. Garantir posição inicial do stream
                stream.Position = 0;

                var nomeArquivo = controleCartaVanHistorico.Guid; // Nome original do GUID

                // Limpar Response Headers primeiro (para evitar duplicatas)
                Response.Headers.Remove("Content-Disposition");

                // Adicionar header manualmente
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{nomeArquivo}\"");


                // 7. Retornar arquivo
                return File(stream, mimeType, controleCartaVanHistorico.Guid);
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
                var controleCartaVanHistorico = context.ControleCartaVanHistorico
                    .FirstOrDefault(c => c.IdControleCartaVanHistorico == id);

                if (controleCartaVanHistorico == null)
                {
                    return NotFound("Documento não encontrado.");
                }

                // Gerar URL SAS para VISUALIZAÇÃO (não download)
                var urlVisualizacao = await _blobStorageService.GenerateViewUrlAsync(
                    controleCartaVanHistorico.Guid,
                    TimeSpan.FromMinutes(duracaoMinutos)
                );

                if (string.IsNullOrEmpty(urlVisualizacao))
                {
                    return NotFound("Arquivo não encontrado no storage.");
                }

                return Ok(new
                {
                    url = urlVisualizacao,
                    nomeArquivo = controleCartaVanHistorico.Guid,
                    expiracaoMinutos = duracaoMinutos,
                    expiracaoEm = DateTime.UtcNow.AddMinutes(duracaoMinutos),
                    // Identificar se o arquivo pode ser visualizado no browser
                    podeVisualizar = PodeVisualizarNoBrowser(controleCartaVanHistorico.Guid)
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
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var controleCartaVanHistorico = context.ControleCartaVanHistorico.FirstOrDefault(x => x.IdControleCartaVanHistorico == id);
            if(controleCartaVanHistorico == null)
                return BadRequest("Histórico de controle de carta van não encontrado!");

            controleCartaVanHistorico.SetUsuarioExclusao(User.Identity.Name);
            context.ControleCartaVanHistorico.Update(controleCartaVanHistorico);
            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var controleCartaVanHistorico = context.ControleCartaVanHistorico.FirstOrDefault(x => x.IdControleCartaVanHistorico == id);
            if (controleCartaVanHistorico == null)
                return BadRequest("Histórico de controle de carta van não encontrado!");

            return Ok(new ControleCartaVanHistoricoResponse()
            {
                IdControleCartaVanHistorico = controleCartaVanHistorico.IdControleCartaVanHistorico,
                IdControleCartaVan = controleCartaVanHistorico.IdControleCartaVan,
                IdEtapa = controleCartaVanHistorico.IdEtapa,
                Data = controleCartaVanHistorico.Data,
                Descricao = controleCartaVanHistorico.Descricao,
                Situacao = controleCartaVanHistorico.Situacao,
            });
        }

    }
}
