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
using System.Data.Entity;
using ERP_API.Models;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class PessoaController : ControllerBase
    {
        protected Context context;
        public PessoaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Pessoa
                  .Select(m => new
                  {
                      m.IdPessoa,
                      m.Sexo,
                      m.DataNascimento,
                      m.Mae,
                      m.Pai,
                      m.Nome,
                      m.TipoPessoa,
                      m.RazaoSocial,
                      m.CpfCnpj,
                      m.Telefone1,
                      m.Telefone2,
                      m.Email,
                      m.Cep,
                      m.Logradouro,
                      m.Numero,
                      m.Complemento,
                      m.Bairro,
                      m.Cidade,
                      m.Estado,
                      m.Referencia,
                      m.InscricaoEstadual,
                      m.InscricaoMunicipal,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarAtivas")]
        public IActionResult ListarAtivas()
        {
            var result = context.Pessoa.Where(x => x.Situacao == "Ativo")
                  .Select(m => new
                  {
                      m.IdPessoa,
                      m.Sexo,
                      m.DataNascimento,
                      m.Mae,
                      m.Pai,
                      m.Nome,
                      m.TipoPessoa,
                      m.RazaoSocial,
                      m.CpfCnpj,
                      m.Telefone1,
                      m.Telefone2,
                      m.Email,
                      m.Cep,
                      m.Logradouro,
                      m.Numero,
                      m.Complemento,
                      m.Bairro,
                      m.Cidade,
                      m.Estado,
                      m.Referencia,
                      m.InscricaoEstadual,
                      m.InscricaoMunicipal,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var result = context.Pessoa.AsQueryable();
            switch (model.Chave)
            {
                case "CpfCnpj":
                    result = result.Where(x => x.CpfCnpj == model.Valor);
                    break;
                case "Nome":
                    result = result.Where(x => x.Nome.Contains(model.Valor.ToUpper()));
                    break;
                case "RazaoSocial":
                    result = result.Where(x => x.RazaoSocial.Contains(model.Valor.ToUpper()));
                    break;
                default:
                    // code block
                    break;
            }

            return Ok(result.Select(m => new
            {
                m.IdPessoa,
                m.Nome,
                m.RazaoSocial,
                m.CpfCnpj,
                m.Situacao
            }).Take(500).ToList());

        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] PessoaRequest model)
        {

            Pessoa pessoa;
            if (model.IdPessoa > 0)
            {
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

                context.Update(pessoa);
            }
            else
            {
                pessoa = new Pessoa(model.Nome, model.Sexo, model.DataNascimento, model.Mae, model.Pai, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, User.Identity.Name);
                context.Pessoa.Add(pessoa);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            pessoa.Excluir(User.Identity.Name);

            context.Update(pessoa);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            if (pessoa == null)
                return BadRequest("Pessoa não encontrada ");

            return Ok(new PessoaResponse()
            {
                IdPessoa = pessoa.IdPessoa,
                Nome = pessoa.Nome,
                TipoPessoa = pessoa.TipoPessoa,
                RazaoSocial = pessoa.RazaoSocial,
                CpfCnpj = pessoa.CpfCnpj,
                Telefone1 = pessoa.Telefone1,
                Telefone2 = pessoa.Telefone2,
                Email = pessoa.Email,
                Cep = pessoa.Cep,
                Logradouro = pessoa.Logradouro,
                Numero = pessoa.Numero,
                Complemento = pessoa.Complemento,
                Bairro = pessoa.Bairro,
                Cidade = pessoa.Cidade,
                Estado = pessoa.Estado,
                Referencia = pessoa.Referencia,
                InscricaoEstadual = pessoa.InscricaoEstadual,
                InscricaoMunicipal = pessoa.InscricaoMunicipal,
                Situacao = pessoa.Situacao
            });
        }


    }
}

