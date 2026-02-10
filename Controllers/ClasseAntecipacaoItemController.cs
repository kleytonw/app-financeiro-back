using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
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
    public class ClasseAntecipacaoItemController : ControllerBase
    {
        public Context context;

        public ClasseAntecipacaoItemController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]

        public IActionResult Listar(int idClasseAntecipacao)
        {
            var result = context.ClasseAntecipacaoItem.Include(x => x.Bandeira).Where(x => x.IdClasseAntecipacao == idClasseAntecipacao)
               .Select(m => new
               {
                   m.IdClasseAntecipacaoItem,
                   m.IdClasseAntecipacao,
                   m.IdBandeira,
                   m.IdMeioPagamento,
                   m.MeioPagamento.NomeMeioPagamento,
                   m.Bandeira.NomeBandeira,
                   m.NumeroDias,
                   m.Valor,
                   m.Percentual,
                   m.Situacao,
               }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ClasseAntecipacaoItemRequest model)
        {
            var bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == model.IdBandeira);
            var classeAntecipacao = context.ClasseAntecipacao.FirstOrDefault(x => x.IdClasseAntecipacao == model.IdClasseAntecipacao);
            var meioPagamento = context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == model.IdMeioPagamento);

            if (model.IdClasseAntecipacaoItem > 0)
            {
                var classeAntecipacaoItem = context.ClasseAntecipacaoItem.FirstOrDefault(x => x.IdClasseAntecipacaoItem == model.IdClasseAntecipacaoItem);
                if (classeAntecipacaoItem == null)
                    return NotFound("Classe de Antecipacao Item não encontrada");

                classeAntecipacaoItem.Alterar(classeAntecipacao, bandeira, meioPagamento, model.NumeroDias, model.Valor, model.Percentual, User.Identity.Name);
                context.SaveChanges();
            }
            else
            {
                var classeAntecipacaoItem = new ClasseAntecipacaoItem(classeAntecipacao, bandeira, meioPagamento, model.NumeroDias, model.Valor, model.Percentual, User.Identity.Name);
                context.ClasseAntecipacaoItem.Add(classeAntecipacaoItem);
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        [Route("salvarClasseAntecipacao")]
        [Authorize]
        public IActionResult SalvarClasseAntecipacaoItem(int idClasseAntecipacao)
        {
            var classeAntecipacaoItem = context.ClasseAntecipacaoItem.Include(x => x.MeioPagamento).FirstOrDefault(x => x.IdClasseAntecipacao == idClasseAntecipacao);
            var classeAntecipacao = context.ClasseAntecipacao.FirstOrDefault(x => x.IdClasseAntecipacao == idClasseAntecipacao);
            var bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == classeAntecipacaoItem.IdBandeira);
            var meioPagamento = context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == classeAntecipacaoItem.IdMeioPagamento);


            if (idClasseAntecipacao > 0)
            {
                if (classeAntecipacaoItem == null)
                    return NotFound("Classe de Antecipacao Item não encontrada");

                classeAntecipacaoItem.Alterar(classeAntecipacao, bandeira, meioPagamento, classeAntecipacaoItem.NumeroDias, classeAntecipacaoItem.Valor, classeAntecipacaoItem.Percentual, User.Identity.Name);
                context.SaveChanges();
            }
            else
            {
                classeAntecipacaoItem = new ClasseAntecipacaoItem(classeAntecipacao, bandeira, meioPagamento, classeAntecipacaoItem.NumeroDias, classeAntecipacaoItem.Valor, classeAntecipacaoItem.Percentual, User.Identity.Name);
                context.ClasseAntecipacaoItem.Add(classeAntecipacaoItem);
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var classeAntecipacaoItem = context.ClasseAntecipacaoItem.FirstOrDefault(x => x.IdClasseAntecipacaoItem == id);
            if (classeAntecipacaoItem == null)
                return NotFound("Classe de Antecipacao Item não encontrada");

            classeAntecipacaoItem.Excluir(User.Identity.Name);
            context.Update(classeAntecipacaoItem);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("deletar")]
        [Authorize]
        public IActionResult Deletar(int id)
        {
            var classeAntecipacaoItem = context.ClasseAntecipacaoItem.FirstOrDefault(x => x.IdClasseAntecipacaoItem == id);
            if (classeAntecipacaoItem == null)
                return BadRequest("Classe de Antecipacao Item não encontrada");

            context.Remove(classeAntecipacaoItem);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var classeAntecipacaoItem = context.ClasseAntecipacaoItem.FirstOrDefault(x => x.IdClasseAntecipacaoItem == id);
            if (classeAntecipacaoItem == null)
                return BadRequest("A classe de antecipacao item não foi encontrada ");

            return Ok(new ClasseAntecipacaoItemResponse()
            {
                IdClasseAntecipacaoItem = classeAntecipacaoItem.IdClasseAntecipacaoItem,
                IdClasseAntecipacao = classeAntecipacaoItem.IdClasseAntecipacao,
                IdBandeira = classeAntecipacaoItem.IdBandeira,
                NumeroDias = classeAntecipacaoItem.NumeroDias,
                Valor = classeAntecipacaoItem.Valor,
                Percentual = classeAntecipacaoItem.Percentual,
                Situacao = classeAntecipacaoItem.Situacao
            });
        }
    }
}