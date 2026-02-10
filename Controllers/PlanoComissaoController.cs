using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.CodeDom;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PlanoComissaoController : ControllerBase
    {
        private Context context;

        public PlanoComissaoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar()
        {
            var result = context.PlanoComissao.
                Select(m => new
                {
                    m.IdPlanoComissao,
                    m.Nivel,
                    m.Percentual,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] PlanoComissaoRequest model)
        {
            if (model.IdPlanoComissao > 0)
            {
                var planoComissao = context.PlanoComissao.FirstOrDefault(x => x.IdPlanoComissao == model.IdPlanoComissao);
                if (planoComissao == null)
                    return BadRequest("Plano de comissão não encontrado");
                planoComissao.Alterar(model.Nivel, model.Percentual, User.Identity.Name);
                context.PlanoComissao.Update(planoComissao);
            }
            else
            {
                var planoComissao = new PlanoComissao(model.Nivel, model.Percentual, User.Identity.Name);
                context.Add(planoComissao);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var planoComissao = context.PlanoComissao.FirstOrDefault(x => x.IdPlanoComissao == id);
            if (planoComissao == null)
                return BadRequest("Plano de comissão não encontrado");

            planoComissao.Excluir(User.Identity.Name);
            context.PlanoComissao.Update(planoComissao);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var planoComissao = context.PlanoComissao.FirstOrDefault(x => x.IdPlanoComissao == id);
            if (planoComissao == null)
                return BadRequest("Plano de comissão não encontrado");

            return Ok(new PlanoComissaoResponse
            {
                IdPlanoComissao = planoComissao.IdPlanoComissao,
                Nivel = planoComissao.Nivel,
                Percentual = planoComissao.Percentual,
            });
        }
    }
}
