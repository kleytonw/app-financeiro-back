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
    public class ConfiguracaoNfseController(Context context) : ControllerBase
    {

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.ConfiguracaoNfse.
                Select(m => new
                {
                    m.IdConfiguracaoNfse,
                    m.NumeroRPS,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ConfiguracaoNfseRequest model)
        {
            if (model.IdConfiguracaoNfse > 0)
            {
                var configuracao = context.ConfiguracaoNfse.FirstOrDefault(x => x.IdConfiguracaoNfse == model.IdConfiguracaoNfse);
                if (configuracao == null)
                    return BadRequest("Configuração Nfse não encontrada");
                configuracao.Alterar(model.NumeroRPS, User.Identity.Name);
                context.ConfiguracaoNfse.Update(configuracao);
            }
            else
            {
                var configuracao = new ConfiguracaoNfse(model.NumeroRPS, User.Identity.Name);
                context.ConfiguracaoNfse.Add(configuracao);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var configuracao = context.ConfiguracaoNfse.FirstOrDefault(x => x.IdConfiguracaoNfse == id);
            if (configuracao == null)
                return BadRequest("Configuração Nfse não encontrada");

            configuracao.Excluir(User.Identity.Name);
            context.ConfiguracaoNfse.Update(configuracao);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var configuracao = context.ConfiguracaoNfse.FirstOrDefault(x => x.IdConfiguracaoNfse == id);
            if (configuracao == null)
                return BadRequest("Configuração Nfse não encontrada");

            return Ok(new ConfiguracaoNfseResponse()
            {
                IdConfiguracaoNfse = configuracao.IdConfiguracaoNfse,
                NumeroRPS = configuracao.NumeroRPS,
                Situacao = configuracao.Situacao
            });
        }
    }
}