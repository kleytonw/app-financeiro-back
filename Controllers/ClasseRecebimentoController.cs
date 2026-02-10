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
    public class ClasseRecebimentoController : ControllerBase
    {
        public Context context;

        public ClasseRecebimentoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar()
        {
            var result = context.ClasseRecebimento
             .Select(m => new
             {
                 m.IdClasseRecebimento,
                 m.Descricao,
                 m.Situacao
             }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ClasseRecebimentoRequest model)
        {
            if (model.IdClasseRecebimento > 0)
            {
                var classeRecebimento = context.ClasseRecebimento.FirstOrDefault(x => x.IdClasseRecebimento == model.IdClasseRecebimento);
                if (classeRecebimento == null)
                    return NotFound("Classe de Recebimento não encontrada");
                classeRecebimento.Alterar(model.Descricao, model.Situacao);
                context.SaveChanges();
            }
            else
            {
                var classeRecebimento = new ClasseRecebimento(model.Descricao, model.Situacao);
                context.ClasseRecebimento.Add(classeRecebimento);
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var classeRecebimento = context.ClasseRecebimento.FirstOrDefault(x => x.IdClasseRecebimento == id);
            if (classeRecebimento == null)
                return NotFound("Classe de Recebimento não encontrada");

            context.Update(classeRecebimento);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var classeRecebimento = context.ClasseRecebimento.FirstOrDefault(x => x.IdClasseRecebimento == id);
            if (classeRecebimento == null)
                return BadRequest("A classe de recebimento não foi encontrada ");


           return Ok(new ClasseRecebimentoResponse()
           {
               IdClasseRecebimento = classeRecebimento.IdClasseRecebimento,
               Descricao = classeRecebimento.Descricao,
               Situacao = classeRecebimento.Situacao
           });
        }
    }

}
