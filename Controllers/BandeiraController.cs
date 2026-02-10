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
    public class BandeiraController : ControllerBase
    {
        protected Context context;

        public BandeiraController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {

            var result = context.Bandeira
                .Select(m => new {
                    m.IdBandeira,
                    m.NomeBandeira,
                    m.CodigoBandeiraCartao,
                    m.CodigoBandeiraCartaoRede,
                    m.Situacao
                }).OrderBy(x => x.NomeBandeira).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] BandeiraRequest model)
        {
            Bandeira bandeira;
            if (model.IdBandeira > 0)
            {
                bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == model.IdBandeira);
                bandeira.Alterar(model.NomeBandeira, model.CodigoBandeiraCartao, model.CodigoBandeiraCartaoRede, User.Identity.Name);
                context.Update(bandeira);
            }
            else
            {
                bandeira = new Bandeira(
                    model.NomeBandeira,
                    model.CodigoBandeiraCartao,
                    model.CodigoBandeiraCartaoRede,
                    User.Identity.Name
                );

                context.Bandeira.Add(bandeira);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == id);
            bandeira.Excluir(User.Identity.Name);

            context.Update(bandeira);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == id);
            if (bandeira == null)
                return BadRequest("Bandeira não encontrado ");

            return Ok(new BandeiraResponse()
            {
                IdBandeira = bandeira.IdBandeira,
                NomeBandeira = bandeira.NomeBandeira,
                CodigoBandeiraCartao = bandeira.CodigoBandeiraCartao,
                CodigoBandeiraCartaoRede = bandeira.CodigoBandeiraCartaoRede,
                Situacao = bandeira.Situacao
            });
        }
    }
}

