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
    public class ConfiguracaoConciliacaoController : ControllerBase
    {
        protected Context context;

        public ConfiguracaoConciliacaoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar()
        {
            var result = context.ConfiguracaoConciliacao
                .Select(m => new
                {
                    m.IdConfiguracaoConciliacao,
                    m.Adquirente,
                    m.TipoTransacao,
                    m.Descricao,
                    m.Situacao
                }).Where(x => x.Situacao == "Ativo").ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisar")]
        [Authorize]
        public IActionResult Pesquisar([FromBody] PesquisarConfiguracaoConcilicaoRequest model)
        {
            var query = context.ConfiguracaoConciliacao.AsQueryable();

            if (model.Chave == "Adquirente")
            {
                query = query.Where(x => x.Adquirente.Contains(model.Valor));
            }
            else if (model.Chave == "TipoTransacao")
            {
                query = query.Where(x => x.TipoTransacao.Contains(model.Valor));
            }
            else if (model.Chave == "Descricao")
            {
                query = query.Where(x => x.Descricao.Contains(model.Valor));
            }

            var result = query.Select(m => new
            {
                m.IdConfiguracaoConciliacao,
                m.Adquirente,
                m.TipoTransacao,
                m.Descricao,
                m.Situacao

            }).Where(x => x.Situacao == "Ativo").ToList();

            return Ok(result);
        }

                [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ConfiguracaoConciliacaoRequest model)
        {
            ConfiguracaoConciliacao configuracao;

            if (model.IdConfiguracaoConciliacao > 0)
            {
                configuracao = context.ConfiguracaoConciliacao.FirstOrDefault(x => x.IdConfiguracaoConciliacao == model.IdConfiguracaoConciliacao);

                configuracao.Alterar(model.Adquirente, model.TipoTransacao, model.Descricao, User.Identity.Name);
                context.Update(configuracao);
            }
            else
            {
                configuracao = new ConfiguracaoConciliacao(model.Adquirente, model.TipoTransacao, model.Descricao, User.Identity.Name);
                context.Add(configuracao);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int idConfiguracaoConciliacao)
        {
            var configuracao = context.ConfiguracaoConciliacao.FirstOrDefault(x => x.IdConfiguracaoConciliacao == idConfiguracaoConciliacao);
            if (configuracao == null) 
                return BadRequest("Configuração de Conciliação não encontrada");

            configuracao.Excluir(User.Identity.Name);
            context.Update(configuracao);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int idConfiguracaoConciliacao)
        {
            var configuracao = context.ConfiguracaoConciliacao.FirstOrDefault(x => x.IdConfiguracaoConciliacao == idConfiguracaoConciliacao);
            if (configuracao == null)
                return BadRequest("Configuração de Conciliação não encontrada");
            return Ok(new ConfiguracaoConciliacaoResponse()
            {
                IdConfiguracaoConciliacao = configuracao.IdConfiguracaoConciliacao,
                Adquirente = configuracao.Adquirente,
                TipoTransacao = configuracao.TipoTransacao,
                Descricao = configuracao.Descricao,
                Situacao = configuracao.Situacao
            });
        }
    }
}
