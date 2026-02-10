using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ContratoOperadoraRecebimentoController : ControllerBase
    {
        public Context context;

        public ContratoOperadoraRecebimentoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar(int idContratoOperadora)
         {
            var result = context.ContratoOperadoraRecebimento.Where(x => x.IdContratoOperadora == idContratoOperadora)
                .Select(m => new
                {
                    m.IdContratoOperadoraRecebimento,
                    m.IdContratoOperadora,
                    m.IdBandeira,
                    m.Bandeira.NomeBandeira,
                    m.MeioPagamento.NomeMeioPagamento,
                    m.NumeroDias,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ContratoOperadoraRecebimentoRequest model)
        {
            var bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == model.IdBandeira);
            var meioPagamento = context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == model.IdMeioPagamento);
            var contratoOperadora = context.ContratoOperadora.FirstOrDefault(x => x.IdContratoOperadora == model.IdContratoOperadora);
            if (model.IdContratoOperadoraRecebimento > 0)
            {
                var contratoOperadoraRecebimento = context.ContratoOperadoraRecebimento.FirstOrDefault(x => x.IdContratoOperadoraRecebimento == model.IdContratoOperadoraRecebimento);
                if (contratoOperadoraRecebimento == null)
                    return NotFound("Contrato Operadora Recebimento não encontrado");
                contratoOperadoraRecebimento.Alterar(contratoOperadora, bandeira, meioPagamento, model.NumeroDias, User.Identity.Name);
                context.SaveChanges();
            }
            else
            {
                var novoContratoOperadoraRecebimento = new ContratoOperadoraRecebimento(contratoOperadora, bandeira, meioPagamento, model.NumeroDias, User.Identity.Name);
                context.ContratoOperadoraRecebimento.Add(novoContratoOperadoraRecebimento);
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var contratoOperadoraRecebimento = context.ContratoOperadoraRecebimento.FirstOrDefault(x => x.IdContratoOperadoraRecebimento == id);
            if (contratoOperadoraRecebimento == null)
                return NotFound("Contrato Operadora Recebimento não encontrado");
            contratoOperadoraRecebimento.Excluir(User.Identity.Name);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("deletar")]
        [Authorize]
        public IActionResult Deletar(int id)
        {
            var contratoOperadoraRecebimento = context.ContratoOperadoraRecebimento.FirstOrDefault(x => x.IdContratoOperadoraRecebimento == id);
            if (contratoOperadoraRecebimento == null)
                return NotFound("Contrato Operadora Recebimento não encontrado");
            context.ContratoOperadoraRecebimento.Remove(contratoOperadoraRecebimento);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("importar")]
        public IActionResult Importar(int idClasseRecebimento, int idContratoOperadora)
        {
            var classeRecebimentoItem = context.ClasseRecebimentoItem
                .Include(x => x.Bandeira)
                .Include(x => x.MeioPagamento)
                .Where(x => x.IdClasseRecebimento == idClasseRecebimento).ToList();

            foreach (var item in classeRecebimentoItem)
            {
                context.Entry(item).Reference(x => x.Bandeira).Load();
                context.Entry(item).Reference(x => x.MeioPagamento).Load();
            }
            var contratoOperadora = context.ContratoOperadora.Include(x => x.Empresa)
                .Include(x => x.Unidade)
                .FirstOrDefault(x => x.IdContratoOperadora == idContratoOperadora);

            if (contratoOperadora == null)
                return BadRequest("Contrato da operadora não encontrado ");

            var contratoOperadoraRecebimentoLista = context.ContratoOperadoraRecebimento
                .Include(x => x.Bandeira)
                .Where(x => x.IdContratoOperadora == idContratoOperadora).ToList();

            if (contratoOperadoraRecebimentoLista.Count > 0)
            {
                context.ContratoOperadoraRecebimento.RemoveRange(contratoOperadoraRecebimentoLista);
                context.SaveChanges();
            }

            var contratoOperadoraRecebimento = new List<ContratoOperadoraRecebimento>();

            foreach (var item in classeRecebimentoItem)
            {
                contratoOperadoraRecebimento.Add(new ContratoOperadoraRecebimento(contratoOperadora,
                        item.Bandeira,
                        item.MeioPagamento,
                        item.NumeroDias,
                        User.Identity.Name));


            }

            context.ContratoOperadoraRecebimento.AddRange(contratoOperadoraRecebimento);
            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var contratoOperadoraRecebimento = context.ContratoOperadoraRecebimento.FirstOrDefault(x => x.IdContratoOperadoraRecebimento == id);
            if (contratoOperadoraRecebimento == null)
                return BadRequest("Contrato Operadora Recebimento não encontrado ");


            return Ok(new ContratoOperadoraRecebimentoResponse()
            {
                IdContratoOperadoraRecebimento = contratoOperadoraRecebimento.IdContratoOperadoraRecebimento,
                IdContratoOperadora = contratoOperadoraRecebimento.IdContratoOperadora,
                IdBandeira = contratoOperadoraRecebimento.IdBandeira,
                IdMeioPagamento = contratoOperadoraRecebimento.IdMeioPagamento,
                NumeroDias = contratoOperadoraRecebimento.NumeroDias,
                Situacao = contratoOperadoraRecebimento.Situacao
            });
        }
    }

}
