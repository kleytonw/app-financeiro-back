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
    public class TipoMensagemController : ControllerBase
    {
        protected Context context;
        public TipoMensagemController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.TipoMensagem
                  .Select(m => new
                  {
                      m.IdTipoMensagem,
                      m.Nome,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] TipoMensagemRequest model)
        {
            TipoMensagem tipoMensagem;
            if (model.IdTipoMensagem > 0)
            {
                tipoMensagem = context.TipoMensagem.FirstOrDefault(x => x.IdTipoMensagem == model.IdTipoMensagem);
                tipoMensagem.Alterar(model.Nome, User.Identity.Name);

                context.Update(tipoMensagem);
            }
            else
            {
                tipoMensagem = new TipoMensagem(model.Nome, User.Identity.Name);
                context.TipoMensagem.Add(tipoMensagem);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var tipoMensagem = context.TipoMensagem.FirstOrDefault(x => x.IdTipoMensagem == id);
            tipoMensagem.Excluir(User.Identity.Name);

            context.Update(tipoMensagem);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var tipoMensagem = context.TipoMensagem.FirstOrDefault(x => x.IdTipoMensagem == id);
            if (tipoMensagem == null)
                return BadRequest("Tipo de Mensagem não encontrado ");

            return Ok(new TipoMensagemResponse()
            {
                IdTipoMensagem = tipoMensagem.IdTipoMensagem,
                Nome = tipoMensagem.Nome,
                Situacao = tipoMensagem.Situacao
            });
        }
    }
}
