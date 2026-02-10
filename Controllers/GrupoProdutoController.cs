using ERP.Infra;
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

namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class GrupoProdutoController : ControllerBase
    {
        protected Context context;
        public GrupoProdutoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
           var result = context.GrupoProduto
                 .Select(m => new 
                 {
                     m.IdGrupoProduto,
                     m.Nome,
                     m.Situacao
                 }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] GrupoProdutoRequest model)
        {
            GrupoProduto grupoProduto;
            if (model.IdGrupoProduto > 0)
            {
                grupoProduto = context.GrupoProduto.FirstOrDefault(x => x.IdGrupoProduto == model.IdGrupoProduto);
                grupoProduto.Alterar(model.Nome, User.Identity.Name);

                context.Update(grupoProduto);
            }
            else
            {
                grupoProduto = new GrupoProduto(model.Nome, User.Identity.Name);
                context.GrupoProduto.Add(grupoProduto);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var grupoProduto = context.GrupoProduto.FirstOrDefault(x => x.IdGrupoProduto == id);
            grupoProduto.Excluir(User.Identity.Name);

            context.Update(grupoProduto);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var grupoProduto = context.GrupoProduto.FirstOrDefault(x => x.IdGrupoProduto == id);
            if (grupoProduto == null)
                return BadRequest("Grupo de Produto não encontrado ");

            return Ok(new GrupoProdutoResponse()
            {
                IdGrupoProduto = grupoProduto.IdGrupoProduto,
                Nome = grupoProduto.Nome,
                Situacao = grupoProduto.Situacao
            });
        }
    }
}