using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.IO;
using ERP.Models;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using OfficeOpenXml;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class VendaController : ControllerBase
    {
        protected Context context;
        public VendaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var result = context.Venda.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
                  .Select(m => new
                  {
                      m.IdVenda,
                      m.DataVenda,
                      m.DataPrevPagamento,
                      m.DataPagamento,
                      m.Cliente,
                      m.ValorBruto,
                      m.ValorDespesa,
                      m.ValorLiquido,
                      m.ValorPagamento,
                      m.Taxa,
                      m.MeioPagamento,
                      m.NomeBandeira,
                      m.NomeOperadora,
                      m.Gravame,
                      m.StatusConciliacao,
                      m.StatusVenda,
                      m.Unidade.Nome,
                      m.Produto,
                      m.ProdutoCliente,
                      m.Modalidade,
                      m.Autorizacao,
                      m.Parcela,
                      m.Terminal,
                      m.Identificador,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarUnidade")]
        public IActionResult ListarUnidade()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var result = context.Unidade.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
                  .Select(m => new
                  {
                      m.IdUnidade,
                      m.Nome,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] VendaRequest model)
        {
            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);

            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            Empresa empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);

            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == model.IdOperadora);
            var bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == model.IdBandeira);
            var contaRecebimento = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == model.IdContaRecebimento);
            var contaGravame = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == model.IdContaGravame);
            Venda venda;

            if (model.IdVenda > 0)
            {
                venda = context.Venda.FirstOrDefault(x => x.IdVenda == model.IdVenda);
                venda.Alterar(model.DataVenda, model.DataPrevPagamento,  model.DataPagamento, model.Cliente, model.ValorBruto, model.ValorDespesa, model.ValorLiquido, model.ValorPagamento, model.Taxa, model.MeioPagamento, bandeira, bandeira.NomeBandeira, operadora, contaRecebimento, contaGravame, operadora.NomeOperadora, model.Gravame, model.StatusConciliacao, model.StatusVenda, unidade, empresa, model.Identificador, model.Produto, model.ProdutoCliente, model.Modalidade, model.Autorizacao, model.Parcela, model.Terminal, User.Identity.Name);

                context.Update(venda);
            }
            else
            {
                venda = new Venda(model.DataVenda, model.DataPrevPagamento, model.DataPagamento, model.Cliente, model.ValorBruto, model.ValorDespesa, model.ValorLiquido, model.ValorPagamento, model.Taxa, model.MeioPagamento, bandeira, bandeira.NomeBandeira, operadora, contaRecebimento, contaGravame, operadora.NomeOperadora, model.Gravame, model.StatusConciliacao, model.StatusVenda, unidade, empresa, model.Identificador, model.Produto, model.ProdutoCliente, model.Modalidade, model.Autorizacao, model.Parcela, model.Terminal, User.Identity.Name);
                context.Venda.Add(venda);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarVendaRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var query = context.Venda.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa).AsQueryable();

            if (model.TipoPeriodo == "dataPagamento")
            {
                query = query.Where(x => x.DataPagamento >= model.DataInicio && x.DataPagamento <= model.DataTermino);
            }
            else if (model.TipoPeriodo == "dataVenda")
            {
                query = query.Where(x => x.DataVenda >= model.DataInicio && x.DataVenda <= model.DataTermino);
            }

            if (model.IdUnidade != 0)
            {
                query = query.Where(x => x.IdUnidade == model.IdUnidade || x.IdUnidade == null);
            }

            if(model.IdOperadora != 0)
            {
                query = query.Where(x => x.IdOperadora == model.IdOperadora);
            }

            if (model.IdBandeira != 0)
            {
                query = query.Where(x => x.IdBandeira == model.IdBandeira);
            }

            if (!string.IsNullOrEmpty(model.Identificador))
            {
                query = query.Where(x => x.Identificador == model.Identificador);
            }

            if (!string.IsNullOrEmpty(model.Status) && model.Status != "Todos")
            {
                query = query.Where(x => x.StatusConciliacao == model.Status);
            }

            var result = query.Select(m => new
            {
                m.IdVenda,
                m.DataVenda,
                m.DataPrevPagamento,
                m.DataPagamento,
                m.Cliente,
                m.ValorBruto,
                m.ValorDespesa,
                m.ValorLiquido,
                m.ValorPagamento,
                m.Taxa,
                m.MeioPagamento,
                m.NomeBandeira,
                m.NomeOperadora,
                m.Gravame,
                m.StatusConciliacao,
                m.StatusVenda,
                m.Unidade.Nome,
                m.Produto,
                ContaVendaRecebimento = m.ContaRecebimento.Conta,
                AgenciaVendaRecebimento =  m.ContaRecebimento.Agencia,
                BancoVendaRecebimento = m.ContaRecebimento.Banco.NomeBanco,
                ContaVendaGravame = m.ContaRecebimento.Conta,
                AgenciaVendaGravame = m.ContaRecebimento.Agencia,
                BancoVendaGravame = m.ContaRecebimento.Banco.NomeBanco,
                m.ProdutoCliente,
                m.Modalidade,
                m.Autorizacao,
                m.Parcela,
                m.Terminal,
                m.Identificador,
                m.Situacao
            }).Take(1000).ToList();

            var totalAReceber = query.Where(x => x.StatusVenda == "Aberto").Sum(x => x.ValorBruto ?? 0); 
            var totalRecebido = query.Where(x => x.StatusVenda == "Pago").Sum(x => x.ValorPagamento ?? 0);

            var totais = new
            {
                TotalReceber = totalAReceber,
                TotalRecebido = totalRecebido
            };
            return Ok(new
            {
                result,
                totais
            });
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
        [Route("importarExcel")]
        [Authorize]
        public IActionResult ImportarExcel([FromForm] IFormFile excelFile, [FromForm] int idUnidade, [FromForm] int idOperadora)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == idUnidade);
            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == idOperadora);
            var contratoOperadora = context.ContratoOperadora.FirstOrDefault(x => x.IdUnidade == idUnidade && x.IdOperadora == idOperadora && x.Situacao == "Ativo");
             if (contratoOperadora == null)
                return BadRequest("Contrato com adquirente não encontrado");
            var contaRecebimento = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == contratoOperadora.IdContaRecebimento);
            var contaGravame = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == contratoOperadora.IdContaGravame);

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


                    var vendas = new List<Venda>();
                    var datasVenda = new HashSet<DateTime>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (DateTime.TryParse(worksheet.Cells[row, 8].Text, out var tempDataVenda))
                        {
                            datasVenda.Add(tempDataVenda);
                        }
                    }

                    var minData = datasVenda.Min();
                    var maxData = datasVenda.Max();

                    var vendasExistentes = context.Venda
                        .Where(x => x.DataVenda >= minData &&
                                    x.DataVenda <= maxData &&
                                    x.IdUnidade == idUnidade &&
                                    x.IdOperadora == idOperadora)
                        .ToList();


                    if (vendasExistentes.Any())
                    {
                        context.Set<Venda>().RemoveRange(vendasExistentes);
                        context.SaveChanges();
                    }
                    for (int row = 2; row <= rowCount; row++)
                    {

                        try
                        {
                            var cliente = worksheet.Cells[row, 1].Text;
                            var produto = worksheet.Cells[row, 2].Text;
                            var meioPagamento = worksheet.Cells[row, 3].Text.Trim().ToLower();

                            switch (meioPagamento)
                            {
                                case "Crédito":
                                case "crédito":
                                case "credito":
                                case "cartão de crédito":
                                case "cartão crédito":
                                case "credit":
                                    meioPagamento = "CREDIT";
                                    break;
                                case "Débito":
                                case "débito":
                                case "debito":
                                case "cartão de débito":
                                case "cartão debito":
                                case "debit":
                                    meioPagamento = "DEBIT";
                                    break;
                                case "Pix":
                                case "pix":
                                    meioPagamento = "PIX";
                                    break;
                                case "Voucher":
                                    meioPagamento = "Voucher";
                                    break;

                                default:
                                    meioPagamento = "Não Informado"; 
                                    break;
                            }

                            var produtoCliente = worksheet.Cells[row, 4].Text;
                            var modalidade = worksheet.Cells[row, 5].Text;
                            var autorizacao = worksheet.Cells[row, 6].Text;
                            var stringBandeira = worksheet.Cells[row, 7].Text;
                            switch (stringBandeira)
                            {
                                case "Maestro":
                                case "Mastercard":
                                    stringBandeira = "Mastercard";
                                    break;
                                case "Visa Electron":
                                case "Visa":
                                case "Brasilcard Credito":
                                    stringBandeira = "Visa";
                                    break;

                                case "ELO Debito":
                                case "ELO Credito":
                                case "Maxxcard Cultura":
                                case "Elo":
                                    stringBandeira = "Elo";
                                    break;
                                case "Alelo Alimentação":
                                case "Alelo Refeicao":
                                case "Alelo":
                                    stringBandeira = "Alelo";
                                    break;
                                case "Sodexo Alimentacao":
                                case "Sodexo Gift":
                                case "Sodexo Refeicao":
                                case "Sodexo":
                                    stringBandeira = "Sodexo";
                                    break;
                                case "Ticket Alimentacao":
                                case "Ticket Flex":
                                case "Ticket Restaurante":
                                case "Ticket":
                                    stringBandeira = "Ticket";
                                    break;
                                case "VR Alimentacao":
                                case "VR Refeicao":
                                case "VR":
                                    stringBandeira = "VR";
                                    break;
                                default:
                                    stringBandeira = "Outros";
                                    break;
                            }
                            var bandeira = context.Bandeira.FirstOrDefault(x => x.NomeBandeira == stringBandeira);
                            if (bandeira == null)
                                return BadRequest($"Bandeira {stringBandeira} não encontrada");
                            DateTime? tempDataVenda = DateTime.TryParse(worksheet.Cells[row, 8].Text, out var parsedDataVenda)
                                 ? parsedDataVenda
                                 : (DateTime?)null;

                            DateTime? tempDataPrevPagamento = DateTime.TryParse(worksheet.Cells[row, 9].Text, out var parsedDataPrevPag)
                                ? parsedDataPrevPag
                                : (DateTime?)null;

                            DateTime? tempDataPag = DateTime.TryParse(worksheet.Cells[row, 10].Text, out var parsedDataPag)
                                ? parsedDataPag
                                : (DateTime?)null;

                            var dataVenda = tempDataVenda.HasValue
                                ? (tempDataVenda.Value >= new DateTime(1753, 1, 1) ? tempDataVenda.Value.Date : new DateTime(1753, 1, 1))
                                : (DateTime?)null;

                            var dataPrevPagamento = tempDataPrevPagamento.HasValue
                                ? (tempDataPrevPagamento.Value >= new DateTime(1753, 1, 1) ? tempDataPrevPagamento.Value.Date : new DateTime(1753, 1, 1))
                                : (DateTime?)null;

                            var dataPagamento = tempDataPag.HasValue
                                ? (tempDataPag.Value >= new DateTime(1753, 1, 1) ? tempDataPag.Value.Date : new DateTime(1753, 1, 1))
                                : (DateTime?)null;




                            var valorBrutoString = Regex.Replace(worksheet.Cells[row, 11].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorBrutoString.Contains(",") && valorBrutoString.Contains("."))
                            {
                                
                                valorBrutoString = valorBrutoString.Replace(".", "");
                                valorBrutoString = valorBrutoString.Replace(",", "."); 
                            }
                            else if (valorBrutoString.Contains(","))
                            {

                                valorBrutoString = valorBrutoString.Replace(",", ".");
                            }
                            var valorBruto = decimal.TryParse(valorBrutoString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorBruto)
                                ? tempValorBruto
                                : 0m;
                            var taxaString = Regex.Replace(worksheet.Cells[row, 12].Text, @"[^\d.,-]", "").Trim();
                            taxaString = taxaString.Replace(",", ".");
                            var taxa = (decimal.TryParse(taxaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempTaxa)
                                ? tempTaxa
                                : (decimal?)null)/100;

                            var valorDespesaString = Regex.Replace(worksheet.Cells[row, 13].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorDespesaString.Contains(",") && valorDespesaString.Contains("."))
                            {

                                valorDespesaString = valorDespesaString.Replace(".", "");
                                valorDespesaString = valorDespesaString.Replace(",", ".");
                            }
                            else if (valorDespesaString.Contains(","))
                            {

                                valorDespesaString = valorDespesaString.Replace(",", ".");
                            }
                            var valorDespesa = decimal.TryParse(valorDespesaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorDespesa)
                                ? tempValorDespesa
                                : (decimal?)null;

                            var valorLiquidoString = Regex.Replace(worksheet.Cells[row, 14].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorLiquidoString.Contains(",") && valorLiquidoString.Contains("."))
                            {

                                valorLiquidoString = valorLiquidoString.Replace(".", "");
                                valorLiquidoString = valorLiquidoString.Replace(",", ".");
                            }
                            else if (valorLiquidoString.Contains(","))
                            {

                                valorLiquidoString = valorLiquidoString.Replace(",", ".");
                            }
                            var valorLiquido = decimal.TryParse(valorLiquidoString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorLiquido)
                                ? tempValorLiquido
                                : (decimal?)null;

                            var valorPagamentoString = Regex.Replace(worksheet.Cells[row, 15].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorPagamentoString.Contains(",") && valorPagamentoString.Contains("."))
                            {

                                valorPagamentoString = valorPagamentoString.Replace(".", "");
                                valorPagamentoString = valorPagamentoString.Replace(",", "."); 
                            }
                            else if (valorPagamentoString.Contains(","))
                            {

                                valorPagamentoString = valorPagamentoString.Replace(",", ".");
                            }
                            var valorPagamento = decimal.TryParse(valorPagamentoString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorPagamento)
                                ? tempValorPagamento
                                : (decimal?)null;
                            var identificador = string.IsNullOrWhiteSpace(worksheet.Cells[row, 16].Text)
                                ? "Sem Identificador"
                                : worksheet.Cells[row, 16].Text.Trim();
                            var gravame = worksheet.Cells[row, 17].Text;
                            var parcela = worksheet.Cells[row, 18].Text;
                            var terminal = worksheet.Cells[row, 19].Text;
                            

                            var venda = new Venda(dataVenda, dataPrevPagamento, dataPagamento, cliente, valorBruto, valorDespesa, valorLiquido, valorPagamento, taxa,
                                                              meioPagamento, bandeira, bandeira.NomeBandeira, operadora, contaRecebimento, contaGravame, operadora.NomeOperadora, gravame, "Não conciliado", "Aberto", unidade, empresa, identificador, produto, produtoCliente, modalidade, autorizacao, parcela, terminal, User.Identity.Name);

                            vendas.Add(venda);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.Venda.AddRange(vendas);
                    context.SaveChanges();
                }
            }
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var venda = context.Venda.FirstOrDefault(x => x.IdVenda == id && x.IdEmpresa == usuarioLogado.IdEmpresa);
            if (venda == null)
                return BadRequest("Venda não encontrado ");

            return Ok(new VendaResponse()
            {
                IdVenda = venda.IdVenda,
                DataVenda = venda.DataVenda,
                DataPrevPagamento = venda.DataPrevPagamento,
                DataPagamento = venda.DataPagamento,
                Cliente = venda.Cliente,
                ValorBruto = venda.ValorBruto,
                ValorDespesa = venda.ValorDespesa,
                ValorLiquido = venda.ValorLiquido,
                ValorPagamento = venda.ValorPagamento,
                Taxa = venda.Taxa,
                MeioPagamento = venda.MeioPagamento,
                IdBandeira = venda.IdBandeira,
                NomeBandeira = venda.NomeBandeira,
                IdOperadora = venda.IdOperadora,
                IdContaRecebimento = venda.IdContaRecebimento,
                IdContaGravame = venda.IdContaGravame,
                NomeOperadora = venda.NomeOperadora,
                Gravame = venda.Gravame,
                StatusConciliacao = venda.StatusConciliacao,
                StatusVenda = venda.StatusVenda,
                IdUnidade = venda.IdUnidade,
                Identificador = venda.Identificador,
                Produto = venda.Produto,
                ProdutoCliente = venda.ProdutoCliente,
                Modalidade = venda.Modalidade,
                Autorizacao = venda.Autorizacao,
                Parcela = venda.Parcela,
                Terminal = venda.Terminal,
                Situacao = venda.Situacao
            });
        }
    }
}

