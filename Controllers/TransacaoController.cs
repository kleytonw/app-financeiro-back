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
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.Entity;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class TransacaoController : ControllerBase
    {
        protected Context context;
        public TransacaoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var result = context.Transacao.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
                  .Select(m => new
                  {
                      m.IdTransacao,
                      m.Unidade.Nome,
                      m.Cliente,
                      m.ValorBruto,
                      m.Taxa,
                      m.Despesa,
                      m.ValorLiquido,
                      m.DataVenda,
                      m.DataMovimentacao,
                      m.MeioPagamento,
                      m.Bandeira,
                      m.NomeOperadora,
                      m.StatusConciliacao,
                      m.Identificador,
                      m.QuantidadeParcela,
                      m.NumeroVenda,
                      m.StatusTransacao,
                      m.Terminal,
                      m.ChargebackStatus,
                      m.TipoCaptura,
                      m.Flex,
                      m.FlexAmount,
                      m.ValorTotalTaxa,
                      m.ValorEmbarqueTransacao,
                      m.Tokenizado,
                      m.Tid,
                      m.NumeroDoPedido,
                      m.NumeroCartao,
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
        public IActionResult Salvar([FromBody] TransacaoRequest model)
        {
            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);

            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            Empresa empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);

            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == model.IdOperadora);
            Transacao transacao;
            
            if (model.IdTransacao > 0)
            {
                transacao = context.Transacao.FirstOrDefault(x => x.IdTransacao == model.IdTransacao);
                transacao.Alterar(model.Cliente,
                                  unidade, 
                                  empresa, 
                                  model.ValorBruto, 
                                  model.Taxa, 
                                  model.Despesa, 
                                  model.ValorLiquido, 
                                  model.DataVenda, 
                                  model.DataMovimentacao, 
                                  model.MeioPagamento, 
                                  model.Bandeira, 
                                  operadora, 
                                  operadora.NomeOperadora, 
                                  model.StatusConciliacao, 
                                  model.ValorPagoConciliacao, 
                                  model.ValorTarifaConciliacao, 
                                  model.Observacao, 
                                  model.Identificador,
                                  model.QuantidadeParcela,
                                  model.NumeroVenda,
                                  model.StatusTransacao,
                                  model.Terminal,
                                  model.ChargebackStatus,
                                  model.TipoCaptura,
                                  model.Flex,
                                  model.FlexAmount,
                                  model.ValorTotalTaxa,
                                  model.ValorEmbarqueTransacao,
                                  model.Tokenizado,
                                  model.Tid,
                                  model.NumeroDoPedido,
                                  model.NumeroCartao,
                                  User.Identity.Name);

                context.Update(transacao);
            }
            else
            {
                transacao = new Transacao(model.Cliente, 
                                          unidade, 
                                          empresa, 
                                          model.ValorBruto, 
                                          model.Taxa, 
                                          model.Despesa, 
                                          model.ValorLiquido, 
                                          model.DataVenda, 
                                          model.DataMovimentacao, 
                                          model.MeioPagamento, 
                                          model.Bandeira, 
                                          operadora, 
                                          operadora.NomeOperadora, 
                                          model.StatusConciliacao,
                                          model.Identificador,
                                          model.QuantidadeParcela,
                                          model.NumeroVenda,
                                          model.StatusTransacao,
                                          model.Terminal,
                                          model.ChargebackStatus,
                                          model.TipoCaptura,
                                          model.Flex,
                                          model.FlexAmount,
                                          model.ValorTotalTaxa,
                                          model.ValorEmbarqueTransacao,
                                          model.Tokenizado,
                                          model.Tid,
                                          model.NumeroDoPedido,
                                          model.NumeroCartao,
                                          "", "", "",
                                          User.Identity.Name);
                context.Transacao.Add(transacao);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarTransacaoRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var query = context.Transacao.AsQueryable();
            if (usuarioLogado.TipoUsuario == "Cliente")
            {
                query = query.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa);
            } 
             
            if (model.TipoPeriodo == "dataMovimentacao")
            {
                query = query.Where(x => x.DataMovimentacao >= model.DataInicio && x.DataMovimentacao <= model.DataTermino);
            }
            else if (model.TipoPeriodo == "dataVenda")
            {
                query = query.Where(x => x.DataVenda >= model.DataInicio && x.DataVenda <= model.DataTermino);
            }

            if (model.IdUnidade != 0)
            {
                 query = query.Where(x => x.IdUnidade == model.IdUnidade || model.IdUnidade == null);
            }

            if (model.IdOperadora != 0)
            {
                query = query.Where(x => x.IdOperadora == model.IdOperadora || model.IdOperadora == null);
            }

            if (model.ValorBruto != 0 && model.ValorBruto != null)
            {
                query = query.Where(x => x.ValorBruto == model.ValorBruto);
            }

            if (!string.IsNullOrEmpty(model.Status) && model.Status != "Todos")
            {
                query = query.Where(x => x.StatusConciliacao == model.Status);
            }

            if (!string.IsNullOrEmpty(model.Identificador))
            {
                query = query.Where(x => x.Identificador.Contains(model.Identificador));
            }

            var result = query.OrderBy(m => m.DataVenda).Select(m => new
            {
                m.IdTransacao,
                m.Cliente,
                m.Unidade.Nome,
                m.ValorBruto,
                m.Taxa,
                m.Despesa,
                m.ValorLiquido,
                m.DataVenda,
                m.DataMovimentacao,
                m.MeioPagamento,
                m.Bandeira,
                m.NomeOperadora,
                m.Observacao,
                m.StatusConciliacao,
                m.Identificador,
                m.QuantidadeParcela,
                m.NumeroVenda,
                m.StatusTransacao,
                m.Terminal,
                m.ChargebackStatus,
                m.TipoCaptura,
                m.Flex,
                m.FlexAmount,
                m.ValorTotalTaxa,
                m.ValorEmbarqueTransacao,
                m.Tokenizado,
                m.Tid,
                m.NumeroDoPedido,
                m.NumeroCartao,
                m.Situacao
            }).Take(1000).ToList();

            var counts = query.GroupBy(x => x.StatusConciliacao)
                 .Select(group => new
                 {
                     Status = group.Key,
                     Count = group.Count()
                 })
                 .ToDictionary(x => x.Status, x => x.Count);

            var statusCounts = new
            {
                Conciliado = counts.ContainsKey("Conciliado") ? counts["Conciliado"] : 0,
                NaoConciliado = counts.ContainsKey("Não conciliado") ? counts["Não conciliado"] : 0,
                Inconsistente = counts.ContainsKey("Inconsistente") ? counts["Inconsistente"] : 0
            };

            return Ok(new
            {
                result,
                statusCounts
            });       
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var transacao = context.Transacao.FirstOrDefault(x => x.IdTransacao == id && x.IdEmpresa == usuarioLogado.IdEmpresa);
            transacao.Excluir(User.Identity.Name);

            context.Update(transacao);
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("importarExcel")]
        [Authorize]
        public async Task<IActionResult> ImportarExcel(
            [FromForm] IFormFile excelFile, 
            [FromForm] int idUnidade,
            [FromForm] int idOperadora, 
            [FromForm] DateTime dataInicial, 
            [FromForm] DateTime dataFinal)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == idUnidade);
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == unidade.IdEmpresa);
            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == idOperadora);
            // var contaRecebimento = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == contratoOperadora.IdContaRecebimento);
            // var contaGravame = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == contratoOperadora.IdContaGravame);


            // exclui as transações da unidade e empresa 
            var transacoesRemove = context.Transacao.Where(x => 
            x.DataMovimentacao.Value.Date >= dataInicial.Date && 
            x.DataMovimentacao.Value.Date <= dataFinal.Date && 
            x.IdUnidade == idUnidade && 
            x.IdOperadora == operadora.IdOperadora).ToList();

            if (transacoesRemove.Count > 0)
            {
                context.Transacao.RemoveRange(transacoesRemove);
                await context.SaveChangesAsync();
            }

            if (excelFile == null || excelFile.Length == 0)
            {
                throw new ArgumentException("Arquivo inválido");
            }

            try
            {

                using (var stream = new MemoryStream())
                {
                    excelFile.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        var transacoes = new List<Transacao>();

                        for (int row = 2; row <= rowCount; row++)
                        {


                            var identificadorTransacao = worksheet.Cells[row, 1].Text;
                            var dataVencimento = DateTime.TryParse(worksheet.Cells[row, 2].Text, out var tempDataVenc) ? tempDataVenc : (DateTime?)null;
                            var dataPagamento = DateTime.TryParse(worksheet.Cells[row, 3].Text, out var tempDataPag) ? tempDataPag : (DateTime?)null;
                            var cliente = worksheet.Cells[row, 4].Text;

                            var valorBrutoString = Regex.Replace(worksheet.Cells[row, 5].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            if (valorBrutoString.Contains(",") && valorBrutoString.Contains("."))
                            {

                                valorBrutoString = valorBrutoString.Replace(".", "");
                                valorBrutoString = valorBrutoString.Replace(",", ".");
                            }
                            else if (valorBrutoString.Contains(","))
                            {

                                valorBrutoString = valorBrutoString.Replace(",", ".");
                            }
                            var valorBruto = decimal.TryParse(valorBrutoString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValor)
                                ? tempValor
                                : (decimal?)null;

                            var taxaString = Regex.Replace(worksheet.Cells[row, 6].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            taxaString = taxaString.Replace(",", ".");
                            var taxa = decimal.TryParse(taxaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempTaxa)
                                ? tempTaxa
                                : (decimal?)null;


                            var valorTaxaString = Regex.Replace(worksheet.Cells[row, 7].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            valorTaxaString = valorTaxaString.Replace(",", ".");
                            var valorTaxa = decimal.TryParse(valorTaxaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorTaxa)
                                ? tempValorTaxa
                                : (decimal?)null;


                            var despesaTaxaString = Regex.Replace(worksheet.Cells[row, 8].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            despesaTaxaString = despesaTaxaString.Replace(",", ".");
                            var despesaTaxa = decimal.TryParse(despesaTaxaString, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempDespesaTaxa)
                                ? tempDespesaTaxa
                                : (decimal?)null;


                            var valorLiquidotring = Regex.Replace(worksheet.Cells[row, 9].Text.Replace(" ", ""), @"[^0-9.,-]", "").Trim();
                            valorLiquidotring = valorLiquidotring.Replace(",", ".");
                            var valorLiquido = decimal.TryParse(valorLiquidotring, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorLiquido)
                                ? tempValorLiquido
                                : (decimal?)null;

                            string meioPagamento = worksheet.Cells[row, 10].Text;

                            if (meioPagamento != "Credito" && meioPagamento != "Debito")
                                throw new Exception($@"Meio de pagamento incorreto linha => {row}");

                            int idBandeira = Convert.ToInt32(worksheet.Cells[row, 11].Text);
                            var bandeira = context.Bandeira.FirstOrDefault(x => x.IdBandeira == idBandeira);
                            if (bandeira == null)
                                throw new Exception("Bandeira não encontrada");


                            var identificador = worksheet.Cells[row, 12].Text;

                            var quantidadeParcela = int.TryParse(worksheet.Cells[row, 13].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempQuantidadeParcela)
                                ? tempQuantidadeParcela
                                : (int?)null;

                            var numeroVenda = long.TryParse(worksheet.Cells[row, 14].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempNumeroVenda)
                                ? tempQuantidadeParcela
                                : (long?)null;

                            var statusTransacao = worksheet.Cells[row, 15].Text;

                            switch (statusTransacao)
                            {
                                case "Aprovada":
                                    Console.WriteLine("Transação aprovada com sucesso.");
                                    break;

                                case "Negada":
                                    throw new InvalidOperationException("A transação foi negada pelo emissor do cartão.");

                                case "Cancelada":
                                    throw new OperationCanceledException("A transação foi cancelada pelo usuário ou sistema.");

                                case "Pendente":
                                    throw new InvalidOperationException("A transação ainda está pendente de confirmação.");

                                case "Estornado":
                                    throw new InvalidOperationException("A transação foi estornada e o valor devolvido ao cliente.");

                                case "Rejeitada":
                                    throw new InvalidOperationException("A transação foi rejeitada por política de risco ou problemas técnicos.");
                                default:
                                    throw new Exception($@"Status transação incorreta linha => {row}");
                            }

                            var terminal = worksheet.Cells[row, 16].Text;
                            var numeroPedido = worksheet.Cells[row, 17].Text;
                            var produto = worksheet.Cells[row, 18].Text;
                            var descricaoProduto = worksheet.Cells[row, 19].Text;
                            var codigoProduto = worksheet.Cells[row, 20].Text;


                            /* var chargebackStatus = worksheet.Cells[row, 13].Text;
                             var tipoCaptura = int.TryParse(worksheet.Cells[row, 14].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempTipoCaptura)
                                 ? tempTipoCaptura
                                 : (int?)null; ;
                             var flex = bool.TryParse(worksheet.Cells[row, 15].Text, out bool tempFlex);
                             var flexAmount = decimal.TryParse(worksheet.Cells[row, 16].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempFlexAmount)
                                 ? tempFlexAmount
                                 : (decimal?)null;  
                             var valorTotalTaxa = decimal.TryParse(worksheet.Cells[row, 17].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorTotalTaxa)
                                  ? tempValorTotalTaxa
                                 : (decimal?)null; ;
                             var valorEmbarqueTransacao = decimal.TryParse(worksheet.Cells[row, 18].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var tempValorEmbarqueTransacao)
                                 ? tempValorEmbarqueTransacao 
                                 : (decimal?)null;
                             var tokenizado = bool.TryParse(worksheet.Cell s[row, 19].Text, out bool tempTokenizado);
                             var tid = worksheet.Cells[row, 20].Text;
                            var numeroDoPedido = worksheet.Cells[row, 21].Text; 
                            var numeroCartao = worksheet.Cells[row, 22].Text;*/

                            var transacao = new Transacao(cliente, unidade, empresa, valorBruto, taxa, despesaTaxa, valorLiquido, dataVencimento, dataPagamento,
                                                          meioPagamento, bandeira.NomeBandeira, operadora, operadora.NomeOperadora, "Não conciliado", identificador, quantidadeParcela, numeroVenda, statusTransacao,
                                                          terminal, "", 0, false, null, valorTaxa, null, null, identificadorTransacao, numeroPedido, null, produto, "", codigoProduto, User.Identity.Name, descricaoProduto);

                            transacoes.Add(transacao);
                        }

                        context.Set<Transacao>().AddRange(transacoes);
                        context.SaveChanges();
                    }
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
            return Ok();
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var transacao = context.Transacao.FirstOrDefault(x => x.IdTransacao == id && x.IdEmpresa == usuarioLogado.IdEmpresa);
            if (transacao == null)
                return BadRequest("Transação não encontrado ");

            return Ok(new TransacaoResponse()
            {
                IdTransacao = transacao.IdTransacao,
                IdUnidade = transacao.IdUnidade,
                Cliente = transacao.Cliente,
                ValorBruto = transacao.ValorBruto,
                Taxa = transacao.Taxa,
                Despesa = transacao.Despesa,
                ValorLiquido = transacao.ValorLiquido,
                DataVenda = transacao.DataVenda,
                DataMovimentacao = transacao.DataMovimentacao,
                MeioPagamento = transacao.MeioPagamento,
                Bandeira = transacao.Bandeira,
                IdOperadora = transacao.IdOperadora,
                NomeOperadora = transacao.NomeOperadora,
                StatusConciliacao = transacao.StatusConciliacao,
                ValorPagoConciliacao = transacao.ValorPagoConciliacao,
                ValorTarifaConciliacao = transacao.ValorTarifaConciliacao,
                Observacao = transacao.Observacao,
                Identificador = transacao.Identificador,
                QuantidadeParcela = transacao.QuantidadeParcela,
                NumeroVenda = transacao.NumeroVenda,
                StatusTransacao = transacao.StatusTransacao,
                Terminal = transacao.Terminal,
                ChargebackStatus = transacao.ChargebackStatus,
                TipoCaptura = transacao.TipoCaptura,
                Flex = transacao.Flex,
                FlexAmount = transacao.FlexAmount,
                ValorTotalTaxa = transacao.ValorTotalTaxa,
                ValorEmbarqueTransacao = transacao.ValorEmbarqueTransacao,
                Tokenizado = transacao.Tokenizado,
                Tid = transacao.Tid,
                NumeroDoPedido = transacao.NumeroDoPedido,
                NumeroCartao = transacao.NumeroCartao,
                Situacao = transacao.Situacao
            });
        }
    }
}

