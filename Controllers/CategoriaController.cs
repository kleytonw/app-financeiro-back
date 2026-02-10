using ERP.Infra;
using ERP.Models;
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
    public class CategoriaController : ControllerBase
    {
        protected Context context;
        public CategoriaController(Context context)
        {
            this.context = context;
        }


        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Categoria
                .Select(c => new
                {
                    c.IdCategoria,
                    c.Nome,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }


        [HttpGet]
        [Route("listarAtivas")]
        public IActionResult ListarAtivas()
        {
            var result = context.Categoria.Where(x => x.Situacao == "Ativo")
                .Select(c => new
                {
                    c.IdCategoria,
                    c.Nome,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] CategoriaRequest model)
        {
            Categoria categoria;
            if (model.IdCategoria > 0)
            {
                categoria = context.Categoria.FirstOrDefault(x => x.IdCategoria == model.IdCategoria);
                categoria.Alterar(model.Nome, User.Identity.Name);
            }
            else
            {
                categoria = new Categoria(
                    model.Nome,
                    User.Identity.Name
                );

                context.Categoria.Add(categoria);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var categoria = context.Categoria.FirstOrDefault(x => x.IdCategoria == id);
            categoria.Excluir(User.Identity.Name);

            context.Update(categoria);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var categoria = context.Categoria.FirstOrDefault(x => x.IdCategoria == id);
            if (categoria == null)
                return BadRequest("Categoria não encontrado ");

            return Ok(new CategoriaResponse()
            {
                IdCategoria = categoria.IdCategoria,
                Nome = categoria.Nome,
                Situacao = categoria.Situacao
            });
        }
    }
}
