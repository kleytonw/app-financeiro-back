using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ERP.Models;
using ERP_API.Domain.Entidades;
using ERP.Domain.Entidades;
using System.Data.Entity;


namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class TabelaPrecoController : ControllerBase
    {
        protected Context context;

        public TabelaPrecoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.TabelaPreco
                .Select(m => new {
                    m.IdTabelaPreco,
                    m.Nome,
                    m.DataInicio,
                    m.DataTermino,
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
        [Route("listarTabelaPrecoItem")]
        public IActionResult ListarTabelaPrecoItem([FromQuery] int id)
        {
            var result = context.TabelaPrecoItem.Include(x => x.TabelaPreco).Where(x => x.IdTabelaPreco == id)
                .Select(m => new {
                    m.IdTabelaPrecoItem,
                    m.IdTabelaPreco,
                    m.IdProduto,
                    m.Produto.NomeProduto,
                    m.ValorVenda
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] TabelaPrecoRequest model)
        {
            
            TabelaPreco tabelaPreco;
            if(model.DataTermino.Date <= model.DataInicio.Date)
            {
                return BadRequest("A data de Termino precisa ser maior que a data de ínico");
            }

            if (model.IdTabelaPreco > 0)
            {
                tabelaPreco = context.TabelaPreco.FirstOrDefault(x => x.IdTabelaPreco == model.IdTabelaPreco);
                tabelaPreco.Alterar(model.Nome, model.DataInicio, model.DataTermino, User.Identity.Name);
            }
            else
            {
                tabelaPreco = new TabelaPreco(
                    model.Nome,
                    model.DataInicio,
                    model.DataTermino,
                    User.Identity.Name
                );

                context.TabelaPreco.Add(tabelaPreco);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("salvarProduto")]
        public IActionResult SalvarProduto([FromBody] TabelaPrecoItemRequest model)
        {

            TabelaPrecoItem tabelaPrecoItem;
            TabelaPreco tabelaPreco;
            Produto produto;

            produto = context.Produto.FirstOrDefault(x => x.IdProduto == model.IdProduto);
            tabelaPreco = context.TabelaPreco.FirstOrDefault(x => x.IdTabelaPreco == model.IdTabelaPreco);

            if (model.IdProduto == 0)
            {
                BadRequest("É necessario informar o produto");
            }

            var checkProdutoExiste = context.TabelaPrecoItem.FirstOrDefault(x => x.IdProduto == model.IdProduto && x.IdTabelaPreco == model.IdTabelaPreco);

            if (checkProdutoExiste != null)
            {

                checkProdutoExiste.Alterar(tabelaPreco, produto, model.ValorVenda, User.Identity.Name);
                context.TabelaPrecoItem.Update(checkProdutoExiste);
            }
            else
            {
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
            var tabelaPreco = context.TabelaPreco.FirstOrDefault(x => x.IdTabelaPreco == id);
            tabelaPreco.Excluir(User.Identity.Name);

            context.Update(tabelaPreco);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluirProduto")]
        public IActionResult ExcluirProduto(int id)
        {
            var tabelaPrecoItem = context.TabelaPrecoItem.FirstOrDefault(x => x.IdTabelaPrecoItem == id);
            tabelaPrecoItem.Excluir(User.Identity.Name);

            context.Remove(tabelaPrecoItem);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var tabelaPreco = context.TabelaPreco.FirstOrDefault(x => x.IdTabelaPreco == id);
            if (tabelaPreco == null)
                return BadRequest("Tabela de Preço não encontrado ");

            return Ok(new TabelaPrecoResponse()
            {
                IdTabelaPreco = tabelaPreco.IdTabelaPreco,
                Nome = tabelaPreco.Nome,
                DataInicio = tabelaPreco.DataInicio,
                DataTermino = tabelaPreco.DataTermino,
                Situacao = tabelaPreco.Situacao
            });
        }
    }
}

