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
    public class ProdutoController : ControllerBase
    {
        protected Context context;
        public ProdutoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
           var result = context.Produto
                 .Select(m => new 
                 {
                     m.IdProduto,
                     m.NomeProduto,
                     m.Situacao
                 }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarSetor")]
        public IActionResult ListarSetor()
        {
            var result = context.Setor
                  .Select(m => new
                  {
                      m.IdSetor,
                      m.Nome
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarUnidades")]
        public IActionResult ListarUnidades()
        {
            var result = context.UnidadeMedida
                  .Select(m => new
                  {
                      m.IdUnidadeMedida,
                      m.Nome
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarFornecedores")]
        public IActionResult ListarFornecedores()
        {
            var result = context.Fornecedor
                  .Select(m => new
                  {
                      m.IdPessoa,
                      m.Pessoa.Nome
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarProdutoSetor")]
        public IActionResult ListarProdutoSetor(int idProduto)
        {
            var result = context.SetorProduto.Include(x => x.Setor).Where(x => x.IdProduto == idProduto)
                  .Select(m => new
                  {
                      m.Setor.Nome,
                      m.IdSetorProduto,
                      m.IdProduto,
                      m.Produto.NomeProduto
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ProdutoRequest model)
        {

            Produto produto;
            UnidadeMedida unidadeMedidaCompra;
            UnidadeMedida unidadeMedidaVenda;
            UnidadeMedida unidadeMedidaArmazenamento;
            Fornecedor fornecedor;
            if (model.IdProduto > 0)
            {
                produto = context.Produto.FirstOrDefault(x => x.IdProduto == model.IdProduto);
                unidadeMedidaCompra = context.UnidadeMedida.FirstOrDefault(x => x.IdUnidadeMedida == model.IdUnidadeMedidaCompra);
                unidadeMedidaVenda = context.UnidadeMedida.FirstOrDefault(x => x.IdUnidadeMedida == model.IdUnidadeMedidaVenda);
                unidadeMedidaArmazenamento = context.UnidadeMedida.FirstOrDefault(x => x.IdUnidadeMedida == model.IdUnidadeMedidaArmazenamento);
                fornecedor = context.Fornecedor.FirstOrDefault(x => x.IdPessoa == model.IdFornecedor);
                produto.Alterar(
                    model.NomeProduto, 
                    model.CodigoProduto,
                    context.GrupoProduto.FirstOrDefault(x=>x.IdGrupoProduto==model.IdGrupoProduto),
                    fornecedor,
                    model.PermitirCompra,
                    unidadeMedidaCompra,
                    model.PrecoDeCompra,
                    model.PermitirVenda,
                    unidadeMedidaVenda,
                    model.PrecoDeVenda,
                    model.ControleDeEstoque,
                    model.QuantidadeDeEstoque,
                    model.EstoqueMinimo,
                    unidadeMedidaArmazenamento,
                    model.LinkFoto,
                    model.Descricao,
                    model.Ean,
                    model.ValorVenda,
                    model.ValorCusto,
                    User.Identity.Name);

                context.Update(produto);
            }
            else
            {
                unidadeMedidaCompra = context.UnidadeMedida.FirstOrDefault(x => x.IdUnidadeMedida == model.IdUnidadeMedidaCompra);
                unidadeMedidaVenda = context.UnidadeMedida.FirstOrDefault(x => x.IdUnidadeMedida == model.IdUnidadeMedidaVenda);
                unidadeMedidaArmazenamento = context.UnidadeMedida.FirstOrDefault(x => x.IdUnidadeMedida == model.IdUnidadeMedidaArmazenamento);
                fornecedor = context.Fornecedor.FirstOrDefault(x => x.IdPessoa == model.IdFornecedor);
                produto = new Produto(
                    model.NomeProduto,
                    model.CodigoProduto, 
                    context.GrupoProduto.FirstOrDefault(x=>x.IdGrupoProduto==model.IdGrupoProduto),
                    fornecedor,
                    model.PermitirCompra,
                    unidadeMedidaCompra,
                    model.PrecoDeCompra,
                    model.PermitirVenda,
                    unidadeMedidaVenda,
                    model.PrecoDeVenda,
                    model.ControleDeEstoque,
                    model.QuantidadeDeEstoque,
                    model.EstoqueMinimo,
                    unidadeMedidaArmazenamento,
                    model.LinkFoto,
                    model.Descricao,
                    model.Ean, 
                    model.ValorVenda,
                    model.ValorCusto, 
                    User.Identity.Name);
                context.Produto.Add(produto);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("salvarProdutoSetor")]
        public IActionResult SalvarProdutoSetor([FromBody] ProdutoRequest model)
        {

            var produto = context.Produto.FirstOrDefault(x => x.IdProduto == model.IdProduto);
            var setor = context.Setor.FirstOrDefault(x => x.IdSetor == model.IdSetor);
            if (produto == null)
            {
                return BadRequest("É necessário informar o Produto ");
            }


            if (setor == null)
            {
                return BadRequest("É necessário informar o Setor ");
            }

            
            var checkSetorProdutoRepetido = context.SetorProduto.FirstOrDefault(x => x.IdProduto == model.IdProduto && x.IdSetor == model.IdSetor);
            if (checkSetorProdutoRepetido != null)
            {
                return BadRequest("O Setor já foi cadastrado no Produto");
            }

            var setorProduto = new SetorProduto(setor, produto, User.Identity.Name);
            context.SetorProduto.Add(setorProduto);

            context.SaveChanges();
            return Ok();

        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var result = context.Produto.AsQueryable();
            switch (model.Chave)
            {
                case "Nome":
                    result = result.Where(x => x.NomeProduto.Contains(model.Valor.ToUpper()));
                    break;
                case "IdProduto":
                    result = result.Where(x => x.IdProduto == int.Parse(model.Valor));
                    break;
                case "Situacao":
                    result = result.Where(x => x.Situacao.Contains(model.Valor.ToUpper()));
                    break;
                default:
                    // code block
                    break;
            }

            return Ok(result.Select(m => new
            {
                m.IdProduto,
                m.NomeProduto,
                m.IdGrupoProduto,
                m.Ean,
                m.Situacao
            }).Take(500).ToList());
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var produto = context.Produto.FirstOrDefault(x => x.IdProduto == id);
            produto.Excluir(User.Identity.Name);

            context.Update(produto);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluirProdutoSetor")]
        public IActionResult ExcluirProdutoSetor(int idSetorProduto)
        {
            var setorProduto = context.SetorProduto.FirstOrDefault(x => x.IdSetorProduto == idSetorProduto);
            setorProduto.Excluir(User.Identity.Name);

            context.Remove(setorProduto);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var produto = context.Produto.Include(x=>x.GrupoProduto).FirstOrDefault(x => x.IdProduto == id);
            if (produto == null)
                return BadRequest("Produto não encontrado ");

            return Ok(new ProdutoResponse()
            {
                IdProduto = produto.IdProduto,
                NomeProduto = produto.NomeProduto,
                Ean = produto.Ean,
                PermitirCompra = produto.PermitirCompra,
                IdUnidadeMedidaCompra = produto.IdUnidadeMedidaCompra,
                PrecoDeCompra = produto.PrecoDeCompra,
                IdFornecedor = produto.IdFornecedor,
                PermitirVenda = produto.PermitirVenda,
                IdUnidadeMedidaVenda = produto.IdUnidadeMedidaVenda,
                PrecoDeVenda = produto.PrecoDeVenda,
                ControleDeEstoque = produto.ControleDeEstoque,
                QuantidadeDeEstoque = produto.QuantidadeDeEstoque,
                EstoqueMinimo = produto.EstoqueMinimo,
                IdUnidadeMedidaArmazenamento = produto.IdUnidadeMedidaArmazenamento,
                LinkFoto = produto.LinkFoto,
                Descricao = produto.Descricao,
                CodigoProduto = produto.CodigoProduto,
                IdGrupoProduto = produto.IdGrupoProduto,
                ValorVenda = produto.ValorVenda,
                ValorCusto = produto.ValorCusto,
                Situacao = produto.Situacao
            });
        }

        [HttpGet]
        [Route("obterProdutoSetor")]
        public IActionResult ObterProdutoSetor(int id)
        {
            Produto produto;

            produto = context.Produto.FirstOrDefault(x => x.IdProduto == id);
            if (produto == null)
                return BadRequest("Produto não encontrado ");

            return Ok(new ProdutoResponse()
            {
                IdSetor = produto.IdProduto,
                NomeProduto = produto.NomeProduto,
                Situacao = produto.Situacao
            });
        }
    }
}
