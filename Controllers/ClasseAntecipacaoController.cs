using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ClasseAntecipacaoController : ControllerBase
    {
        public Context context;
        public ClasseAntecipacaoController(Context context)
        {
            this.context = context;
        }
        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar()
        {
            var result = context.ClasseAntecipacao
             .Select(m => new
             {
                 m.IdClasseAntecipacao,
                 m.Descricao,
                 m.Situacao
             }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ClasseAntecipacaoRequest model)
        {
            if (model.IdClasseAntecipacao > 0)
            {
                var classeAntecipacao = context.ClasseAntecipacao.FirstOrDefault(x => x.IdClasseAntecipacao == model.IdClasseAntecipacao);
                if (classeAntecipacao == null)
                    return NotFound("Classe de Antecipacao não encontrada");
                classeAntecipacao.Alterar(model.Descricao, model.Situacao);
                context.SaveChanges();
            }
            else
            {
                var classeAntecipacao = new ClasseAntecipacao(model.Descricao, model.Situacao);
                context.ClasseAntecipacao.Add(classeAntecipacao);
                context.SaveChanges();
            }
            return Ok();
        }
        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var classeAntecipacao = context.ClasseAntecipacao.FirstOrDefault(x => x.IdClasseAntecipacao == id);
            if (classeAntecipacao == null)
                return NotFound("Classe de Antecipacao não encontrada");
            context.Update(classeAntecipacao);
            context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var classeAntecipacao = context.ClasseAntecipacao.FirstOrDefault(x => x.IdClasseAntecipacao == id);
            if (classeAntecipacao == null)
                return BadRequest("A classe de antecipacao não foi encontrada ");
            return Ok(new ClasseAntecipacaoResponse()
            {
                IdClasseAntecipacao = classeAntecipacao.IdClasseAntecipacao,
                Descricao = classeAntecipacao.Descricao,
                Situacao = classeAntecipacao.Situacao
            });
        }
    }
}