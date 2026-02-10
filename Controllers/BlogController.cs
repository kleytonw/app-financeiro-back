using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;
using ERP.Models;
using ERP.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using ERP_API.Models;
using ERP_API.Domain.Entidades;
using System.Reflection.PortableExecutable;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class BlogController : ControllerBase
    {
        protected Context context;

        public BlogController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Blog
                .Select(m => new {
                    m.IdBlog,
                    m.Empresa.Nome,
                    m.Autor,
                    m.Titulo,
                    m.Subtitulo,
                    m.Descricao,
                    m.Data,
                    m.LinkFoto,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("listarEmpresa")]
        public IActionResult ListarEmpresa()
        {
            var result = context.Empresa
                .Select(m => new {
                    m.IdEmpresa,
                    m.Nome
                }).ToList();

            return Ok(result);
        }


        [HttpPost]
        [Route("listarBlogPorEmpresa")]
        public IActionResult ListarBlogPorEmpresa(BlogRequest model)
        {
            var result = context.Blog.Where(x => x.IdEmpresa == model.IdEmpresa)
                .Select(m => new
                {
                    m.IdBlog,
                    m.Empresa.Nome,
                    m.Autor,
                    m.Titulo,
                    m.Subtitulo,
                    m.Descricao,
                    m.Data,
                    m.LinkFoto,
                    m.Situacao

                }).ToList();

            return Ok(result);
             
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] BlogRequest model)
        {
            Blog blog;
            Empresa empresa;
            if (model.IdBlog > 0)
            {
                blog = context.Blog.FirstOrDefault(x => x.IdBlog == model.IdBlog);
                empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
                blog.Alterar(empresa, model.Autor, model.Titulo, model.Subtitulo, model.Descricao, model.Data, model.LinkFoto, User.Identity.Name);
            }
            else
            {
                empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
                blog = new Blog(
                    empresa,
                    model.Autor,
                    model.Titulo,
                    model.Subtitulo,
                    model.Descricao,
                    model.Data,
                    model.LinkFoto,
                    User.Identity.Name
                );

                context.Blog.Add(blog);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var blog = context.Blog.FirstOrDefault(x => x.IdBlog == id);
            blog.Excluir(User.Identity.Name);

            context.Update(blog);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var blog = context.Blog.FirstOrDefault(x => x.IdBlog == id);
            if (blog == null)
                return BadRequest("Blog não encontrado ");

            return Ok(new BlogResponse()
            {
                IdBlog = blog.IdBlog,
                IdEmpresa = blog.IdEmpresa,
                Autor = blog.Autor,
                Titulo = blog.Titulo,
                Subtitulo = blog.Subtitulo,
                Descricao = blog.Descricao,
                Data = blog.Data,

                LinkFoto = blog.LinkFoto,
                Situacao = blog.Situacao
            });
        }
    }
}
