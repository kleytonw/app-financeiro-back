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
using ERP_API.Models;
using ERP_API.Domain.Entidades;

namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ConsultorController : ControllerBase
    {
        protected Context context;
        public ConsultorController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Consultor
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
        [Route("listarRegiao")]
        public IActionResult ListarRegiao()
        {
            var result = context.Regiao
                  .Select(m => new
                  {
                      m.IdRegiao,
                      m.NomeRegiao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarConsultorRegiao")]
        public IActionResult ListarConsultorRegiao(int idPessoa)
        {
            var result = context.RegiaoConsultor.Include(x => x.Regiao).Where(x => x.IdPessoa == idPessoa)
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
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var result = context.Consultor.AsQueryable();
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
        public IActionResult Salvar([FromBody] ConsultorRequest model)
        {

            Consultor consultor;
            Pessoa pessoa;
            if (model.IdPessoa > 0)
            {
                consultor = context.Consultor.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
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
                consultor.Alterar(pessoa, User.Identity.Name);

                context.Update(consultor);
            }
            else
            {
                pessoa = new Pessoa(model.Nome, model.Sexo, model.DataNascimento, model.Mae, model.Pai, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, User.Identity.Name);
                consultor = new Consultor(pessoa, User.Identity.Name);
                context.Consultor.Add(consultor);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("salvarConsultorRegiao")]
        public IActionResult SalvarProdutoSetor([FromBody] ConsultorRequest model)
        {

            var consultor = context.Consultor.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
            var regiao = context.Regiao.FirstOrDefault(x => x.IdRegiao == model.IdRegiao);
            if (consultor == null)
            {
                return BadRequest("É necessário informar o Consultor ");
            }

            if (regiao == null)
            {
                return BadRequest("É necessário informar o Região ");
            }


            var checkConsultorRegiaoRepetido = context.RegiaoConsultor.FirstOrDefault(x => x.IdPessoa == model.IdPessoa && x.IdRegiao == model.IdRegiao);
            if (checkConsultorRegiaoRepetido != null)
            {
                return BadRequest("A Regiao já foi cadastrado no Consultor");
            }

            var consultorRegiao = new RegiaoConsultor(regiao, consultor, User.Identity.Name);
            context.RegiaoConsultor.Add(consultorRegiao);

            context.SaveChanges();
            return Ok();

        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var consultor = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            if (consultor == null)
                return BadRequest("Fornecedor não encontrado ");

            return Ok(new FornecedorResponse()
            {
                IdPessoa = consultor.IdPessoa,
                Nome = consultor.Nome,
                RazaoSocial = consultor.RazaoSocial,
                Telefone1 = consultor.Telefone1,
                Telefone2 = consultor.Telefone2,
                Email = consultor.Email,
                CpfCnpj = consultor.CpfCnpj,
                Cep = consultor.Cep,
                Sexo = consultor.Sexo,
                Estado = consultor.Estado,
                Cidade = consultor.Cidade,
                Bairro = consultor.Bairro,
                Logradouro = consultor.Logradouro,
                Numero = consultor.Numero,
                Complemento = consultor.Complemento,
                Referencia = consultor.Referencia,
                DataNascimento = consultor.DataNascimento,
                Mae = consultor.Mae,
                Pai = consultor.Pai,
                TipoPessoa =  consultor.TipoPessoa,
                InscricaoEstadual = consultor.InscricaoEstadual,
                InscricaoMunicipal = consultor.InscricaoMunicipal,
                Situacao = consultor.Situacao

            });
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            var consultor = context.Consultor.FirstOrDefault(x => x.IdPessoa == id);

            pessoa.Excluir(User.Identity.Name);
            consultor.Excluir(User.Identity.Name);

            context.Update(pessoa);
            context.Update(consultor);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluirConsultorRegiao")]
        public IActionResult ExcluirProdutoSetor(int idRegiaoConsultor)
        {
            var consultorRegiao = context.RegiaoConsultor.FirstOrDefault(x => x.IdRegiaoConsultor == idRegiaoConsultor);
            consultorRegiao.Excluir(User.Identity.Name);

            context.Remove(consultorRegiao);
            context.SaveChanges();
            return Ok();
        }


    }
}

