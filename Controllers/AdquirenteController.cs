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
using System.Collections.ObjectModel;
using ERP_API.Models;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using ERP_API.Domain.Entidades;
using System.Data.Entity;


namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AdquirenteController : ControllerBase
    {
        protected Context context;
        public AdquirenteController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Adquirente
                  .Select(m => new
                  {
                      m.IdPessoa,
                      m.Pessoa.Nome,
                      m.Pessoa.RazaoSocial,
                      m.Pessoa.CpfCnpj,
                      m.Situacao
                  }).Take(500).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarAtivos")]
        public IActionResult ListarAtivos()
        {
            var result = context.Adquirente
                  .Select(m => new
                  {
                      m.IdPessoa,
                      m.Pessoa.Nome,
                      m.Pessoa.RazaoSocial,
                      m.Pessoa.CpfCnpj,
                      m.Situacao
                  }).Where(x => x.Situacao == "Ativo").Take(500).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var result = context.Adquirente.AsQueryable();
            switch (model.Chave)
            {
                case "CpfCnpj":
                    result = result.Where(x => x.Pessoa.CpfCnpj == model.Valor);
                    break;
                case "Nome":
                    result = result.Where(x => x.Pessoa.Nome.Contains(model.Valor.ToUpper()));
                    break;
                case "RazaoSocial":
                    result = result.Where(x => x.Pessoa.RazaoSocial.Contains(model.Valor.ToUpper()));
                    break;
                default:
                    // code block
                    break;
            }

            return Ok(result.Select(m => new
            {
                m.IdPessoa,
                m.Pessoa.Nome,
                m.Pessoa.RazaoSocial,
                m.Pessoa.CpfCnpj,
                m.Pessoa.Situacao
            }).Take(500).ToList());

        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] AdquirenteRequest model)
        {

            Adquirente adquirente;
            Pessoa pessoa;
            if (model.IdPessoa > 0)
            {
                adquirente = context.Adquirente.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
                pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);

                pessoa.Alterar(
                    model.Nome,
                    model.Sexo,
                    model.DataNascimento,
                    model.Mae,
                    model.Pai,
                    model.TipoPessoa,
                    model.RazaoSocial,
                    model.CpfCnpj,
                    model.Telefone1,
                    model.Telefone2,
                    model.Email,
                    model.Cep,
                    model.Logradouro,
                    model.Numero,
                    model.Complemento,
                    model.Bairro,
                    model.Cidade,
                    model.Estado,
                    model.Referencia,
                    model.InscricaoEstadual,
                    model.InscricaoMunicipal,
                    User.Identity.Name);
                adquirente.Alterar(pessoa, User.Identity.Name);

                context.Update(adquirente);
            }
            else
            {
                pessoa = new Pessoa(model.Nome, model.Sexo, model.DataNascimento, model.Mae, model.Pai, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, User.Identity.Name);
                adquirente = new Adquirente(pessoa, User.Identity.Name);
                context.Adquirente.Add(adquirente);
            }
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var adquirente = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            if (adquirente == null)
                return BadRequest("Adquirente não encontrado ");

            return Ok(new AdquirenteResponse()
            {
                IdPessoa = adquirente.IdPessoa,
                Nome = adquirente.Nome,
                RazaoSocial = adquirente.RazaoSocial,
                Telefone1 = adquirente.Telefone1,
                Telefone2 = adquirente.Telefone2,
                Email = adquirente.Email,
                CpfCnpj = adquirente.CpfCnpj,
                Cep = adquirente.Cep,
                Sexo = adquirente.Sexo,
                Estado = adquirente.Estado,
                Cidade = adquirente.Cidade,
                Bairro = adquirente.Bairro,
                Logradouro = adquirente.Logradouro,
                Numero = adquirente.Numero,
                Complemento = adquirente.Complemento,
                Referencia = adquirente.Referencia,
                DataNascimento = adquirente.DataNascimento,
                Mae = adquirente.Mae,
                Pai = adquirente.Pai,
                TipoPessoa = adquirente.TipoPessoa, 
                InscricaoEstadual = adquirente.InscricaoEstadual,
                InscricaoMunicipal = adquirente.InscricaoMunicipal,
                Situacao = adquirente.Situacao

            });
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var pessoa = context.Pessoa.FirstOrDefault(p => p.IdPessoa == id);
            var adquirente = context.Adquirente.FirstOrDefault(x => x.IdPessoa == id);

            pessoa.Excluir(User.Identity.Name);
            adquirente.Excluir(User.Identity.Name);

            context.Update(pessoa);
            context.Update(adquirente);
            context.SaveChanges();
            return Ok();
        }


    }
}


