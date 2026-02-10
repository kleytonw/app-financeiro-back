using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System;
using ERP.Infra;
using ERP_API.Service.Parceiros.Interface;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ERP_API.Service.Parceiros;
using System.Linq;
using ERP_API.Models;




using System.Collections.Generic;
using ERP.Domain.Entidades;
using System.Data.Entity;
using ERP_API.Domain.Entidades;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PagSeguroController : ControllerBase
    {
        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        private readonly IConfiguration _configuration;
        protected Context context;
        private readonly IPagBankService _pagBankService;

        public PagSeguroController(Context context, IPagBankService pagBankService, IConfiguration configuration)
        {
            this.context = context;
            _configuration = configuration;
            _pagBankService = pagBankService;

        }

        [HttpPost]
        [Route("buscar-transacao")]
        [AllowAnonymous]
        public async Task<IActionResult> BuscarTransacao([FromBody] IntegrarTransacaoModelRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);   
            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == model.IdOperadora);
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);
            var unidades = new List<Unidade>();

            if (model.IdUnidade == 0)
            {
                unidades = context.Unidade.Include(x => x.Empresa)
                    .Where(x => x.Situacao == "Ativo")
                    .ToList();
            }
            else
            {
                unidades = context.Unidade.Include(x => x.Empresa)
                    .Where(x => x.IdUnidade == model.IdUnidade && x.Situacao == "Ativo")
                    .ToList();
            }

            DateTime dataInicio = model.DataInicio;
            DateTime dataTermino = model.DataTermino;

            if (dataInicio > dataTermino)
            {
                return BadRequest("Data Início não pode ser maior que a Data Término");
            }
            if (dataInicio != dataTermino)
            {
                return BadRequest("Data Início e Data Término devem ser iguais para buscar transações na PagSeguro");
            }

            var transacoesExistentes = context.Transacao
                .Where(t => t.DataMovimentacao.Value.Date >= dataInicio.Date &&
                            t.DataMovimentacao.Value.Date <= dataTermino.Date &&
                            t.NomeOperadora == "PagSeguro")
                .ToList();

            if (transacoesExistentes.Any())
            {
                context.Transacao.RemoveRange(transacoesExistentes);
                await context.SaveChangesAsync();
            }

            foreach (var unidade in unidades)
            {
                var unidadeParametro = context.UnidadeParametro
                  .Where(x => x.IdUnidade == unidade.IdUnidade &&
                              x.IdOperadora == operadora.IdOperadora)
                  .ToList();

                decimal? totalBruto = 0;
                decimal? totalDespesa = 0;
                decimal? totalLiquido = 0;

                var token = unidadeParametro.FirstOrDefault(x => x.Chave == "Token")?.Valor;
                if (string.IsNullOrEmpty(token))
                    continue;
                var numeroEstabelecimento = unidadeParametro.FirstOrDefault(x => x.Chave == "NumeroEstabelecimento")?.Valor;
                if (string.IsNullOrEmpty(numeroEstabelecimento))
                    continue;
                var numeroPagina = 1;
                var totalPaginas = 1;
                var todasTransacoes = new List<Transacao>();

                ConsultaTransacaoPagBankResponseModel responseTransacao;
                var transacoesPagina = new List<Transacao>();

                do
                {
                    var transacoesPagBank = new ConsultaPagBankRequestModel
                    {
                        DataConsulta = dataInicio,
                        PageNumber = numeroPagina,
                        PageSize = 1000,
                        Token = token,
                    };
                    try
                    {
                        responseTransacao = await _pagBankService.ConsultaTransacaoPagBankAsync(transacoesPagBank);
                        totalPaginas = responseTransacao.Pagination.TotalPages;
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"Erro ao consultar transacao: {ex.Message}");
                    }

                    if (responseTransacao.Detalhes != null && responseTransacao.Detalhes.Any())
                    {

                        foreach (var transaction in responseTransacao.Detalhes)
                        {
                            /*var transacao = new Transacao(
                                unidade.Nome,
                                unidade,
                                empresa,
                                transaction.ValorTotalTransacao,
                                ((transaction.TaxaIntermediacao + transaction.TarifaIntermediacao)/transaction.ValorTotalTransacao),
                                (transaction.TaxaIntermediacao + transaction.TarifaIntermediacao),
                                transaction.ValorLiquidoTransacao,
                                transaction.DataVendaAjuste,
                                transaction.DataInicialTransacao,
                                MeioPagamentoDescricao(transaction.ArranjoUr),
                                transaction.InstituicaoFinanceira,
                                operadora,
                                operadora.NomeOperadora,
                                "Não conciliado",
                                transaction.Nsu,
                                int.Parse(transaction.QuantidadeParcelas),
                                int.Parse(transaction.CodigoVenda),
                                transaction.CodigoTransacao,
                                //Vem como inteiro
                                transaction.StatusPagamento,
                                null,
                                null,
                                //Vem como valor inteiro
                                transaction.MeioCaptura,
                                null,
                                null,
                                //Valor total da taxa
                                transaction.TarifaIntermediacao + transaction.TaxaIntermediacao,
                                transaction.Tid,
                                null,
                                null,
                                //Numero do cartao?
                                transaction.CartaoBin,
                                null,
                                null,
                                null,
                                User.Identity.Name);*/

                            //transacoesPagina.Add(transacao);
                        }

                        todasTransacoes.AddRange(transacoesPagina);
                        await context.SaveChangesAsync();

                        numeroPagina++;
                    }
                } while (numeroPagina <= totalPaginas);

                if (todasTransacoes.Any())
                {
                    context.Transacao.AddRange(todasTransacoes);
                    await context.SaveChangesAsync();
                }
            }

            return Ok("Transações buscadas com sucesso");
        }

        [NonAction]
        public string MeioPagamentoDescricao(string meioPgamento)
        {
            if (meioPgamento.Contains("_"))
            {
                meioPgamento = meioPgamento.Split('_')[0];
                return meioPgamento;
            }
            else
                return meioPgamento;
        }

        [HttpPost]
        [Route("pesquisarConsultarPagamento")]
        public async Task<IActionResult> PesquisarConsultarPagamento([FromBody] PesquisarConsultaPagamento model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);

            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == model.IdOperadora);
            if (operadora == null)
                return BadRequest("Adquirente sem cadastro ativo no contrato");
            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);
            if (unidade == null || model.IdUnidade == 0)
                return BadRequest("Unidade não encontrada");
            var unidadeParametro = context.UnidadeParametro
                .Where(x => x.IdUnidade == unidade.IdUnidade &&
                            x.IdOperadora == operadora.IdOperadora);
            if (!unidadeParametro.Any())
                return BadRequest("Parametros da unidade não encontrado");
            var token = unidadeParametro.FirstOrDefault(x => x.Chave == "Token")?.Valor;
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token não encontrado");
            var numeroEstabelecimento = unidadeParametro.FirstOrDefault(x => x.Chave == "NumeroEstabelecimento")?.Valor;
            if (string.IsNullOrEmpty(numeroEstabelecimento))
                return BadRequest("Número do estabelecimento não encontrado");
            if (model.DataInicio > model.DataTermino)
                return BadRequest("Data Início não pode ser maior que a Data Término");
            if (model.DataInicio != model.DataTermino)
                return BadRequest("Data Início e Data Término devem ser iguais para buscar transações na PagSeguro");
            var numeroPagina = 1;
            var totalPaginas = 1;

            var pagamentos = new List<Pagamento>();

            do
            {
                var pagamentosPagBank = new ConsultaPagBankRequestModel
                {
                    DataConsulta = model.DataInicio,
                    PageNumber = numeroPagina,
                    PageSize = 1000,
                    Token = token,
                };

                ConsultaPagamentoPagBankResponseModel responsePagamento;
                try
                {
                    responsePagamento = await _pagBankService.ConsultaPagamentoPagBankAsync(pagamentosPagBank);

                }

                catch (Exception ex)
                {
                    return BadRequest($"Erro ao consultar pagamento: {ex.Message}");
                }

                var pagamentosExistentes = context.Pagamento
                    .Where(t => t.DataPagamento.Value.Date >= model.DataInicio.Date &&
                                t.DataPagamento.Value.Date <= model.DataTermino.Date &&
                                t.IdUnidade == model.IdUnidade &&
                                t.IdOperadora == model.IdOperadora)
                    .ToList();

                if (pagamentosExistentes.Any())
                {
                    context.Pagamento.RemoveRange(pagamentosExistentes);
                    await context.SaveChangesAsync();
                }

                if (responsePagamento.Detalhes != null && responsePagamento.Detalhes.Any())
                {

                    foreach (var payment in responsePagamento.Detalhes)
                    {
                        //Aparentemente instuição financeira é diferente de bandeira
                        var bandeira = context.Bandeira.FirstOrDefault(x => x.NomeBandeira == payment.Instituicao_financeira);
                        if (bandeira == null)
                            return BadRequest("Bandeira não encontrada");
                        var banco = context.Banco.FirstOrDefault(x => x.NomeBanco == payment.Instituicao_financeira);
                        if (banco == null)
                            return BadRequest("Banco não encontrado");

                        /*var pagamento = new Pagamento(operadora,
                                                      empresa,
                                                      unidade,
                                                      DateTime.Parse(payment.Data_inicial_transacao,
                                                      banco,
                                                      banco.NomeBanco,
                                                      //Sem agência no retorno
                                                      //Sem numero da conta no retorno
                                                      //
                                                      bandeira,
                                                      //Não tem codigo da bandeira,
                                                      //Nome da Bandeira?
                                                      payment.Instituicao_financeira,
                                                      unidade.Nome,
                                                      payment.Valor_total_transacao,
                                                      //Vem como valor interio
                                                      payment.Status_pagamento,
                                                      payment.Arranjo_ur,
                                                      User.Identity.Name);
                        pagamentos.Add(pagamento);*/
                    }

                    context.Pagamento.AddRange(pagamentos);
                }

                numeroPagina++;
            } while (numeroPagina <= totalPaginas);

            if (pagamentos.Any())
            {
                await context.SaveChangesAsync();
            }

            var retornoPagamentos = context.Pagamento.
                                    Where(x => x.IdUnidade == model.IdUnidade &&
                                    x.DataPagamento.Value.Date >= model.DataInicio.Date &&
                                    x.DataPagamento.Value.Date <= model.DataTermino.Date).AsQueryable().Select(m => new
                                    {
                                        m.IdPagamento,
                                        m.IdEmpresa,
                                        m.IdOperadora,
                                        m.IdUnidade,
                                        m.IdDiagnostico,
                                        m.DataPagamento,
                                        m.IdBanco,
                                        m.IdBandeira,
                                        m.ValorPagamento,
                                        m.StatusPagamento,
                                        m.TipoPagamento,
                                        m.StatusConciliado,
                                        m.Banco.NomeBanco,
                                        m.Agencia,
                                        m.Conta,
                                        m.Bandeira.NomeBandeira,
                                        m.RazaoSocial
                                    }).ToList();

            var groupedNetAmount = retornoPagamentos
                   .GroupBy(p => p.StatusPagamento)
                    .Select(g => new
                    {
                        Status = g.Key,
                        TotalNetAmount = g.Sum(p => p.ValorPagamento)
                    })
                    .ToList();

            var typedNetAmount = retornoPagamentos
                  .GroupBy(p => p.TipoPagamento)
                   .Select(g => new
                   {
                       Type = g.Key,
                       TotalNetAmount = g.Sum(p => p.ValorPagamento)
                   })
                   .ToList();

            var response = new
            {
                Pagamentos = retornoPagamentos,
                TotaisPorStatus = groupedNetAmount,
                TotaisPorType = typedNetAmount
            };

            return Ok(response);
        }
    }
}
