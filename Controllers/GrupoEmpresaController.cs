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
    public class GrupoEmpresaController : ControllerBase
    {
        protected Context context;
        public GrupoEmpresaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.GrupoEmpresa
                  .Select(m => new
                  {
                      m.IdGrupoEmpresa,
                      m.Nome,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] GrupoEmpresaRequest model)
        {
            GrupoEmpresa grupoEmpresa;
            if (model.IdGrupoEmpresa > 0)
            {
                grupoEmpresa = context.GrupoEmpresa.FirstOrDefault(x => x.IdGrupoEmpresa == model.IdGrupoEmpresa);
                grupoEmpresa.Alterar(model.Nome, User.Identity.Name);

                context.Update(grupoEmpresa);
            }
            else
            {
                grupoEmpresa = new GrupoEmpresa(model.Nome, User.Identity.Name);
                context.GrupoEmpresa.Add(grupoEmpresa);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var grupoEmpresa = context.GrupoEmpresa.FirstOrDefault(x => x.IdGrupoEmpresa == id);
            grupoEmpresa.Excluir(User.Identity.Name);

            context.Update(grupoEmpresa);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var grupoEmpresa = context.GrupoEmpresa.FirstOrDefault(x => x.IdGrupoEmpresa == id);
            if (grupoEmpresa == null)
                return BadRequest("Grupo de Empresa não encontrado ");

            return Ok(new GrupoEmpresaResponse()
            {
                IdGrupoEmpresa = grupoEmpresa.IdGrupoEmpresa,
                Nome = grupoEmpresa.Nome,
                Situacao = grupoEmpresa.Situacao
            });
        }
    }
}
