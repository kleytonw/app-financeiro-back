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
using ERP_API.Models;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class MensagemLogController : ControllerBase
    {
        protected Context context;
        public MensagemLogController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.MensagemLog
                  .Select(m => new
                  {
                      m.IdMensagemLog,
                      m.Descricao,
                      m.LogMensagemErro,
                      m.DataInclusao,
                      m.Mensagem.Texto,
                      m.Situacao
                  }).Take(500).ToList();
            return Ok(result);
        }


        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var result = context.MensagemLog.Where(x => x.DataInclusao.Date >= model.DataInicio.Date && x.DataInclusao.Date <= model.DataFim.Date);

            return Ok(result.Select(m => new
            {
                m.IdMensagemLog,
                m.IdMensagem,
                m.Descricao,
                m.DataInclusao,
                m.LogMensagemErro,
                m.Mensagem.Texto,
                m.Situacao
            }).Take(500).ToList());
        }

        [HttpGet]
        [Route("listarMensagem")]
        public IActionResult ListarMensagem()
        {
            var result = context.Mensagem
                  .Select(m => new
                  {
                      m.IdMensagem,
                      m.Texto,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarMensagemErro")]
        public IActionResult ListarMensagemErro()
        {
            var result = context.MensagemLog
                  .Select(m => new
                  {
                      m.Descricao,
                      m.LogMensagemErro,
                      m.DataInclusao,
                      m.IdMensagemLog,
                      m.IdMensagem,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] MensagemLogRequest model)
        {
            MensagemLog mensagemLog;
            Mensagem mensagem;
            if (model.IdMensagemLog > 0)
            {
                mensagemLog = context.MensagemLog.FirstOrDefault(x => x.IdMensagemLog == model.IdMensagemLog);
                mensagem = context.Mensagem.FirstOrDefault(x => x.IdMensagem == model.IdMensagem);
                mensagemLog.Alterar(mensagem ,model.Descricao, model.LogMensagemErro, User.Identity.Name);

                context.Update(mensagemLog);
            }
            else
            {
                mensagemLog = context.MensagemLog.FirstOrDefault(x => x.IdMensagemLog == model.IdMensagemLog);
                mensagem = context.Mensagem.FirstOrDefault(x => x.IdMensagem == model.IdMensagem);
                mensagemLog = new MensagemLog(mensagem, model.Descricao, model.LogMensagemErro, User.Identity.Name);
                context.MensagemLog.Add(mensagemLog);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var mensagemLog = context.MensagemLog.FirstOrDefault(x => x.IdMensagemLog == id);
            mensagemLog.Excluir(User.Identity.Name);

            context.Update(mensagemLog);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluirMensagemLog")]
        public IActionResult ExcluirSetorProduto(int idMensagemLog)
        {
            var mensagemLog = context.MensagemLog.FirstOrDefault(x => x.IdMensagemLog == idMensagemLog);
            mensagemLog.Excluir(User.Identity.Name);

            context.Remove(mensagemLog);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var mensagem = context.Mensagem.FirstOrDefault(x => x.IdMensagem == id);
            if (mensagem == null)
                return BadRequest("Tipo de Mensagem não encontrado ");

            return Ok(new MensagemLogResponse()
            {
                IdMensagem = mensagem.IdMensagem,
                Texto = mensagem.Texto,
            });
        }
    }
}

