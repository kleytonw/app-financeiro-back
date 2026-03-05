using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TransacaoController(Context context) : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Transacao.Include(x => x.Categoria)
                .Include(x => x.Cartao).
                Include(x => x.Dependente)
               .Where(x => x.Situacao == "Ativo")
                   .AsNoTracking()
                   .Select(m => new
                   {
                       m.IdTransacao,
                       NomeCategoria = m.Categoria.Nome,
                       NomeCartao = m.Cartao.Nome,
                       NomeDependente = m.Dependente.Nome,
                       m.NumeroParcelas,
                       m.ParcelaAtual,
                       m.DataCompra,
                       m.Valor,
                       m.Descricao,
                       m.Situacao
                   }).Take(500).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("listarPorMes")]
        public IActionResult ListarPorMes(int mes, int ano)
        {
            var result = context.Transacao.Include(x => x.Categoria)
                .Include(x => x.Cartao).
                Include(x => x.Dependente)
               .Where(x => x.Situacao == "Ativo" && x.DataCompra.Month == mes && x.DataCompra.Year == ano) 
                   .AsNoTracking()
                   .Select(m => new
                   {
                       m.IdTransacao,
                       NomeCategoria = m.Categoria.Nome,
                       NomeCartao = m.Cartao.Nome,
                       NomeDependente = m.Dependente.Nome,
                       m.NumeroParcelas,
                       m.ParcelaAtual,
                       m.DataCompra,
                       m.Valor,
                       m.Descricao,
                       m.Situacao
                   }).Take(500).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("Salvar")]
        public IActionResult Salvar([FromBody] TransacaoRequest model)
        {
            Categoria categoria = context.Categoria.FirstOrDefault(x => x.IdCategoria == model.IdCategoria);
            if (categoria == null)
                return BadRequest("Categoria não encontrada");
            Cartao cartao = context.Cartao.FirstOrDefault(x => x.IdCartao == model.IdCartao);
            if (cartao == null)
                return BadRequest("Cartão não encontrado");
            Dependente dependente = context.Dependente.FirstOrDefault(x => x.IdDependente == model.IdDependente);
            if (dependente == null)
                return BadRequest("Dependente não encontrado");
            Transacao transacao;

            if (model.IdTransacao > 0)
            {
                transacao = context.Transacao.FirstOrDefault(x => x.IdTransacao == model.IdTransacao);
                if (transacao == null)
                    return BadRequest("Transação não encontrada");
                transacao.Alterar(
                    categoria,
                    cartao,
                    dependente,
                    model.NumeroParcelas,
                    model.ParcelaAtual,
                    model.DataCompra,
                    model.Valor,
                    model.Descricao,
                    User.Identity.Name);
                context.Update(transacao);

            }
            else
            {
                if (model.NumeroParcelas > 0)
                {
                    for (int i = 0; i < model.NumeroParcelas; i++)
                    {
                        var valorParcela = model.Valor / model.NumeroParcelas;
                        var dataCompraParcela = model.DataCompra.AddMonths(i);
                        var descricaoParcela = $"{model.Descricao} - Parcela {i + 1}/{model.NumeroParcelas}";
                        var transacaoParcela = new Transacao(
                            categoria,
                            cartao,
                            dependente,
                            model.NumeroParcelas,
                            i + 1,
                            dataCompraParcela,
                            valorParcela,
                            descricaoParcela,
                            User.Identity.Name);
                        context.Add(transacaoParcela);
                    }
                }

                else
                {
                    var transacaoParcela = new Transacao(
                           categoria,
                           cartao,
                           dependente,
                           model.NumeroParcelas,
                           model.ParcelaAtual,
                           model.DataCompra,
                           model.Valor,
                           model.Descricao,
                           User.Identity.Name);
                    context.Add(transacaoParcela);
                }

        
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var transacao = context.Transacao.FirstOrDefault(x => x.IdTransacao == id);
            if (transacao == null)
                return BadRequest("Transação não encontrada");
            transacao.Excluir(User.Identity.Name);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var transacao = context.Transacao.Include(x => x.Categoria)
                .Include(x => x.Cartao)
                .Include(x => x.Dependente)
                .FirstOrDefault(x => x.IdTransacao == id);
            if (transacao == null)
                return BadRequest("Transação não encontrada");
            return Ok(new TransacaoResponse
            {
                IdTransacao = transacao.IdTransacao,
                IdCategoria = transacao.IdCategoria,
                IdCartao = transacao.IdCartao,
                IdDependente = transacao.IdDependente,
                NumeroParcelas = transacao.NumeroParcelas,
                ParcelaAtual = transacao.ParcelaAtual,
                DataCompra = transacao.DataCompra,
                Valor = transacao.Valor,
                Descricao = transacao.Descricao,
                Situacao = transacao.Situacao
            });
        }
    }
}
