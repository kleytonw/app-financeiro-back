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
using Microsoft.EntityFrameworkCore;
using ERP_API.Models;
using ERP_API.Domain.Entidades;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class LocalizacaoController : ControllerBase
    {
        protected Context context;
        public LocalizacaoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Localizacao
                  .Select(m => new
                  {
                      m.IdLocalizacao,
                      m.Nome,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] LocalizacaoRequest model)
        {
            Localizacao localizacao;
            if (model.IdLocalizacao > 0)
            {
                localizacao = context.Localizacao.FirstOrDefault(x => x.IdLocalizacao == model.IdLocalizacao);
                localizacao.Alterar(model.Nome, User.Identity.Name);

                context.Update(localizacao);
            }
            else
            {
                localizacao = new Localizacao(model.Nome, User.Identity.Name);
                context.Localizacao.Add(localizacao);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var localizacao = context.Localizacao.FirstOrDefault(x => x.IdLocalizacao == id);
            localizacao.Excluir(User.Identity.Name);

            context.Update(localizacao);
            context.SaveChanges();
            return Ok();
        }

      
        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var localizacao = context.Localizacao.FirstOrDefault(x => x.IdLocalizacao == id);
            if (localizacao == null)
                return BadRequest("Localizacao não encontrada ");

            return Ok(new LocalizacaoResponse()
            {
                IdLocalizacao = localizacao.IdLocalizacao,
                Nome = localizacao.Nome,
                Situacao = localizacao.Situacao
            });
        }

    }
}
