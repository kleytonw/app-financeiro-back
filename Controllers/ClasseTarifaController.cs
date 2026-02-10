using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Hangfire.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ClasseTarifaController : ControllerBase
    {
        protected Context context;
        public ClasseTarifaController(Context context)
        {
            this.context = context;
        }
        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.ClasseTarifa
                .Select(m => new
                {
                    m.IdClasseTarifa,
                    m.Nome,
                    m.Situacao
                }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ClasseTarifaRequest model)
        {

            if (model.IdClasseTarifa > 0)
            {
                var classeTarifa = context.ClasseTarifa.FirstOrDefault(x => x.IdClasseTarifa == model.IdClasseTarifa);
                if (classeTarifa == null)
                    return NotFound("Classe Tarifa não encontrada");

                classeTarifa.Alterar(model.Nome,
                                     User.Identity.Name);
                context.ClasseTarifa.Update(classeTarifa);
                context.SaveChanges();
            }
            else
            {
                var classeTarifa = new ClasseTarifa(model.Nome,
                                                    User.Identity.Name);

                context.ClasseTarifa.Add(classeTarifa);
                context.SaveChanges();
            }

            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var classeTarifa = context.ClasseTarifa.FirstOrDefault(x => x.IdClasseTarifa == id);
            if (classeTarifa == null)
                return BadRequest("Classe Tarifa não encontrada");

            classeTarifa.Excluir(User.Identity.Name);
            context.Update(classeTarifa);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var classeTarifa = context.ClasseTarifa.FirstOrDefault(x => x.IdClasseTarifa == id);
            if (classeTarifa == null)
                return NotFound("Classe Tarifa não encontrada");

            return Ok(new ClasseTarifaResponse()
            {
                IdClasseTarifa = classeTarifa.IdClasseTarifa,
                Nome = classeTarifa.Nome,
                Situacao = classeTarifa.Situacao
            });
        }
    }

}
