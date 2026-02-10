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
    public class ServicoController : ControllerBase
    {
        protected Context context;
        public ServicoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Servico
                  .Select(m => new
                  {
                      m.IdServico,
                      m.Nome,
                      m.Valor,
                      m.Descricao,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ServicoRequest model)
        {
            Servico servico;
            if (model.IdServico > 0)
            {
                servico = context.Servico.FirstOrDefault(x => x.IdServico == model.IdServico);
                servico.Alterar(model.Nome, model.Valor, model.Descricao, User.Identity.Name);

                context.Update(servico);
            }
            else
            {
                servico = new Servico(model.Nome, model.Valor, model.Descricao, User.Identity.Name);
                context.Servico.Add(servico);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var servico = context.Servico.FirstOrDefault(x => x.IdServico == id);
            servico.Excluir(User.Identity.Name);

            context.Update(servico);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var servico = context.Servico.FirstOrDefault(x => x.IdServico == id);
            if (servico == null)
                return BadRequest("Serviço não encontrado ");

            return Ok(new ServicoResponse()
            {
                IdServico = servico.IdServico,
                Nome = servico.Nome,
                Valor = servico.Valor,
                Descricao = servico.Descricao,
                Situacao = servico.Situacao
            });
        }
    }
}