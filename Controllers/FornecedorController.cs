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
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ERP_API.Models;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using ERP_API.Domain.Entidades;
using System.Data.Entity;
using MySqlX.XDevAPI;


namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FornecedorController : ControllerBase
    {
        protected Context context;
        public FornecedorController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Fornecedor
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
            var result = context.Fornecedor.AsQueryable();
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
        public IActionResult Salvar([FromBody] FornecedorRequest model)
        {

            Fornecedor fornecedor;
            Pessoa pessoa;
            if (model.IdPessoa > 0)
            {
                fornecedor = context.Fornecedor.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
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
                fornecedor.Alterar(pessoa, User.Identity.Name);

                context.Update(fornecedor);
            }
            else
            {
                pessoa = new Pessoa(model.Nome, model.Sexo, model.DataNascimento, model.Mae, model.Pai, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, User.Identity.Name);
                fornecedor = new Fornecedor(pessoa, User.Identity.Name);
                context.Fornecedor.Add(fornecedor);
            }
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var fornecedor = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            if (fornecedor == null)
                return BadRequest("Fornecedor não encontrado ");

            return Ok(new FornecedorResponse()
            {
                IdPessoa = fornecedor.IdPessoa,
                Nome = fornecedor.Nome,
                RazaoSocial = fornecedor.RazaoSocial,
                Telefone1 = fornecedor.Telefone1,
                Telefone2 = fornecedor.Telefone2,
                Email = fornecedor.Email,
                CpfCnpj = fornecedor.CpfCnpj,
                Cep = fornecedor.Cep,
                Sexo = fornecedor.Sexo,
                Estado = fornecedor.Estado,
                Cidade = fornecedor.Cidade,
                Bairro = fornecedor.Bairro,
                Logradouro = fornecedor.Logradouro,
                Numero = fornecedor.Numero,
                Complemento = fornecedor.Complemento,
                Referencia = fornecedor.Referencia,
                DataNascimento = fornecedor.DataNascimento,
                Mae = fornecedor.Mae,
                Pai = fornecedor.Pai,
                TipoPessoa = fornecedor.TipoPessoa,
                InscricaoEstadual = fornecedor.InscricaoEstadual,
                InscricaoMunicipal = fornecedor.InscricaoMunicipal,
                Situacao = fornecedor.Situacao

            });
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            var fornecedor = context.Fornecedor.FirstOrDefault(x => x.IdPessoa == id);

            pessoa.Excluir(User.Identity.Name);
            fornecedor.Excluir(User.Identity.Name);

            context.Update(pessoa);
            context.Update(fornecedor);
            context.SaveChanges();
            return Ok();
        }


    }
}
