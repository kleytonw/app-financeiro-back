using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class TabelaPrecoItemController : ControllerBase
    {
        protected Context context;

        public TabelaPrecoItemController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.TabelaPrecoItem
                .Select(m => new {
                    m.IdTabelaPrecoItem,
                    m.IdTabelaPreco,
                    m.IdProduto,
                    m.Situacao
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

        [HttpGet]
        [Route("listarTabelaPreco")]
        public IActionResult ListarTabelaPreco()
        {
            var result = context.TabelaPreco
                .Select(m => new {
                    m.IdTabelaPreco,
                    m.Nome
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] TabelaPrecoItemRequest model)
        {
            Produto produto;
            TabelaPreco tabelaPreco;
            TabelaPrecoItem tabelaPrecoItem;
            if (model.IdTabelaPrecoItem > 0)
            {
                produto = context.Produto.FirstOrDefault(x => x.IdProduto == model.IdProduto);
                tabelaPreco = context.TabelaPreco.FirstOrDefault(x => x.IdTabelaPreco == model.IdTabelaPreco);
                tabelaPrecoItem = context.TabelaPrecoItem.FirstOrDefault(x => x.IdTabelaPrecoItem == model.IdTabelaPrecoItem);
                tabelaPrecoItem.Alterar(tabelaPreco, produto, model.ValorVenda, User.Identity.Name);
            }
            else
            {
                produto = context.Produto.FirstOrDefault(x => x.IdProduto == model.IdProduto);
                tabelaPreco = context.TabelaPreco.FirstOrDefault(x => x.IdTabelaPreco == model.IdTabelaPreco);
                tabelaPrecoItem = context.TabelaPrecoItem.FirstOrDefault(x => x.IdTabelaPrecoItem == model.IdTabelaPrecoItem);
                tabelaPrecoItem = new TabelaPrecoItem(
                    tabelaPreco,
                    produto,
                    model.ValorVenda,
                    User.Identity.Name
                );

                context.TabelaPrecoItem.Add(tabelaPrecoItem);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var tabelaPrecoItem = context.TabelaPrecoItem.FirstOrDefault(x => x.IdTabelaPrecoItem == id);
            tabelaPrecoItem.Excluir(User.Identity.Name);

            context.Update(tabelaPrecoItem);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var tabelaPrecoItem = context.TabelaPrecoItem.FirstOrDefault(x => x.IdTabelaPrecoItem == id);
            if (tabelaPrecoItem == null)
                return BadRequest("Tabela de Preço por item não encontrado ");

            return Ok(new TabelaPrecoItemResponse()
            {
                IdTabelaPrecoItem = tabelaPrecoItem.IdTabelaPrecoItem,
                IdTabelaPreco = tabelaPrecoItem.IdTabelaPreco,
                IdProduto = tabelaPrecoItem.IdProduto,
                ValorVenda = tabelaPrecoItem.ValorVenda,
                Situacao = tabelaPrecoItem.Situacao
            });
        }
    }
}
