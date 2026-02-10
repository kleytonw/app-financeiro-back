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
    public class ClasseRecebimentoItemController : ControllerBase
    {
        public Context context;

        public ClasseRecebimentoItemController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]

        public IActionResult Listar(int idClasseRecebimento)
        {
            var result = context.ClasseRecebimentoItem.Include(x => x.Bandeira).Where(x => x.IdClasseRecebimento == idClasseRecebimento)
               .Select(m => new
               {
                   m.IdClasseRecebimentoItem,
                   m.IdClasseRecebimento,
                   m.IdBandeira,
                   m.IdMeioPagamento,
                   m.MeioPagamento.NomeMeioPagamento,
                   m.Bandeira.NomeBandeira,
                   m.NumeroDias,
                   m.Situacao,
               }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ClasseRecebimentoItemRequest model)
        {
            var bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == model.IdBandeira);
            var classeRecebimento = context.ClasseRecebimento.FirstOrDefault(x => x.IdClasseRecebimento == model.IdClasseRecebimento);
            var meioPagamento = context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == model.IdMeioPagamento);

            if (model.IdClasseRecebimentoItem > 0)
            {
                var classeRecebimentoItem = context.ClasseRecebimentoItem.FirstOrDefault(x => x.IdClasseRecebimentoItem == model.IdClasseRecebimentoItem);
                if (classeRecebimentoItem == null)
                    return NotFound("Classe de Recebimento Item não encontrada");

                classeRecebimentoItem.Alterar(classeRecebimento, bandeira,meioPagamento, model.NumeroDias, User.Identity.Name);
                context.SaveChanges();
            }
            else
            {
                var classeRecebimentoItem = new ClasseRecebimentoItem(classeRecebimento, bandeira, meioPagamento, model.NumeroDias, User.Identity.Name);
                context.ClasseRecebimentoItem.Add(classeRecebimentoItem);
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        [Route("salvarClasseRecebimento")]
        [Authorize]
        public IActionResult SalvarClasseRecebimentoItem(int idClasseRecebimento)
        {
            var classeRecebimentoItem = context.ClasseRecebimentoItem.Include(x => x.MeioPagamento).FirstOrDefault(x => x.IdClasseRecebimento == idClasseRecebimento);
            var classeRecebimento = context.ClasseRecebimento.FirstOrDefault(x => x.IdClasseRecebimento == idClasseRecebimento);
            var bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == classeRecebimentoItem.IdBandeira);
            var meioPagamento = context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == classeRecebimentoItem.IdMeioPagamento);


            if (idClasseRecebimento > 0)
            {
                if (classeRecebimentoItem == null)
                    return NotFound("Classe de Recebimento Item não encontrada");

                classeRecebimentoItem.Alterar(classeRecebimento, bandeira, meioPagamento,  classeRecebimentoItem.NumeroDias, User.Identity.Name);
                context.SaveChanges();
            }
            else
            {
                classeRecebimentoItem = new ClasseRecebimentoItem(classeRecebimento, bandeira, meioPagamento, classeRecebimentoItem.NumeroDias, User.Identity.Name);
                context.ClasseRecebimentoItem.Add(classeRecebimentoItem);
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var classeRecebimentoItem = context.ClasseRecebimentoItem.FirstOrDefault(x => x.IdClasseRecebimentoItem == id);
            if (classeRecebimentoItem == null)
                return NotFound("Classe de Recebimento Item não encontrada");

            classeRecebimentoItem.Excluir(User.Identity.Name);
            context.Update(classeRecebimentoItem);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("deletar")]
        [Authorize]
        public IActionResult Deletar(int id)
        {
            var classeRecebimentoItem = context.ClasseRecebimentoItem.FirstOrDefault(x => x.IdClasseRecebimentoItem == id);
            if (classeRecebimentoItem == null)
                return NotFound("Classe de Recebimento Item não encontrada");

            context.Remove(classeRecebimentoItem);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var classeRecebimentoItem = context.ClasseRecebimentoItem.FirstOrDefault(x => x.IdClasseRecebimentoItem == id);
            if (classeRecebimentoItem == null)
                return BadRequest("A classe de recebimento item não foi encontrada ");

            return Ok(new ClasseRecebimentoItemResponse()
            {
                IdClasseRecebimentoItem = classeRecebimentoItem.IdClasseRecebimentoItem,
                IdClasseRecebimento = classeRecebimentoItem.IdClasseRecebimento,
                IdBandeira = classeRecebimentoItem.IdBandeira,
                NumeroDias = classeRecebimentoItem.NumeroDias,
                Situacao = classeRecebimentoItem.Situacao
            });
        }
    }
}
