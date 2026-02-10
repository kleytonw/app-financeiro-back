using ERP.Infra;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RamoAtividadeController : ControllerBase
    {
        protected Context context;
        public RamoAtividadeController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.RamoAtividade
                  .Select(m => new
                  {
                      m.IdRamoAtividade,
                      m.Nome,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] RamoAtividadeRequest model)
        
        {
            RamoAtividade ramoAtividade;
            if (model.IdRamoAtividade > 0)
            {
                ramoAtividade = context.RamoAtividade.FirstOrDefault(x => x.IdRamoAtividade == model.IdRamoAtividade);
                ramoAtividade.Alterar(model.Nome, User.Identity.Name);

                context.Update(ramoAtividade);
            }
            else
            {
                ramoAtividade = new RamoAtividade(model.Nome, User.Identity.Name);
                context.RamoAtividade.Add(ramoAtividade);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var ramoAtividade = context.RamoAtividade.FirstOrDefault(x => x.IdRamoAtividade == id);
            ramoAtividade.Excluir(User.Identity.Name);

            context.Update(ramoAtividade);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var ramoAtividade = context.RamoAtividade.FirstOrDefault(x => x.IdRamoAtividade == id);
            if (ramoAtividade == null)
                return BadRequest("Ramo de Atividade não encontrado ");

            return Ok(new RamoAtividadeResponse()
            {
                IdRamoAtividade = ramoAtividade.IdRamoAtividade,
                Nome = ramoAtividade.Nome,
                Situacao = ramoAtividade.Situacao
            });
        }
    }
}