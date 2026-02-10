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

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class CaracteristicaController : ControllerBase
    {
        protected Context context;
        public CaracteristicaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Caracteristica
                  .Select(m => new
                  {
                      m.IdCaracteristica,
                      m.Nome,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] CaracteristicaRequest model)
        {
            Caracteristica caracteristica;
            if (model.IdCaracteristica > 0)
            {
                caracteristica = context.Caracteristica.FirstOrDefault(x => x.IdCaracteristica == model.IdCaracteristica);
                caracteristica.Alterar(model.Nome, User.Identity.Name);

                context.Update(caracteristica);
            }
            else
            {
                caracteristica = new Caracteristica(model.Nome, User.Identity.Name);
                context.Caracteristica.Add(caracteristica);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var caracteristica = context.Caracteristica.FirstOrDefault(x => x.IdCaracteristica == id);
            caracteristica.Excluir(User.Identity.Name);

            context.Update(caracteristica);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var caracteristica = context.Caracteristica.FirstOrDefault(x => x.IdCaracteristica == id);
            if (caracteristica == null)
                return BadRequest("Caracteristica não encontrada ");

            return Ok(new CaracteristicaResponse()
            {
                IdCaracteristica = caracteristica.IdCaracteristica,
                Nome = caracteristica.Nome,
                Situacao = caracteristica.Situacao
            });
        }

    }
}
