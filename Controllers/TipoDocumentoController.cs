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
    public class TipoDocumentoController : ControllerBase
    {
        protected Context context;
        public TipoDocumentoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.TipoDocumento
                .Select(c => new
                {
                    c.IdTipoDocumento,
                    c.Nome,
                    c.Obrigatorio,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarAtivas")]
        public IActionResult ListarAtivas()
        {
            var result = context.TipoDocumento.Where(x => x.Situacao == "Ativo")
                .Select(c => new
                {
                    c.IdTipoDocumento,
                    c.Nome,
                    c.Obrigatorio,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] TipoDocumentoRequest model)
        {
            TipoDocumento tipoDocumento;
            if (model.IdTipoDocumento > 0)
            {
                tipoDocumento = context.TipoDocumento.FirstOrDefault(x => x.IdTipoDocumento == model.IdTipoDocumento);
                tipoDocumento.Alterar(model.Nome, model.Obrigatorio, User.Identity.Name);
            }
            else
            {
                tipoDocumento = new TipoDocumento(
                    model.Nome,
                    model.Obrigatorio,
                    User.Identity.Name
                );

                context.TipoDocumento.Add(tipoDocumento);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var tipoDocumento = context.TipoDocumento.FirstOrDefault(x => x.IdTipoDocumento == id);
            tipoDocumento.Excluir(User.Identity.Name);

            context.Update(tipoDocumento);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var tipoDocumento = context.TipoDocumento.FirstOrDefault(x => x.IdTipoDocumento == id);
            if (tipoDocumento == null)
                return BadRequest("Tipo de documento não encontrado ");

            return Ok(new TipoDocumentoResponse()
            {
                IdTipoDocumento = tipoDocumento.IdTipoDocumento,
                Nome = tipoDocumento.Nome,
                Obrigatorio = tipoDocumento.Obrigatorio,
                Situacao = tipoDocumento.Situacao
            });
        }
    }
}
