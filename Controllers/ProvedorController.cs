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
    public class ProvedorController : ControllerBase
    {
        protected Context context;
        public ProvedorController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Provedor
                  .Select(m => new
                  {
                      m.IdProvedor,
                      m.NomeProvedor,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ProvedorRequest model)
        {
            Provedor provedor;
            if (model.IdProvedor > 0)
            {
                provedor = context.Provedor.FirstOrDefault(x => x.IdProvedor == model.IdProvedor);
                provedor.Alterar(model.Nome, User.Identity.Name);

                context.Update(provedor);
            }
            else
            {
                provedor = new Provedor(model.Nome, User.Identity.Name);
                context.Provedor.Add(provedor);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var provedor = context.Provedor.FirstOrDefault(x => x.IdProvedor == id);
            provedor.Excluir(User.Identity.Name);

            context.Update(provedor);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var provedor = context.Provedor.FirstOrDefault(x => x.IdProvedor == id);
            if (provedor == null)
                return BadRequest("Provedor não encontrado ");

            return Ok(new ProvedorResponse()
            {
                IdProvedor = provedor.IdProvedor,
                Nome = provedor.NomeProvedor,
                Situacao = provedor.Situacao
            });
        }
    }
}
