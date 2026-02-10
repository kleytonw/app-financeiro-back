using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ERP_API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ServicoNfseController : ControllerBase
    {
        protected Context context;

        public ServicoNfseController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.ServicoNfse.Select(
                m => new
                {
                    m.IdServicoNfse,
                    m.Codigo,
                    m.CodigoNBS,
                    m.Nome,
                    m.AliquotaISS,
                    m.DescricaoServico,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ServicoNfseRequest model)
        {
            ServicoNfse servicoNfse;

            if (model.IdServicoNfse > 0)
            {
                servicoNfse = context.ServicoNfse.FirstOrDefault(x => x.IdServicoNfse == model.IdServicoNfse);
                servicoNfse.Alterar(
                    model.Codigo,
                    model.CodigoNBS,
                    model.Nome,
                    model.AliquotaISS,
                    model.DescricaoServico,
                    User.Identity.Name
                );
                context.Update(servicoNfse);
            }
            else
            {
                servicoNfse = new ServicoNfse(
                    model.Codigo,
                    model.CodigoNBS,
                    model.Nome,
                    model.AliquotaISS,
                    model.DescricaoServico,
                    User.Identity.Name
                );
                context.ServicoNfse.Add(servicoNfse);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var servicoNfse = context.ServicoNfse.FirstOrDefault(x => x.IdServicoNfse == id);
            if (servicoNfse == null)
                return BadRequest("O id do Serviço Nfse é obrigatótio");

            servicoNfse.Excluir(User.Identity.Name);
            context.Update(servicoNfse);

            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var servicoNfse = context.ServicoNfse.FirstOrDefault(x => x.IdServicoNfse == id);
            if (servicoNfse == null)
                return BadRequest("O id do Serviço Nfse é obrigatótio");

            return Ok(new ServicoNfseResponse()
            {
                IdServicoNfse = servicoNfse.IdServicoNfse,
                Codigo = servicoNfse.Codigo,
                CodigoNBS = servicoNfse.CodigoNBS,
                Nome = servicoNfse.Nome,
                AliquotaISS = servicoNfse.AliquotaISS,
                DescricaoServico = servicoNfse.DescricaoServico
            });
        }
    }
}
