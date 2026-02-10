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
    public class PlanoController : ControllerBase
    {
        protected Context context;
        public PlanoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        [AllowAnonymous]
        public IActionResult Listar()
        {
            var result = context.Plano.Where(x => x.Situacao == "Ativo")
                  .Select(m => new
                  {
                      m.IdPlano,
                      m.Nome,
                      m.Valor,
                      m.ValorRepasse,
                      m.ValorAdesao,
                      m.Descricao,
                      m.QuantidadeVendasInicial,
                      m.QuantidadeVendasFinal,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] PlanoRequest model)
        {
            Plano plano;
            if (model.IdPlano > 0)
            {
                plano = context.Plano.FirstOrDefault(x => x.IdPlano == model.IdPlano);
                plano.Alterar(model.Nome, model.Valor, model.ValorAdesao, model.ValorRepasse, model.Descricao, model.QuantidadeVendasInicial, model.QuantidadeVendasFinal, User.Identity.Name);

                context.Update(plano);
            }
            else
            {
                plano = new Plano(model.Nome, model.Valor,model.ValorAdesao, model.ValorRepasse, model.Descricao, model.QuantidadeVendasInicial, model.QuantidadeVendasFinal, User.Identity.Name);
                context.Plano.Add(plano);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var plano = context.Plano.FirstOrDefault(x => x.IdPlano == id);
            plano.Excluir(User.Identity.Name);

            context.Update(plano);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var plano = context.Plano.FirstOrDefault(x => x.IdPlano == id);
            if (plano == null)
                return BadRequest("Plano não encontrado ");

            return Ok(new PlanoResponse()
            {
                IdPlano = plano.IdPlano,
                Nome = plano.Nome,
                Valor = plano.Valor,
                ValorAdesao = plano.ValorAdesao,
                ValorRepasse = plano.ValorRepasse,
                Descricao = plano.Descricao,
                QuantidadeVendasInicial = plano.QuantidadeVendasInicial,
                QuantidadeVendasFinal = plano.QuantidadeVendasFinal,
                Situacao = plano.Situacao
            });
        }
    }
}
