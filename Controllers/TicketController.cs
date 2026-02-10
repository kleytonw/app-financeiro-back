using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;
using ERP.Models;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using ERP_API.Service;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TicketController : ControllerBase
    {
        protected Context context;

        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        public TicketController(Context context)
        {
            this.context = context;
            
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            if (usuarioLogado == null)
                return BadRequest("Usuário não encontrado.");

            if (usuarioLogado.TipoUsuario != "Administrador")
            {
                var result = context.Ticket.Include(x => x.Cliente.Pessoa)
                      .Where(x => x.IdPessoa == usuarioLogado.IdPessoa && x.Situacao == "Ativo")
                      .Select(m => new
                      {
                          m.IdTicket,
                          m.Cliente.Pessoa.Nome,
                          m.TipoSuporte.NomeTipoSuporte,
                          m.Mensagem,
                          m.Status,
                          m.DataAbertura,
                          m.DataAndamento,
                          m.DataConclusao,
                          TempoAtendimento =  m.DataConclusao != null ? (m.DataAbertura.Value - m.DataConclusao.Value).TotalMinutes : 0,
                          m.Situacao
                      }).ToList();

                return Ok(result);
            }
            else
            {
                var result = context.Ticket.Include(x => x.Cliente.Pessoa)
                      .Where(x => x.Situacao == "Ativo")
                      .Select(m => new
                      {
                          m.IdTicket,
                          m.Cliente.Pessoa.Nome,
                          m.TipoSuporte.NomeTipoSuporte,
                          m.Mensagem,
                          m.Status,
                          DataAbertura = m.DataAbertura ,
                          m.DataAndamento,
                          m.DataConclusao,
                          TempoAtendimento = m.DataConclusao != null ? (m.DataAbertura.Value - m.DataConclusao.Value).TotalMinutes : 0,
                          m.Situacao
                      }).ToList();
                return Ok(result);
            }
        }

        [HttpPost]
        [Route("pesquisar")]
        [Authorize]
        public IActionResult Pesquisar([FromBody] TicketFiltroRequest filtro)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            if (usuarioLogado == null)
                return BadRequest("Usuário não encontrado.");

            var query = context.Ticket.Include(x => x.Cliente.Pessoa).AsQueryable();

            if (usuarioLogado.TipoUsuario != "Administrador")
            {
                query = query.Where(x => x.IdPessoa == usuarioLogado.IdPessoa);
            }

            if (filtro.TipoPeriodo == "DataAbertura")
            {
                query = query.Where(x => x.DataAbertura.Value.Date >= filtro.DataInicial.Value.AddDays(-1) && x.DataAbertura.Value.Date <= filtro.DataFinal.Value);
            }
            else if (filtro.TipoPeriodo == "DataAndamento")
            {
                query = query.Where(x => x.DataAndamento.Value.Date >= filtro.DataInicial.Value.AddDays(-1) && x.DataAndamento.Value.Date <= filtro.DataFinal.Value);
            }
            else if (filtro.TipoPeriodo == "DataConclusao")
            {
                query = query.Where(x => x.DataConclusao.Value.Date >= filtro.DataInicial.Value.AddDays(-1) && x.DataConclusao.Value.Date <= filtro.DataFinal.Value);
            }
            else
            {
                return BadRequest("Tipo de período inválido.");
            }


            var result = query
                  .Select(m => new
                  {
                      m.IdTicket,
                      m.Cliente.Pessoa.Nome,
                      m.TipoSuporte.NomeTipoSuporte,
                      m.Mensagem,
                      m.Status,
                      m.DataAbertura,
                      m.DataAndamento,
                      m.DataConclusao,
                      TempoAtendimento = m.DataConclusao != null ? (m.DataConclusao.Value - m.DataAbertura.Value).TotalMinutes : 0,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarCliente")]
        public IActionResult ListarCliente()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            if (usuarioLogado == null)
                return BadRequest("Usuário não encontrado.");

            if (usuarioLogado.TipoUsuario == "Administrador")
            {

                var result = context.Cliente.Include(x => x.Pessoa)
                 .Select(m => new
                 {
                     m.IdPessoa,
                     m.Pessoa.Nome
                 }).ToList();
                return Ok(result);
            }
            else
            {
                var cliente = context.Cliente.Include(x => x.Pessoa)
                    .FirstOrDefault(x => x.IdPessoa == usuarioLogado.IdPessoa);
                if (cliente == null)
                    return BadRequest("Cliente não encontrado.");
                return Ok(new List<object>()
                {
                    new
                    {
                        cliente.IdPessoa,
                        cliente.Pessoa.Nome
                    }
                });

            }
        }

        [HttpGet]
        [Route("atenderTicket")]
        [Authorize]
        public IActionResult AtenderTicket(int id)
        {
            var ticket = context.Ticket.FirstOrDefault(x => x.IdTicket == id);
            if (ticket == null)
                return BadRequest("Ticket não encontrado.");

            ticket.EmAtendimento(User.Identity.Name);
            context.Update(ticket);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("concluirTicket")]
        [Authorize]
        public IActionResult ConcluirTicket(int id)
        {
            var ticket = context.Ticket.FirstOrDefault(x => x.IdTicket == id);
            if (ticket == null)
                return BadRequest("Ticket não encontrado.");

            ticket.ConclusaoAntendimento(User.Identity.Name);
            context.Update(ticket);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("listarTipoSuporte")]
        public IActionResult ListarTipoSuporte()
        {
            var result = context.TipoSuporte
                  .Select(m => new
                  {
                      m.IdTipoSuporte,
                      m.NomeTipoSuporte
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public async Task<IActionResult> Salvar([FromBody] TicketRequest model)
        {
            Ticket ticket;
            Cliente cliente;
            TipoSuporte tipoSuporte;
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            if (model.IdTicket > 0)
            {
                ticket = context.Ticket.FirstOrDefault(x => x.IdTicket == model.IdTicket);
                cliente = context.Cliente.Include(x => x.Pessoa).FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
                tipoSuporte = context.TipoSuporte.FirstOrDefault(x => x.IdTipoSuporte == model.IdTipoSuporte);
                ticket.Alterar(cliente, tipoSuporte, model.Titulo, model.Mensagem, User.Identity.Name);
                context.Update(ticket);
            }
            else
            {
                cliente = context.Cliente.Include(x => x.Pessoa).FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
                tipoSuporte = context.TipoSuporte.FirstOrDefault(x => x.IdTipoSuporte == model.IdTipoSuporte);
                ticket = new Ticket(cliente, tipoSuporte, model.Titulo, model.Mensagem, User.Identity.Name);
                context.Ticket.Add(ticket);
            }

            context.SaveChanges();

            // Se for um novo ticket criado por cliente, envia email para o administrador
            if (model.IdTicket == 0 && usuarioLogado.TipoUsuario == "Cliente")
            {
                await EnviarEmailNovoTicket(ticket, cliente);
            }

            return Ok();
        }

        private async Task EnviarEmailNovoTicket(Ticket ticket, Cliente cliente)
        {
            var subject = $"CONCICARD - Novo chamado - {ticket.IdTicket} - {cliente.Pessoa.Nome}";

            var templateHtml = $@"
                <html>
                <body>
                    <h3>Novo chamado aberto - #{ticket.IdTicket}</h3>
                    <p><strong>Cliente:</strong> {cliente.Pessoa.Nome}</p>
                    <p><strong>Título:</strong> {ticket.Titulo}</p>
                    <hr/>
                    <p><strong>Mensagem:</strong></p>
                    <p>{ticket.Mensagem}</p>
                    <hr/>
                    <p>Equipe Concicard</p>
                </body>
                </html>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Suporte");
            mailMsg.To.Add(new MailAddress("atendimento@concicard.com.br"));
            mailMsg.Subject = subject;
            mailMsg.IsBodyHtml = true;
            mailMsg.Body = templateHtml;

            smtpClientSendGrid.Credentials = credentialsSendGrid;
            await smtpClientSendGrid.SendMailAsync(mailMsg);
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var ticket = context.Ticket.FirstOrDefault(x => x.IdTicket == id);
            ticket.Excluir(User.Identity.Name);

            context.Update(ticket);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var ticket = context.Ticket.Include(x => x.Cliente)
                                       .ThenInclude(c => c.Pessoa)
                                       .Include(x => x.TipoSuporte).FirstOrDefault(x => x.IdTicket == id);
            if (ticket == null)
                return BadRequest("Ticket não encontrado ");

            return Ok(new TicketResponse()
            {
                IdTicket = ticket.IdTicket,
                IdPessoa = ticket.IdPessoa,
                NomePessoa = ticket.Cliente?.Pessoa?.Nome,
                IdTipoSuporte = ticket.IdTipoSuporte,
                NomeTipoSuporte = ticket.TipoSuporte.NomeTipoSuporte,
                UsuarioConclusao = ticket.UsuarioConclusao,
                UsuarioAtendimento = ticket.UsuarioAtendimento,
                DataAbertura = ticket.DataAbertura,
                DataConclusao = ticket.DataConclusao,
                DataAndamento = ticket.DataAndamento,
                Titulo = ticket.Titulo,
                Mensagem = ticket.Mensagem,
                Status = ticket.Status,
                Situacao = ticket.Situacao
            });
        }
    }
}
