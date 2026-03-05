using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ERP_API.Models;
using Microsoft.EntityFrameworkCore;
using ERP_API.Domain.Entidades;


namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DependenteController : ControllerBase
    {
        protected Context context;
        public DependenteController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Dependente.Where(x => x.Situacao == "Ativo")    
                  .Select(m => new
                  {
                      m.IdDependente,
                      m.Nome,
                      m.Situacao
                  }).Take(500).ToList();
            return Ok(result);
        }

        //[HttpPost]
        //[Route("pesquisar")]
        //public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        //{
        //    var result = context.Adquirente.AsQueryable();
        //    switch (model.Chave)
        //    {
        //        case "CpfCnpj":
        //            result = result.Where(x => x.Pessoa.CpfCnpj == model.Valor);
        //            break;
        //        case "Nome":
        //            result = result.Where(x => x.Pessoa.Nome.Contains(model.Valor.ToUpper()));
        //            break;
        //        case "RazaoSocial":
        //            result = result.Where(x => x.Pessoa.RazaoSocial.Contains(model.Valor.ToUpper()));
        //            break;
        //        default:
        //            // code block
        //            break;
        //    }

        //    return Ok(result.Select(m => new
        //    {
        //        m.IdPessoa,
        //        m.Pessoa.Nome,
        //        m.Pessoa.RazaoSocial,
        //        m.Pessoa.CpfCnpj,
        //        m.Pessoa.Situacao
        //    }).Take(500).ToList());

        //}

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] DependenteRequest model)
        {

            Dependente dependente;
            if (model.IdDependente > 0)
            {
                dependente = context.Dependente.FirstOrDefault(x => x.IdDependente == model.IdDependente);
                if (dependente == null)
                    return BadRequest();

                dependente.Alterar(
                    model.Nome,
                    User.Identity.Name);

                context.Update(dependente);
            }
            else
            {
                dependente = new Dependente(model.Nome, User.Identity.Name);
                context.Add(dependente);
            }
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var dependente = context.Dependente.FirstOrDefault(x => x.IdDependente == id);
            if (dependente == null)
                return BadRequest("Dependente não encontrado ");

            return Ok(new DependenteResponse()
            {
                IdDependente = dependente.IdDependente,
                Nome = dependente.Nome,
                Situacao = dependente.Situacao

            });
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var dependente = context.Dependente.FirstOrDefault(x => x.IdDependente == id);

            dependente.Excluir(User.Identity.Name);

            context.Update(dependente);
            context.SaveChanges();
            return Ok();
        }


    }
}


