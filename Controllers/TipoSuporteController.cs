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
using System.Data.Entity;

namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TipoSuporteController : ControllerBase
    {
        protected Context context;
        public TipoSuporteController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.TipoSuporte
                  .Select(m => new
                  {
                      m.IdTipoSuporte,
                      m.NomeTipoSuporte,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] TipoSuporteRequest model)
        {
            TipoSuporte tipoSuporte;

            if (model.IdTipoSuporte > 0)
            {
                tipoSuporte = context.TipoSuporte.FirstOrDefault(x => x.IdTipoSuporte == model.IdTipoSuporte);
                tipoSuporte.Alterar(model.NomeTipoSuporte, User.Identity.Name);

                context.Update(tipoSuporte);
            }
            else
            {
                tipoSuporte = context.TipoSuporte.FirstOrDefault(x => x.IdTipoSuporte == model.IdTipoSuporte);
                tipoSuporte = new TipoSuporte(model.NomeTipoSuporte, User.Identity.Name);
                context.TipoSuporte.Add(tipoSuporte);
            }
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var tipoSuporte = context.TipoSuporte.FirstOrDefault(x => x.IdTipoSuporte == id);
            tipoSuporte.Excluir(User.Identity.Name);

            context.Update(tipoSuporte);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var tipoSuporte = context.TipoSuporte.FirstOrDefault(x => x.IdTipoSuporte == id);
            if (tipoSuporte == null)
                return BadRequest("Tipo de Suporte não encontrado ");

            return Ok(new TipoSuporteResponse()
            {
                IdTipoSuporte = tipoSuporte.IdTipoSuporte,
                NomeTipoSuporte = tipoSuporte.NomeTipoSuporte,
                Situacao = tipoSuporte.Situacao
            });
        }
    }
}
