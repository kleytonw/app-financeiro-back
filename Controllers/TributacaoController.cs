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

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class TributacaoController : ControllerBase
    {
        protected Context context;
        public TributacaoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Tributacao
                  .Select(m => new
                  {
                      m.IdTributacao,
                      m.Nome,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] TributacaoRequest model)
        {
            Tributacao tributacao;
            if (model.IdTributacao > 0)
            {
                tributacao = context.Tributacao.FirstOrDefault(x => x.IdTributacao == model.IdTributacao);
                tributacao.Alterar(model.Nome, User.Identity.Name);

                context.Update(tributacao);
            }
            else
            {
                tributacao = new Tributacao(model.Nome, User.Identity.Name);
                context.Tributacao.Add(tributacao);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var tributacao = context.Tributacao.FirstOrDefault(x => x.IdTributacao == id);
            tributacao.Excluir(User.Identity.Name);

            context.Update(tributacao);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var tributacao = context.Tributacao.FirstOrDefault(x => x.IdTributacao == id);
            if (tributacao == null)
                return BadRequest("Tributacao não encontrada ");

            return Ok(new TributacaoResponse()
            {
                IdTributacao = tributacao.IdTributacao,
                Nome = tributacao.Nome,
                Situacao = tributacao.Situacao
            });
        }
    }
}

