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

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class CursoController : ControllerBase
    {
        protected Context context;
        public CursoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Curso
                  .Select(m => new
                  {
                      m.IdCurso,
                      m.NomeCurso,
                      m.Valor,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarDisciplina")]
        public IActionResult ListarDisciplina()
        {
            var result = context.Disciplina
                  .Select(m => new
                  {
                      m.IdDisciplina,
                      m.Nome
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarCursoDisciplina")]
        public IActionResult ListarCursoDisciplina(int idCurso)
        {
            var result = context.DisciplinaCurso.Include(x => x.Disciplina).Where(x => x.IdCurso == idCurso)
                  .Select(m => new
                  {
                      m.Disciplina.Nome,
                      m.IdDisciplinaCurso,
                      m.IdCurso,
                      m.Curso.NomeCurso
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] CursoRequest model)
        {
            Curso curso;
            if (model.IdCurso > 0)
            {
                curso = context.Curso.FirstOrDefault(x => x.IdCurso== model.IdCurso);
                curso.Alterar(model.NomeCurso, model.Valor, User.Identity.Name);

                context.Update(curso);
            }
            else
            {
                curso = new Curso(model.NomeCurso, model.Valor, User.Identity.Name);
                context.Curso.Add(curso);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("salvarCursoDisciplina")]
        public IActionResult SalvarCursoDisciplina([FromBody] CursoRequest model)
        {

            var curso = context.Curso.FirstOrDefault(x => x.IdCurso == model.IdCurso);
            var disciplina = context.Disciplina.FirstOrDefault(x => x.IdDisciplina == model.IdDisciplina);
            if (curso == null)
            {
                return BadRequest("É necessário informar o Curso ");
            }


            if (disciplina == null)
            {
                return BadRequest("É necessário informar a Disciplina ");
            }


            var checkCursoDisciplinaRepetido = context.DisciplinaCurso.FirstOrDefault(x => x.IdCurso == model.IdCurso && x.IdDisciplina == model.IdDisciplina);
            if (checkCursoDisciplinaRepetido != null)
            {
                return BadRequest("A Disciplina já foi cadastrado no Curso");
            }

            var disciplinaCurso = new DisciplinaCurso(disciplina, curso, User.Identity.Name);
            context.DisciplinaCurso.Add(disciplinaCurso);

            context.SaveChanges();
            return Ok();

        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var curso = context.Curso.FirstOrDefault(x => x.IdCurso == id);
            curso.Excluir(User.Identity.Name);

            context.Update(curso);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluirCursoDisciplina")]
        public IActionResult ExcluirProdutoSetor(int idDisciplinaCurso)
        {
            var disciplinaCurso = context.DisciplinaCurso.FirstOrDefault(x => x.IdDisciplinaCurso == idDisciplinaCurso);
            disciplinaCurso.Excluir(User.Identity.Name);

            context.Remove(disciplinaCurso);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var curso = context.Curso.FirstOrDefault(x => x.IdCurso == id);
            if (curso == null)
                return BadRequest("Curso não encontrado ");

            return Ok(new CursoResponse()
            {
                IdCurso = curso.IdCurso,
                NomeCurso = curso.NomeCurso,
                Valor = curso.Valor,
                Situacao = curso.Situacao
            });
        }

        [HttpGet]
        [Route("obterCursoDisciplina")]
        public IActionResult ObterCursoDisciplina(int id)
        {
            Curso curso;

            curso = context.Curso.FirstOrDefault(x => x.IdCurso == id);
            if (curso == null)
                return BadRequest("Curso não encontrado ");

            return Ok(new CursoResponse()
            {
                IdCurso = curso.IdCurso,
                NomeCurso = curso.NomeCurso,
                Situacao = curso.Situacao
            });
        }
    }
}