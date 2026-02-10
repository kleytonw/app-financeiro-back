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
    public class OperadoraController : ControllerBase
    {
        protected Context context;

        public OperadoraController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {

            var result = context.Operadora
                .Select(m => new {
                    m.IdOperadora,
                    m.NomeOperadora,
                    m.Situacao
                }).ToList();

            return Ok(result);
        } 

        [HttpGet]
        [Route("listarAtivas")]
        public IActionResult ListarAtivas()
        {

            var result = context.Operadora
                .Select(m => new {
                    m.IdOperadora,
                    m.NomeOperadora,
                    m.Situacao
                }).Where(x=>x.Situacao=="Ativo").ToList();

            return Ok(result);
        }



        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] OperadoraRequest model)
        {
            Operadora operadora;
            if (model.IdOperadora > 0)
            {
                operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == model.IdOperadora);
                operadora.Alterar(model.NomeOperadora, User.Identity.Name);
            }
            else
            {
                operadora = new Operadora(
                    model.NomeOperadora,
                    User.Identity.Name
                );

                context.Operadora.Add(operadora);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == id);
            operadora.Excluir(User.Identity.Name);

            context.Update(operadora);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == id);
            if (operadora == null)
                return BadRequest("A operadora de cartão não foi encontrada ");

            return Ok(new OperadoraResponse()
            {
                IdOperadora = operadora.IdOperadora,
                NomeOperadora = operadora.NomeOperadora,
                Situacao = operadora.Situacao
            });
        }
    }
}


