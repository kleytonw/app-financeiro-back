using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using ERP.Models;
using ERP.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using ERP_API.Models;
using ERP_API.Domain.Entidades;

namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class VendedorController : ControllerBase
    {
        protected Context context;
        public VendedorController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Vendedor
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
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] VendedorRequest model)
        {

            Vendedor vendedor;
            Pessoa pessoa;
            if (model.IdPessoa > 0)
            {
                vendedor = context.Vendedor.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
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
                vendedor.Alterar(pessoa, User.Identity.Name);

                context.Update(vendedor);
            }
            else
            {
                pessoa = new Pessoa(model.Nome, model.Sexo, model.DataNascimento, model.Mae, model.Pai, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, User.Identity.Name);
                vendedor = new Vendedor(pessoa, User.Identity.Name);
                context.Vendedor.Add(vendedor);
            }
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var vendedor = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            if (vendedor == null)
                return BadRequest("Fornecedor não encontrado ");

            return Ok(new FornecedorResponse()
            {
                IdPessoa = vendedor.IdPessoa,
                Nome = vendedor.Nome,
                RazaoSocial = vendedor.RazaoSocial,
                Telefone1 = vendedor.Telefone1,
                Telefone2 = vendedor.Telefone2,
                Email = vendedor.Email,
                CpfCnpj = vendedor.CpfCnpj,
                Cep = vendedor.Cep,
                Sexo = vendedor.Sexo,
                Estado = vendedor.Estado,
                Cidade = vendedor.Cidade,
                Bairro = vendedor.Bairro,
                Logradouro = vendedor.Logradouro,
                Numero = vendedor.Numero,
                Complemento = vendedor.Complemento,
                Referencia = vendedor.Referencia,
                DataNascimento = vendedor.DataNascimento,
                Mae = vendedor.Mae,
                Pai = vendedor.Pai,
                TipoPessoa = vendedor.TipoPessoa,
                InscricaoEstadual = vendedor.InscricaoEstadual,
                InscricaoMunicipal = vendedor.InscricaoMunicipal,
                Situacao = vendedor.Situacao

            });
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            var vendedor = context.Vendedor.FirstOrDefault(x => x.IdPessoa == id);

            pessoa.Excluir(User.Identity.Name);
            vendedor.Excluir(User.Identity.Name);

            context.Update(pessoa);
            context.Update(vendedor);
            context.SaveChanges();
            return Ok();
        }


    }
}

