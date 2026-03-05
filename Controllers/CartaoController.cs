using ERP.Infra;
using ERP.Models;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CartaoController(Context context) : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Cartao.Where(x => x.Situacao == "Ativo")
                  .Select(m => new
                  {
                      m.IdCartao,
                      m.Nome,
                      m.Bandeira,
                      m.UltimosDigitos,
                      m.DiaFechamento,
                      m.DiaVencimento,
                      m.LimiteTotal,
                      m.Situacao
                  }).Take(500).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] CartaoRequest model)
        {

            Cartao cartao;
            if (model.IdCartao > 0)
            {
                cartao = context.Cartao.FirstOrDefault(x => x.IdCartao == model.IdCartao);
                if (cartao == null)
                    return BadRequest();

                cartao.Alterar(
                    model.Nome,
                    model.Bandeira,
                    model.UltimosDigitos,
                    model.DiaFechamento,
                    model.DiaVencimento,
                    model.LimiteTotal,
                    User.Identity.Name);

                context.Update(cartao);
            }
            else
            {
                cartao = new Cartao(model.Nome, model.Bandeira, model.UltimosDigitos, model.DiaFechamento, model.DiaVencimento, model.LimiteTotal,  User.Identity.Name);
                context.Add(cartao);
            }
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var cartao = context.Cartao.FirstOrDefault(x => x.IdCartao == id);
            if (cartao == null)
                return BadRequest("Cartão não encontrado ");

            return Ok(new CartaoResponse()
            {
                IdCartao = cartao.IdCartao,
                Nome = cartao.Nome,
                Bandeira = cartao.Bandeira,
                UltimosDigitos = cartao.UltimosDigitos,
                DiaFechamento = cartao.DiaFechamento,
                DiaVencimento = cartao.DiaVencimento,
                LimiteTotal = cartao.LimiteTotal,
                Situacao = cartao.Situacao

            });
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var cartao = context.Cartao.FirstOrDefault(x => x.IdCartao == id);

            cartao.Excluir(User.Identity.Name);

            context.Update(cartao);
            context.SaveChanges();
            return Ok();
        }
    }
}
