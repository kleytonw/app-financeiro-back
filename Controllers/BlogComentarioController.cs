using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ERP.Models;
using ERP.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using ERP_API.Models;
using ERP_API.Domain.Entidades; 

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class BlogComentarioController : ControllerBase
    {
        protected Context context;

        public BlogComentarioController(Context context)
        {
            this.context = context;
        }


        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.BlogComentario
                .Select(m => new {
                    m.IdBlogComentario,
                    m.Blog.Titulo,
                    m.Nome,
                    m.Email,
                    m.Comentario,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] BlogComentarioRequest model)
        {
            BlogComentario blogComentario;
            Blog blog;

            if (model.IdBlogComentario > 0)
            {
                blogComentario = context.BlogComentario.FirstOrDefault(x => x.IdBlogComentario == model.IdBlogComentario);
                blog = context.Blog.FirstOrDefault( x => x.IdBlog == model.IdBlog);
                if (blogComentario == null)
                    return NotFound("Comentário não encontrado.");

                blogComentario.Alterar(blog, model.Nome, model.Email, model.Comentario, User.Identity.Name);
            }
            else
            {
                blog = context.Blog.FirstOrDefault(x => x.IdBlog == model.IdBlog);
                blogComentario = new BlogComentario(
                    blog,
                    model.Nome,
                    model.Email,
                    model.Comentario,
                    User.Identity.Name
                );

                context.BlogComentario.Add(blogComentario);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var blogComentario = context.BlogComentario.FirstOrDefault(x => x.IdBlogComentario == id);
            blogComentario.Excluir(User.Identity.Name);

            context.Update(blogComentario);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("excluirComentario")]
        public IActionResult ExcluirSetorProduto(int idBlogComentario)
        {
            var blogComentario = context.BlogComentario.FirstOrDefault(x => x.IdBlogComentario == idBlogComentario);
            blogComentario.Excluir(User.Identity.Name);

            context.Remove(blogComentario);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var blog = context.Blog.FirstOrDefault(x => x.IdBlog == id);
            //if (blogComentario == null)
                //return BadRequest("BlogComentário não encontrado.");

            return Ok(new BlogComentarioResponse()
            {
                IdBlog = blog.IdBlog,
                Titulo = blog.Titulo,
            });
        }
    }
}
