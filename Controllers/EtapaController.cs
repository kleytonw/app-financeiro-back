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
    public class EtapaController: ControllerBase
    {
        private Context context;

        public EtapaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar()
        {
            var result = context.Etapa.Where(x => x.Situacao == "Ativo").Select(
                m => new
                {
                    m.IdEtapa,
                    m.Nome,
                    m.Descricao,
                    m.EtapaConcluida,
                    m.Situacao,
                });

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] EtapaRequest model)
        {
            Etapa etapa;

            if (model.IdEtapa > 0)
            {
                etapa = context.Etapa.FirstOrDefault(x => x.IdEtapa == model.IdEtapa);

                etapa.Alterar(model.Nome, model.Descricao, model.EtapaConcluida, User.Identity.Name);
                context.Update(etapa);
            }

            else
            {
                etapa = new Etapa(model.Nome, model.Descricao, model.EtapaConcluida, User.Identity.Name);
                context.Etapa.Add(etapa);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var etapa = context.Etapa.FirstOrDefault(x =>x.IdEtapa == id);
            if (etapa == null)
                return BadRequest("Etapa não encontrada");
            etapa.Excluir(User.Identity.Name);

            context.Update(etapa);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var etapa = context.Etapa.FirstOrDefault(x => x.IdEtapa == id);
            if (etapa == null)
                return BadRequest("Etapa não encontrada");

            return Ok(new EtapaResponse()
            {
                IdEtapa = etapa.IdEtapa,
                Nome = etapa.Nome,
                Descricao = etapa.Descricao,
                EtapaConcluida = etapa.EtapaConcluida,
                Situacao = etapa.Situacao,
            });
        }
    }
}
