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
using ERP_API.Models;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class MovimentacaoLogController : ControllerBase
    {
        protected Context context;
        public MovimentacaoLogController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.MovimentacaoLog.Include(x => x.Empresa)
                  .Select(m => new
                  {
                      m.IdMovimentacaoLog,
                      m.Empresa.Nome,
                      m.DataMovimentacaoLog,
                      m.ChaveAcesso,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var result = context.MovimentacaoLog.Where(x => x.DataMovimentacaoLog.Date >= model.DataInicio.Date && x.DataMovimentacaoLog.Date <= model.DataFim.Date && x.IdEmpresa == model.IdEmpresa);

            return Ok(result.Select(m => new
            {
                m.IdMovimentacaoLog,
                m.IdEmpresa,
                m.Empresa.Nome,
                m.ChaveAcesso,
                m.DataMovimentacaoLog,
                m.Situacao
            }).Take(500).ToList());
        }

        [HttpGet]
        [Route("listarEmpresa")]
        public IActionResult ListarEmpresa()
        {
            var result = context.Empresa
                  .Select(m => new
                  {
                      m.Nome,
                      m.IdEmpresa,
                  }).ToList();
            return Ok(result);
        }





        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var movimentacaoLog = context.MovimentacaoLog.FirstOrDefault(x => x.IdMovimentacaoLog == id);
            movimentacaoLog.Excluir(User.Identity.Name);

            context.Update(movimentacaoLog);
            context.SaveChanges();
            return Ok();
        }

    }
}


