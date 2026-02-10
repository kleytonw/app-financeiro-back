using ERP.Infra;
using ERP_API.Service.Parceiros;
using ERP_API.Service.Parceiros.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Data.Entity;
using System.Collections.Generic;

namespace ERP_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    public class ConciliadoraDashboardController : ControllerBase
    {
        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        protected Context context;
        protected IConciliadoraDashBoardService conciliadoraDashBoardService;
        private IConfiguration _config;

        public ConciliadoraDashboardController(Context context, IConciliadoraDashBoardService conciliadoraService, IConfiguration config)
        {
            this.context = context;
            this.conciliadoraDashBoardService = conciliadoraService;
            _config = config;
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> ConciliadoraLoginDashboard()
        {
            string username = "operacoes@sovarejo.com";
            string password = "F285C92FDCFDAE0B68FA287B7E8C11A2FED5C94D";

            var responseAuthetication = await conciliadoraDashBoardService.LoginAsync(username, password);

            return Ok(responseAuthetication);
        }


        [HttpPost]
        [Route("GetVendas")]

        public async Task<IActionResult> ConciliadoraVendasDashboard([FromBody] ConciliadoraDashboardRequest model)
        {

            var responseDashboard = await conciliadoraDashBoardService.GetVendasAsync(model.Token, model.Body);

            return Ok(responseDashboard);
        }

        [HttpPost]
        [Route("GetPagamentos")]
        public async Task<IActionResult> ConciliadoraPagamentosDashboard([FromBody] ConciliadoraDashboardRequest model)
        {

            var responseDashboard = await conciliadoraDashBoardService.GetPagamentosAsync(model.Token, model.Body);

            return Ok(responseDashboard);
        }

        [HttpPost]
        [Route("GetDebitos")]
        public async Task<IActionResult> ConciliadoraDebitosDashboard([FromBody] ConciliadoraDashboardRequest model)
        {

            var responseDashboard = await conciliadoraDashBoardService.GetDebitosAsync(model.Token, model.Body);

            return Ok(responseDashboard);
        }

        [HttpPost]
        [Route("GetTaxas")]
        public async Task<IActionResult> ConciliadoraTaxasDashboard([FromBody] ConciliadoraDashboardRequest model)
        {

            var responseDashboard = await conciliadoraDashBoardService.GetTaxasAsync(model.Token, model.Body);

            return Ok(responseDashboard);
        }

        [HttpPost]
        [Route("GetInformacoesComplementares")]
        public async Task<IActionResult> ConciliadoraInformacoesComplementaresDashboard([FromBody] ConciliadoraDashboardRequest model)
        {

            var responseDashboard = await conciliadoraDashBoardService.GetInformacoesComplementaresAsync(model.Token, model.Body);

            return Ok(responseDashboard);
        }

        [HttpPost]
        [Route("previaEmail")]
        public async Task<IActionResult> PreviaEmail([FromBody] PreviaEmailRequest model)
        {
            var responseDashboard = await conciliadoraDashBoardService.GetTaxasAsync(model.Data.Token, model.Data.Body);
            var resoponseDashboardInformacoesComplementares = await conciliadoraDashBoardService.GetInformacoesComplementaresAsync(model.Data.Token, model.Data.Body);

            var cultura = CultureInfo.GetCultureInfo("pt-BR");
            var dt = DateTime.Parse(model.Data.Body.filter.StartDate, cultura);
            var mesExtensoDe = dt.ToString("MMMM 'de' yyyy", cultura);
            

            foreach (var refoIds in model.Data.Body.filter.RefOIds)
            {
                var cliente = context.Cliente.Include(x => x.Pessoa).FirstOrDefault(x => x.IdentificadorConciliadora == refoIds.ToString());

                var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == cliente.IdPessoa);

                string subject = $"Relatório de {mesExtensoDe} Concicard - {pessoa.Nome}";

                // Construir o HTML do email
                string templateHtml = GetEmailTemplate();

                // Substituir placeholders do cabeçalho
                templateHtml = templateHtml.Replace("{NOME_CLIENTE}", cliente.Pessoa.Nome);
                templateHtml = templateHtml.Replace("{MES_ANO}", mesExtensoDe);

                // Construir HTML dos adquirentes
                StringBuilder adquirentesHtml = new StringBuilder();

                // Assumindo que responseDashboard tem uma lista de adquirentes
                // Ajuste conforme a estrutura real do seu objeto
                foreach (var adquirente in responseDashboard.TaxasVendas)
                {
                    string adquirenteSection = $@"
                        <div class='adquirente-section'>
                            <div class='adquirente-header'>
                                <div class='adquirente-name'>{adquirente.Adquirente}</div>
                            </div>
                            <div class='metrics-grid'>
                                <div class='metric-item'>
                                    <div class='metric-label'>Valor Bruto</div>
                                    <div class='metric-value bruto'>R$ {adquirente.ValorBruto:N2}</div>
                                </div>
                                <div class='metric-item'>
                                    <div class='metric-label'>Valor Líquido</div>
                                    <div class='metric-value liquido'>R$ {adquirente.ValorLiquido:N2}</div>
                                </div>
                                <div class='metric-item'>
                                    <div class='metric-label'>Taxa Média</div>
                                    <div class='metric-value taxa'>{(adquirente.TaxaMedia/100):P2}</div>
                                </div>
                            </div>
                        </div>";

                    adquirentesHtml.AppendLine(adquirenteSection);
                }

                templateHtml = templateHtml.Replace("{ADQUIRENTES_LOOP}", adquirentesHtml.ToString());


                // Configurar e enviar email
                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Relatórios");
                mailMsg.Subject = subject;


                //mailMsg.To.Add(new MailAddress(cliente.Pessoa.Email.Trim()));
                mailMsg.To.Add(new MailAddress(model.Email));

                // Adicionar o HTML ao email
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = templateHtml;
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
                templateHtml, null, MediaTypeNames.Text.Html));

                smtpClientSendGrid.Credentials = credentialsSendGrid;

                try
                {
                    await smtpClientSendGrid.SendMailAsync(mailMsg);
                }
                catch (Exception ex)
                {
                    // Log do erro aqui
                    throw new Exception($"Erro ao enviar email para {cliente.Pessoa.Nome}: {ex.Message}");
                }
            }

            return Ok(new { message = "Emails enviados com sucesso!" });
        }

        [HttpPost]
        [Route("enviarEmail")]
        public async Task<IActionResult> EnviarEmail([FromBody] ConciliadoraDashboardRequest model)
        {
            var responseDashboard = await conciliadoraDashBoardService.GetTaxasAsync(model.Token, model.Body);

            var cultura = CultureInfo.GetCultureInfo("pt-BR");
            var dt = DateTime.Parse(model.Body.filter.StartDate, cultura);
            var mesExtensoDe = dt.ToString("MMMM 'de' yyyy", cultura);

            foreach (var refoIds in model.Body.filter.RefOIds)
            {
                var cliente = context.Cliente.Include(x => x.Pessoa).FirstOrDefault(x => x.IdentificadorConciliadora == refoIds.ToString());

                var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == cliente.IdPessoa);

                string subject = $"Relatório de {mesExtensoDe} Concicard - {pessoa.Nome}";

                // Construir o HTML do email
                string templateHtml = GetEmailTemplate();

                // Substituir placeholders do cabeçalho
                templateHtml = templateHtml.Replace("{NOME_CLIENTE}", cliente.Pessoa.Nome);
                templateHtml = templateHtml.Replace("{MES_ANO}", mesExtensoDe);

                // Construir HTML dos adquirentes
                StringBuilder adquirentesHtml = new StringBuilder();

                // Assumindo que responseDashboard tem uma lista de adquirentes
                // Ajuste conforme a estrutura real do seu objeto
                foreach (var adquirente in responseDashboard.TaxasVendas)
                {
                    string adquirenteSection = $@"
                        <div class='adquirente-section'>
                            <div class='adquirente-header'>
                                <div class='adquirente-name'>{adquirente.Adquirente}</div>
                            </div>
                            <div class='metrics-grid'>
                                <div class='metric-item'>
                                    <div class='metric-label'>Valor Bruto</div>
                                    <div class='metric-value bruto'>R$ {adquirente.ValorBruto:N2}</div>
                                </div>
                                <div class='metric-item'>
                                    <div class='metric-label'>Valor Líquido</div>
                                    <div class='metric-value liquido'>R$ {adquirente.ValorLiquido:N2}</div>
                                </div>
                                <div class='metric-item'>
                                    <div class='metric-label'>Taxa Média</div>
                                    <div class='metric-value taxa'>{(adquirente.TaxaMedia / 100):P2}</div>
                                </div>
                            </div>
                        </div>";

                    adquirentesHtml.AppendLine(adquirenteSection);
                }

                templateHtml = templateHtml.Replace("{ADQUIRENTES_LOOP}", adquirentesHtml.ToString());


                // Configurar e enviar email
                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Relatórios");
                mailMsg.Subject = subject;


                mailMsg.To.Add(new MailAddress(pessoa.Email.Trim()));
                mailMsg.To.Add(new MailAddress("kleytonwillian@gmail.com"));
                mailMsg.To.Add(new MailAddress("renato@genialsoft.com.br"));

                // Adicionar o HTML ao email
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = templateHtml;
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
                templateHtml, null, MediaTypeNames.Text.Html));

                smtpClientSendGrid.Credentials = credentialsSendGrid;

                try
                {
                    await smtpClientSendGrid.SendMailAsync(mailMsg);
                }
                catch (Exception ex)
                {
                    // Log do erro aqui
                    throw new Exception($"Erro ao enviar email para {cliente.Pessoa.Nome}: {ex.Message}");
                }
            }

            return Ok(new { message = "Emails enviados com sucesso!" });
        }



        private string GetEmailTemplate()
        {
            return @"<!DOCTYPE html>
                <html lang='pt-BR'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        body {
                            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                            line-height: 1.6;
                            color: #333;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }
                        .container {
                            max-width: 700px;
                            margin: 20px auto;
                            background-color: #ffffff;
                            border-radius: 10px;
                            box-shadow: 0 0 20px rgba(0,0,0,0.1);
                            overflow: hidden;
                        }
                        .header {
                            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                            color: white;
                            padding: 30px;
                            text-align: center;
                        }
                        .header h1 {
                            margin: 0;
                            font-size: 28px;
                            font-weight: 600;
                        }
                        .header p {
                            margin: 10px 0 0 0;
                            font-size: 16px;
                            opacity: 0.95;
                        }
                        .content {
                            padding: 30px;
                        }
                        .greeting {
                            font-size: 18px;
                            color: #555;
                            margin-bottom: 25px;
                            border-left: 4px solid #667eea;
                            padding-left: 15px;
                        }
                        .summary-box {
                            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
                            border-radius: 8px;
                            padding: 20px;
                            margin-bottom: 30px;
                        }
                        .summary-title {
                            font-size: 18px;
                            font-weight: 600;
                            color: #333;
                            margin-bottom: 15px;
                        }
                        .summary-grid {
                            display: grid;
                            grid-template-columns: repeat(2, 1fr);
                            gap: 15px;
                        }
                        .summary-item {
                            background: white;
                            padding: 12px;
                            border-radius: 6px;
                            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
                        }
                        .summary-label {
                            font-size: 12px;
                            color: #666;
                            text-transform: uppercase;
                            letter-spacing: 0.5px;
                            margin-bottom: 5px;
                        }
                        .summary-value {
                            font-size: 20px;
                            font-weight: bold;
                            color: #333;
                        }
                        .adquirente-section {
                            margin-bottom: 25px;
                            background: #fafafa;
                            border-radius: 8px;
                            padding: 20px;
                            border-left: 4px solid #667eea;
                        }
                        .adquirente-header {
                            display: flex;
                            align-items: center;
                            margin-bottom: 15px;
                            padding-bottom: 10px;
                            border-bottom: 2px solid #e0e0e0;
                        }
                        .adquirente-icon {
                            width: 40px;
                            height: 40px;
                            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                            border-radius: 50%;
                            display: flex;
                            align-items: center;
                            justify-content: center;
                            margin-right: 15px;
                            color: white;
                            font-weight: bold;
                            font-size: 18px;
                        }
                        .adquirente-name {
                            font-size: 20px;
                            font-weight: 600;
                            color: #333;
                        }
                        .metrics-grid {
                            display: grid;
                            grid-template-columns: repeat(3, 1fr);
                            gap: 15px;
                        }
                        .metric-item {
                            background: white;
                            padding: 15px;
                            border-radius: 6px;
                            text-align: center;
                            box-shadow: 0 2px 8px rgba(0,0,0,0.05);
                        }
                        .metric-label {
                            font-size: 12px;
                            color: #666;
                            text-transform: uppercase;
                            letter-spacing: 0.5px;
                            margin-bottom: 8px;
                        }
                        .metric-value {
                            font-size: 18px;
                            font-weight: bold;
                            color: #333;
                        }
                        .metric-value.bruto {
                            color: #4CAF50;
                        }
                        .metric-value.liquido {
                            color: #2196F3;
                        }
                        .metric-value.taxa {
                            color: #FF9800;
                        }
                        .footer {
                            background: #f8f9fa;
                            padding: 25px;
                            text-align: center;
                            border-top: 1px solid #e0e0e0;
                        }
                        .footer-text {
                            color: #666;
                            font-size: 14px;
                            margin-bottom: 10px;
                        }
                        .footer-company {
                            font-weight: 600;
                            color: #667eea;
                            font-size: 16px;
                        }
                        .divider {
                            height: 1px;
                            background: linear-gradient(to right, transparent, #e0e0e0, transparent);
                            margin: 30px 0;
                        }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Relatório Mensal Concicard</h1>
                            <p>{NOME_CLIENTE} - {MES_ANO}</p>
                        </div>
                        <div class='content'>
                            <div class='greeting'>
                                Prezado(a) {NOME_CLIENTE},<br>
                                Segue abaixo o relatório detalhado das suas transações com cartão.
                            </div>
                            <div class='summary-box'>
                                <div class='summary-title'>📊 Resumo Geral</div>
                                    {ADQUIRENTES_LOOP}
                            </div>
                        </div>
                        <div class='footer'>
                            <div class='footer-text'>
                                Este é um e-mail automático. Em caso de dúvidas, entre em contato conosco.
                            </div>
                            <div class='footer-company'>
                                Concicard - Gestão Inteligente de Cartões
                            </div>
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
    
}
