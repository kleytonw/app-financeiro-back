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
    public class ContratoOperadoraTaxaController : ControllerBase
    {
        protected Context context;

        public ContratoOperadoraTaxaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var result = context.ContratoOperadoraTaxa.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
                .Select(m => new {
                    m.IdContratoOperadoraTaxa,
                    m.ContratoOperadora.Operadora.NomeOperadora,
                    m.MeioPagamento.NomeMeioPagamento,
                    m.Taxa,
                    m.Valor,
                    m.ParcelaInicio,
                    m.ParcelaFim,
                    m.Bandeira.NomeBandeira,
                    m.Tipo,
                    m.Unidade.Nome,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("listarPorUnidade")]
        public IActionResult ListarPorUnidade(int idUnidade, int idContratoOperadora)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            if (usuarioLogado.TipoUsuario == "Administrador")
            {
                var result = context.ContratoOperadoraTaxa.Where(x =>  x.IdContratoOperadora == idContratoOperadora && x.IdUnidade == idUnidade)
                    .Select(m => new
                    {
                        m.IdContratoOperadoraTaxa,
                        m.ContratoOperadora.Operadora.NomeOperadora,
                        m.MeioPagamento.NomeMeioPagamento,
                        m.Taxa,
                        m.Valor,
                        m.ParcelaInicio,
                        m.ParcelaFim,
                        m.Bandeira.NomeBandeira,
                        m.Tipo,
                        m.Unidade.Nome,
                        m.Situacao
                    }).ToList();

                return Ok(result);
            }
            else
            {
                var result = context.ContratoOperadoraTaxa.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa && x.IdContratoOperadora == idContratoOperadora && x.IdUnidade == idUnidade)
                    .Select(m => new
                    {
                        m.IdContratoOperadoraTaxa,
                        m.ContratoOperadora.Operadora.NomeOperadora,
                        m.MeioPagamento.NomeMeioPagamento,
                        m.Taxa,
                        m.Valor,
                        m.ParcelaInicio,
                        m.ParcelaFim,
                        m.Bandeira.NomeBandeira,
                        m.Tipo,
                        m.Unidade.Nome,
                        m.Situacao
                    }).ToList();

                return Ok(result);
            }

        }

        [HttpGet]
        [Route("importar")]
        public IActionResult Importar(int idClasseTarifa, int idContratoOperadora)
        {
            var classeTarifaItem = context.ClasseTarifaItem.Include(x => x.MeioPagamento)
                .Include(x => x.Bandeira)
                .Where(x => x.IdClasseTarifa == idClasseTarifa).ToList();
            var contratoOperadora = context.ContratoOperadora.Include(x => x.Empresa)
                .Include(x => x.Unidade)
                .FirstOrDefault(x => x.IdContratoOperadora == idContratoOperadora);

            if (contratoOperadora == null)
                return BadRequest("Contrato da operadora não encontrado ");

            var contratoOperadoraTaxaLista = context.ContratoOperadoraTaxa
                .Include(x => x.MeioPagamento)
                .Include(x => x.Bandeira)
                .Where(x => x.IdContratoOperadora == idContratoOperadora).ToList();

            if(contratoOperadoraTaxaLista.Count > 0)
            {
                context.ContratoOperadoraTaxa.RemoveRange(contratoOperadoraTaxaLista);
                context.SaveChanges();
            }

            var contratoOperadoraTaxa = new List<ContratoOperadoraTaxa>();

            foreach (var item in classeTarifaItem)
            {
                contratoOperadoraTaxa.Add(new ContratoOperadoraTaxa(contratoOperadora,
                        item.MeioPagamento,
                        item.Bandeira,
                        item.Taxa ?? 0,
                        item.Valor ?? 0,
                        item.ParcelaInicio,
                        item.ParcelaFim,
                        item.Tipo,
                        contratoOperadora.Empresa,
                        contratoOperadora.Unidade,
                        User.Identity.Name));
                
                                                                 
            }

            context.ContratoOperadoraTaxa.AddRange(contratoOperadoraTaxa);
            context.SaveChanges();

            return Ok();
        }


        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var query = context.ContratoOperadoraTaxa.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa).AsQueryable();

            if(model.IdUnidade != 0)
            {
                query = query.Where(x => x.IdUnidade == model.IdUnidade);
            }

            if(model.IdMeioPagamento != 0)
            {
                query = query.Where(x => x.IdMeioPagamento == model.IdMeioPagamento);
            }

          var result = query.Select(m => new
           {
                m.IdContratoOperadoraTaxa,
                m.ContratoOperadora.Operadora.NomeOperadora,
                m.MeioPagamento.NomeMeioPagamento,
                m.Taxa,
                m.Valor,
                m.ParcelaInicio,
                m.ParcelaFim,
                m.Bandeira.NomeBandeira,
                m.Tipo,
                m.Unidade.Nome,
                m.IdUnidade,
                m.Situacao
            }).ToList();

            return Ok(result);



        }


        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ContratoOperadoraTaxaRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            Empresa empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);

            ContratoOperadora contratoOperadora = context.ContratoOperadora
                .FirstOrDefault(x => x.IdContratoOperadora == model.IdContratoOperadora);
            MeioPagamento meioPagamento = context.MeioPagamento
                .FirstOrDefault(x => x.IdMeioPagamento == model.IdMeioPagamento);
            Unidade unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);
            Bandeira bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == model.IdBandeira);
            if (model.IdContratoOperadoraTaxa > 0)
            {
                var contratoOperadoraTaxa = context.ContratoOperadoraTaxa
                    .FirstOrDefault(x => x.IdContratoOperadoraTaxa == model.IdContratoOperadoraTaxa);

                if (contratoOperadoraTaxa == null)
                    return BadRequest("Contrato da operadora não encontrado ");

                contratoOperadoraTaxa.Alterar(contratoOperadora, meioPagamento, bandeira, model.Taxa, model.Valor,
                    model.ParcelaInicio, model.ParcelaFim, model.Tipo, empresa, unidade, User.Identity.Name);

                context.SaveChanges();
                return Ok();
            }
            else
            {
                if (meioPagamento.NomeMeioPagamento != "Cartão de crédito")
                {
                    bool pixExiste = context.ContratoOperadoraTaxa.Any(x =>
                        x.IdBandeira == model.IdBandeira &&
                        x.IdMeioPagamento == model.IdMeioPagamento &&
                        x.IdUnidade == model.IdUnidade &&
                        x.IdContratoOperadora == model.IdContratoOperadora
                    );

                    if (pixExiste)
                    {
                        return BadRequest("Já existe uma taxa cadastrada com essa para  esse Contrato.");
                    }
                }
                else
                {
                    bool taxaExiste = context.ContratoOperadoraTaxa.Any(x =>
                        x.IdContratoOperadora == model.IdContratoOperadora &&
                        x.IdMeioPagamento == model.IdMeioPagamento &&
                        x.IdBandeira == model.IdBandeira &&
                        x.IdUnidade == model.IdUnidade &&
                        x.ParcelaInicio <= model.ParcelaFim &&
                        x.ParcelaFim >= model.ParcelaInicio
                    );

                    if (taxaExiste)
                    {
                        return BadRequest("Já existe uma taxa cadastrada para o intervalo de parcelas informado.");
                    }
                }

                ContratoOperadoraTaxa contratoOperadoraTaxa;

                if (meioPagamento.NomeMeioPagamento != "Cartão de crédito")
                {
                    contratoOperadoraTaxa = new ContratoOperadoraTaxa(
                        contratoOperadora,
                        meioPagamento,
                        bandeira,
                        model.Taxa,
                        model.Valor,
                        null,
                        null,
                        null,
                        empresa,
                        unidade,
                        User.Identity.Name
                    );
                }
                else
                {

                    contratoOperadoraTaxa = new ContratoOperadoraTaxa(
                        contratoOperadora,
                        meioPagamento,
                        bandeira,
                        model.Taxa,
                        model.Valor,
                        model.ParcelaInicio,
                        model.ParcelaFim,
                        model.Tipo,
                        empresa,
                        unidade,
                        User.Identity.Name
                    );
                }


                context.ContratoOperadoraTaxa.Add(contratoOperadoraTaxa);
            }
            context.SaveChanges();
            return Ok();
        }



        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var contratoOperadoraTaxa = context.ContratoOperadoraTaxa.FirstOrDefault(x => x.IdContratoOperadoraTaxa == id);
            contratoOperadoraTaxa.Excluir(User.Identity.Name);

            context.Update(contratoOperadoraTaxa);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("deletar")]
        public IActionResult Deletar(int idContratoOperadoraTaxa)
        {
            var contratoOperadoraTaxa = context.ContratoOperadoraTaxa.FirstOrDefault(x => x.IdContratoOperadoraTaxa == idContratoOperadoraTaxa);
            contratoOperadoraTaxa.Excluir(User.Identity.Name);

            context.Remove(contratoOperadoraTaxa);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var contratoOperadoraTaxa = context.ContratoOperadoraTaxa.FirstOrDefault(x => x.IdContratoOperadoraTaxa == id);
            if (contratoOperadoraTaxa == null)
                return BadRequest("Contrato da operadora não encontrado ");

            return Ok(new ContratoOperadoraTaxaResponse()
            {
                IdContratoOperadoraTaxa = contratoOperadoraTaxa.IdContratoOperadora,
                IdContratoOperadora = contratoOperadoraTaxa.IdContratoOperadora,
                IdMeioPagamento = contratoOperadoraTaxa.IdMeioPagamento,
                IdBandeira = contratoOperadoraTaxa.IdBandeira,
                Taxa = contratoOperadoraTaxa.Taxa,
                Valor = contratoOperadoraTaxa.Valor,
                ParcelaInicio = contratoOperadoraTaxa.ParcelaFim,
                ParcelaFim = contratoOperadoraTaxa.ParcelaFim,
                Tipo = contratoOperadoraTaxa.Tipo,
                IdUnidade = contratoOperadoraTaxa.IdUnidade,
                Situacao = contratoOperadoraTaxa.Situacao
            });
        }
    }
}

