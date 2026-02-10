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
    public class AfiliadoController : ControllerBase
    {
        protected Context context;
        public AfiliadoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Afiliado
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
            var result = context.Afiliado.AsQueryable();
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
        public IActionResult Salvar([FromBody] AfiliadoRequest model)
        {

            Afiliado afiliado;
            Pessoa pessoa;
            if (model.IdPessoa > 0)
            {
                afiliado = context.Afiliado.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
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
                afiliado.Alterar(pessoa, User.Identity.Name);

                context.Update(afiliado);
            }
            else
            {
                pessoa = new Pessoa(model.Nome, model.Sexo, model.DataNascimento, model.Mae, model.Pai, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, User.Identity.Name);
                afiliado = new Afiliado(pessoa, User.Identity.Name);
                context.Afiliado.Add(afiliado);
            }
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var afiliado = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            if (afiliado == null)
                return BadRequest("Afilaido não encontrado ");

            return Ok(new AfiliadoResponse()
            {
                IdPessoa = afiliado.IdPessoa,
                Nome = afiliado.Nome,
                RazaoSocial = afiliado.RazaoSocial,
                Telefone1 = afiliado.Telefone1,
                Telefone2 = afiliado.Telefone2,
                Email = afiliado.Email,
                CpfCnpj = afiliado.CpfCnpj,
                Cep = afiliado.Cep,
                Sexo = afiliado.Sexo,
                Estado = afiliado.Estado,
                Cidade = afiliado.Cidade,
                Bairro = afiliado.Bairro,
                Logradouro = afiliado.Logradouro,
                Numero = afiliado.Numero,
                Complemento = afiliado.Complemento,
                Referencia = afiliado.Referencia,
                DataNascimento = afiliado.DataNascimento,
                Mae = afiliado.Mae,
                Pai = afiliado.Pai,
                TipoPessoa = afiliado.TipoPessoa,
                InscricaoEstadual = afiliado.InscricaoEstadual,
                InscricaoMunicipal = afiliado.InscricaoMunicipal,
                Situacao = afiliado.Situacao

            });
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var afiliado = context.Afiliado.FirstOrDefault(x => x.IdPessoa == id);
            var pessoa = context.Pessoa.FirstOrDefault(p => p.IdPessoa == id);

            pessoa.Excluir(User.Identity.Name);
            afiliado.Excluir(User.Identity.Name);

            context.Update(pessoa);
            context.Update(afiliado);
            context.SaveChanges();
            return Ok();
        }


    }
}

