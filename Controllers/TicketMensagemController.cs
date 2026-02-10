using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using ERP_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Net.Mail;
using System;
using System.Net.Mime;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class TicketMensagemController : ControllerBase
    {
        public Context context;
        private IBlobStorageService _blobStorageService;

        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
        public TicketMensagemController(Context context, IBlobStorageService blobStorageService)
        {
            this.context = context;
            this._blobStorageService = blobStorageService;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar(int idTicket)
        {
            var result = context.TicketMensagem
                .Where(tm => tm.IdTicket == idTicket)
                .Select(m => new
                {
                    m.IdTicketMensagem,
                    m.IdTicket,
                    m.Mensagem,
                    m.Arquivo,
                    m.Data,
                    m.Usuario,
                    TipoUsuario = context.Usuario
                        .Where(u => u.Login == m.Usuario)
                        .Select(u => u.TipoUsuario)
                        .FirstOrDefault() ?? "Sistema"
                })
                .OrderBy(m => m.Data)
                .ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public async Task<IActionResult> Salvar([FromForm] int idTicket, [FromForm] string mensagem, [FromForm] IFormFile arquivo)
        {
            try
            {
                Ticket ticket = await context.Ticket
                    .Include(t => t.Cliente)
                    .ThenInclude(x => x.Pessoa)
                    .FirstOrDefaultAsync(t => t.IdTicket == idTicket);

                if (ticket == null)
                    return BadRequest("Ticket inválido.");

                string nomeArquivo = null;

                if (arquivo != null)
                {
                    nomeArquivo = await _blobStorageService.UploadAsync(arquivo);
                }

                var ticketMensagem = new TicketMensagem(ticket, mensagem, nomeArquivo, User.Identity.Name);
                context.TicketMensagem.Add(ticketMensagem);
                await context.SaveChangesAsync();

                // Enviar email
                await EnviarEmailNotificacao(ticket, mensagem, User.Identity.Name);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao salvar mensagem do ticket: " + ex.Message);
            }
        }

        private async Task EnviarEmailNotificacao(Ticket ticket, string mensagem, string usuarioResposta)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var subject = $"CONCICARD - Interação chamado - {ticket.IdTicket} - {ticket.Cliente.Pessoa.Nome}";

            var templateHtml = $@"
                    <html>
                    <body>
                        <h3>Nova interação no chamado #{ticket.IdTicket}</h3>
                        <p><strong>Cliente:</strong> {ticket.Cliente.Pessoa.Nome}</p>
                        <p><strong>Título:</strong> {ticket.Titulo}</p>
                        <hr/>
                        <p><strong>Mensagem:</strong></p>
                        <p>{mensagem}</p>
                        <hr/>
                        <p>Equipe Concicard</p>
                    </body>
                    </html>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Suporte");
            mailMsg.Subject = subject;
            mailMsg.IsBodyHtml = true;
            mailMsg.Body = templateHtml;

            // Verifica se quem respondeu é o cliente ou o administrador
            bool isAdmin = usuarioLogado.TipoUsuario == "Administrador"; // Ajuste conforme suas roles

            if (isAdmin)
            {
                // Admin respondeu -> envia email para o cliente
                if (!string.IsNullOrEmpty(ticket.Cliente.Pessoa.Email))
                {
                    // Cliente respondeu -> envia email para os administradores
                    mailMsg.To.Add(new MailAddress("kleytonwillian@gmail.com"));
                    mailMsg.To.Add(new MailAddress("renato@genialsoft.com.br"));
                    //mailMsg.To.Add(new MailAddress(ticket.Cliente.Pessoa.Email.Trim()));
                }
            }
            else
            {
                // Cliente respondeu -> envia email para os administradores
                mailMsg.To.Add(new MailAddress("kleytonwillian@gmail.com"));
                mailMsg.To.Add(new MailAddress("renato@genialsoft.com.br"));
            }

            if (mailMsg.To.Count > 0)
            {
                smtpClientSendGrid.Credentials = credentialsSendGrid;
                await smtpClientSendGrid.SendMailAsync(mailMsg);
            }
        }

        [HttpGet]
        [Route("download")]
        [Authorize]
        public async Task<IActionResult> Download(int id)
        {
            var nomeArquivo = context.TicketMensagem.FirstOrDefault(tm => tm.IdTicketMensagem == id)?.Arquivo;
            if (string.IsNullOrEmpty(nomeArquivo))
                return NotFound("Arquivo não encontrado.");

            // Desestrutura a tupla retornada
            var (stream, contentType) = await _blobStorageService.DownloadAsync(nomeArquivo);

            if (stream == null)
                return NotFound("Arquivo não encontrado.");

            return File(stream, contentType, nomeArquivo);
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var ticketMensagem = context.TicketMensagem.FirstOrDefault(tm => tm.IdTicketMensagem == id);
            if (ticketMensagem == null)
                return NotFound("TicketMensagem não encontrado.");

            ticketMensagem.Excluir(User.Identity.Name);

            context.TicketMensagem.Update(ticketMensagem);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var ticketMensagem = context.TicketMensagem.FirstOrDefault(tm => tm.IdTicketMensagem == id);
            if (ticketMensagem == null)
                return NotFound("TicketMensagem não encontrado.");

            return Ok(new TicketMensagemResponse
            {
                IdTicketMensagem = ticketMensagem.IdTicketMensagem,
                IdTicket = ticketMensagem.IdTicket,
                Mensagem = ticketMensagem.Mensagem,
                Usuario = ticketMensagem.Usuario,
                Data = ticketMensagem.Data,
                Arquivo = ticketMensagem.Arquivo
            });
        }
    }
}
