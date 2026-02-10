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
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RegiaoController : ControllerBase
    {
        protected Context context;
        public RegiaoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Regiao
                  .Select(m => new
                  {
                      m.IdRegiao,
                      m.NomeRegiao,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarConsultores")]
        public IActionResult ListarConsultores()
        {
            var result = context.Consultor
                  .Select(m => new
                  {
                      m.IdPessoa,
                      m.Pessoa.Nome
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarRegiaoConsultor")]
        public IActionResult ListarSetorProduto(int idRegiao)
        {
            var result = context.RegiaoConsultor.Include(x => x.Consultor).Where(x => x.IdRegiao == idRegiao)
                  .Select(m => new
                  {
                      m.Regiao.NomeRegiao,
                      m.IdRegiaoConsultor,
                      m.IdPessoa,
                      m.Consultor.Pessoa.Nome
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] RegiaoRequest model)
        {
            Regiao regiao;
            if (model.IdRegiao > 0)
            {
                regiao = context.Regiao.FirstOrDefault(x => x.IdRegiao == model.IdRegiao);
                regiao.Alterar(model.NomeRegiao, User.Identity.Name);

                context.Update(regiao);
            }
            else
            {
                regiao = new Regiao(model.NomeRegiao, User.Identity.Name);
                context.Regiao.Add(regiao);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("salvarRegiaoConsultor")]
        public IActionResult SalvarSetorProduto([FromBody] RegiaoRequest model)
        {

            var consultor = context.Consultor.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
            var regiao = context.Regiao.FirstOrDefault(x => x.IdRegiao == model.IdRegiao);

            if (consultor == null)
            {
                return BadRequest("É necessário informar o Consultor ");
            }

            if (regiao == null)
            {
                return BadRequest("É necessário informar a Região ");
            }

            var checkRegiaoConsultorRepetido = context.RegiaoConsultor.FirstOrDefault(x => x.IdPessoa == model.IdPessoa && x.IdRegiao == model.IdRegiao);
            if (checkRegiaoConsultorRepetido != null)
            {
                return BadRequest("O Consultor já foi cadastrado na Região");
            }

            var regiaoConsultor = new RegiaoConsultor(regiao, consultor, User.Identity.Name);
            context.RegiaoConsultor.Add(regiaoConsultor);

            context.SaveChanges();
            return Ok();

        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var regiao = context.Regiao.FirstOrDefault(x => x.IdRegiao == id);
            regiao.Excluir(User.Identity.Name);

            context.Update(regiao);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluirRegiaoConsultor")]
        public IActionResult ExcluirSetorProduto(int idRegiaoConsultor)
        {
            var regiaoConsultor = context.RegiaoConsultor.FirstOrDefault(x => x.IdRegiaoConsultor == idRegiaoConsultor);
            regiaoConsultor.Excluir(User.Identity.Name);

            context.Remove(regiaoConsultor);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var regiao = context.Regiao.FirstOrDefault(x => x.IdRegiao == id);
            if (regiao == null)
                return BadRequest("Regiao não encontrado ");

            return Ok(new RegiaoResponse()
            {
                IdRegiao = regiao.IdRegiao,
                NomeRegiao = regiao.NomeRegiao,
                Situacao = regiao.Situacao
            });
        }
    }
}
