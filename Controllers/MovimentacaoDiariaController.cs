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
using System.Data.Entity;
using ERP_API.Models;
using ERP_API.Domain;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class MovimentacaoDiariaController : ControllerBase
    {
        protected Context context;
        public MovimentacaoDiariaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.Include(x => x.Cliente.Pessoa).FirstOrDefault(x => x.Login == User.Identity.Name);

            var result = context.MovimentacaoDiaria
                  .Select(m => new
                  {
                     m.IdMovimentacaoDiaria,
                     m.IdCliente,
                     m.TipoMovimentacao,
                     m.DataMovimentacao,
                     m.FornecedorCliente,
                     m.CpfCnpjFornecedorCliente,
                     m.NotaFiscal,
                     m.Produto,
                     m.Categoria,
                     m.SKU,
                     m.NCM,
                     m.CFOP,
                     m.Quantidade,
                     m.UnidadeMedida,
                     m.ValorUnitario,
                     m.ValorDesconto,
                     m.ValorTotal,
                     m.CodigoBarras,
                     m.CMV_Aquisicao,
                     m.CMV_Contabil,
                     m.CMV_Tributos,
                     m.CMV_Total,
                     m.Promocao,
                     m.Margem,
                     m.Observacao,
                  }).ToList();
            return Ok(result);
        }


        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarMovimentacaoDiariaRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);


            var query = context.MovimentacaoDiaria.Where(x => x.DataMovimentacao >= model.DataInicio && x.DataMovimentacao <= model.DataFim);

            if(model.IdCliente != 0 && model.IdCliente != null)
            {
                query = query.Where(x => x.IdCliente == model.IdCliente);
            }

            var result = query.Select(m => new
            {
                m.IdMovimentacaoDiaria,
                m.IdCliente,
                m.TipoMovimentacao,
                m.DataMovimentacao,
                m.FornecedorCliente,
                m.CpfCnpjFornecedorCliente,
                m.NotaFiscal,
                m.Produto,
                m.Categoria,
                m.SKU,
                m.NCM,
                m.CFOP,
                m.Quantidade,
                m.UnidadeMedida,
                m.ValorUnitario,
                m.ValorDesconto,
                m.ValorTotal,
                m.CodigoBarras,
                m.CMV_Aquisicao,
                m.CMV_Contabil,
                m.CMV_Tributos,
                m.CMV_Total,
                m.Margem,
                m.Observacao,
            }).Take(1000).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var venda = context.Venda.FirstOrDefault(x => x.IdVenda == id && x.IdEmpresa == usuarioLogado.IdEmpresa);
            venda.Excluir(User.Identity.Name);

            context.Update(venda);
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("importar")]
        [Authorize]
        public IActionResult ImportarExcel([FromForm] IFormFile excelFile)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

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


                    var movimentacoes = new List<MovimentacaoDiaria>();
                    var datasMovimentacao = new HashSet<DateTime>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (DateTime.TryParse(worksheet.Cells[row, 3].Text, out var tempDataVenda))
                        {
                            datasMovimentacao.Add(tempDataVenda);
                        }
                    }

                    var minData = datasMovimentacao.Min();
                    var maxData = datasMovimentacao.Max();

                    var movimentacoesExistentes = context.Venda
                        .Where(x => x.DataVenda >= minData &&
                                    x.DataVenda <= maxData)
                        .ToList();


                    if (movimentacoesExistentes.Any())
                    {
                        context.Set<Venda>().RemoveRange(movimentacoesExistentes);
                        context.SaveChanges();
                    }
                    for (int row = 2; row <= rowCount; row++)
                    {

                        try
                        {
                            var tipoMovimentacao = worksheet.Cells[row, 1].Text;
                            var idCliente = int.Parse(worksheet.Cells[row, 2].Text);

                            DateTime? tempDataMovimentacao = DateTime.TryParse(worksheet.Cells[row, 3].Text, out var parsedDataMovimentacao)
                                          ? parsedDataMovimentacao
                                          : (DateTime?)null;

                            var dataMovimentacao = tempDataMovimentacao.Value >= new DateTime(1753, 1, 1) ? tempDataMovimentacao.Value.Date : new DateTime(1753, 1, 1);


                            var fornecedorCliente = worksheet.Cells[row, 4].Text.Trim().ToLower();

                            var cpfCnpjFornecedorCliente = worksheet.Cells[row, 5].Text;
                            var notaFiscal = worksheet.Cells[row, 6].Text;
                            var produto = worksheet.Cells[row, 7].Text;
                            var categoria = worksheet.Cells[row, 8].Text;

                            var sku = worksheet.Cells[row, 9].Text;
                            var ncm = worksheet.Cells[row, 10].Text;
                            var cfop = worksheet.Cells[row, 11].Text;
                            var quantidade = int.Parse(worksheet.Cells[row, 12].Text.Trim());
                            var unidadeMedida = worksheet.Cells[row, 13].Text;

                            var valorUnitarioString = Regex.Replace(worksheet.Cells[row, 14].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorUnitarioString.Contains(",") && valorUnitarioString.Contains("."))
                            {

                                valorUnitarioString = valorUnitarioString.Replace(".", "");
                                valorUnitarioString = valorUnitarioString.Replace(",", ".");
                            }
                            else if (valorUnitarioString.Contains(","))
                            {

                                valorUnitarioString = valorUnitarioString.Replace(",", ".");
                            }
                            var valorUnitario = decimal.TryParse(valorUnitarioString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorUnitario)
                                ? tempValorUnitario
                                : 0m;

                            var valorDescontoString = Regex.Replace(worksheet.Cells[row, 14].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorDescontoString.Contains(",") && valorDescontoString.Contains("."))
                            {

                                valorDescontoString = valorDescontoString.Replace(".", "");
                                valorDescontoString = valorDescontoString.Replace(",", ".");
                            }
                            else if (valorDescontoString.Contains(","))
                            {

                                valorDescontoString = valorDescontoString.Replace(",", ".");
                            }
                            var valorDesconto = decimal.TryParse(valorDescontoString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorDesconto)
                                ? tempValorDesconto
                                : (decimal?)null;


                            var valorTotalString = Regex.Replace(worksheet.Cells[row, 15].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorTotalString.Contains(",") && valorTotalString.Contains("."))
                            {

                                valorTotalString = valorTotalString.Replace(".", "");
                                valorTotalString = valorTotalString.Replace(",", ".");
                            }
                            else if (valorTotalString.Contains(","))
                            {

                                valorTotalString = valorTotalString.Replace(",", ".");
                            }
                            var valorTotal = decimal.TryParse(valorTotalString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorTotal)
                                ? tempValorTotal
                                : (decimal?)null;

                            var codigoBarras = worksheet.Cells[row, 16].Text;

                            var cmvAquisicaoString = Regex.Replace(worksheet.Cells[row, 16].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (cmvAquisicaoString.Contains(",") && cmvAquisicaoString.Contains("."))
                            {

                                cmvAquisicaoString = cmvAquisicaoString.Replace(".", "");
                                cmvAquisicaoString = cmvAquisicaoString.Replace(",", ".");
                            }
                            else if (cmvAquisicaoString.Contains(","))
                            {

                                cmvAquisicaoString = cmvAquisicaoString.Replace(",", ".");
                            }

                            var cmvAquisicao = decimal.TryParse(cmvAquisicaoString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempCmvAquisicao)
                                ? tempCmvAquisicao
                                : (decimal?)null;

                            var cmvContabilString = Regex.Replace(worksheet.Cells[row, 17].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (cmvContabilString.Contains(",") && cmvContabilString.Contains("."))
                            {

                                cmvContabilString = cmvContabilString.Replace(".", "");
                                cmvContabilString = cmvContabilString.Replace(",", ".");
                            }
                            else if (cmvContabilString.Contains(","))
                            {

                                cmvContabilString = cmvContabilString.Replace(",", ".");
                            }
                            var cmvContabil = decimal.TryParse(cmvContabilString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempCmvContabil)
                                ? tempCmvContabil
                                : (decimal?)null;

                            var cmvTributosString = Regex.Replace(worksheet.Cells[row, 18].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (cmvTributosString.Contains(",") && cmvContabilString.Contains("."))
                            {

                                cmvTributosString = cmvTributosString.Replace(".", "");
                                cmvTributosString = cmvTributosString.Replace(",", ".");
                            }
                            else if (cmvTributosString.Contains(","))
                            {

                                cmvTributosString = cmvTributosString.Replace(",", ".");
                            }
                            var cmvTributos = decimal.TryParse(cmvTributosString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempCmvTribubtos)
                                ? tempCmvTribubtos
                                : (decimal?)null;


                            var cmvTotalString = Regex.Replace(worksheet.Cells[row, 19].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (cmvTotalString.Contains(",") && cmvTotalString.Contains("."))
                            {

                                cmvTotalString = cmvTotalString.Replace(".", "");
                                cmvTotalString = cmvTotalString.Replace(",", ".");
                            }
                            else if (cmvTotalString.Contains(","))
                            {

                                cmvTotalString = cmvTotalString.Replace(",", ".");
                            }
                            var cmvTotal = decimal.TryParse(cmvTotalString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempCmvTotal)
                                ? tempCmvTotal
                                : (decimal?)null;

                            var margemString = Regex.Replace(worksheet.Cells[row, 20].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (margemString.Contains(",") && margemString.Contains("."))
                            {

                                margemString = margemString.Replace(".", "");
                                margemString = margemString.Replace(",", ".");
                            }
                            else if (margemString.Contains(","))
                            {

                                margemString = margemString.Replace(",", ".");
                            }
                            var margem = decimal.TryParse(margemString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempMargem)
                                ? tempMargem
                                : (decimal?)null;


                            var observacao = worksheet.Cells[row, 21].Text.Trim();

                            bool promocao = false;

                            var promocaoString = worksheet.Cells[row, 22].Text;

                            if(promocaoString == "S")
                            {
                                 promocao = true;
                            }
                            else
                            {
                                 promocao = false;
                            }


                            var movimentacaoDiaria = new MovimentacaoDiaria(tipoMovimentacao, idCliente, dataMovimentacao, fornecedorCliente, cpfCnpjFornecedorCliente, notaFiscal, produto, categoria, sku,
                                                              ncm, cfop, quantidade, unidadeMedida, valorUnitario, valorDesconto, valorTotal, codigoBarras, cmvAquisicao, cmvContabil, cmvTributos, cmvTotal, promocao, margem, observacao, User.Identity.Name);

                            movimentacoes.Add(movimentacaoDiaria);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.MovimentacaoDiaria.AddRange(movimentacoes);
                    context.SaveChanges();
                }
            }
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {

            var movimentacao = context.MovimentacaoDiaria.FirstOrDefault(x => x.IdCliente == id);
            if (movimentacao == null)
                return BadRequest("Movimentacção não encontrado ");

            return Ok(new MovimentacaoDiariaResponse()
            {
                IdMovimentacaoDiaria = movimentacao.IdMovimentacaoDiaria,
                IdCliente = movimentacao.IdCliente,
                TipoMovimentacao = movimentacao.TipoMovimentacao,
                DataMovimentacao = movimentacao.DataMovimentacao,
                FornecedorCliente = movimentacao.FornecedorCliente,
                CpfCnpjFornecedorCliente = movimentacao.CpfCnpjFornecedorCliente,
                NotaFiscal = movimentacao.NotaFiscal,
                Produto = movimentacao.Produto,
                Categoria = movimentacao.Categoria,
                SKU = movimentacao.SKU,
                NCM = movimentacao.NCM,
                CFOP = movimentacao.CFOP,
                Quantidade = movimentacao.Quantidade,
                UnidadeMedida = movimentacao.UnidadeMedida,
                ValorUnitario = movimentacao.ValorUnitario,
                ValorDesconto = movimentacao.ValorDesconto,
                ValorTotal = movimentacao.ValorTotal,
                CodigoBarras = movimentacao.CodigoBarras,
                CMV_Aquisicao = movimentacao.CMV_Aquisicao,
                CMV_Contabil = movimentacao.CMV_Contabil,
                CMV_Tributos = movimentacao.CMV_Tributos,
                CMV_Total = movimentacao.CMV_Total,
                Promocao = movimentacao.Promocao,
                Margem = movimentacao.Margem,
                Observacao = movimentacao.Observacao,
            });
        }
    }
}
