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
using System.Reflection.PortableExecutable;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class MeioPagamentoController : ControllerBase
    {
        protected Context context;

        public MeioPagamentoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {

            var result = context.MeioPagamento
                .Select(m => new {
                    m.IdMeioPagamento,
                    m.NomeMeioPagamento,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("listarAtivas")]
        public IActionResult ListarAtivas()
        {

            var result = context.MeioPagamento.Where(x => x.Situacao == "Ativo")
                .Select(m => new {
                    m.IdMeioPagamento,
                    m.NomeMeioPagamento,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] MeioPagamentoRequest model)
        {
            MeioPagamento meioPagamento;
            if (model.IdMeioPagamento > 0)
            {
                meioPagamento = context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == model.IdMeioPagamento);
                meioPagamento.Alterar(model.NomeMeioPagamento, User.Identity.Name);
            }
            else
            {
                meioPagamento = new MeioPagamento(
                    model.NomeMeioPagamento,
                    User.Identity.Name
                );

                context.MeioPagamento.Add(meioPagamento);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var meioPagamento = context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == id);
            meioPagamento.Excluir(User.Identity.Name);

            context.Update(meioPagamento);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var meioPagamento = context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == id);
            if (meioPagamento == null)
                return BadRequest("Meio de pagamento não encontrado ");

            return Ok(new MeioPagamentoResponse()
            {
                IdMeioPagamento = meioPagamento.IdMeioPagamento,
                NomeMeioPagamento = meioPagamento.NomeMeioPagamento,
                Situacao = meioPagamento.Situacao
            });
        }
    }
}

