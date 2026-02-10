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

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class GrupoComissaoController : ControllerBase
    {
        protected Context context;
        public GrupoComissaoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.GrupoComissao
                  .Select(m => new
                  {
                      m.IdGrupoComissao,
                      m.Nome,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] GrupoComissaoRequest model)
        {
            GrupoComissao grupoComissao;
            if (model.IdGrupoComissao > 0)
            {
                grupoComissao = context.GrupoComissao.FirstOrDefault(x => x.IdGrupoComissao == model.IdGrupoComissao);
                grupoComissao.Alterar(model.Nome, User.Identity.Name);

                context.Update(grupoComissao);
            }
            else
            {
                grupoComissao = new GrupoComissao(model.Nome, User.Identity.Name);
                context.GrupoComissao.Add(grupoComissao);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var grupoComissao = context.GrupoComissao.FirstOrDefault(x => x.IdGrupoComissao == id);
            grupoComissao.Excluir(User.Identity.Name);

            context.Update(grupoComissao);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var grupoComissao = context.GrupoComissao.FirstOrDefault(x => x.IdGrupoComissao == id);
            if (grupoComissao == null)
                return BadRequest("Grupo de Comissão não encontrado ");

            return Ok(new GrupoComissaoResponse()
            {
                IdGrupoComissao = grupoComissao.IdGrupoComissao,
                Nome = grupoComissao.Nome,
                Situacao = grupoComissao.Situacao
            });
        }
    }
}

