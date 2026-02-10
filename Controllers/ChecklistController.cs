using Dapper;
using ERP.Domain;
using ERP.Infra;
using ERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERP.Controllers
    {
    [ApiController]
    [Route("api/[controller]")]
    public class ChecklistController : ControllerBase
        {
        private IConfiguration _config;
        protected Context context;
        public ChecklistController (Context context,
            IConfiguration config)   // : base(context)
            {
            _config = config;
            this.context = context;
            }

        [HttpPost]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar ([FromBody] FiltroChecklistModel model)
            {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            Expression<Func<Checklist, bool>> filtroNome = registro => true;
            #region filtros
            if ( !string.IsNullOrEmpty(model.Nome) )
                filtroNome = (Checklist registro) => registro.Nome.Contains(model.Nome);
            #endregion

            var resultado = context.Checklist
                .Where(filtroNome).Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
                .Select(m => new
                    {
                    m.IdChecklist,
                    m.Nome,
                    m.Situacao
                    }).ToList();

            return Ok(resultado);
            }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar ([FromBody] ChecklistModel model)
            {
            var usuarioLogado = context.Usuario.Include(x => x.Empresa).FirstOrDefault(x => x.Login == User.Identity.Name);
            Checklist checklist;
            if ( model.IdChecklist > 0 )
                {
                checklist = context.Checklist.FirstOrDefault(x => x.IdChecklist == model.IdChecklist);
                checklist.Alterar(model.Nome, User.Identity.Name);

                context.Update(checklist);
                }
            else
                {
                checklist = new Checklist(model.Nome, usuarioLogado.Empresa, User.Identity.Name);
                context.Checklist.Add(checklist);
                }
            context.SaveChanges();
            return Ok();
            }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir (int IdChecklist)
            {
            var d = context.Checklist.FirstOrDefault(x => x.IdChecklist == IdChecklist);
            d.Excluir(User.Identity.Name);

            context.Update(d);
            context.SaveChanges();
            return Ok();
            }


        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter (int IdChecklist)
            {
            var d = context.Checklist.FirstOrDefault(x => x.IdChecklist == IdChecklist);
            if ( d == null )
                return BadRequest("Checklist não encontrado");

            var res = new
                {
                d.IdChecklist,
                d.Nome,
                d.Situacao
                };
            return Ok(res);
            }
        }
    }