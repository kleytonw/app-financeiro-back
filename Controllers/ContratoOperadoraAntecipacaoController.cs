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
    public class ContratoOperadoraAntecipacaoController : ControllerBase
    {
        public Context context;

        public ContratoOperadoraAntecipacaoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar(int idContratoOperadora)
        {
            var result = context.ContratoOperadoraAntecipacao.Where(x => x.IdContratoOperadora == idContratoOperadora)
                .Select(m => new
                {
                    m.IdContratoOperadoraAntecipacao,
                    m.IdContratoOperadora,
                    m.IdBandeira,
                    m.Bandeira.NomeBandeira,
                    m.MeioPagamento.NomeMeioPagamento,
                    m.NumeroDias,
                    m.Valor,
                    m.Percentual,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ContratoOperadoraAntecipacaoRequest model)
        {
            var bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == model.IdBandeira);
            var meioPagamento = context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == model.IdMeioPagamento);
            var contratoOperadora = context.ContratoOperadora.FirstOrDefault(x => x.IdContratoOperadora == model.IdContratoOperadora);
            if (model.IdContratoOperadoraAntecipacao > 0)
            {
                var contratoOperadoraAntecipacao = context.ContratoOperadoraAntecipacao.FirstOrDefault(x => x.IdContratoOperadoraAntecipacao == model.IdContratoOperadoraAntecipacao);
                if (contratoOperadoraAntecipacao == null)
                    return NotFound("Contrato Operadora Antecipacao não encontrado");
                contratoOperadoraAntecipacao.Alterar(contratoOperadora, bandeira, meioPagamento, model.NumeroDias, model.Valor, model.Percentual, User.Identity.Name);
                context.SaveChanges();
            }
            else
            {
                var novoContratoOperadoraAntecipacao = new ContratoOperadoraAntecipacao(contratoOperadora, bandeira, meioPagamento, model.NumeroDias, model.Valor, model.Percentual, User.Identity.Name);
                context.ContratoOperadoraAntecipacao.Add(novoContratoOperadoraAntecipacao);
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var contratoOperadoraAntecipacao = context.ContratoOperadoraAntecipacao.FirstOrDefault(x => x.IdContratoOperadoraAntecipacao == id);
            if (contratoOperadoraAntecipacao == null)
                return NotFound("Contrato Operadora Antecipacao não encontrado");
            contratoOperadoraAntecipacao.Excluir(User.Identity.Name);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("deletar")]
        [Authorize]
        public IActionResult Deletar(int id)
        {
            var contratoOperadoraAntecipacao = context.ContratoOperadoraAntecipacao.FirstOrDefault(x => x.IdContratoOperadoraAntecipacao == id);
            if (contratoOperadoraAntecipacao == null)
                return NotFound("Contrato Operadora Antecipacao não encontrado");
            context.ContratoOperadoraAntecipacao.Remove(contratoOperadoraAntecipacao);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("importar")]
        public IActionResult Importar(int idClasseAntecipacao, int idContratoOperadora)
        {
            var classeAntecipacaoItem = context.ClasseAntecipacaoItem
                .Include(x => x.Bandeira)
                .Include(x => x.MeioPagamento)
                .Where(x => x.IdClasseAntecipacao == idClasseAntecipacao).ToList();

            foreach (var item in classeAntecipacaoItem)
            {
                context.Entry(item).Reference(x => x.Bandeira).Load();
                context.Entry(item).Reference(x => x.MeioPagamento).Load();
            }
            var contratoOperadora = context.ContratoOperadora.Include(x => x.Empresa)
                .Include(x => x.Unidade)
                .FirstOrDefault(x => x.IdContratoOperadora == idContratoOperadora);

            if (contratoOperadora == null)
                return BadRequest("Contrato da operadora não encontrado ");

            var contratoOperadoraAntecipacaoLista = context.ContratoOperadoraAntecipacao
                .Include(x => x.Bandeira)
                .Where(x => x.IdContratoOperadora == idContratoOperadora).ToList();

            if (contratoOperadoraAntecipacaoLista.Count > 0)
            {
                context.ContratoOperadoraAntecipacao.RemoveRange(contratoOperadoraAntecipacaoLista);
                context.SaveChanges();
            }

            var contratoOperadoraAntecipacao = new List<ContratoOperadoraAntecipacao>();

            foreach (var item in classeAntecipacaoItem)
            {
                contratoOperadoraAntecipacao.Add(new ContratoOperadoraAntecipacao(contratoOperadora,
                        item.Bandeira,
                        item.MeioPagamento,
                        item.NumeroDias,
                        item.Valor,
                        item.Percentual,
                        User.Identity.Name));


            }

            context.ContratoOperadoraAntecipacao.AddRange(contratoOperadoraAntecipacao);
            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var contratoOperadoraAntecipacao = context.ContratoOperadoraAntecipacao.FirstOrDefault(x => x.IdContratoOperadoraAntecipacao == id);
            if (contratoOperadoraAntecipacao == null)
                return BadRequest("Contrato Operadora Antecipacao não encontrado ");


            return Ok(new ContratoOperadoraAntecipacaoResponse()
            {
                IdContratoOperadoraAntecipacao = contratoOperadoraAntecipacao.IdContratoOperadoraAntecipacao,
                IdContratoOperadora = contratoOperadoraAntecipacao.IdContratoOperadora,
                IdBandeira = contratoOperadoraAntecipacao.IdBandeira,
                IdMeioPagamento = contratoOperadoraAntecipacao.IdMeioPagamento,
                NumeroDias = contratoOperadoraAntecipacao.NumeroDias,
                Valor = contratoOperadoraAntecipacao.Valor,
                Percentual = contratoOperadoraAntecipacao.Percentual,
                Situacao = contratoOperadoraAntecipacao.Situacao
            });
        }
    }

}
