using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NfseController(Context context) : ControllerBase
    {

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Nfse
                  .Select(m => new
                  {
                      m.IdNfse,
                      m.IdCliente,
                      m.NomeCliente,
                      m.ChaveAcesso,
                      m.NumeroRPS,
                      m.Serie,
                      m.DataHoraInclusao,
                      m.DataHoraCancelamento,
                      m.StatusNotaFiscal,
                      m.IdServicoNfse,
                      m.Valor,
                      m.CodigoServico,
                      m.CodigoNBS,
                      m.Situacao
                  }).Take(500).OrderByDescending(x => x.DataHoraInclusao).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] NfsePesquisarRequest model)
        {
            var result = context.Nfse
                  .Where(m => m.DataHoraInclusao >= model.DataInicio && m.DataHoraInclusao <= model.DataFim && m.IdCliente == model.IdCliente
                  )
                  .Select(m => new
                  {
                      m.IdNfse,
                      m.IdCliente,
                      m.NomeCliente,
                      m.ChaveAcesso,
                      m.NumeroRPS,
                      m.Serie,
                      m.DataHoraInclusao,
                      m.DataHoraCancelamento,
                      m.StatusNotaFiscal,
                      m.IdServicoNfse,
                      m.Valor,
                      m.CodigoServico,
                      m.CodigoNBS,
                      m.Situacao
                  }).Take(500).OrderByDescending(x => x.DataHoraInclusao).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] NfseRequest model)
        {
            Cliente cliente = context.Cliente.FirstOrDefault(c => c.IdPessoa == model.IdCliente);
            if (cliente == null)
                return BadRequest("Cliente não encontrado.");
            ServicoNfse servicoNfse = context.ServicoNfse.FirstOrDefault(s => s.IdServicoNfse == model.IdServicoNfse);
            if (servicoNfse == null)
                return BadRequest("Serviço Nfse não encontrado.");

            Nfse nfse = new Nfse(
                 cliente,
                 model.NomeCliente,
                 model.Serie,
                 model.DataHoraInclusao,
                 "Pendente",
                 servicoNfse,
                 model.Valor,
                 model.CodigoServico,
                 model.CodigoNBS,
                 model.ObservacoesNotaFiscal,
                 User.Identity.Name
             );
            context.Nfse.Add(nfse);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var nfse = context.Nfse.FirstOrDefault(n => n.IdNfse == id);
            if (nfse == null)
                return BadRequest("Nfse não encontrada.");

            nfse.Excluir(User.Identity.Name);
            context.Nfse.Update(nfse);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var nfse = context.Nfse.FirstOrDefault(n => n.IdNfse == id);
            if (nfse == null)
                return BadRequest("Nfse não encontrada.");

            return Ok(new NfseResponse
            {
                IdNfse = nfse.IdNfse,
                IdCliente = nfse.IdCliente,
                NomeCliente = nfse.NomeCliente,
                ChaveAcesso = nfse.ChaveAcesso,
                NumeroRPS = nfse.NumeroRPS,
                Serie = nfse.Serie,
                DataHoraInclusao = nfse.DataHoraInclusao,
                DataHoraCancelamento = nfse.DataHoraCancelamento,
                StatusNotaFiscal = nfse.StatusNotaFiscal,
                IdServicoNfse = nfse.IdServicoNfse,
                Valor = nfse.Valor,
                CodigoServico = nfse.CodigoServico,
                CodigoNBS = nfse.CodigoNBS,
                ObservacoesNotaFiscal = nfse.ObservacoesNotaFiscal,
                Situacao = nfse.Situacao,
            });
        }
    }
}
