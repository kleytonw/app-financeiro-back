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
    public class CarrinhoController : ControllerBase
    {
        protected Context context;
        protected Context context1;

        public CarrinhoController(Context context)
        {
            this.context = context;
            this.context1 = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Blog
                .Select(m => new {
                    m.IdBlog,
                    m.Empresa.Nome,
                    m.Autor,
                    m.Titulo,
                    m.Subtitulo,
                    m.Descricao,
                    m.Data,
                    m.LinkFoto,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("listarCliente")]
        public IActionResult ListarCliente()
        {
            var result = context.Cliente
                .Select(m => new {
                    m.IdPessoa,
                    m.Pessoa.Nome
                }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("listarVendedor")]
        public IActionResult ListarVendedor()
        {
            var result = context.Vendedor
                .Select(m => new {
                    m.IdPessoa,
                    m.Pessoa.Nome
                }).ToList();

            return Ok(result);
        }


        [HttpGet]
        [Route("listarProduto")]
        public IActionResult ListarProduto()
        {
            var result = context.Produto
                .Select(m => new {
                    m.IdProduto,
                    m.NomeProduto
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("novo")]
        public IActionResult Novo([FromBody] CarrinhoRequest model)
        {
            Cliente cliente;
            Vendedor vendedor;

            cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
            vendedor = context.Vendedor.FirstOrDefault(x => x.IdPessoa == model.IdVendedor);

            // cria um novo carrinho
            var carrinho = new Carrinho(model.DataFinal, cliente, vendedor, User.Identity.Name);

            foreach (var item in model.CarrinhoItens)
            {
                var produto = context.Produto.FirstOrDefault(x => x.IdProduto == item.IdProduto);
                if (produto == null)
                    return BadRequest("Produto não encontrado");
                carrinho.Itens.Add(new LancamentoItem(produto, item.Preco, item.Quantidade, User.Identity.Name));
            }
            carrinho.CalcularTotal();

            context.Carrinho.Add(carrinho);
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("editar")]
        public IActionResult Editar([FromBody] CarrinhoRequest model)
        {
            Cliente cliente;
            Vendedor vendedor;

            cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
            vendedor = context.Vendedor.FirstOrDefault(x => x.IdPessoa == model.IdVendedor);

            // editar carrinho
            var carrinho = context.Carrinho.Include(x => x.Itens).FirstOrDefault(x => x.IdCarrinho == model.IdCarrinho);
            carrinho.Alterar(model.DataFinal, cliente, vendedor, User.Identity.Name);

            carrinho.Itens.Clear(); // limpa o carrinho e adiciona novamente os itens

            foreach (var item in model.CarrinhoItens)
            {
                var produto = context.Produto.FirstOrDefault(x => x.IdProduto == item.IdProduto);
                if (produto == null)
                    return BadRequest("Produto não encontrado");
                carrinho.Itens.Add(new LancamentoItem(produto, item.Preco, item.Quantidade, User.Identity.Name));
            }
            carrinho.CalcularTotal();

            context.Carrinho.Update(carrinho);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var carrinho = context.Carrinho.FirstOrDefault(x => x.IdCarrinho == id);
            carrinho.Excluir(User.Identity.Name);

            context.Update(carrinho);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var carrinho = context.Carrinho.Include(x=>x.Itens).FirstOrDefault(x => x.IdCarrinho == id);
            if (carrinho == null)
                return BadRequest("Carrinho não encontrado ");

            var carrinhoResponse = new CarrinhoResponse()
            {
                IdCarrinho = carrinho.IdCarrinho,
                IdCliente = carrinho.IdCliente,
                IdVendedor = carrinho.IdVendedor,
                DataFinal = carrinho.DataFinal,
                Situacao = carrinho.Situacao
            };

            foreach(var item in carrinho.Itens)
            {
                carrinhoResponse.CarrinhoItens.Add(new LancamentoItemDTO()
                {
                    IdLancamentoItem = item.IdLancamentoItem,
                    IdProduto = item.IdProduto,
                    NomeProduto = item.NomeProduto,
                    Preco = item.Preco,
                    Quantidade = item.Quantidade,
                    Subtotal = item.Subtotal
                });
            }
            return Ok(carrinhoResponse);
        }
    }
}

