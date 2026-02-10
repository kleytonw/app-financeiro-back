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
    public class ParceiroParametroController : ControllerBase
    {
        protected Context context;
        public ParceiroParametroController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {

            var result = context.ParceiroParametro
                  .Select(m => new
                  {
                      m.IdParceiroParametro,
                      m.Valor,
                      m.Chave,
                      m.ParceiroSistema.NomeParceiroSistema,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ParceiroParametroRequest model)
        {

            ParceiroParametro parceiroParametro;
            var parceiroSistema = context.ParceiroSistema.FirstOrDefault(x => x.IdParceiroSistema == model.IdParceiroSistema);
            if (model.IdParceiroParametro > 0)
            {
                parceiroParametro = context.ParceiroParametro.FirstOrDefault(x => x.IdParceiroParametro == model.IdParceiroParametro);
                parceiroParametro.Alterar(model.Chave, model.Valor, parceiroSistema, User.Identity.Name);

                context.Update(parceiroParametro);
            }
            else
            {
                parceiroParametro = new ParceiroParametro(model.Chave, model.Valor, parceiroSistema, User.Identity.Name);
                context.ParceiroParametro.Add(parceiroParametro);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var parceiroParametro = context.ParceiroParametro.FirstOrDefault(x => x.IdParceiroParametro == id);
            parceiroParametro.Excluir(User.Identity.Name);

            context.Update(parceiroParametro);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("deletar")]
        public IActionResult Deletar(int idParceiroParametro)
        {
            var parceiroParametro = context.ParceiroParametro.FirstOrDefault(x => x.IdParceiroParametro == idParceiroParametro);
            parceiroParametro.Excluir(User.Identity.Name);

            context.Remove(parceiroParametro);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var parceiroParametro = context.ParceiroParametro.FirstOrDefault(x => x.IdParceiroParametro == id);
            if (parceiroParametro == null)
                return BadRequest("O Parâmetro do parceiro não foi encontrado ");

            return Ok(new ParceiroParametroResponse()
            {
                IdParceiroParametro = parceiroParametro.IdParceiroParametro,
                IdParceiroSistema = parceiroParametro.IdParceiroSistema,
                Valor = parceiroParametro.Valor,
                Chave = parceiroParametro.Chave,
                Situacao = parceiroParametro.Situacao
            });
        }

    }
}
