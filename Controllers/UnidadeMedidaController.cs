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
    public class UnidadeMedidaController : ControllerBase
    {
        protected Context context;
        public UnidadeMedidaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.UnidadeMedida
                  .Select(m => new
                  {
                      m.IdUnidadeMedida,
                      m.Nome,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] UnidadeMedidaRequest model)
        {
            UnidadeMedida unidadeMedida;
            if (model.IdUnidadeMedida > 0)
            {
                unidadeMedida = context.UnidadeMedida.FirstOrDefault(x => x.IdUnidadeMedida == model.IdUnidadeMedida);
                unidadeMedida.Alterar(model.Nome, User.Identity.Name);

                context.Update(unidadeMedida);
            }
            else
            {
                unidadeMedida = new UnidadeMedida(model.Nome, User.Identity.Name);
                context.UnidadeMedida.Add(unidadeMedida);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var unidadeMedida = context.UnidadeMedida.FirstOrDefault(x => x.IdUnidadeMedida == id);
            unidadeMedida.Excluir(User.Identity.Name);

            context.Update(unidadeMedida);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var unidadeMedida = context.UnidadeMedida.FirstOrDefault(x => x.IdUnidadeMedida == id);
            if (unidadeMedida == null)
                return BadRequest("Unidade de Medida não encontrada ");

            return Ok(new UnidadeMedidaResponse()
            {
                IdUnidadeMedida = unidadeMedida.IdUnidadeMedida,
                Nome = unidadeMedida.Nome,
                Situacao = unidadeMedida.Situacao
            });
        }

    }
}
