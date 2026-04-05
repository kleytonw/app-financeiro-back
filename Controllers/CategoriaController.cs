using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CategoriaController(Context context) : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Categoria.Where(x => x.Situacao == "Ativo")
                   .AsNoTracking()
                   .Select(m => new
                   {
                       m.IdCategoria,
                       m.Nome,
                       m.Cor,
                       m.Situacao
                   }).Take(500).ToList();

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
                if (categoria == null)
                    return BadRequest("Categoria não encontrada");

                categoria.Alterar(
                    model.Nome,
                    model.Cor,
                    User.Identity.Name);

                context.Update(categoria);
            }
            else
            {
                categoria = new Categoria(
                    model.Nome,
                    model.Cor,
                    User.Identity.Name);

                context.Add(categoria);
            }

            context.SaveChanges();
            return Ok(new CategoriaResponse
            {
                IdCategoria = categoria.IdCategoria,
                Nome = categoria.Nome,
                Cor = categoria.Cor,
                Situacao = categoria.Situacao
            });

        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var categoria = context.Categoria.FirstOrDefault(x => x.IdCategoria == id);
            if (categoria == null)
                return BadRequest("Categoria não encontrada");

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
                return BadRequest("Categoria não encontrada");


            return Ok(new CategoriaResponse
            {
                IdCategoria = categoria.IdCategoria,
                Nome = categoria.Nome,
                Cor = categoria.Cor,
                Situacao = categoria.Situacao
            });
        }
    }

}
