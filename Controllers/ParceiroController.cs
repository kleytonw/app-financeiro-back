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
using ERP.Domain.Entidades;
//using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ERP_API.Models;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using ERP_API.Domain.Entidades;
using System.Data.Entity;
using MySqlX.XDevAPI;
using Microsoft.EntityFrameworkCore.Query;


namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ParceiroController : ControllerBase
    {
        protected Context context;
        public ParceiroController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Parceiro
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

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var result = context.Parceiro.AsQueryable();
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
        public IActionResult Salvar([FromBody] ParceiroRequest model)
        {

            Parceiro parceiro;
            Pessoa pessoa;
            if (model.IdPessoa > 0)
            {
                parceiro = context.Parceiro.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
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
                parceiro.Alterar(pessoa, User.Identity.Name);

                context.Update(parceiro);
            }
            else
            {
                pessoa = new Pessoa(model.Nome, model.Sexo, model.DataNascimento, model.Mae, model.Pai, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, User.Identity.Name);
                parceiro = new Parceiro(pessoa, User.Identity.Name);
                context.Parceiro.Add(parceiro);
            }
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var parceiro = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            if (parceiro == null)
                return BadRequest("Parceiro não encontrado ");

            return Ok(new ParceiroResponse()
            {
                IdPessoa = parceiro.IdPessoa,
                Nome = parceiro.Nome,
                RazaoSocial = parceiro.RazaoSocial,
                Telefone1 = parceiro.Telefone1,
                Telefone2 = parceiro.Telefone2,
                Email = parceiro.Email,
                CpfCnpj = parceiro.CpfCnpj,
                Cep = parceiro.Cep,
                Sexo = parceiro.Sexo,
                Estado = parceiro.Estado,
                Cidade = parceiro.Cidade,
                Bairro = parceiro.Bairro,
                Logradouro = parceiro.Logradouro,
                Numero = parceiro.Numero,
                Complemento = parceiro.Complemento,
                Referencia = parceiro.Referencia,
                DataNascimento = parceiro.DataNascimento,
                Mae = parceiro.Mae,
                Pai = parceiro.Pai,
                TipoPessoa = parceiro.TipoPessoa,
                InscricaoEstadual = parceiro.InscricaoEstadual,
                InscricaoMunicipal = parceiro.InscricaoMunicipal,
                Situacao = parceiro.Situacao

            });
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var pessoa = context.Pessoa.FirstOrDefault(x =>x.IdPessoa == id);
            var parceiro = context.Parceiro.FirstOrDefault(x => x.IdPessoa == id);

            pessoa.Excluir(User.Identity.Name);
            parceiro.Excluir(User.Identity.Name);

            context.Update(pessoa);
            context.Update(parceiro);
            context.SaveChanges();
            return Ok();
        }


    }
}
