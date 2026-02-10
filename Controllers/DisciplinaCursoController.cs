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
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DisciplinaCursoController : ControllerBase
    {
        protected Context context;
        public DisciplinaCursoController(Context context)
        {
            this.context = context;
        }



        [HttpPost]
        [Route("salvarDisciplinaCurso")]
        public IActionResult SalvarSetorProduto([FromBody] DisciplinaRequest model)
        {

            var curso = context.Curso.FirstOrDefault(x => x.IdCurso == model.IdCurso);
            var disciplina = context.Disciplina.FirstOrDefault(x => x.IdDisciplina == model.IdDisciplina);



            var disciplinaCurso = new DisciplinaCurso(disciplina, curso, User.Identity.Name);
            context.DisciplinaCurso.Add(disciplinaCurso);

            context.SaveChanges();
            return Ok();
        }
    }
}

