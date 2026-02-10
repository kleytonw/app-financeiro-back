using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class PlanoContaController : ControllerBase
    {
        private Context _context;

        public PlanoContaController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar()
        {
            var result = _context.PlanoConta
                  .Select(m => new
                  {
                      m.IdPlanoConta,
                      m.Codigo,
                      m.Descricao,
                      m.Classificacao,
                      m.Tipo,
                      m.Situacao
                  }).ToList();


            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] PlanoContaRequest model)
        {
            if (model.IdPlanoConta > 0)
            {
                var planoConta = _context.PlanoConta.FirstOrDefault(c => c.IdPlanoConta == model.IdPlanoConta);
                if (planoConta == null)
                    return BadRequest("Plano de Conta não encontrado.");

                planoConta.Alterar(model.Codigo, model.Descricao, model.Classificacao, model.Tipo, User.Identity.Name);

                _context.PlanoConta.Update(planoConta);
            }
            else
            {
                var planoConta = new PlanoConta(model.Codigo, model.Descricao, model.Classificacao, model.Tipo, User.Identity.Name);
                _context.PlanoConta.Add(planoConta);
            }

            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var planoConta = _context.PlanoConta.FirstOrDefault(c => c.IdPlanoConta == id);
            if (planoConta == null)
                return BadRequest("Plano de Conta não encontrado.");

            planoConta.Excluir(User.Identity.Name);
            _context.PlanoConta.Update(planoConta);

            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var planoConta = _context.PlanoConta
                .Where(c => c.IdPlanoConta == id)
                .Select(m => new PlanoContaResponse
                {
                    IdPlanoConta = m.IdPlanoConta,
                    Codigo = m.Codigo,
                    Descricao = m.Descricao,
                    Classificacao = m.Classificacao,
                    Tipo = m.Tipo,
                    Situacao = m.Situacao
                }).FirstOrDefault();

            if (planoConta == null)
                return BadRequest("Plano de Conta não encontrado.");

            return Ok(planoConta);
        }
    }
}

