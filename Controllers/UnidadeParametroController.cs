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
using System.Diagnostics;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class UnidadeParametroController : ControllerBase
    {
        protected Context context;
        public UnidadeParametroController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var result = context.UnidadeParametro.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
                  .Select(m => new
                  {
                      m.IdUnidadeParametro,
                      m.Unidade.Nome,
                      m.Operadora.NomeOperadora,
                      m.Valor,
                      m.Chave,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarParametrosPorId")]
        public IActionResult listarParametrosPorId(int idUnidade)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var result = context.UnidadeParametro.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa && x.IdUnidade == idUnidade)
                  .Select(m => new
                  {
                      m.IdUnidadeParametro,
                      m.Unidade.Nome,
                      m.Operadora.NomeOperadora,
                      m.Valor,
                      m.Chave,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] UnidadeParametroRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);
            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == model.IdOperadora);

            UnidadeParametro unidadeParametro;
            if (model.IdUnidadeParametro > 0)
            {
                unidadeParametro = context.UnidadeParametro.FirstOrDefault(x => x.IdUnidadeParametro == model.IdUnidadeParametro);
                unidadeParametro.Alterar(unidade, operadora, empresa, model.Chave, model.Valor, User.Identity.Name);

                context.Update(unidadeParametro);
            }
            else
            {
                unidadeParametro = new UnidadeParametro(unidade, operadora, empresa, model.Chave, model.Valor, User.Identity.Name);
                context.UnidadeParametro.Add(unidadeParametro);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var unidadeParametro = context.UnidadeParametro.FirstOrDefault(x => x.IdUnidadeParametro == id);
            unidadeParametro.Excluir(User.Identity.Name);

            context.Update(unidadeParametro);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("deletar")]
        public IActionResult Deletar(int idUnidadeParametro)
        {
           var unidadeParametro = context.UnidadeParametro.FirstOrDefault(x => x.IdUnidadeParametro == idUnidadeParametro);
            unidadeParametro.Excluir(User.Identity.Name);

            context.Remove(unidadeParametro);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var unidadeParametro = context.UnidadeParametro.FirstOrDefault(x => x.IdUnidadeParametro == id);
            if (unidadeParametro == null)
                return BadRequest("Unidade de Parâmetro não encontrada ");

            return Ok(new UnidadeParametroResponse()
            {
                IdUnidadeParametro = unidadeParametro.IdUnidadeParametro,
                IdUnidade = unidadeParametro.IdUnidade,
                IdOperadora = unidadeParametro.IdOperadora,
                Valor = unidadeParametro.Valor,
                Chave = unidadeParametro.Chave,
                Situacao = unidadeParametro.Situacao
            });
        }

    }
}
