using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ERP.Infra;
using System;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;
using ERP.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;


namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ContatoSiteController : ControllerBase
    {
        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        private IConfiguration _config;
        protected Context context;

        public ContatoSiteController(Context context, IConfiguration config)
        {
            _config = config;
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.ContatoSite.OrderBy(m => m.Data)
                .Select(m => new {
                    m.IdContatoSite,
                    m.Empresa.Nome,
                    m.NomeContato,
                    m.Telefone,
                    m.Email,
                    m.Titulo,
                    m.Mensagem,
                    m.Data,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("enviarEmail")]
        [AllowAnonymous]
        public IActionResult EnviarEmail([FromBody] ContatoSiteRequest model)
        {
            //var usuario = context.Usuario.FirstOrDefault(x => x.IdUsuario == id);

            if (model == null)
                return BadRequest("Usuário não encontrado");

            //#region RandonSenha
            //const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //StringBuilder sb = new StringBuilder();
            // Random rnd = new Random();

            // for (int i = 0; i < 6; i++)
            // {
            //     int index = rnd.Next(chars.Length);
            //     sb.Append(chars[index]);
            // }
            // string novasenha = sb.ToString();
            //  #endregion

            //usuario.PrimeiroAcesso = "S";
            //  usuario.Senha = novasenha;

            // context.Update(usuario);
            // context.SaveChanges();

            string subject = " Contato de " + model.NomeContato;
            string templateHtml = $"<h2> Confirmação de Contato </h2> <h3>Nome: {model.NomeContato}</h3>" +
                                                                      $"<h3> Telefone: {model.Telefone} </h3> " +
                                                                      $"<h3> Data: {model.Data} </h3> " +
                                                                      $"<h3>  E-mail:{model.Email}</h3>"+
                                                                      $"<h3> Título:{model.Titulo}" +
                                                                       $"<h3> Mensagem:{model.Mensagem}";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Contato de Cliente");
            mailMsg.Subject = subject;
            mailMsg.To.Add(new MailAddress("renato@genialsoft.com.br", "Contato de Cliente!"));
            string html = templateHtml;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));


            smtpClientSendGrid.Credentials = credentialsSendGrid;

            try
            {
                smtpClientSendGrid.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
            return Ok();
        }

        [HttpGet]
        [Route("listarEmpresa")]
        public IActionResult ListarEmpresa()
        {
            var result = context.Empresa
                .Select(m => new {
                    m.IdEmpresa,
                    m.Nome
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("listarContatos")]
        public IActionResult ListarContatos(ContatoSiteRequest model)
        {
            var result = context.ContatoSite.Where(x => x.Data.Date >= model.DataInicio.Date && x.Data.Date <= model.DataFim)
                .Select(m => new
                {
                    m.IdContatoSite,
                    m.Empresa.Nome,
                    m.NomeContato,
                    m.Telefone,
                    m.Email,
                    m.Titulo,
                    m.Mensagem,
                    m.Data,
                    m.Situacao

                }).ToList();

            return Ok(result);

        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ContatoSiteRequest model)
        {
            ContatoSite contatoSite;
            Empresa empresa;

            if (model.IdContatoSite > 0)
            {
                contatoSite = context.ContatoSite.FirstOrDefault(x => x.IdContatoSite == model.IdContatoSite);
                empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
                contatoSite.Alterar(model.NomeContato , model.Telefone, model.Email, model.Titulo, empresa, model.Mensagem, DateTime.Now, User.Identity.Name);
            }
            else
            {
                empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
                contatoSite = new ContatoSite(
                    model.NomeContato,
                    model.Telefone,
                    model.Email,
                    model.Titulo,
                    empresa,
                    model.Mensagem,
                    DateTime.Now,
                    User.Identity.Name
                );

                context.ContatoSite.Add(contatoSite);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var contatoSite = context.ContatoSite.FirstOrDefault(x => x.IdContatoSite == id);
            contatoSite.Excluir(User.Identity.Name);

            context.Update(contatoSite);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var contatoSite = context.ContatoSite.FirstOrDefault(x => x.IdContatoSite == id);
            if (contatoSite == null)
                return BadRequest("Contato não encontrado");

            return Ok(new ContatoSiteResponse()
            {
                IdContatoSite = contatoSite.IdContatoSite,
                NomeContato = contatoSite.NomeContato,
                Telefone = contatoSite.Telefone,
                Email = contatoSite.Email,
                Titulo = contatoSite.Titulo,
                IdEmpresa = contatoSite.IdEmpresa,
                Mensagem = contatoSite.Mensagem,
                Data = contatoSite.Data,
                Situacao = contatoSite.Situacao
            });
        }
    }
}

