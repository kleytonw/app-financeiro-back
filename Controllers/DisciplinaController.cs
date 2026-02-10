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
using ERP_API.Domain.Entidades;
using System.Data.Entity;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class DisciplinaController : ControllerBase
    {
        protected Context context;
        public DisciplinaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Disciplina
                  .Select(m => new
                  {
                      m.IdDisciplina,
                      m.Nome,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarDisciplinaCurso")]
        public IActionResult ListarDisciplinaCurso (int idDisciplina)
        {
            var result = context.DisciplinaCurso.Include(x => x.Curso).Where(x => x.IdDisciplina == idDisciplina)
                  .Select(m => new
                  {
                      m.Disciplina.Nome,
                      m.IdDisciplinaCurso,
                      m.IdCurso,
                      m.Curso.NomeCurso
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarCurso")]
        public IActionResult ListarCurso()
        {
            var result = context.Curso
                  .Select(m => new
                  {
                      m.IdCurso,
                      m.NomeCurso
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] DisciplinaRequest model)
        {
            Disciplina disciplina;
            if (model.IdDisciplina > 0)
            {
                disciplina = context.Disciplina.FirstOrDefault(x => x.IdDisciplina == model.IdDisciplina);
                disciplina.Alterar(model.Nome, User.Identity.Name);

                context.Update(disciplina);
            }
            else
            {
                disciplina = new Disciplina(model.Nome,User.Identity.Name);
                context.Disciplina.Add(disciplina);
            }
            context.SaveChanges();
            return Ok();
        }


        [HttpPost]
        [Route("salvarDisciplinaCurso")]
        public IActionResult SalvarDisciplinaCurso([FromBody] DisciplinaRequest model)
        {

            var curso = context.Curso.FirstOrDefault(x => x.IdCurso == model.IdCurso);
            var disciplina = context.Disciplina.FirstOrDefault(x => x.IdDisciplina == model.IdDisciplina);

            if (curso == null)
            {
                return BadRequest("É necessário informar o Curso ");
            }

            if (disciplina == null)
            {
                return BadRequest("É necessário informar o Disciplina ");
            }

            var checkDisciplinaCursoRepetido = context.DisciplinaCurso.FirstOrDefault(x => x.IdCurso == model.IdCurso && x.IdDisciplina == model.IdDisciplina);
            if (checkDisciplinaCursoRepetido != null)
            {
                return BadRequest("O Curso já foi cadastrado na Disciplina");
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
            var disciplina = context.Disciplina.FirstOrDefault(x => x.IdDisciplina == id);
            disciplina.Excluir(User.Identity.Name);

            context.Update(disciplina);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluirDisciplinaCurso")]
        public IActionResult ExcluirDisciplinaCurso(int idDisciplinaCurso)
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
            var disciplina = context.Disciplina.FirstOrDefault(x => x.IdDisciplina == id);
            if (disciplina == null)
                return BadRequest("Disciplina não encontrado ");

            return Ok(new DisciplinaResponse()
            {
                IdDisciplina = disciplina.IdDisciplina,
                Nome = disciplina.Nome,
                Situacao = disciplina.Situacao
            });
        }

        [HttpGet]
        [Route("obterDisciplinaCurso")]
        public IActionResult ObterDisciplinaCurso(int id)
        {
            Disciplina disciplina;

            disciplina = context.Disciplina.FirstOrDefault(x => x.IdDisciplina == id);
            if (disciplina == null)
                return BadRequest("Setor não encontrado ");

            return Ok(new DisciplinaResponse()
            {
                IdDisciplina = disciplina.IdDisciplina,
                Nome = disciplina.Nome,
                Situacao = disciplina.Situacao
            });
        }
    }
}
