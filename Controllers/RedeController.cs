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
using ERP_API.Domain.Entidades;
using ERP_API.Service.Parceiros;
using ERP_API.Service.Parceiros.Interface;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Identity.Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Data.Entity;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Policy;
using ERP_API.Models;
using static ERP_API.Service.Parceiros.ConsultaVendaRedeResponseModel;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class RedeController : ControllerBase
    {
        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        private readonly IConfiguration _configuration;
        protected Context context;
        private readonly IRedeService _redeService;
        public RedeController(Context context, IRedeService redeService, IConfiguration configuration)
        {
            this.context = context;
            _redeService = redeService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("recebeExtratoRede")]
        [AllowAnonymous]
        public IActionResult RecebeExtratoRede([FromBody] RecebeExtratoRedeWebhook model)
        {
            var recebeExtratoRedewebook = new LogWebhookExtratoTecnospeed(model.Data, model.Happen, model.Balance, model.UniqueId, model.CreatedAt, model.AccountHash, User.Identity.Name);
            context.LogWebhookExtratoTecnopeed.Add(recebeExtratoRedewebook);
            context.SaveChanges();
            return Ok();
        }

        //[NonAction]
        //public async Task ConsultaPorUniqueId (string uniqueId)
        //{
            //var retorno = await _redeService.Cons
        //}

        [HttpPost]
        [Route("buscar-transacao")]
        [AllowAnonymous]
        public async Task<IActionResult> BuscarTransacao([FromBody] IntegrarTransacaoModelRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);
            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == model.IdOperadora);
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
                    .Where(x => x.IdUnidade == model.IdUnidade && x.Situacao == "Ativo").ToList();
            }

            DateTime dataInicio =   model.DataInicio;
            DateTime dataFim =  model.DataTermino;

            if(dataInicio > dataFim)
                return BadRequest("Data Inicio não pode ser maior que Data Fim");
            if ((dataFim - dataInicio).TotalDays > 62)
                return BadRequest("O espaço entre a Data de Início e a Data de Fim não pode ser maior que 62 dias");

            var transacoesExistentes = context.Transacao
                     .Where(t => 
                                 t.DataMovimentacao.Value.Date >= dataInicio.Date && 
                                 t.DataMovimentacao.Value.Date <= dataFim.Date && 
                                 t.IdUnidade == model.IdUnidade &&
                                 t.NomeOperadora == "Rede")
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


                var username = unidadeParametro.FirstOrDefault(x => x.Chave == "UserName")?.Valor;
                if (string.IsNullOrEmpty(username))
                    continue;
                var password = unidadeParametro.FirstOrDefault(x => x.Chave == "Password")?.Valor;
                if (string.IsNullOrEmpty(password))
                    continue;
                var url = unidadeParametro.FirstOrDefault(x => x.Chave == "UrlProducao")?.Valor;
                if (string.IsNullOrEmpty(url))
                    continue;
                var parentCompanyNumber = unidadeParametro.FirstOrDefault(x => x.Chave == "Matriz")?.Valor;
                if (string.IsNullOrEmpty(parentCompanyNumber))
                    continue;
                var subsidiaries = unidadeParametro.FirstOrDefault(x => x.Chave == "Filial")?.Valor;
                if (string.IsNullOrEmpty(subsidiaries))
                    continue;

                var loginRequest = new LoginRequestModel
                {
                    UserName = username,
                    Password = password,
                    Url = url + "/oauth2/token"
                };
                LoginResponseModel responseLogin;
                try
                {
                    responseLogin = await _redeService.LoginRedeAsync(loginRequest);
                }

                catch(Exception ex)
                {
                    return BadRequest($"Erro ao consultar pagamento: {ex.Message}");
                }
                while (dataInicio <= dataFim)
                {
                    string pageKey = null; 
                    bool hasNextKey;

                    do
                    {
                        var transacoesRede = new ConsultarVendaRedeRequestModel
                        {
                            Authorization = responseLogin.access_token,
                            ParentMerchantId = parentCompanyNumber,
                            Subsidiaries = subsidiaries,
                            StartDate = dataInicio.ToString("yyyy-MM-dd"),
                            EndDate = dataInicio.ToString("yyyy-MM-dd"),
                            Url = url + "/merchant-statement/v1/sales",
                            PageKey = pageKey
                        };

                        ConsultaVendaResponseModel responseVenda;

                        try
                        {
                             responseVenda = await _redeService.ConsultaVendaRedeAsync(transacoesRede);
                        }

                        catch (Exception ex)
                        {
                            return BadRequest($"Erro ao consultar transacao: {ex.Message}");
                        }

                        if (responseVenda?.Content?.Transactions != null && responseVenda.Content.Transactions.Any())
                        {
                           

                            var transacoes = new List<Transacao>();

                            foreach (var transaction in responseVenda.Content.Transactions)
                            {
                                var bandeira = context.Bandeira.FirstOrDefault(x => x.CodigoBandeiraCartaoRede == transaction.BrandCode.ToString());

                                var transacao = new Transacao(
                                    transaction.Merchant.CompanyName,
                                    unidade,
                                    empresa,
                                    transaction.Amount,
                                    transaction.MdrAmount,
                                    transaction.DiscountAmount,
                                    transaction.NetAmount,
                                    string.IsNullOrEmpty(transaction.MovementDate) ? (DateTime?)null : DateTime.Parse(transaction.MovementDate),
                                    string.IsNullOrEmpty(transaction.SaleDate) ? (DateTime?)null : DateTime.Parse(transaction.SaleDate),
                                    transaction.Modality.Type,
                                    bandeira?.NomeBandeira,
                                    operadora,
                                    operadora.NomeOperadora,
                                    "Não conciliado",
                                    transaction.Nsu.ToString(),
                                    transaction.InstallmentQuantity,
                                    transaction.SaleSummaryNumber,
                                    transaction.Status,
                                    transaction.Device,
                                    transaction.ChargebackStatus,
                                    transaction.CaptureTypeCode,
                                    transaction.Flex,
                                    transaction.FlexAmount,
                                    transaction.FeeTotal,
                                    transaction.BoardingFeeAmount,
                                    transaction.Tokenized,
                                    transaction.Tid,
                                    transaction.OrderNumber,
                                    transaction.CardNumber,
                                    transaction.Modality?.Product,
                                    transaction.Modality?.Type,
                                    transaction.Modality?.ProductCode.ToString(),
                                    User.Identity.Name);

                                transacoes.Add(transacao);
                            }
                            context.Set<Transacao>().AddRange(transacoes);
                            await context.SaveChangesAsync();
                        }

                        hasNextKey = responseVenda.Cursor.HasNextKey;
                        pageKey = responseVenda.Cursor.NextKey;

                    } while (hasNextKey);

                    dataInicio = dataInicio.AddDays(1);
                }

                var qtdeTransacao = context.Transacao.Count(x => x.DataMovimentacao.Value.Date == DateTime.UtcNow.Date.AddDays(-1) && x.IdUnidade == unidade.IdUnidade);
                totalBruto = context.Transacao.Where(x => x.IdUnidade == unidade.IdUnidade && x.DataMovimentacao.Value.Date == DateTime.UtcNow.Date.AddDays(-1)).Sum(x => x.ValorBruto);
                totalLiquido = context.Transacao.Where(x => x.IdUnidade == unidade.IdUnidade && x.DataMovimentacao == DateTime.UtcNow.Date.AddDays(-1)).Sum(x => x.ValorLiquido);
                totalDespesa = context.Transacao.Where(x => x.IdUnidade == unidade.IdUnidade && x.DataMovimentacao == DateTime.UtcNow.Date.AddDays(-1)).Sum(x => x.Despesa);
                EnviarEmailResumoRede(unidade.Nome, unidade.Email, qtdeTransacao, totalBruto, totalLiquido, totalDespesa);
            }
          
            return Ok(new { message = "Transações processadas com sucesso!" });
        }

        [HttpPost]
        [Route("pesquisarConsultarPagamento")]
        [AllowAnonymous]

        public async Task<IActionResult> PesquisarConsultarPagamento([FromBody] PesquisarConsultaPagamento model)
        {
            var usuariologado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
             var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuariologado.IdEmpresa);

            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == model.IdOperadora);
            if (operadora == null)
                return BadRequest("Adquirente sem cadastro ativo no contrato");
            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);
            if (unidade == null || model.IdUnidade == 0)
                return BadRequest("Unidade não encontrada");
            var unidadeParametro = context.UnidadeParametro.Where(x => x.IdUnidade == model.IdUnidade && x.IdOperadora == model.IdOperadora);
            if (!unidadeParametro.Any())
                return BadRequest("Parametros da unidade não encontrados");
            var username = unidadeParametro.FirstOrDefault(x => x.Chave == "UserName")?.Valor;
            var password = unidadeParametro.FirstOrDefault(x => x.Chave == "Password")?.Valor;
            var url = unidadeParametro.FirstOrDefault(x => x.Chave == "UrlProducao")?.Valor;
            var parentCompanyNumber = unidadeParametro.FirstOrDefault(x => x.Chave == "Matriz")?.Valor;
            var subsidiaries = unidadeParametro.FirstOrDefault(x => x.Chave == "Filial")?.Valor;


            var loginRequest = new LoginRequestModel
            {
                UserName = username,
                Password = password,
                Url = url + "/oauth2/token"
            };

            var pagamentos = new List<Pagamento>();

            LoginResponseModel responseLogin;

            try
            {
                responseLogin = await _redeService.LoginRedeAsync(loginRequest);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao fazer Login: {ex.Message}");
            }

            string pageKey = null;
            bool hasNextKey;

            if (model.DataInicio > model.DataTermino)
                return BadRequest("Data Inicio não pode ser maior que Data Termino");

            if ((model.DataTermino - model.DataInicio).TotalDays > 30 )
                return BadRequest("O espaço entre a Data de Início e a Data de Termino não pode ser maior que 30 dias");

            do
            {
                var transacoesRede = new ConsultaPagamentoRedeRequestModel
                {
                    Authorization = responseLogin.access_token,
                    ParentCompanyNumber = parentCompanyNumber,
                    Subsidiaries = subsidiaries,
                    StartDate = model.DataInicio.Date.ToString("yyyy-MM-dd"),
                    EndDate = model.DataTermino.Date.ToString("yyyy-MM-dd"),
                    Url = url + "/merchant-statement/v1/payments",
                    PageKey = pageKey
                };

                ConsultaPagamentoRedeResponseModel responsePagamento;

                try
                {
                    responsePagamento = await _redeService.ConsultaPagamentoRedeAsync(transacoesRede);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Erro ao consultar pagamento: {ex.Message}");
                }


                var pagamentosExistentes = context.Pagamento.Where(x => x.IdUnidade == model.IdUnidade 
                                                                   && x.DataPagamento.Value.Date >= model.DataInicio.Date && x.DataPagamento.Value.Date <= model.DataTermino.Date
                                                                   && x.IdOperadora == model.IdOperadora).ToList();
                if (pagamentosExistentes.Any())
                {
                    context.Pagamento.RemoveRange(pagamentosExistentes);
                    await context.SaveChangesAsync();
                }

                if (responsePagamento?.Content?.Payments != null && responsePagamento.Content.Payments.Any())
                {

                    foreach (var payment in responsePagamento?.Content?.Payments)
                    {
                        var bandeira = context.Bandeira.FirstOrDefault(x => x.CodigoBandeiraCartaoRede == payment.BrandCode.ToString());
                        if(bandeira == null)
                            return BadRequest("Bandeira não encontrada");
                        var banco = context.Banco.FirstOrDefault(x => x.CodigoBancoTecnoSpeed == payment.BankCode.ToString());
                        if (banco == null)
                            return BadRequest("Banco não encontrado");

                        var pagamento = new Pagamento(operadora,
                                                      empresa,
                                                      unidade, 
                                                      DateTime.Parse(payment.PaymentDate), 
                                                      banco, 
                                                      payment.BankCode.ToString(), 
                                                      banco.NomeBanco, 
                                                      payment.BankBranchCode.ToString(), 
                                                      payment.AccountNumber.ToString(), 
                                                      bandeira, 
                                                      payment.BrandCode.ToString(), 
                                                      bandeira.NomeBandeira, 
                                                      payment.CompanyName, 
                                                      payment.NetAmount, payment.Status, payment.Type, User.Identity.Name);
                        pagamentos.Add(pagamento);
                    }

                    context.Pagamento.AddRange(pagamentos);
                }

                hasNextKey = responsePagamento.Cursor.HasNextKey;
                pageKey = responsePagamento.Cursor.NextKey;

            } while (hasNextKey);

            await context.SaveChangesAsync();

            var retornoPagamentos = context.Pagamento.
                                     Where(x => x.IdUnidade == model.IdUnidade && 
                                     x.DataPagamento.Value.Date >= model.DataInicio.Date && 
                                     x.DataPagamento.Value.Date <= model.DataTermino.Date).AsQueryable().Select( m => new
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

        [HttpGet]
        [Route("consultarPagamento")]
        [AllowAnonymous]

        public async Task<IActionResult> ConsultarPagamento()
        {
            var loginRequest = new LoginRequestModel
            {
                UserName = "4277bc9c-ff93-4661-9cd8-d34fec7ac55c",
                Password = "7lKUVde12z",
                Url = "https://api.userede.com.br/redelabs/oauth2/token"
            };

            var pagamentos = new List<PaymentResponse>();

            LoginResponseModel responseLogin;
            try
            {
                 responseLogin = await _redeService.LoginRedeAsync(loginRequest);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao fazer Login: {ex.Message}");
            }

            string pageKey = null;
            bool hasNextKey;

            do
            {
                var transacoesRede = new ConsultaPagamentoRedeRequestModel
                {
                    Authorization = responseLogin.access_token,
                    ParentCompanyNumber = "77511751",
                    Subsidiaries = "77511751",
                    StartDate = "2025-02-01",
                    EndDate = "2025-02-10",
                    Url = "https://api.userede.com.br/redelabs/merchant-statement/v1/payments",
                    PageKey = pageKey
                };
                ConsultaPagamentoRedeResponseModel responsePagamento;

                try
                {
                    responsePagamento = await _redeService.ConsultaPagamentoRedeAsync(transacoesRede);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Erro ao consultar pagamento: {ex.Message}");
                }

                if (responsePagamento?.Content?.Payments != null && responsePagamento.Content.Payments.Any())
                {

                    foreach (var payment in responsePagamento?.Content?.Payments)
                    {

                        pagamentos.Add(payment);
                    }
                }

                hasNextKey = responsePagamento.Cursor.HasNextKey;
                pageKey = responsePagamento.Cursor.NextKey;

            } while (hasNextKey);

            return Ok(pagamentos);

        }

        [HttpGet]
        [Route("consultarPagamentoDiario")]
        [AllowAnonymous]

        public async Task<IActionResult> ConsultarPagamentoDiario()
        {
            var loginRequest = new LoginRequestModel
            {
                UserName = "4277bc9c-ff93-4661-9cd8-d34fec7ac55c",
                Password = "7lKUVde12z",
                Url = "https://api.userede.com.br/redelabs/oauth2/token"
            };

            var pagamentos = new List<PaymentsDailyResponse>();

            LoginResponseModel responseLogin;

            try
            {
                responseLogin = await _redeService.LoginRedeAsync(loginRequest);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao fazer Login: {ex.Message}");
            }

                var transacoesRede = new ConsultarPagamentoDiarioRedeRequestModel
                {
                    Authorization = responseLogin.access_token,
                    ParentCompanyNumber = 77511751,
                    StartDate = "2025-02-01",
                    EndDate = "2025-02-10",
                    Url = "https://api.userede.com.br/redelabs/merchant-statement/v2/payments/daily",
                };

                var responsePagamentoDiario = await _redeService.ConsultarPagamentoDiarioRedeAsync(transacoesRede);
               
                if (responsePagamentoDiario?.Content?.PaymentsDaily != null && responsePagamentoDiario.Content.PaymentsDaily.Any())
                {

                    foreach (var payment in responsePagamentoDiario?.Content?.PaymentsDaily)
                    {

                        pagamentos.Add(payment);
                    }
                }

             return Ok(pagamentos);

        }

        [NonAction]
        private void EnviarEmailResumoRede(string empresa, string email, int qtdeTransacoesProcessadas, decimal? totalBruto, decimal? totalLiquido, decimal? totalDespesa)
        {
            string subject = empresa + "Só Varejo Integração Rede ITAÚ";
            string templateHtml = $@"
                        <h1>Só Varejo Integração Rede ITAÚ</h1>
                        
                        <p><strong>Data:</strong> {DateTime.UtcNow.Date.AddDays(-1).ToString("dd/MM/yyyyy")}</p>
                        <p><strong>Empresa:</strong> {empresa}</p>
                        <p><strong>Transações:</strong> {qtdeTransacoesProcessadas}</p>
                        <p><strong>Valor Total Bruto:</strong> {totalBruto.GetValueOrDefault().ToString("C", new System.Globalization.CultureInfo("pt-BR"))}</p>
                        <p><strong>Valor Total Líquido:</strong> {totalLiquido.GetValueOrDefault().ToString("C", new System.Globalization.CultureInfo("pt-BR"))}</p>
                        <p><strong>Valor Total de Despesas:</strong> {totalDespesa.GetValueOrDefault().ToString("C", new System.Globalization.CultureInfo("pt-BR"))}</p>
                   

                        <p>Equipe Só Varejo</p>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Confirmação de Cadastro");
            mailMsg.Subject = subject;
            mailMsg.To.Add(new MailAddress(email, "Só Varejo Integração Rede ITAÚ"));
            mailMsg.CC.Add(new MailAddress("kleytonwillian@gmail.com", "Kleyton"));
            mailMsg.CC.Add(new MailAddress("renato@genialsoft.com.br", "Renato"));
            string html = templateHtml;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));


            smtpClientSendGrid.Credentials = credentialsSendGrid;

            try
            {
                smtpClientSendGrid.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
        }


    }
}
