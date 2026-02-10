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
    public class BancoController : ControllerBase
    {
        protected Context context;

        public BancoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {

            var result = context.Banco
                .Select(m => new {
                    m.IdBanco,
                    m.NomeBanco,
                    m.CodigoBancoTecnoSpeed,
                    m.Situacao
                }).OrderBy(x=>x.NomeBanco).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] BancoRequest model)
        {
            Banco banco;
            if (model.IdBanco > 0)
            {
                banco = context.Banco.FirstOrDefault(x => x.IdBanco == model.IdBanco);
                banco.Alterar(model.NomeBanco, model.CodigoBancoTecnoSpeed, User.Identity.Name);
            }
            else
            {
                banco = new Banco(
                    model.NomeBanco,
                    model.CodigoBancoTecnoSpeed,
                    User.Identity.Name
                );

                context.Banco.Add(banco);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var banco = context.Banco.FirstOrDefault(x => x.IdBanco == id);
            if (banco == null)
                return BadRequest("Banco não encontrado ");
            banco.Excluir(User.Identity.Name);

            context.Update(banco);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var banco = context.Banco.FirstOrDefault(x => x.IdBanco == id);
            if (banco == null)
                return BadRequest("Banco não encontrado ");

            return Ok(new BancoResponse()
            {
                IdBanco = banco.IdBanco,
                NomeBanco = banco.NomeBanco,
                CodigoBancoTecnoSpeed = banco.CodigoBancoTecnoSpeed,
                Situacao = banco.Situacao
            });
        }
    }
}

