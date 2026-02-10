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
    public class MovimentacaoEstoqueController : ControllerBase
    {
        protected Context context;
        public MovimentacaoEstoqueController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.MovimentacaoEstoque
                  .Select(m => new
                  {
                      m.IdMovimentacaoEstoque,
                      m.IdProduto,
                      m.Produto.NomeProduto,
                      m.Data,
                      m.Tipo,
                      m.Quantidade,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarProduto")]
        public IActionResult ListarProduto()
        {
            var result = context.Produto
                  .Select(m => new
                  {
                      m.IdProduto,
                      m.NomeProduto
                  }).ToList().OrderBy(m => m.NomeProduto);
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] MovimentacaoEstoqueRequest model)
        {
            MovimentacaoEstoque movimentacaoEstoque;
            Produto produto;
            if (model.IdMovimentacaoEstoque > 0)
            {
                movimentacaoEstoque = context.MovimentacaoEstoque.FirstOrDefault(x => x.IdMovimentacaoEstoque == model.IdMovimentacaoEstoque);
                produto = context.Produto.FirstOrDefault(x => x.IdProduto == model.IdProduto);
                movimentacaoEstoque.Alterar(produto, model.Data, model.Tipo, model.Quantidade, User.Identity.Name);

                context.Update(movimentacaoEstoque);
            }
            else
            {
                produto = context.Produto.FirstOrDefault(x => x.IdProduto == model.IdProduto);
                movimentacaoEstoque = new MovimentacaoEstoque(produto, model.Data, model.Tipo, model.Quantidade, User.Identity.Name);
                context.MovimentacaoEstoque.Add(movimentacaoEstoque);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var movimentacaoEstoque = context.MovimentacaoEstoque.FirstOrDefault(x => x.IdMovimentacaoEstoque == id);
            
            movimentacaoEstoque.Excluir(User.Identity.Name);

            context.Update(movimentacaoEstoque);
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var result = context.MovimentacaoEstoque.Include(c => c.Produto).Where(x => x.Data.Date >= model.DataInicio.Date && x.Data.Date <= model.DataFim.Date &&  x.Produto.NomeProduto.Contains(model.Valor));

            return Ok(result.Select(m => new
            {
                m.IdMovimentacaoEstoque,
                m.IdProduto,
                m.Produto.NomeProduto,
                m.Data,
                m.Tipo,
                m.Quantidade,
                m.Situacao
            }).Take(500).ToList());
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var movimentacaoEstoque = context.MovimentacaoEstoque.FirstOrDefault(x => x.IdMovimentacaoEstoque == id);
            if (movimentacaoEstoque == null)
                return BadRequest("Movimentação de Estoque não encontrada ");

            return Ok(new MovimentacaoEstoqueResponse()
            {
                IdMovimentacaoEstoque = movimentacaoEstoque.IdMovimentacaoEstoque,
                IdProduto = movimentacaoEstoque.IdProduto,
                Data = movimentacaoEstoque.Data,
                Tipo = movimentacaoEstoque.Tipo,
                Quantidade = movimentacaoEstoque.Quantidade,
                Situacao = movimentacaoEstoque.Situacao
            });
        }

    }
}
