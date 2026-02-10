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
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SetorController : ControllerBase
    {
        protected Context context;
        public SetorController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Setor
                  .Select(m => new
                  {
                      m.IdSetor,
                      m.Nome,
                      SetorPai = context.Setor.FirstOrDefault(x => x.IdSetor == m.IdSetorPai).Nome,
                      m.NumeroOrdem,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarSetores")]
        public IActionResult ListarSetores()
        {
            var result = context.Setor
                  .Where(m => m.NumeroOrdem != null)
                  .OrderBy(m => m.Nome)
                  .Select(m => new
                  {
                      m.IdSetor,
                      m.Nome,
                      SetorPai = context.Setor.FirstOrDefault(x => x.IdSetor == m.IdSetorPai).Nome,
                      m.NumeroOrdem,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarProduto")]
        public IActionResult ListarProduto()
        {
            var result = context.Produto
                  .Select(m => new
                  {
                      m.IdProduto,
                      m.NomeProduto
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarSetorProduto")]
        public IActionResult ListarSetorProduto(int idSetor)
        {
            var result = context.SetorProduto.Include(x => x.Produto).Where(x => x.IdSetor == idSetor)
                  .Select(m => new
                  {
                      m.Setor.Nome,
                      m.IdSetorProduto,
                      m.IdProduto,
                      m.Produto.NomeProduto
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] SetorRequest model)
        {
            Setor setor;
            if (model.IdSetor > 0)
            {
                setor = context.Setor.FirstOrDefault(x => x.IdSetor == model.IdSetor);
                var setorPai = context.Setor.FirstOrDefault(x => x.IdSetor == model.IdSetorPai);
                setor.Alterar(model.Nome, setorPai?.IdSetor, model.NumeroOrdem, User.Identity.Name);

                context.Update(setor);
            }
            else
            {
                var setorPai = context.Setor.FirstOrDefault(x => x.IdSetor == model.IdSetorPai);
                setor = new Setor(model.Nome, setorPai?.IdSetor, model.NumeroOrdem,  User.Identity.Name);
                context.Setor.Add(setor);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("salvarSetorProduto")]
        public IActionResult SalvarSetorProduto([FromBody] SetorRequest model)
        {

           var produto = context.Produto.FirstOrDefault(x => x.IdProduto == model.IdProduto);
           var setor = context.Setor.FirstOrDefault(x => x.IdSetor == model.IdSetor);

            if (produto == null)
            {
                return BadRequest("É necessário informar o Produto ");
            }

            if (setor == null)
            {
                return BadRequest("É necessário informar o Setor ");
            }

            var checkSetorProdutoRepetido = context.SetorProduto.FirstOrDefault(x => x.IdProduto == model.IdProduto && x.IdSetor == model.IdSetor);
           if(checkSetorProdutoRepetido != null)
            {
                return BadRequest("O Produto já foi cadastrado no Setor");
            } 
           
            var setorProduto = new SetorProduto(setor, produto, User.Identity.Name);
            context.SetorProduto.Add(setorProduto);
            
            context.SaveChanges();
            return Ok();

        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var result = context.Setor.AsQueryable();
            result = result.Where(x => x.Nome.Contains(model.Valor.ToUpper()));

            return Ok(result.Select(m => new
            {
                m.IdSetor,
                m.Nome,
                SetorPai = context.Setor.FirstOrDefault(x => x.IdSetor == m.IdSetorPai).Nome,
                m.NumeroOrdem,
                m.Situacao
            }).Take(500).ToList());

        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var setor = context.Setor.FirstOrDefault(x => x.IdSetor == id);
            setor.Excluir(User.Identity.Name);

            context.Update(setor);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluirSetorProduto")]
        public IActionResult ExcluirSetorProduto(int idSetorProduto)
        {
            var setorProduto = context.SetorProduto.FirstOrDefault(x => x.IdSetorProduto == idSetorProduto);
            setorProduto.Excluir(User.Identity.Name);

            context.Remove(setorProduto);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var setor = context.Setor.FirstOrDefault(x => x.IdSetor == id);
            if (setor == null)
                return BadRequest("Setor não encontrado ");

            return Ok(new SetorResponse()
            {
                IdSetor = setor.IdSetor,
                Nome = setor.Nome,
                NumeroOrdem = setor.NumeroOrdem,
                IdSetorPai = setor.IdSetorPai,
                Situacao = setor.Situacao
            });
        }

        [HttpGet]
        [Route("obterSetorProduto")]
        public IActionResult ObterSetorProduto(int id)
        {
            Setor setor;

             setor = context.Setor.FirstOrDefault(x => x.IdSetor == id);
            if (setor == null)
                return BadRequest("Setor não encontrado ");

            return Ok(new SetorResponse()
            {
                IdSetor = setor.IdSetor,
                Nome = setor.Nome,
                NumeroOrdem = setor.NumeroOrdem,
                IdSetorPai =  setor.IdSetorPai,
                Situacao = setor.Situacao
            });
        }
    }
}
