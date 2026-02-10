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
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class MensagemController : ControllerBase
    {
        protected Context context;
        public MensagemController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Mensagem
                  .Select(m => new
                  {
                      m.IdMensagem,
                      m.Data,
                      m.Texto,
                      m.TipoMensagem.Nome,
                      m.Provedor.NomeProvedor,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarTipoMensagem")]
        public IActionResult ListarTipoMensagem()
        {
            var result = context.TipoMensagem
                  .Select(m => new
                  {
                      m.IdTipoMensagem,
                      m.Nome
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarProvedor")]
        public IActionResult ListarProvedor()
        {
            var result = context.Provedor
                  .Select(m => new
                  {
                      m.IdProvedor,
                      m.NomeProvedor
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] MensagemRequest model)
        {
            Provedor provedor;
            TipoMensagem tipoMensagem;
            Mensagem mensagem;
            if (model.IdMensagem > 0)
            {
                provedor = context.Provedor.FirstOrDefault(x => x.IdProvedor == model.IdProvedor);
                tipoMensagem = context.TipoMensagem.FirstOrDefault(x => x.IdTipoMensagem == model.IdTipoMensagem);
                mensagem = context.Mensagem.FirstOrDefault(x => x.IdMensagem == model.IdMensagem);
                mensagem.Alterar(model.Data, model.Texto, tipoMensagem, model.Telefone, model.Email, provedor, User.Identity.Name);

                context.Update(mensagem);
            }
            else
            {
                provedor = context.Provedor.FirstOrDefault(x => x.IdProvedor == model.IdProvedor);
                tipoMensagem = context.TipoMensagem.FirstOrDefault(x => x.IdTipoMensagem == model.IdTipoMensagem);
                mensagem = new Mensagem(model.Data, model.Texto, tipoMensagem, model.Telefone, model.Email, provedor, User.Identity.Name);
                context.Mensagem.Add(mensagem);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var mensagem = context.Mensagem.FirstOrDefault(x => x.IdMensagem == id);
            mensagem.Excluir(User.Identity.Name);

            context.Update(mensagem);
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

            return Ok(new MensagemResponse()
            {
                IdMensagem = mensagem.IdMensagem,
                Data = mensagem.Data,
                Texto = mensagem.Texto,
                NomeTipoMensagem = mensagem.TipoMensagem.Nome,
                Telefone = mensagem.Telefone,
                Email = mensagem.Email,
                NomeProvedor = mensagem.Provedor.NomeProvedor,
                Situacao = mensagem.Situacao
            });
        }
    }
}

