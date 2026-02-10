using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Hangfire.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Linq;
namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]

    public class ClasseTarifaItemController : ControllerBase
    {
        protected Context context;
        public ClasseTarifaItemController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar(int idClasseTarifa)
        {
            var result = context.ClasseTarifaItem.Where(x => x.IdClasseTarifa == idClasseTarifa)
                .Include(x => x.Bandeira)
                .Include(x => x.MeioPagamento)
                .ToList()
                .Select(m => new
                {
                    m.IdClasseTarifaItem,
                    m.IdClasseTarifa,
                    m.IdMeioPagamento,
                    NomeMeioPagamento = context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == m.IdMeioPagamento)?.NomeMeioPagamento,
                    m.IdBandeira,
                    NomeBandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == m.IdBandeira)?.NomeBandeira,
                    m.Taxa,
                    m.Valor,
                    m.Tipo,
                    m.ParcelaInicio,
                    m.ParcelaFim,
                    m.Situacao,
                });
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ClasseTarifaItemRequest model)
        {
            var classeTarifa = context.ClasseTarifa.FirstOrDefault(x => x.IdClasseTarifa == model.IdClasseTarifa);
            var meioPagamento = context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == model.IdMeioPagamento);
            var bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == model.IdBandeira);

            if (meioPagamento.NomeMeioPagamento != "Cartão de crédito")
            {
                bool pixExiste = context.ClasseTarifaItem.Any(x =>
                    x.IdBandeira == model.IdBandeira &&
                    x.IdMeioPagamento == model.IdMeioPagamento &&
                    x.IdClasseTarifaItem == model.IdClasseTarifaItem
                );

                if (pixExiste)
                {
                    return BadRequest("Já existe uma taxa cadastrada com essa para  esse Contrato.");
                }
            }
            else
            {
                bool taxaExiste = context.ClasseTarifaItem.Any(x =>
                    x.IdClasseTarifaItem == model.IdClasseTarifaItem &&
                    x.IdMeioPagamento == model.IdMeioPagamento &&
                    x.IdBandeira == model.IdBandeira &&
                    x.ParcelaInicio <= model.ParcelaFim &&
                    x.ParcelaFim >= model.ParcelaInicio
                );
            }

                if (model.IdClasseTarifaItem > 0)
            {
                var classeTarifaItem = context.ClasseTarifaItem.FirstOrDefault(x => x.IdClasseTarifa == model.IdClasseTarifa);
                if (classeTarifa == null)
                    return NotFound("Classe Tarifa não encontrada");

                 classeTarifaItem.Alterar(classeTarifa,
                                         meioPagamento,
                                         bandeira,
                                         model.Taxa,
                                         model.Valor,
                                         model.Tipo,
                                         model.ParcelaInicio,
                                         model.ParcelaFim,
                                         User.Identity.Name);
                context.SaveChanges();
            }
            else
            {
                var classeTarifaItem = new ClasseTarifaItem(classeTarifa,
                                                    meioPagamento,
                                                    bandeira,
                                                    model.Taxa,
                                                    model.Valor,
                                                    model.Tipo,
                                                    model.ParcelaInicio,
                                                    model.ParcelaFim,
                                                    User.Identity.Name);

                context.ClasseTarifaItem.Add(classeTarifaItem);
                context.SaveChanges();
            }

            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var classeTarifaItem = context.ClasseTarifaItem.FirstOrDefault(x => x.IdClasseTarifaItem == id);
            if (classeTarifaItem == null)
                return NotFound("Classe Tarifa não encontrada");

            classeTarifaItem.Excluir(User.Identity.Name);
            context.Update(classeTarifaItem);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("deletar")]
        public IActionResult Deletar(int id)
        {
            var classeTarifaItem = context.ClasseTarifaItem.FirstOrDefault(x => x.IdClasseTarifaItem == id);
            classeTarifaItem.Excluir(User.Identity.Name);

            context.Remove(classeTarifaItem);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var classeTarifaItem = context.ClasseTarifaItem.FirstOrDefault(x => x.IdClasseTarifaItem == id);
            if (classeTarifaItem == null)
                return NotFound("Classe Tarifa não encontrada");

            return Ok(new ClasseTarifaItemResponse()
            {
                IdClasseTarifaItem = classeTarifaItem.IdClasseTarifaItem,
                IdClasseTarifa = classeTarifaItem.IdClasseTarifa,
                IdMeioPagamento = classeTarifaItem.IdMeioPagamento,
                IdBandeira = classeTarifaItem.IdBandeira,
                Taxa = classeTarifaItem.Taxa,
                Valor = classeTarifaItem.Valor,
                Tipo = classeTarifaItem.Tipo,
                ParcelaInicio = classeTarifaItem.ParcelaInicio,
                ParcelaFim = classeTarifaItem.ParcelaFim,
                Situacao = classeTarifaItem.Situacao
            });
        }

    }
}
