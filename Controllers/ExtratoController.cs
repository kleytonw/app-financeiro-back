using EFCore.BulkExtensions;
using ERP.Domain.Entidades;
using ERP.Infra;
using ERP.Models;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using ERP_API.Service.Parceiros;
using ERP_API.Service.Parceiros.Interface;
using ERP_API.Service.Pluggy.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OFXSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [Authorize]
    public class ExtratoController : ControllerBase
    {
        protected Context context;
        private IPluggyService _pluggyService;

        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587))
        {
            EnableSsl = true
        };

        public ExtratoController(Context context, IPluggyService pluggyService)
        {
            this.context = context;
            _pluggyService = pluggyService;
        }

        [HttpPost]
        [Route("listar")]
        public IActionResult Listar([FromBody] ExtratoRequest model)
        {
            var result = context.Extrato.Include(x => x.Cliente.Pessoa).Where(x => x.IdCliente == model.IdCliente && x.IdClienteContaBancaria == model.IdClienteContaBancaria)
                .Select(m => new
                {
                    m.IdExtrato,
                    m.ClienteContaBancaria.Conta,
                    m.Cliente.Pessoa.Nome,
                    m.Descricao,
                    m.Valor,
                    Tipo = m.Tipo.ToString(),
                    m.DataLancamento,
                    m.Situacao
                }).OrderByDescending(x => x.DataLancamento).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("listarClienteContaBancaria")]
        public IActionResult ListarClienteContaBancaria([FromBody] ExtratoRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var result = context.Extrato.Where(x => x.IdClienteContaBancaria == model.IdClienteContaBancaria && x.IdCliente == model.IdCliente)
                .Select(m => new
                {
                    m.IdExtrato,
                    m.ClienteContaBancaria.Conta,
                    m.Cliente.Pessoa.Nome,
                    m.Descricao,
                    m.Valor,
                    Tipo = m.Tipo.ToString(),
                    m.DataLancamento,
                    m.Categoria,
                    m.Situacao
                }).OrderByDescending(x => x.DataLancamento).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ExtratoRequest model)
        {
            var clienteContaBancaria = context.ClienteContaBancaria.FirstOrDefault(x => x.IdClienteContaBancaria == model.IdClienteContaBancaria);
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);



            Extrato extrato;
            if (model.IdExtrato > 0)
            {
                extrato = context.Extrato.FirstOrDefault(x => x.IdExtrato == model.IdExtrato);
                extrato.Alterar(clienteContaBancaria, cliente, model.Descricao, model.Valor, model.Tipo, model.DataLancamento,model.Pagador, model.CpfCnpjPagador, model.Categoria, model.Banco, model.MetodoPagamento, User.Identity.Name);
            }
            else
            {
                extrato = new Extrato(
                    clienteContaBancaria,
                    cliente,
                    model.Descricao,
                    model.Valor,
                    model.Tipo,
                    model.DataLancamento,
                    model.Pagador,
                    model.CpfCnpjPagador,
                    model.Categoria,
                    model.Banco,
                    model.MetodoPagamento,
                    User.Identity.Name
                );

                context.Extrato.Add(extrato);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("import")]
        public async Task<IActionResult> Import([FromForm] ImportarExtratoModel model)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
            var clienteContaBancaria = context.ClienteContaBancaria.FirstOrDefault(x => x.IdClienteContaBancaria == model.IdClienteContaBancaria);


            var extratos = new List<Extrato>();
            try
            {
                if (model.OfxFile == null || model.OfxFile.Length == 0)
                {
                    return BadRequest("Arquivo OFX não enviado ou inválido.");
                }

                var enviarExtrato = new EnviarExtratoTecnospeedRequestModel()
                {
                    File = model.OfxFile,
                };

                var tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    model.OfxFile.CopyTo(stream);
                }

                if (!System.IO.File.Exists(tempFilePath))
                {
                    return StatusCode(500, "Erro ao salvar o arquivo OFX.");
                }


                string ofxContent = System.IO.File.ReadAllText(tempFilePath);
                var ofxParser = new OFXDocumentParser();

                // Remover completamente as linhas com datas inválidas
                ofxContent = System.Text.RegularExpressions.Regex.Replace(
                    ofxContent,
                    @"<DTSERVER>0+\s*\r?\n?",
                    "",
                    RegexOptions.Multiline);

                ofxContent = System.Text.RegularExpressions.Regex.Replace(
                    ofxContent,
                    @"<DTASOF>0+\s*\r?\n?",
                    "",
                    RegexOptions.Multiline);

                // Converter vírgulas para pontos nos valores
                ofxContent = System.Text.RegularExpressions.Regex.Replace(
                    ofxContent,
                    @"<TRNAMT>(-?\d+),(\d+)",
                    "<TRNAMT>$1.$2");

                //ofxContent = Regex.Replace(ofxContent, @"<TRNTYPE>([^\r\n<]+)(?!</TRNTYPE>)", "<TRNTYPE>$1</TRNTYPE>");
                ofxContent = Regex.Replace(ofxContent, @"<DTPOSTED>([^\r\n<]+)(?!</DTPOSTED>)", "<DTPOSTED>$1</DTPOSTED>");
                ofxContent = Regex.Replace(ofxContent, @"<TRNAMT>([^\r\n<]+)(?!</TRNAMT>)", "<TRNAMT>$1</TRNAMT>");
                ofxContent = Regex.Replace(ofxContent, @"<FITID>([^\r\n<]+)(?!</FITID>)", "<FITID>$1</FITID>");
                ofxContent = Regex.Replace(ofxContent, @"<CHECKNUM>([^\r\n<]+)(?!</CHECKNUM>)", "<CHECKNUM>$1</CHECKNUM>");
                ofxContent = Regex.Replace(ofxContent, @"<NAME>([^<]+)(?!</NAME>)($|\r|\n|<)", "<NAME>$1</NAME>$2");

                ofxContent = Regex.Replace(ofxContent, @"</TRNTYPE></TRNTYPE>", "</TRNTYPE>");
                ofxContent = Regex.Replace(ofxContent, @"</DTPOSTED></DTPOSTED>", "</DTPOSTED>");
                ofxContent = Regex.Replace(ofxContent, @"</TRNAMT></TRNAMT>", "</TRNAMT>");
                ofxContent = Regex.Replace(ofxContent, @"</FITID></FITID>", "</FITID>");
                ofxContent = Regex.Replace(ofxContent, @"</CHECKNUM></CHECKNUM>", "</CHECKNUM>");
                ofxContent = Regex.Replace(ofxContent, @"</NAME></NAME>", "</NAME>");

                var ofxDocument = ofxParser.Import(ofxContent);

                var saldoTotal = ofxDocument.Balance.LedgerBalance / 100;
                DateTime dataDolSaldo = ofxDocument.Balance.LedgerBalanceDate;
                var saldoParcial = ofxDocument.Balance.AvaliableBalance / 100;


                var datasExtrato = ofxDocument.Transactions.Select(t => t.Date.Date).Distinct().ToList();

                var extratosExistentes = context.Extrato
                .Where(e => e.IdClienteContaBancaria == model.IdClienteContaBancaria &&
                            e.IdCliente == model.IdCliente &&
                            datasExtrato.Contains(e.DataLancamento.Date))
                .ToList();

                if (extratosExistentes.Any())
                {
                    context.Extrato.RemoveRange(extratosExistentes);
                    await context.SaveChangesAsync();
                }

                string descricao = "";

                int linhaAtual = 0;

                try
                {
                    foreach (var transaction in ofxDocument.Transactions)
                    {
                        linhaAtual++;

                        descricao = string.IsNullOrWhiteSpace(transaction.Memo)
                                      ? transaction.Name
                                      : transaction.Memo;

                        var extrato = new Extrato(
                            clienteContaBancaria,
                            cliente,
                            descricao,
                            transaction.Amount,
                            transaction.TransType,
                            transaction.Date,
                            null,
                            null,
                            null,
                            null,
                            null,
                            User.Identity.Name
                        );
                        extratos.Add(extrato);
                    }

                    context.Extrato.AddRange(extratos);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao processar transação na linha {linhaAtual}: {ex.Message}", ex);
                }
                context.Extrato.AddRange(extratos);

                DateTime? dataParaSalvar = DateTime.MinValue.Equals(dataDolSaldo) ? (DateTime?)null : dataDolSaldo;

                clienteContaBancaria.SetSaldo(saldoTotal, dataParaSalvar, User.Identity.Name);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao importar o OFX!");
            }

            return Ok();
        }

        [HttpPost]
        [Route("pesquisar")]
        public async Task<IActionResult> Pesquisar([FromBody] PesquisarExtrato model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var lista = new List<ExtratoOpenFinanceResponseModel>();


            var clienteContaBancaria = await context.ClienteContaBancaria.Include(x=>x.Cliente)
                .ThenInclude(x=>x.Pessoa)
                .FirstOrDefaultAsync(x => x.IdClienteContaBancaria == model.IdClienteContaBancaria);

            if (clienteContaBancaria == null)
                throw new Exception("Conta bancária não encontrada");

            int totalRegistro = context.Extrato.Count(x => x.DataLancamento.Date >= model.DataInicio.Date
                                            && x.DataLancamento.Date <= model.DataTermino.Date
                                            && x.IdCliente == model.IdCliente
                                            && x.IdClienteContaBancaria == model.IdClienteContaBancaria);
           


                #region BUSCA EXTRATO SISTEMA
                var buscaExtrato = context.Extrato.Include(x => x.Cliente).ThenInclude(x => x.Pessoa).Where(x => x.DataLancamento.Date >= model.DataInicio.Date
                                                && x.DataLancamento.Date <= model.DataTermino.Date
                                                && x.IdCliente == model.IdCliente
                                                && x.IdClienteContaBancaria == model.IdClienteContaBancaria).ToList();

                foreach (var item in buscaExtrato)
                {
                    lista.Add(new ExtratoOpenFinanceResponseModel()
                    {
                        Descricao = item.Descricao,
                        Valor = item.Valor,
                        DataLancamento = item.DataLancamento,
                        Conta = item.ClienteContaBancaria.Conta,
                        IdCliente = item.IdCliente,
                        IdClienteContaBancaria = item.IdClienteContaBancaria,
                        Nome = item.Cliente.Pessoa.Nome,
                        Tipo = item.Tipo.ToString(),
                        Pagador = item.Pagador,
                        CpfCnpjPagador = item.CpfCnpjPagador,
                        Categoria = item.Categoria,
                        Banco = item.Banco,
                        MetodoPagamento = item.MetodoPagamento,
                    });

                }
                return Ok(lista.OrderByDescending(x => x.DataLancamento));
                #endregion


            
            //else
            //{
            //    try
            //    {
            //        if (string.IsNullOrEmpty(clienteContaBancaria.AccountIdOpenFinance))
            //            throw new Exception("Identificador open finance não encontrado");


            //        var query = await _pluggyService.GetAllTransactionsAsync(Guid.Parse(clienteContaBancaria.AccountIdOpenFinance), model.DataInicio, model.DataTermino);

            //        //#region APAGAR EXTRATO 
            //        //var extratos = context.Extrato.Where(x => x.DataLancamento.Date >= model.DataInicio.Date
            //        //                                && x.DataLancamento.Date <= model.DataTermino.Date
            //        //                                && x.IdCliente == model.IdCliente
            //        //                                && x.IdClienteContaBancaria == model.IdClienteContaBancaria).ToList();

            //        //context.RemoveRange(extratos);
            //        //context.SaveChanges();
            //        //#endregion

            //        #region INSERIR NOVO EXTRATO CONCILIADORA - SISTEMA
            //        List<Extrato> listaNovoExtrato = new List<Extrato>();
            //        foreach (var item in query)
            //        {

            //            listaNovoExtrato.Add(new Extrato(
            //             clienteContaBancaria,
            //             clienteContaBancaria?.Cliente,
            //             $"{item?.Description ?? string.Empty} {item?.DescriptionRaw ?? string.Empty}".Trim(),
            //             item?.Amount ?? 0,
            //             item?.Type == "CREDIT" ? OFXTransactionType.CREDIT : OFXTransactionType.DEBIT,
            //             item?.Date ?? DateTime.MinValue,
            //             item?.PaymentData?.Payer?.Name ?? string.Empty,
            //             item?.PaymentData?.Payer?.DocumentNumber?.Value ?? string.Empty,
            //             item?.Category,
            //             item.PaymentData.Payer?.RoutingNumber,
            //             item.PaymentData.PaymentMethod,
            //             "extrato"
            //         ));
            //        }

            //        if (listaNovoExtrato.Count > 0)
            //            await context.Extrato.AddRangeAsync(listaNovoExtrato);

            //        context.SaveChanges();
            //        #endregion


            //        #region BUSCA EXTRATO SISTEMA
            //        var buscaExtrato = context.Extrato.Include(x => x.Cliente).ThenInclude(x => x.Pessoa).Where(x => x.DataLancamento.Date >= model.DataInicio.Date
            //                                        && x.DataLancamento.Date <= model.DataTermino.Date
            //                                        && x.IdCliente == model.IdCliente
            //                                        && x.IdClienteContaBancaria == model.IdClienteContaBancaria).ToList();

            //        foreach (var item in buscaExtrato)
            //        {
            //            lista.Add(new ExtratoOpenFinanceResponseModel()
            //            {
            //                Descricao = item.Descricao,
            //                Valor = item.Valor,
            //                DataLancamento = item.DataLancamento,
            //                Conta = item.ClienteContaBancaria.Conta,
            //                IdCliente = item.IdCliente,
            //                IdClienteContaBancaria = item.IdClienteContaBancaria,
            //                Nome = item.Cliente.Pessoa.Nome,
            //                Tipo = item.Tipo.ToString(),
            //                Pagador = item.Pagador,
            //                CpfCnpjPagador = item.CpfCnpjPagador,
            //                Categoria = item.Categoria,
            //                Banco = item.Banco,
            //                MetodoPagamento = item.MetodoPagamento,
            //            });

            //        }
            //        return Ok(lista);
            //        #endregion


            //    }

            //    catch (Exception ex)
            //    {
            //        return BadRequest(ex.Message);
            //    }
            //}
        }


        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var extrato = context.Extrato.FirstOrDefault(x => x.IdExtrato == id);
            extrato.Excluir(User.Identity.Name);

            context.Update(extrato);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obterContaBancaria")]
        public IActionResult ObterContaBancaria(int idContaBancaria)
        {
            var contaBancaria = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == idContaBancaria);
            if (contaBancaria == null)
                return BadRequest("Conta não encontrada");

            return Ok(new ContaBancariaResponse()
            {
                Conta = contaBancaria.Conta
            });
        }

        [HttpGet]
        [Route("obterClienteContaBancaria")]
        public IActionResult ObterClienteContaBancaria(int idClienteContaBancaria)
        {
            var clienteContaBancaria = context.ClienteContaBancaria.FirstOrDefault(x => x.IdClienteContaBancaria == idClienteContaBancaria);
            if (clienteContaBancaria == null)
                return BadRequest("Conta não encontrada");
            return Ok(new ContaBancariaResponse()
            {
                Conta = clienteContaBancaria.Conta
            });
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var extrato = context.Extrato.FirstOrDefault(x => x.IdExtrato == id);
            if (extrato == null)
                return BadRequest("Extrato não encontrado ");

            return Ok(new ExtratoResponse()
            {
                IdExtrato = extrato.IdExtrato,
                IdClienteContaBancaria = extrato.IdClienteContaBancaria,
                IdCliente = extrato.IdCliente,
                Descricao = extrato.Descricao,
                Valor = extrato.Valor,
                Tipo = extrato.Tipo,
                DataLancamento = extrato.DataLancamento,
                Situacao = extrato.Situacao
            });
        }



        [HttpGet]
        [Route("obter-conta-bancaria-open-finance")]
        public async Task<IActionResult> ObterContaBancariaOpenFinanceAsync(int idClienteContaBancaria)
        {
            var clienteContaBancaria = context.ClienteContaBancaria.FirstOrDefault(x => x.IdClienteContaBancaria == idClienteContaBancaria);
            if (clienteContaBancaria == null)
                return BadRequest("Conta não encontrada");

            if (string.IsNullOrEmpty(clienteContaBancaria.AccountIdOpenFinance))
                return BadRequest("Conta não vinculada ao Open Finance");

            var a = await _pluggyService.GetAccountByIdAsync(clienteContaBancaria.AccountIdOpenFinance);
            return Ok(a);
        }

        [HttpPost]
        [Route("atualizar-extrato-conta-bancaria")]
        public async Task<IActionResult> AtualizarExtratoContaBancaria([FromBody] PesquisarExtrato model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var lista = new List<ExtratoOpenFinanceResponseModel>();

            var clienteContaBancaria = await context.ClienteContaBancaria.Include(x => x.Cliente)
                .ThenInclude(x => x.Pessoa)
                .FirstOrDefaultAsync(x => x.IdClienteContaBancaria == model.IdClienteContaBancaria);

            if (clienteContaBancaria == null)
                throw new Exception("Conta bancária não encontrada");

            if (string.IsNullOrEmpty(clienteContaBancaria.AccountIdOpenFinance))
                throw new Exception("Identificador open finance não encontrado");

            var query = await _pluggyService.GetAllTransactionsAsync(
                Guid.Parse(clienteContaBancaria.AccountIdOpenFinance),
                model.DataInicio,
                model.DataTermino);

            #region APAGAR EXTRATO EM LOTE
            await context.Extrato
                .Where(x => x.DataLancamento.Date >= model.DataInicio.Date
                        && x.DataLancamento.Date <= model.DataTermino.Date
                        && x.IdCliente == model.IdCliente
                        && x.IdClienteContaBancaria == model.IdClienteContaBancaria)
                .ExecuteDeleteAsync();
            #endregion

            #region INSERIR NOVO EXTRATO EM LOTE
            TimeZoneInfo brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

            var listaNovoExtrato = query.Select(item => new Extrato(
                clienteContaBancaria,
                clienteContaBancaria?.Cliente,
                $"{item?.Description ?? string.Empty} {item?.DescriptionRaw ?? string.Empty}".Trim(),
                item?.Amount ?? 0,
                item?.Type == "CREDIT" ? OFXTransactionType.CREDIT : OFXTransactionType.DEBIT,
                item?.Date != null
                    ? TimeZoneInfo.ConvertTimeFromUtc(item.Date.Value.ToUniversalTime(), brasiliaTimeZone)
                    : DateTime.MinValue,
                item?.PaymentData?.Payer?.Name ?? string.Empty,
                item?.PaymentData?.Payer?.DocumentNumber?.Value ?? string.Empty,
                item?.Category,
                item.PaymentData.Payer?.RoutingNumber,
                item.PaymentData.PaymentMethod,
                "extrato"
            )).ToList();

            if (listaNovoExtrato.Any())
            {
                await context.Extrato.AddRangeAsync(listaNovoExtrato);
                await context.SaveChangesAsync();
            }
            #endregion

            #region BUSCA EXTRATO SISTEMA
            var buscaExtrato = await context.Extrato
                .Include(x => x.Cliente)
                    .ThenInclude(x => x.Pessoa)
                .Where(x => x.DataLancamento.Date >= model.DataInicio.Date
                        && x.DataLancamento.Date <= model.DataTermino.Date
                        && x.IdCliente == model.IdCliente
                        && x.IdClienteContaBancaria == model.IdClienteContaBancaria)
                .Select(item => new ExtratoOpenFinanceResponseModel
                {
                    Descricao = item.Descricao,
                    Valor = item.Valor,
                    DataLancamento = item.DataLancamento,
                    Conta = item.ClienteContaBancaria.Conta,
                    IdCliente = item.IdCliente,
                    IdClienteContaBancaria = item.IdClienteContaBancaria,
                    Nome = item.Cliente.Pessoa.Nome,
                    Tipo = item.Tipo.ToString(),
                    Pagador = item.Pagador,
                    CpfCnpjPagador = item.CpfCnpjPagador,
                    Categoria = item.Categoria,
                    Banco = item.Banco,
                    MetodoPagamento = item.MetodoPagamento,
                })
                .OrderByDescending(x => x.DataLancamento)
                .ToListAsync();

            return Ok(buscaExtrato);
            #endregion
        }

        [HttpGet]
        [Route("atualizar-extratos-contas-bancarias")]
        [AllowAnonymous]
        public async Task<IActionResult> AtualizarExtratosContasBancarias()
        {
            var contasBancarias = await context.ClienteContaBancaria
                .Include(x => x.Cliente)
                .Where(x => !string.IsNullOrEmpty(x.AccountIdOpenFinance))
                .ToListAsync();

            var dataInicio = DateTime.Now.AddDays(-1).Date;
            var dataFim = DateTime.Now.AddDays(-1).Date.AddDays(1).AddTicks(-1);

            TimeZoneInfo brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

            var erros = new List<string>();
            int contasAtualizadasComSucesso = 0;

            foreach (var conta in contasBancarias)
            {
                try
                {
                    var query = await _pluggyService.GetAllTransactionsAsync(
                        Guid.Parse(conta.AccountIdOpenFinance),
                        dataInicio,
                        dataFim);

                    await context.Extrato
                        .Where(x => x.DataLancamento.Date >= dataInicio.Date
                                && x.DataLancamento.Date <= dataFim.Date
                                && x.IdClienteContaBancaria == conta.IdClienteContaBancaria)
                        .ExecuteDeleteAsync();

                    var listaNovoExtrato = query.Select(item => new Extrato(
                        conta,
                        conta?.Cliente,
                        $"{item?.Description ?? string.Empty} {item?.DescriptionRaw ?? string.Empty}".Trim(),
                        item?.Amount ?? 0,
                        item?.Type == "CREDIT" ? OFXTransactionType.CREDIT : OFXTransactionType.DEBIT,
                        item?.Date != null
                            ? TimeZoneInfo.ConvertTimeFromUtc(item.Date.Value.ToUniversalTime(), brasiliaTimeZone)
                            : DateTime.MinValue,
                        item?.PaymentData?.Payer?.Name ?? string.Empty,
                        item?.PaymentData?.Payer?.DocumentNumber?.Value ?? string.Empty,
                        item?.Category,
                        item.PaymentData.Payer?.RoutingNumber,
                        item.PaymentData.PaymentMethod,
                        "extrato"
                    )).ToList();

                    if (listaNovoExtrato.Any())
                    {
                        await context.Extrato.AddRangeAsync(listaNovoExtrato);
                        await context.SaveChangesAsync();
                    }

                    contasAtualizadasComSucesso++;
                }
                catch (Exception ex)
                {
                    erros.Add($"Erro ao processar conta {conta.Conta} (ID: {conta.IdClienteContaBancaria}): {ex.Message}");
                }
            }

            // Envia e-mail com o resumo
            await EnviarEmailContasAtualizadas(contasAtualizadasComSucesso, erros.Count, erros);

            if (erros.Any())
                return Ok(new { sucesso = true, mensagem = "Processo concluído com erros", erros });

            return Ok(new { sucesso = true, mensagem = "Extratos atualizados com sucesso" });
        }

        private async Task EnviarEmailContasAtualizadas(int totalContasAtualizadas, int totalContasComErros, List<string> detalhesErros = null)
        {
            var dataProcessamento = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            var subject = $"CONCICARD - Atualização de Extratos: {totalContasAtualizadas} sucesso | {totalContasComErros} erros";

            var errosHtml = string.Empty;
            if (detalhesErros != null && detalhesErros.Any())
            {
                errosHtml = $@"
            <div style='margin-top: 20px;'>
                <h3 style='color: #c62828;'>Detalhes dos Erros:</h3>
                <ul style='background-color: #ffebee; padding: 15px 30px; border-radius: 5px;'>
                    {string.Join("", detalhesErros.Select(e => $"<li style='margin: 5px 0; color: #c62828;'>{e}</li>"))}
                </ul>
            </div>";
            }

            var templateHtml = $@"
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                .header {{ background-color: #1e88e5; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                .content {{ background-color: #f9f9f9; padding: 20px; border: 1px solid #ddd; }}
                .info-row {{ margin: 10px 0; padding: 10px; background-color: white; border-left: 4px solid #1e88e5; }}
                .info-row.success {{ border-left-color: #43a047; }}
                .info-row.error {{ border-left-color: #c62828; }}
                .label {{ font-weight: bold; color: #1e88e5; }}
                .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                .status-badge {{ padding: 5px 15px; border-radius: 20px; display: inline-block; margin: 5px; }}
                .badge-success {{ background-color: #e8f5e9; color: #2e7d32; }}
                .badge-error {{ background-color: #ffebee; color: #c62828; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h2>Atualização de Extratos Bancários</h2>
                </div>
                <div class='content'>
                    <p style='text-align: center;'>
                        <span class='status-badge badge-success'>✓ {totalContasAtualizadas} Atualizadas</span>
                        <span class='status-badge badge-error'>✗ {totalContasComErros} Erros</span>
                    </p>

                    <div class='info-row'>
                        <span class='label'>Data/Hora do Processamento:</span> {dataProcessamento}
                    </div>

                    <div class='info-row success'>
                        <span class='label' style='color: #43a047;'>Contas Atualizadas com Sucesso:</span> {totalContasAtualizadas}
                    </div>

                    <div class='info-row error'>
                        <span class='label' style='color: #c62828;'>Contas com Erro:</span> {totalContasComErros}
                    </div>

                    {errosHtml}
                </div>
                <div class='footer'>
                    <p>Equipe Concicard - Sistema de Gestão</p>
                    <p>Este é um e-mail automático, por favor não responda.</p>
                </div>
            </div>
        </body>
        </html>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Sistema");
            mailMsg.To.Add(new MailAddress("renato@genialsoft.com.br"));
            mailMsg.To.Add(new MailAddress("kleytonwillian@gmail.com"));
            mailMsg.Subject = subject;
            mailMsg.IsBodyHtml = true;
            mailMsg.Body = templateHtml;

            smtpClientSendGrid.Credentials = credentialsSendGrid;
            await smtpClientSendGrid.SendMailAsync(mailMsg);
        }


        [HttpGet]
        [Route("atualizar-conta-bancaria-open-finance")]
        public async Task<IActionResult> AtualizarContaBancariaOpenFinanceAsync(int idClienteContaBancaria)
        {
            var clienteContaBancaria = context.ClienteContaBancaria.FirstOrDefault(x => x.IdClienteContaBancaria == idClienteContaBancaria);
            if (clienteContaBancaria == null)
                return BadRequest("Conta não encontrada");

            if (string.IsNullOrEmpty(clienteContaBancaria.ItemIdOpenFinance))
                return BadRequest("Item não vinculada ao Open Finance");

            var a = await _pluggyService.UpdateItemAsync(clienteContaBancaria.ItemIdOpenFinance);
            return Ok(a);
        }
    }
}


