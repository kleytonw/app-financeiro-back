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
using System.Reflection.PortableExecutable;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ContratoOperadoraController : ControllerBase
    {
        protected Context context;

        public ContratoOperadoraController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            if (usuarioLogado.TipoUsuario == "Administrador")
            {
                 var result = context.ContratoOperadora
                   .Select(m => new
                   {
                       m.IdContratoOperadora,
                       m.Operadora.NomeOperadora,
                       m.DataInicio,
                       m.DataTermino,
                       m.IdUnidade,
                       m.Unidade.Nome,
                       ContaRecebimento = m.ContaRecebimento.Conta,
                       AgenciaRecebimento = m.ContaRecebimento.Agencia,
                       BancoRecebimento = m.ContaRecebimento.Banco.NomeBanco,
                       ContaGravame = m.ContaGravame.Conta,
                       AgenciaGravame = m.ContaGravame.Agencia,
                       BancoGravame = m.ContaGravame.Banco.NomeBanco,
                       m.Situacao
                   }).Where(x => x.Situacao == "Ativo").ToList();

                return Ok(result);
            }

            else
            {

                var result = context.ContratoOperadora.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
                    .Select(m => new
                    {
                        m.IdContratoOperadora,
                        m.Operadora.NomeOperadora,
                        m.DataInicio,
                        m.DataTermino,
                        m.IdUnidade,
                        m.Unidade.Nome,
                        ContaRecebimento = m.ContaRecebimento.Conta,
                        AgenciaRecebimento = m.ContaRecebimento.Agencia,
                        BancoRecebimento = m.ContaRecebimento.Banco.NomeBanco,
                        ContaGravame = m.ContaGravame.Conta,
                        AgenciaGravame = m.ContaGravame.Agencia,
                        BancoGravame = m.ContaGravame.Banco.NomeBanco,
                        m.Situacao
                    }).Where(x => x.Situacao == "Ativo").ToList();

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("listaroperadora")]
        public IActionResult ListarOperadora()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var result = context.Operadora
                .Select(m => new {
                    m.IdOperadora,
                    m.NomeOperadora
                }).ToList();

            return Ok(result);
        }


        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);


            var query = context.ContratoOperadora.AsQueryable();

            if(model.IdUnidade != 0)
            {
                query = query.Where(x => x.IdUnidade == model.IdUnidade);
            }

            if(model.IdOperadora != 0)
            {
                query = query.Where(x => x.IdOperadora == model.IdOperadora);
            }

            if (usuarioLogado.TipoUsuario != "Administrador")
            {
                query = query.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa);
            }
                var result = query
               .Select(m => new
               {
                   m.IdContratoOperadora,
                   m.Operadora.NomeOperadora,
                   m.DataInicio,
                   m.DataTermino,
                   m.Unidade.Nome,
                   m.IdUnidade,
                   ContaRecebimente = m.ContaRecebimento.Conta,
                   AgenciaRecebimento = m.ContaRecebimento.Agencia,
                   BancoRecebimento = m.ContaRecebimento.Banco.NomeBanco,
                   ContaGravame = m.ContaGravame.Conta,
                   AgenciaGravame = m.ContaGravame.Agencia,
                   BancoGravame = m.ContaGravame.Banco.NomeBanco,
                   m.Situacao
               }).ToList();

                return Ok(result);


            

        }


        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ContratoOperadoraRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            Empresa empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);

            ContratoOperadora contratoOperadora;
            ContaBancaria contaRecebimento = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == model.IdContaRecebimento);
            ContaBancaria contaGravame = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == model.IdContaGravame);
            Operadora operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == model.IdOperadora);
            Unidade unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);

            if (contaRecebimento == null)
                return BadRequest("Conta de recebimento não encontrada");

            var checkContratoOperadora = context.ContratoOperadora.FirstOrDefault(x => x.IdOperadora == model.IdOperadora && x.IdUnidade == model.IdUnidade &&  x.Situacao == "Ativo");
            if (checkContratoOperadora != null)
                return BadRequest("Não é possível cadastrar mais de um contrato ativo para a mesma operadora!");

            if (model.IdContratoOperadora > 0)
            {
                contratoOperadora = context.ContratoOperadora.FirstOrDefault(x => x.IdContratoOperadora == model.IdContratoOperadora);
                contratoOperadora.Alterar(operadora, model.DataInicio, model.DataTermino, empresa, unidade, contaRecebimento, contaGravame, User.Identity.Name);
            }
            else { 
            
                contratoOperadora = new ContratoOperadora(
                    operadora,
                    model.DataInicio,
                    model.DataTermino,
                    empresa,
                    unidade,
                    contaRecebimento,
                    contaGravame,
                    User.Identity.Name
                );

                context.ContratoOperadora.Add(contratoOperadora);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var contratoOperadora = context.ContratoOperadora.FirstOrDefault(x => x.IdContratoOperadora == id);
            contratoOperadora.Excluir(User.Identity.Name);

            context.Update(contratoOperadora);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var contratoOperadora = context.ContratoOperadora.FirstOrDefault(x => x.IdContratoOperadora == id);
            if (contratoOperadora == null)
                return BadRequest("Contrato da operadora não encontrado ");

            return Ok(new ContratoOperadoraResponse()
            {
                IdContratoOperadora = contratoOperadora.IdContratoOperadora,
                IdOperadora = contratoOperadora.IdOperadora,
                DataInicio = contratoOperadora.DataInicio.Date,
                DataTermino = contratoOperadora.DataTermino.Date,
                IdUnidade = contratoOperadora.IdUnidade,
                IdEmpresa = contratoOperadora.IdEmpresa,
                IdContaRecebimento = contratoOperadora.IdContaRecebimento,
                IdContaGravame = contratoOperadora.IdContaGravame,
                Situacao = contratoOperadora.Situacao
            });
        }
    }
}
