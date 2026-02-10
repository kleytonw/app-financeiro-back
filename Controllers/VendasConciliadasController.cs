using ERP.Models;
using ERP_API.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System;
using ERP.Infra;
using System.Linq;
using ERP_API.Models;
using ERP_API.Service.Parceiros.Interface;
using System.Threading.Tasks;
using ERP_API.Infrastructure.Mapping;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class VendasConciliadasController : ControllerBase
    {
        protected Context context;
        protected IConciliadoraService conciliadoraService;
        public VendasConciliadasController(Context context, IConciliadoraService _conciliadoraService)
        {
            this.context = context;
            this.conciliadoraService = _conciliadoraService;
        }


        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.VendasConciliadas
                  .Select(m => new
                  {
                      m.IdVendasConciliadas,
                      m.IdentificadorConciliadora,
                      m.DataInicial,
                      m.DataFinal,
                      m.Versao,
                      m.Lote,
                      m.NomeSistema,
                      m.Produto,
                      m.DescricaoTipoProduto,
                      m.CodigoAutorizacao,
                      m.IdentificadorPagamento,
                      m.DataVenda,
                      m.DataVencimento,
                      m.ValorVendaParcela,
                      m.ValorLiquidoParcela,
                      m.TotalVenda,
                      m.Taxa,
                      m.Parcela,
                      m.TotalParcelas,
                      m.ValorBrutoMoeda,
                      m.ValorLiquidoMoeda,
                      m.CotacaoMoeda,
                      m.Moeda,
                      m.NSU,
                      m.TID,
                      m.Terminal,
                      m.MeioCaptura,
                      m.Operadora,
                      m.Modalidade,
                      m.Status,
                      m.Observacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarVendaConciliadaRequest model)
        {
            var query = context.VendasConciliadas
                .AsNoTracking()
                .Where(x => x.DataVenda >= model.DataInicio && x.DataVenda <= model.DataFim)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.NSU))
            {
                query = query.Where(x => x.NSU == model.NSU);
            }

            if (!string.IsNullOrEmpty(model.IdentificadorConciliadora))
            {
                query = query.Where(x => x.IdentificadorConciliadora == model.IdentificadorConciliadora);
            }

            if (!string.IsNullOrEmpty(model.MeioCaptura))
            {
                query = query.Where(x => x.MeioCaptura == model.MeioCaptura);
            }

            var totais = query
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalConciliado = g.Count(x => x.Status == "Conciliada"),
                    TotalNaoConciliado = g.Count(x => x.Status == "Não Conciliada"),
                    TotalPendente = g.Count(x => x.Status == "Pendente")
                })
                .FirstOrDefault() ?? new { TotalConciliado = 0, TotalNaoConciliado = 0, TotalPendente = 0 };

            var result = query
                .OrderByDescending(x => x.DataVenda)
                .Skip(model.Skip)
                .Take(model.Take)
                .Select(m => new
                {
                    m.IdVendasConciliadas,
                    m.IdentificadorConciliadora,
                    m.DataInicial,
                    m.DataFinal,
                    m.Versao,
                    m.Lote,
                    m.NomeSistema,
                    m.Produto,
                    m.DescricaoTipoProduto,
                    m.CodigoAutorizacao,
                    m.IdentificadorPagamento,
                    m.DataVenda,
                    m.DataVencimento,
                    m.ValorVendaParcela,
                    m.ValorLiquidoParcela,
                    m.TotalVenda,
                    m.Taxa,
                    m.Parcela,
                    m.TotalParcelas,
                    m.ValorBrutoMoeda,
                    m.ValorLiquidoMoeda,
                    m.CotacaoMoeda,
                    m.Moeda,
                    m.NSU,
                    m.TID,
                    m.Terminal,
                    m.MeioCaptura,
                    m.Operadora,
                    m.Modalidade,
                    m.Status,
                    m.Observacao,
                    m.ValorBrutoConciliadora
                })
                .ToList();

            return Ok(new
            {
                result,
                totais
            });
        }

        [HttpPost]
        [Route("conciliar")]
        [Authorize]
        public async Task<IActionResult> Conciliar(PesquisarConciliacaoBancariaRequest request)
        {
            var vendasErp = await context.VendasConciliadas
                .Where(x => x.IdentificadorConciliadora == request.IdentificadorConciliadora
                         && x.DataVenda.Date >= request.DataInicio
                         && x.DataVenda.Date <= request.DataFim.Date)
                .ToListAsync();

            var apiKey = await context.Cliente
                .Where(x => x.IdentificadorConciliadora == request.IdentificadorConciliadora)
                .Select(x => x.ApiKeyConciliadora)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("Conciliadora não configurada para o cliente.");
            }

            var (success, errorMessage, data) = await conciliadoraService.Vendas(
                request.IdentificadorConciliadora,
                request.DataInicio,
                request.DataFim.AddDays(1),
                apiKey,
                null, null, null, null, null, null);

            if (!success || data?.Value == null)
            {
                return BadRequest($"Erro ao buscar vendas da conciliadora: {errorMessage}");
            }

            var conciliacoesUsadas = new HashSet<string>();
            var vendasParaAtualizar = new List<VendasConciliadas>();
            int totalConciliadas = 0;
            int totalNaoConciliadas = 0;

            foreach (var item in vendasErp)
            {
                var conciliacao = data.Value.FirstOrDefault(vc =>
                    vc.Nsu == item.NSU &&
                    vc.DataVenda.Date == item.DataVenda.Date
                );

                if (conciliacao == null)
                {
                    if (item.Status != "Não Conciliada")
                    {
                        item.Conciliar(item.NSU, 0, 0);
                        vendasParaAtualizar.Add(item);
                        totalNaoConciliadas++;
                    }
                    continue;
                }

                item.Conciliar(conciliacao.Nsu, conciliacao.ValorBruto, conciliacao.AdqId);
                conciliacoesUsadas.Add($"{conciliacao.Nsu}_{conciliacao.DataVenda.Date:yyyyMMdd}_{conciliacao.ValorBruto}_{conciliacao.AdqId}");
                vendasParaAtualizar.Add(item);

                if (item.Status == "Conciliada")
                    totalConciliadas++;
                else
                    totalNaoConciliadas++;
            }

            if (vendasParaAtualizar.Any())
            {
                await context.BulkUpdateAsync(vendasParaAtualizar, new BulkConfig
                {
                    BatchSize = 1000,
                    SetOutputIdentity = false,
                    BulkCopyTimeout = 180
                });
            }

            return Ok(new
            {
                message = $"Conciliação finalizada. {totalConciliadas} conciliadas, {totalNaoConciliadas} não conciliadas.",
                totalProcessadas = vendasParaAtualizar.Count,
                totalConciliadas,
                totalNaoConciliadas,
                totalVendasAnalisadas = vendasErp.Count
            });
        }

        [HttpPost]
        [Route("importar")]
        [Authorize]
        public IActionResult ImportarExcel([FromForm] IFormFile excelFile, [FromForm] string identificadorConciliadora)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("Arquivo inválido");
            }

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    var vendas = new List<VendasConciliadas>();
                    var datasVenda = new HashSet<DateTime>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (DateTime.TryParse(worksheet.Cells[row, 10].Text, out var tempDataVenda))
                        {
                            datasVenda.Add(tempDataVenda.Date);
                        }
                    }

                    var minData = datasVenda.Min();
                    var maxData = datasVenda.Max();

                    var vendasExistentes = context.VendasConciliadas
                        .Where(x => x.DataVenda.Date >= minData &&
                                    x.DataVenda.Date <= maxData &&
                                    x.IdentificadorConciliadora == identificadorConciliadora)
                        .ToList();

                    if (vendasExistentes.Any())
                    {
                        context.Set<VendasConciliadas>().RemoveRange(vendasExistentes);
                        context.SaveChanges();
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text) &&
                            string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Text) &&
                            string.IsNullOrWhiteSpace(worksheet.Cells[row, 10].Text))
                        {
                            break;
                        }

                        try
                        {
                            DateTime dataInicial = DateTime.Parse(worksheet.Cells[row, 1].Text);
                            DateTime dataFinal = DateTime.Parse(worksheet.Cells[row, 2].Text);
                            var versao = worksheet.Cells[row, 3].Text;
                            var lote = int.Parse(worksheet.Cells[row, 4].Text.Trim().ToLower());
                            var nomeSistema = worksheet.Cells[row, 5].Text;
                            var produto = int.Parse(worksheet.Cells[row, 6].Text);
                            var descricaoTipoProduto = worksheet.Cells[row, 7].Text;
                            var codigoAutorizacao = worksheet.Cells[row, 8].Text;
                            var identificadorPagamento = worksheet.Cells[row, 9].Text;
                            DateTime dataVenda = DateTime.Parse(worksheet.Cells[row, 10].Text);
                            DateTime? dataVencimento = DateTime.TryParse(worksheet.Cells[row, 11].Text, out var tempDataVencimento) ? tempDataVencimento : null;

                            var valorVendaParcelaString = Regex.Replace(worksheet.Cells[row, 12].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorVendaParcelaString.Contains(",") && valorVendaParcelaString.Contains("."))
                            {
                                valorVendaParcelaString = valorVendaParcelaString.Replace(".", "");
                                valorVendaParcelaString = valorVendaParcelaString.Replace(",", ".");
                            }
                            else if (valorVendaParcelaString.Contains(","))
                            {
                                valorVendaParcelaString = valorVendaParcelaString.Replace(",", ".");
                            }
                            var valorVendaParcela = decimal.TryParse(valorVendaParcelaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorVendaParcela)
                                ? tempValorVendaParcela
                                : 0m;

                            var valorVendaLiquidaParcelaString = Regex.Replace(worksheet.Cells[row, 13].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorVendaLiquidaParcelaString.Contains(",") && valorVendaLiquidaParcelaString.Contains("."))
                            {
                                valorVendaLiquidaParcelaString = valorVendaLiquidaParcelaString.Replace(".", "");
                                valorVendaLiquidaParcelaString = valorVendaLiquidaParcelaString.Replace(",", ".");
                            }
                            else if (valorVendaLiquidaParcelaString.Contains(","))
                            {
                                valorVendaLiquidaParcelaString = valorVendaLiquidaParcelaString.Replace(",", ".");
                            }
                            var valorVendaLiquidaParcela = decimal.TryParse(valorVendaLiquidaParcelaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorVendaLiquidaParcela)
                                ? tempValorVendaLiquidaParcela
                                : 0m;

                            var valorTotalVendaParcelaString = Regex.Replace(worksheet.Cells[row, 14].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorTotalVendaParcelaString.Contains(",") && valorTotalVendaParcelaString.Contains("."))
                            {
                                valorTotalVendaParcelaString = valorTotalVendaParcelaString.Replace(".", "");
                                valorTotalVendaParcelaString = valorTotalVendaParcelaString.Replace(",", ".");
                            }
                            else if (valorTotalVendaParcelaString.Contains(","))
                            {
                                valorTotalVendaParcelaString = valorTotalVendaParcelaString.Replace(",", ".");
                            }
                            var valorTotalVendaParcela = decimal.TryParse(valorTotalVendaParcelaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorTotalVendaLiquidaParcela)
                                ? tempValorTotalVendaLiquidaParcela
                                : 0m;

                            var taxaString = Regex.Replace(worksheet.Cells[row, 15].Text, @"[^\d.,-]", "").Trim();
                            taxaString = taxaString.Replace(",", ".");
                            var taxa = (decimal.TryParse(taxaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempTaxa)
                                ? tempTaxa
                                : (decimal?)null);

                            var parcela = int.Parse(worksheet.Cells[row, 16].Text.Trim().ToLower());
                            var totalParcelas = int.Parse(worksheet.Cells[row, 17].Text.Trim().ToLower());

                            var valorBrutoMoedaString = Regex.Replace(worksheet.Cells[row, 18].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorBrutoMoedaString.Contains(",") && valorBrutoMoedaString.Contains("."))
                            {
                                valorBrutoMoedaString = valorBrutoMoedaString.Replace(".", "");
                                valorBrutoMoedaString = valorBrutoMoedaString.Replace(",", ".");
                            }
                            else if (valorBrutoMoedaString.Contains(","))
                            {
                                valorBrutoMoedaString = valorBrutoMoedaString.Replace(",", ".");
                            }
                            var valorBrutoMoeda = decimal.TryParse(valorBrutoMoedaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorBrutoMoeda)
                                ? tempValorBrutoMoeda
                                : (decimal?)null;

                            var valorLiquidoMoedaString = Regex.Replace(worksheet.Cells[row, 19].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorLiquidoMoedaString.Contains(",") && valorLiquidoMoedaString.Contains("."))
                            {
                                valorLiquidoMoedaString = valorLiquidoMoedaString.Replace(".", "");
                                valorLiquidoMoedaString = valorLiquidoMoedaString.Replace(",", ".");
                            }
                            else if (valorLiquidoMoedaString.Contains(","))
                            {
                                valorLiquidoMoedaString = valorLiquidoMoedaString.Replace(",", ".");
                            }
                            var valorLiquidoMoeda = decimal.TryParse(valorLiquidoMoedaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorLiquidoMoeda)
                                ? tempValorLiquidoMoeda
                                : (decimal?)null;

                            var cotacaoMoedaString = Regex.Replace(worksheet.Cells[row, 20].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (cotacaoMoedaString.Contains(",") && cotacaoMoedaString.Contains("."))
                            {
                                cotacaoMoedaString = cotacaoMoedaString.Replace(".", "");
                                cotacaoMoedaString = cotacaoMoedaString.Replace(",", ".");
                            }
                            else if (cotacaoMoedaString.Contains(","))
                            {
                                cotacaoMoedaString = cotacaoMoedaString.Replace(",", ".");
                            }
                            var cotacaoMoeda = decimal.TryParse(cotacaoMoedaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempCotacaoMoeda)
                                ? tempCotacaoMoeda
                                : (decimal?)null;

                            var moeda = worksheet.Cells[row, 21].Text;
                            var nSU = worksheet.Cells[row, 22].Text;
                            var tID = worksheet.Cells[row, 23].Text;
                            var terminal = worksheet.Cells[row, 24].Text;
                            var meioCaptura = worksheet.Cells[row, 25].Text;
                            var operadora = int.Parse(worksheet.Cells[row, 26].Text);
                            var modalidade = worksheet.Cells[row, 27].Text;

                            var venda = new VendasConciliadas(identificadorConciliadora, dataInicial, dataFinal, versao, lote, nomeSistema, produto, descricaoTipoProduto, codigoAutorizacao, identificadorPagamento,
                                                              dataVenda, dataVencimento, valorVendaParcela, valorVendaLiquidaParcela, valorTotalVendaParcela, taxa, parcela, totalParcelas, valorBrutoMoeda, valorLiquidoMoeda, cotacaoMoeda, moeda, nSU, tID, terminal, meioCaptura, operadora, modalidade, "Não Conciliada", User.Identity.Name);

                            vendas.Add(venda);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.VendasConciliadas.AddRange(vendas);
                    context.SaveChanges();
                }
            }
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var venda = context.VendasConciliadas.FirstOrDefault(x => x.IdVendasConciliadas == id);
            if (venda == null)
                return BadRequest("Venda não encontrado ");

            return Ok(new VendasConciliadasResponse()
            {
                IdVendasConciliadas = venda.IdVendasConciliadas,
                IdentificadorConciliadora = venda.IdentificadorConciliadora,
                DataInicial = venda.DataInicial,
                DataFinal = venda.DataFinal,
                Versao = venda.Versao,
                Lote = venda.Lote,
                NomeSistema = venda.NomeSistema,
                Produto = venda.Produto,
                DescricaoTipoProduto = venda.DescricaoTipoProduto,
                CodigoAutorizacao = venda.CodigoAutorizacao,
                IdentificadorPagamento = venda.IdentificadorPagamento,
                DataVenda = venda.DataVenda,
                DataVencimento = venda.DataVencimento,
                ValorVendaParcela = venda.ValorVendaParcela,
                ValorLiquidoParcela = venda.ValorLiquidoParcela,
                TotalVenda = venda.TotalVenda,
                Taxa = venda.Taxa,
                Parcela = venda.Parcela,
                TotalParcelas = venda.TotalParcelas,
                ValorBrutoMoeda = venda.ValorBrutoMoeda,
                ValorLiquidoMoeda = venda.ValorLiquidoMoeda,
                CotacaoMoeda = venda.CotacaoMoeda,
                Moeda = venda.Moeda,
                NSU = venda.NSU,
                TID = venda.TID,
                Terminal = venda.Terminal,
                MeioCaptura = venda.MeioCaptura,
                Operadora = venda.Operadora,
                Modalidade = venda.Modalidade,
            });
        }
    }
}
