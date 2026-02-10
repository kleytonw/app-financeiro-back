using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ERP.Models;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using System;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class NewsletterController : ControllerBase
    {
        protected Context context;

        public NewsletterController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Newsletter
                .Select(m => new {
                    m.IdNewsletter,
                    m.Empresa.Nome,
                    m.Email,
                    m.Data,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }


        [HttpGet]
        [Route("listarEmpresa")]
        public IActionResult ListarEmpresa()
        {
            var result = context.Empresa
                .Select(m => new {
                    m.IdEmpresa,
                    m.Nome,
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] NewsletterRequest model)
        {
            Newsletter newsletter;
            Empresa empresa;
            empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
                newsletter = new Newsletter(
                    empresa,
                    model.Email,
                   DateTime.Now,
                    User.Identity.Name
                );

                context.Newsletter.Add(newsletter);
            

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var newsletter = context.Newsletter.FirstOrDefault(x => x.IdNewsletter == id);
            newsletter.Excluir(User.Identity.Name);

            context.Update(newsletter);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var newsletter = context.Newsletter.FirstOrDefault(x => x.IdNewsletter == id);
            if (newsletter == null)
                return BadRequest("Newsletter não encontrada");

            return Ok(new NewsletterResponse()
            {
                IdNewsletter = newsletter.IdNewsletter,
                IdEmpresa = newsletter.IdEmpresa,
                Email = newsletter.Email,
                Data = newsletter.Data,
                Situacao = newsletter.Situacao
            });
        }
    }
}
