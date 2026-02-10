using ERP.Infra;
using ERP_API.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using ERP_API.Models;
using Dapper;
using ERP_API.Service.Parceiros.Interface;
using System.Threading.Tasks;
using ERP_API.Service.Parceiros;
using System.Net.Mail;
using MySqlX.XDevAPI;
using System.Net.Mime;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class FinanceiroController : ControllerBase
    {
        private IConfiguration _config;
        protected Context context;
        protected IUniqueService _uniqueService;

        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
        public FinanceiroController(Context context,
            IConfiguration config,
            IUniqueService uniqueService)   // : base(context)
        {
            _config = config;
            this.context = context;
            _uniqueService = uniqueService;
        }


        [HttpPost]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar([FromBody] FiltroFinanceiroModel model)
        {

            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlFields = $@" select IdFinanceiroParcela,
                                       Financeiro.IdFinanceiro,
                                       Financeiro.Nome,
                                       CpfCnpj,
                                       FinanceiroParcela.ValorVencimento,
                                       FinanceiroParcela.DataVencimento,
                                       FinanceiroParcela.DataAcerto,
                                       FinanceiroParcela.ValorAcerto,
                                       FinanceiroParcela.Situacao,
                                       FinanceiroParcela.NumeroNf,
                                       FinanceiroParcela.ValorAcrescimo,
                                       FinanceiroParcela.ValorDesconto,
                                       PlanoConta.Descricao,  
                                       FinanceiroParcela.IdentificadorBoletoUnique";

                var sqlBody = $@" from Financeiro 
                            inner join FinanceiroParcela on Financeiro.IdFinanceiro = FinanceiroParcela.IdFinanceiro
                            left JOIN PlanoConta ON PlanoConta.IdPlanoConta = FinanceiroParcela.IdPlanoConta
                            where Financeiro.Tipo = '{model.Tipo}'";

                if (model.TipoPeriodo == "Vencimento")
                {
                    sqlBody += $@" and CAST(FinanceiroParcela.DataVencimento as date) BETWEEN Convert(date, '{Convert.ToDateTime(model.DataInicial).ToString("yyyy-MM-dd")}', 23) AND Convert(date, '{Convert.ToDateTime(model.DataFinal).ToString("yyyy-MM-dd")}', 23)";

                }
                else if (model.TipoPeriodo == "Acerto")
                {
                    sqlBody += $@" and CAST(FinanceiroParcela.DataAcerto as date) BETWEEN Convert(date, '{Convert.ToDateTime(model.DataInicial).ToString("yyyy-MM-dd")}', 23) AND Convert(date, '{Convert.ToDateTime(model.DataFinal).ToString("yyyy-MM-dd")}', 23)";
                }
                else
                {
                    return BadRequest("Informe um Tipo Período ");
                }

                if (!string.IsNullOrEmpty(model.Nome))
                    sqlBody += $@" and Financeiro.Nome LIKE '%{model.Nome}%' ";

                if (!string.IsNullOrEmpty(model.CpfCnpj))
                    sqlBody += $@" and Financeiro.CpfCnpj = '{model.CpfCnpj}' ";

                if (model.Codigo > 0)
                    sqlBody += $@" and FinanceiroParcela.IdFinanceiroParcela = '{model.CpfCnpj}' ";


                if (model.Situacao != "Todos")
                    sqlBody += $@" and FinanceiroParcela.Situacao = '{model.Situacao}' ";


                sqlBody += @$"  Order by FinanceiroParcela.DataVencimento desc ";

                string sqlList = sqlFields + sqlBody;

                var lista = conn.Query<ListaItemFinanceiroModel>(sqlList).ToList();

                var retorno = new ListaFinanceiroModel
                {
                    TotalAberto = lista.Where(x => x.Situacao == "Aberto").Sum(x => x.ValorVencimento),
                    TotalBaixado = lista.Where(x => x.Situacao == "Baixado").Sum(x => x.ValorAcerto),
                    Itens = lista
                };

                return Ok(retorno);
            }
        }

        [HttpPost]
        [Route("listarMeusRecebimentos")]
        [Authorize]
        public IActionResult ListarMeusRecebimentos([FromBody] FiltroFinanceiroModel model)
        {

            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlFields = $@" select IdFinanceiroParcela,
                                       Financeiro.IdFinanceiro,
                                       Financeiro.Nome,
                                       CpfCnpj,
                                       FinanceiroParcela.ValorVencimento,
                                       FinanceiroParcela.DataVencimento,
                                       FinanceiroParcela.DataAcerto,
                                       FinanceiroParcela.ValorAcerto,
                                       FinanceiroParcela.Situacao,
                                       FinanceiroParcela.NumeroNf,
                                       FinanceiroParcela.ValorAcrescimo,
                                       FinanceiroParcela.ValorDesconto,
                                       PlanoConta.Descricao,  
                                       FinanceiroParcela.IdentificadorBoletoUnique";

                var sqlBody = $@" from Financeiro 
                            inner join FinanceiroParcela on Financeiro.IdFinanceiro = FinanceiroParcela.IdFinanceiro
                            left JOIN PlanoConta ON PlanoConta.IdPlanoConta = FinanceiroParcela.IdPlanoConta
                            where Financeiro.Tipo = '{model.Tipo}' AND Financeiro.IdPessoa = '{usuarioLogado.IdPessoa}'";

                if (!string.IsNullOrEmpty(model.Nome))
                    sqlBody += $@" and Financeiro.Nome LIKE '%{model.Nome}%' ";

                if (!string.IsNullOrEmpty(model.CpfCnpj))
                    sqlBody += $@" and Financeiro.CpfCnpj = '{model.CpfCnpj}' ";

                if (model.Codigo > 0)
                    sqlBody += $@" and FinanceiroParcela.IdFinanceiroParcela = '{model.CpfCnpj}' ";


                if (model.Situacao != "Todos")
                    sqlBody += $@" and FinanceiroParcela.Situacao = '{model.Situacao}' ";


                sqlBody += @$"  Order by FinanceiroParcela.DataVencimento desc ";

                string sqlList = sqlFields + sqlBody;

                var lista = conn.Query<ListaItemFinanceiroModel>(sqlList).ToList();

                var retorno = new ListaFinanceiroModel
                {
                    TotalAberto = lista.Where(x => x.Situacao == "Aberto").Sum(x => x.ValorVencimento),
                    TotalBaixado = lista.Where(x => x.Situacao == "Baixado").Sum(x => x.ValorAcerto),
                    Itens = lista
                };

                return Ok(retorno);
            }
        }


        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] SalvarFinanceiroModel model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);

            var planoConta = context.PlanoConta.FirstOrDefault(x => x.IdPlanoConta == model.IdPlanoConta);

            if (pessoa == null)
                return BadRequest("Não foi possivel recuperar dados de pessoa");

            var financeiro = new Financeiro(pessoa, model.Tipo, usuarioLogado.Login);

            if (model.Total != model.Parcelas.Sum(x => x.Valor))
            {
                return BadRequest(" O Total está diferente do valor das parcelas ");
            }
            foreach (var item in model.Parcelas)
            {
                financeiro.AddParcela(new FinanceiroParcela(item.NumeroParcela, item.DataVencimento, item.Valor, model.Observacao, planoConta, usuarioLogado.Login));
            }

            context.Financeiro.Add(financeiro);
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("criarBoleto")]
        [Authorize]
        public async Task<IActionResult> CriarBoleto([FromBody] List<FinanceiroParcelaModel> model)
        {
            //var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name); // get usuario logado 

            foreach (var financeiroParcela in model)
            {
                var financeiro = context.Financeiro.Include(x => x.Parcelas).Include(x => x.Pessoa).FirstOrDefault(x => x.IdFinanceiro == financeiroParcela.IdFinanceiro);
                if (financeiro == null)
                    return BadRequest("Não foi possível recuperar dados do financeiro.");
                var financeiroParcelas = context.FinanceiroParcela.Include(x => x.Financeiro)
                    .Include(x => x.Financeiro.Pessoa).FirstOrDefault(x => x.IdFinanceiroParcela == financeiroParcela.IdFinanceiroParcela && x.IdFinanceiro == financeiroParcela.IdFinanceiro && x.Situacao == "Aberto");
                if (financeiroParcelas.IdentificadorBoletoUnique != 0 && financeiroParcela.IdentificadorBoletoUnique != null)
                    continue;
                if (financeiroParcelas == null)
                    return BadRequest("Não foi possível recuperar a parcela.");
                if (financeiroParcela.DataVencimento.Date < DateTime.Now.Date)
                    return BadRequest("Não é possivel gerar um boleto com a data vencida");

                var token = await _uniqueService.GerarAccessTokenAsync(_config["unique:login"], _config["unique:password"], _config["unique:url"]);

                var cobrancaRequest = new CobrancaRequest
                {
                    nomeSacado = financeiro.Nome,
                    bairroSacado = financeiro.Pessoa.Bairro,
                    boletoMensagem1 = "",
                    boletoMensagem2 = "",
                    boletoMensagem3 = "",
                    boletoMensagem4 = "",
                    cepSacado = financeiro.Pessoa.Cep,
                    cidadeSacado = financeiro.Pessoa.Cidade,
                    cpfCnpjSacado = financeiro.Pessoa.CpfCnpj,
                    dataVencimento = financeiroParcela.DataVencimento,
                    emailSacado = financeiro.Pessoa.Email,
                    enderecoSacado = financeiro.Pessoa.Logradouro,
                    identificador = financeiroParcela.IdFinanceiroParcela.ToString(),
                    numeroPedido = financeiroParcela.IdFinanceiroParcela.ToString(),
                    numeroSacado = financeiro.Pessoa.Numero,
                    splitsValores = new List<SplitValor>
                {
                    new SplitValor
                {
                    cobrarTarifa = true,
                    agencia = "000001",
                    conta = "000024",
                    valor = financeiroParcelas.ValorVencimento
                }
                 },
                    telefoneSacado = financeiro.Pessoa.Telefone1,
                    valor = financeiroParcelas.ValorVencimento,
                    valorDesconto = financeiroParcela.ValorDesconto ?? 0,
                    valorJuros = financeiroParcela.ValorAcrescimo ?? 0,
                    valorMulta = financeiroParcela.ValorAcrescimo ?? 0
                };

                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(cobrancaRequest, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                var responseuniqueService = await _uniqueService.CriarCobrancaAsync(cobrancaRequest, token, _config["unique:url"]);

                financeiroParcelas.SetIdentificadorBoletoUnique(responseuniqueService.Data.IdTransacao);
                context.FinanceiroParcela.Update(financeiroParcelas);
                context.SaveChanges();
            }

            return Ok();
        }
        [HttpGet]
        [Route("parcelaContrato")]
        [Authorize]
        public IActionResult ParcelaContrato(int id)
        {
            var lista = context.FinanceiroParcela
                .Include(x => x.Financeiro)
                .Where(x => x.Financeiro.IdPessoa == id)
                .Select(m => new
                {
                    m.IdFinanceiroParcela,
                    m.IdFinanceiro,
                    m.Financeiro.Nome,
                    m.ValorVencimento,
                    m.DataVencimento,
                    m.DataAcerto,
                    m.ValorAcerto,
                    m.Situacao
                })
                .OrderBy(x => x.DataVencimento)
                .ToList();

            return Ok(lista);
        }

        [HttpGet]
        [Route("enviarEmail")]
        [Authorize]
        public async Task<IActionResult> EnviarEmail(int idFinanceiroParcela)
        {
            try
            {
                var financeiroParcela = context.FinanceiroParcela
                    .Include(x => x.Financeiro)
                    .ThenInclude(f => f.Pessoa)
                    .FirstOrDefault(x => x.IdFinanceiroParcela == idFinanceiroParcela);

                if (financeiroParcela == null)
                    return BadRequest("Não foi possível recuperar a parcela.");

                var financeiro = financeiroParcela.Financeiro;
                if (financeiro == null)
                    return BadRequest("Não foi possível recuperar dados do financeiro.");

                var pessoa = financeiro.Pessoa;
                if (pessoa == null)
                    return BadRequest("Não foi possível recuperar dados da pessoa.");

                // Validações
                if (financeiroParcela.Situacao == "Baixado")
                    return BadRequest("Boleto já foi baixado");

                if (financeiroParcela.IdentificadorBoletoUnique == 0 || financeiroParcela.IdentificadorBoletoUnique == null)
                    return BadRequest("Boleto sem identificador");

                if (string.IsNullOrWhiteSpace(pessoa.Email))
                    return BadRequest("Cliente não possui e-mail cadastrado");

                // Monta o link do boleto
                var linkBoleto = $"https://app-uniquesec.com.br/imprimir-boleto/{financeiroParcela.IdentificadorBoletoUnique}";

                // Calcula status do vencimento
                var diasParaVencimento = (financeiroParcela.DataVencimento.Date - DateTime.Now.Date).Days;
                string statusVencimento = diasParaVencimento switch
                {
                    < 0 => $"<span style='color: #dc3545; font-weight: bold;'>VENCIDO há {Math.Abs(diasParaVencimento)} dia(s)</span>",
                    0 => "<span style='color: #ffc107; font-weight: bold;'>VENCE HOJE</span>",
                    _ => $"<span style='color: #28a745; font-weight: bold;'>Vence em {diasParaVencimento} dia(s)</span>"
                };

                // Monta o email HTML detalhado
                string subject = $"Boleto Concicard - Fatura #{financeiroParcela.IdFinanceiroParcela}";
                string body = $@"
<!DOCTYPE html>
<html lang='pt-BR'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 0 20px rgba(0,0,0,0.1);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            font-size: 24px;
        }}
        .content {{
            padding: 30px;
        }}
        .info-box {{
            background: #f8f9fa;
            border-left: 4px solid #667eea;
            padding: 15px;
            margin: 15px 0;
            border-radius: 4px;
        }}
        .info-row {{
            display: flex;
            justify-content: space-between;
            padding: 8px 0;
            border-bottom: 1px solid #e0e0e0;
        }}
        .info-row:last-child {{
            border-bottom: none;
        }}
        .info-label {{
            font-weight: 600;
            color: #666;
        }}
        .info-value {{
            color: #333;
            text-align: right;
        }}
        .valor-destaque {{
            font-size: 28px;
            font-weight: bold;
            color: #667eea;
            text-align: center;
            margin: 20px 0;
        }}
        .btn-boleto {{
            display: inline-block;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 15px 40px;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
            text-align: center;
            margin: 20px 0;
            font-size: 16px;
        }}
        .btn-boleto:hover {{
            opacity: 0.9;
        }}
        .footer {{
            background: #f8f9fa;
            padding: 20px;
            text-align: center;
            color: #666;
            font-size: 12px;
            border-top: 1px solid #e0e0e0;
        }}
        .alert {{
            padding: 15px;
            margin: 15px 0;
            border-radius: 5px;
            text-align: center;
        }}
        .alert-info {{
            background-color: #d1ecf1;
            border: 1px solid #bee5eb;
            color: #0c5460;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>💳 Boleto de Pagamento</h1>
            <p>Concicard - Gestão Inteligente de Cartões</p>
        </div>

        <div class='content'>
            <p>Prezado(a) <strong>{pessoa.Nome}</strong>,</p>
            <p>Segue abaixo os detalhes do seu boleto para pagamento:</p>

            <div class='info-box'>
                <h3 style='margin-top: 0; color: #667eea;'>📋 Informações do Boleto</h3>
                <div class='info-row'>
                    <span class='info-label'>Número da Fatura:</span>
                    <span class='info-value'>#{financeiroParcela.IdFinanceiroParcela}</span>
                </div>
            </div>

            <div class='info-box'>
                <h3 style='margin-top: 0; color: #667eea;'>👤 Dados do Cliente</h3>
                <div class='info-row'>
                    <span class='info-label'>Nome:</span>
                    <span class='info-value'>{pessoa.Nome}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>CPF/CNPJ:</span>
                    <span class='info-value'>{pessoa.CpfCnpj}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Email:</span>
                    <span class='info-value'>{pessoa.Email}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Telefone:</span>
                    <span class='info-value'>{pessoa.Telefone1 ?? "N/A"}</span>
                </div>
            </div>

            <div class='info-box'>
                <h3 style='margin-top: 0; color: #667eea;'>💰 Informações Financeiras</h3>
                <div class='info-row'>
                    <span class='info-label'>Data de Vencimento:</span>
                    <span class='info-value'>{financeiroParcela.DataVencimento:dd/MM/yyyy}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Status:</span>
                    <span class='info-value'>{statusVencimento}</span>
                </div>
                {(financeiroParcela.ValorDesconto.HasValue && financeiroParcela.ValorDesconto > 0 ? $@"
                <div class='info-row'>
                    <span class='info-label'>Desconto:</span>
                    <span class='info-value' style='color: #28a745;'>- R$ {financeiroParcela.ValorDesconto:N2}</span>
                </div>" : "")}
                {(financeiroParcela.ValorAcrescimo.HasValue && financeiroParcela.ValorAcrescimo > 0 ? $@"
                <div class='info-row'>
                    <span class='info-label'>Acréscimo/Juros:</span>
                    <span class='info-value' style='color: #dc3545;'>+ R$ {financeiroParcela.ValorAcrescimo:N2}</span>
                </div>" : "")}
            </div>

            <div class='valor-destaque'>
                R$ {financeiroParcela.ValorVencimento:N2}
            </div>

            <div style='text-align: center;'>
                <a href='{linkBoleto}' class='btn-boleto' target='_blank'>
                    🖨️ ACESSAR E IMPRIMIR BOLETO
                </a>
            </div>

            <div class='alert alert-info'>
                <strong>📌 Link direto do boleto:</strong><br>
                <a href='{linkBoleto}' target='_blank'>{linkBoleto}</a>
            </div>

            <p style='margin-top: 30px; font-size: 14px; color: #666;'>
                <strong>Instruções:</strong><br>
                • Clique no botão acima para visualizar e imprimir seu boleto<br>
                • O pagamento pode ser realizado em qualquer banco, lotérica ou via internet banking<br>
                • Após o pagamento, a baixa será processada automaticamente<br>
                • Em caso de dúvidas, entre em contato conosco
            </p>

            <p style='font-size: 12px; color: #999; margin-top: 20px;'>
                Caso já tenha realizado o pagamento, por favor, desconsidere este aviso.
            </p>
        </div>

        <div class='footer'>
            <p><strong>Concicard - Gestão Inteligente de Cartões</strong></p>
            <p>Este é um e-mail automático. Por favor, não responda.</p>
            <p>Em caso de dúvidas, entre em contato conosco.</p>
        </div>
    </div>
</body>
</html>";

                // Envia o email
                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Cobrança");
                mailMsg.Subject = subject;
                //mailMsg.To.Add(new MailAddress(pessoa.Email.Trim()));
                mailMsg.To.Add(new MailAddress("kleytonwillian@gmail.com"));
                mailMsg.To.Add(new MailAddress("renato@genialsoft.com.br"));
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = body;

                smtpClientSendGrid.Credentials = credentialsSendGrid;

                await smtpClientSendGrid.SendMailAsync(mailMsg);

                return Ok(new
                {
                    Message = "Email enviado com sucesso",
                    Destinatario = pessoa.Email,
                    IdFinanceiroParcela = idFinanceiroParcela,
                    IdentificadorBoleto = financeiroParcela.IdentificadorBoletoUnique,
                    LinkBoleto = linkBoleto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao enviar email: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("listaContaReceberTresDias")]
        [AllowAnonymous]
        public async Task<IActionResult> ListaContaReceberTresDias()
        {
            var dataLimite = DateTime.Now.Date.AddDays(3); // 3 dias antes do vencimento

                var lista = context.FinanceiroParcela
                 .Include(x => x.Financeiro)
                     .ThenInclude(f => f.Pessoa) // ⭐ Carrega Pessoa de uma vez
                 .Where(x => x.Financeiro.Tipo == "Contas a Receber"
                     && x.Situacao == "Aberto"
                     && x.DataVencimento.Date == dataLimite
                     &&(x.DataEnvioLembrete == null || x.DataEnvioLembrete.Value.Date != dataLimite))
                 .OrderBy(x => x.DataVencimento)
                 .ToList();

            foreach (var item in lista)
            {

                var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == item.Financeiro.IdPessoa);

                string subject = $"Lembrete de pagamento Concicard -  fatura {item.IdFinanceiroParcela}";

                string body= $@"<p>Prezado(a) {item.Financeiro.Nome},</p>
                                <p>Este é um lembrete amigável de que sua fatura com vencimento em {item.DataVencimento.ToString("dd/MM/yyyy")} no valor de R$ {item.ValorVencimento.ToString("F2")} está se aproximando.</p>
                                <p>Para evitar qualquer inconveniente, solicitamos que efetue o pagamento até a data de vencimento.</p>
                                <p>Caso já tenha realizado o pagamento, por favor, desconsidere este aviso.</p>
                                <p>Agradecemos pela sua atenção e cooperação.</p>
                                <p>Atenciosamente,</p>
                                <p>Equipe Concicard</p>";

                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Cobrança");
                mailMsg.Subject = subject;


                mailMsg.To.Add(new MailAddress(pessoa.Email.Trim()));
                mailMsg.To.Add(new MailAddress("kleytonwillian@gmail.com"));
                mailMsg.To.Add(new MailAddress("renato@genialsoft.com.br"));

                // Adicionar o HTML ao email
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = body;
                smtpClientSendGrid.Credentials = credentialsSendGrid;

                try
                {
                    await smtpClientSendGrid.SendMailAsync(mailMsg);
                    item.SetDataLemrebrete(DateTime.Now);
                    context.FinanceiroParcela.Update(item);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Log do erro aqui
                    throw new Exception($"Erro ao enviar email para {pessoa.Nome}: {ex.Message}");
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("listaContaReceberAtual")]
        [AllowAnonymous]
        public async Task<IActionResult> ListaContaReceberAtual()
        {
            try
            {
                var dataHoje = DateTime.Now.Date;

                var lista = context.FinanceiroParcela
                    .Include(x => x.Financeiro)
                    .ThenInclude(x => x.Pessoa)
                    .Where(x => x.Financeiro.Tipo == "Contas a Receber"
                        && x.Situacao == "Aberto"
                        && x.DataVencimento.Date == dataHoje
                        && (x.DataEnvioLembreteVencimento == null || x.DataEnvioLembreteVencimento.Value.Date != dataHoje)) // ⭐ Campo diferente
                    .OrderBy(x => x.DataVencimento)
                    .ToList();

                var resultados = new List<string>();
                var erros = new List<string>();

                foreach (var item in lista)
                {
                    var pessoa = item.Financeiro.Pessoa;

                    // Validação de e-mail
                    if (pessoa == null || string.IsNullOrWhiteSpace(pessoa.Email))
                    {
                        erros.Add($"Parcela {item.IdFinanceiroParcela}: Cliente sem e-mail cadastrado");
                        continue;
                    }

                    string subject = $"Lembrete de pagamento Concicard - fatura {item.IdFinanceiroParcela}";
                    string body = $@"<p>Prezado(a) {item.Financeiro.Nome},</p>
                        <p>Este é um lembrete amigável de que sua fatura de número {item.IdFinanceiroParcela} no valor de R$ {item.ValorVencimento:F2} vence hoje.</p>
                        <p>Para evitar qualquer inconveniente, solicitamos que efetue o pagamento.</p>
                        <p>Segue o link do boleto para pagamento: <a href='https://app-uniquesec.com.br/imprimir-boleto/{item.IdentificadorBoletoUnique}'>Clique aqui para acessar o boleto</a></p>
                        <p>Caso já tenha realizado o pagamento, por favor, desconsidere este aviso.</p>
                        <p>Agradecemos pela sua atenção e cooperação.</p>
                        <p>Atenciosamente,</p>
                        <p>Equipe Concicard</p>";

                    MailMessage mailMsg = new MailMessage();
                    mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Cobrança");
                    mailMsg.Subject = subject;
                    mailMsg.To.Add(new MailAddress(pessoa.Email.Trim()));
                    mailMsg.To.Add(new MailAddress("kleytonwillian@gmail.com"));
                    mailMsg.To.Add(new MailAddress("renato@genialsoft.com.br"));
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = body;

                    smtpClientSendGrid.Credentials = credentialsSendGrid;

                    try
                    {
                        await smtpClientSendGrid.SendMailAsync(mailMsg);

                        // ⭐ MARCAR COMO ENVIADO (campo diferente do outro método)
                        item.SetDataLembreteVencimento(DateTime.Now.Date);
                        resultados.Add($"E-mail enviado com sucesso para {pessoa.Nome} (Parcela {item.IdFinanceiroParcela})");
                    }
                    catch (Exception ex)
                    {
                        erros.Add($"Erro ao enviar para {pessoa.Nome} (Parcela {item.IdFinanceiroParcela}): {ex.Message}");
                    }
                    finally
                    {
                        mailMsg.Dispose();
                    }
                }

                // ⭐ SALVAR ALTERAÇÕES
                await context.SaveChangesAsync();

                return Ok(new
                {
                    TotalProcessados = lista.Count,
                    Sucessos = resultados.Count,
                    Erros = erros.Count,
                    Detalhes = new { resultados, erros }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro geral: {ex.Message}");
            }
        }

        //[HttpGet]
        //[Route("listaContaReceberAtraso")]
        //[AllowAnonymous]
        //public async Task<IActionResult> ListaContaReceberAtraso()
        //{
        //    var lista = context.FinanceiroParcela
        //             .Include(x => x.Financeiro)
        //             .Where(x => x.Financeiro.Tipo == "Contas a Receber"
        //             && x.Situacao == "Aberto"
        //             && x.DataVencimento.Date < DateTime.UtcNow.Date
        //             && x.DataVencimento.Date >= DateTime.UtcNow.AddDays(-3).Date)
        //             .OrderBy(x => x.DataVencimento)
        //             .ToList();

        //    foreach (var item in lista)
        //    {
        //        if (!string.IsNullOrEmpty(item.IdentificadorBoletoUnique.ToString()) && item.IdentificadorBoletoUnique != 0)
        //        {
        //            var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == item.Financeiro.IdPessoa);

        //            string subject = $"Lembrete de pagamento Concicard -  fatura {item.IdFinanceiroParcela}";

        //            string body = $@"<p>Prezado(a) {item.Financeiro.Nome},</p>
        //                        <p>Este é um lembrete amigável de que sua fatura de número {item.IdFinanceiroParcela} no valor de R$ {item.ValorVencimento.ToString("F2")} está vencida.</p>
        //                        <p>Para evitar qualquer inconveniente, solicitamos que efetue o pagamento.</p>
        //                        <p>Segue o link do boleto para pagamento: https://app-uniquesec.com.br/imprimir-boleto/{item.IdentificadorBoletoUnique}</p>
        //                        <p>Caso já tenha realizado o pagamento, por favor, desconsidere este aviso.</p>
        //                        <p>Agradecemos pela sua atenção e cooperação.</p>
        //                        <p>Atenciosamente,</p>
        //                        <p>Equipe Concicard</p>";

        //            MailMessage mailMsg = new MailMessage();
        //            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Cobrança");
        //            mailMsg.Subject = subject;


        //            mailMsg.To.Add(new MailAddress(pessoa.Email.Trim()));
        //            mailMsg.To.Add(new MailAddress("kleytonwillian@gmail.com"));
        //            mailMsg.To.Add(new MailAddress("renato@genialsoft.com.br"));

        //            // Adicionar o HTML ao email
        //            mailMsg.IsBodyHtml = true;
        //            mailMsg.Body = body;
        //            smtpClientSendGrid.Credentials = credentialsSendGrid;

        //            try
        //            {
        //                await smtpClientSendGrid.SendMailAsync(mailMsg);
        //            }
        //            catch (Exception ex)
        //            {
        //                // Log do erro aqui
        //                throw new Exception($"Erro ao enviar email para {pessoa.Nome}: {ex.Message}");
        //            }
        //        }
        //        else 
        //        {
        //            continue;
        //        }
        //    }
        //    return Ok();
        //}

        [HttpPost]
        [Route("cancelarBoletos")]
        [Authorize]
        public async Task<IActionResult> CancelarBoletos([FromBody] List<FinanceiroParcelaModel> model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name); 
            foreach (var financeiroParcela in model)
            {
                var financeiro = context.Financeiro.Include(x => x.Parcelas).Include(x => x.Pessoa).FirstOrDefault(x => x.IdFinanceiro == financeiroParcela.IdFinanceiro);
                if (financeiro == null)
                    return BadRequest("Não foi possível recuperar dados do financeiro.");
                var financeiroParcelas = context.FinanceiroParcela.Include(x => x.Financeiro)
                    .Include(x => x.Financeiro.Pessoa).FirstOrDefault(x => x.IdFinanceiroParcela == financeiroParcela.IdFinanceiroParcela && x.IdFinanceiro == financeiroParcela.IdFinanceiro && x.Situacao == "Aberto");
                if (financeiroParcelas == null)
                    return BadRequest("Não foi possível recuperar a parcela.");
                if (financeiroParcelas.IdentificadorBoletoUnique > 0)
                {
                    var token = await _uniqueService.GerarAccessTokenAsync(_config["unique:login"], _config["unique:password"], _config["unique:url"]);
                    var responseCancelamento = await _uniqueService.CancelarCobrancaAsync(financeiroParcelas.IdentificadorBoletoUnique , token, _config["unique:url"]);
                    if (responseCancelamento.IsSuccessStatusCode == true)
                    {
                        financeiro.ExcluirParcela(financeiroParcelas, usuarioLogado.Login);
                        context.FinanceiroParcela.Update(financeiroParcelas);
                        context.SaveChanges();
                        continue;

                    }
                }
                financeiro.ExcluirParcela(financeiroParcelas, usuarioLogado.Login);
                context.FinanceiroParcela.Update(financeiroParcelas);
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        [Route("excluirParcela")]
        [Authorize]
        public async Task<IActionResult> ExcluirParcela(int idFinanceiro, int idFinanceiroParcela)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name); // get usuario logado 

            var financeiro = context.Financeiro.Include(x => x.Parcelas).FirstOrDefault(x => x.IdFinanceiro == idFinanceiro);
            if (financeiro == null)
                return BadRequest("Não foi possível recuperar dados do financeiro.");

            var financeiroParcela = context.FinanceiroParcela.FirstOrDefault(x => x.IdFinanceiroParcela == idFinanceiroParcela && x.IdFinanceiro == idFinanceiro);
            if (financeiroParcela == null)
                return BadRequest("Não foi possível recuperar a parcela.");

            var token = await _uniqueService.GerarAccessTokenAsync(_config["unique:login"], _config["unique:password"], _config["unique:url"]);

            if (financeiroParcela.IdentificadorBoletoUnique > 0)
            {
                var responseCancelamento = await _uniqueService.CancelarCobrancaAsync(financeiroParcela.IdentificadorBoletoUnique , token, _config["unique:url"]);
            }

            financeiro.ExcluirParcela(financeiroParcela, usuarioLogado.Login);

                context.FinanceiroParcela.Update(financeiroParcela);
                context.SaveChanges();
                return Ok();
            
        }


        [HttpPost]
        [Route("baixarParcela")]
        [Authorize]
        public IActionResult BaixarParcela([FromBody] BaixarParcelModel model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name); // get usuario logado 

            var financeiro = context.Financeiro.Include(x => x.Parcelas).Include(x => x.Pessoa).FirstOrDefault(x => x.IdFinanceiro == model.IdFinanceiro);
            if (financeiro == null)
                return BadRequest("Não foi possível recuperar dados do financeiro.");

            var financeiroParcela = context.FinanceiroParcela.FirstOrDefault(x => x.IdFinanceiroParcela == model.IdFinanceiroParcela && x.IdFinanceiro == model.IdFinanceiro && x.Situacao == "Aberto");
            if (financeiroParcela == null)
                return BadRequest("Não foi possível recuperar a parcela.");


            financeiroParcela.BaixarConta(
                dataVencimento: model.DataVencimento,
                dataAcerto: model.DataAcerto,
                numeroNf: model.NumeroNf,
                observacao: model.Observacao,
                valorDesconto: model.ValorDesconto,
                valorAcrescimo: model.ValorAcrescimo,
                valorAcerto: model.ValorAcerto,
                valorVencimento: model.ValorVencimento,
                meioPagamento: context.MeioPagamento.FirstOrDefault(x => x.IdMeioPagamento == model.IdMeioPagamento),
                usuarioBaixa: usuarioLogado.Login);

            context.FinanceiroParcela.Update(financeiroParcela);


            string tipo = string.Empty;
            string descricaoExtrato = string.Empty;


            financeiro.RecalcularFinanceiro();

            context.Financeiro.Update(financeiro);
            context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("gerarparcelas")]
        public IActionResult GerarParcelasFinanceiro([FromBody] GerarParcelaFinanceiroModel model)
        {
            var parcelas = new List<ItemParcelaFinanceiroModel>();

            int quantidadeParcelas = model.QuantidadeParcelas;
            DateTime DataVencimentoPrimeiraParcela = model.DataVencimentoPrimeiraParcela;
            Decimal valorParcela = model.Valor / quantidadeParcelas;
            // DateTime? DataPagamento = null;

            for (int i = 1; i <= quantidadeParcelas; i++)
            {
                parcelas.Add(new ItemParcelaFinanceiroModel()
                {
                    Valor = Math.Round(valorParcela, 2),
                    DataVencimento = DataVencimentoPrimeiraParcela,
                    // DataPagamento = DataPagamento,
                    NumeroParcela = i
                });
                DataVencimentoPrimeiraParcela = DataVencimentoPrimeiraParcela.AddMonths(1);
            }
            var somadevalores = parcelas.Sum(x => x.Valor);
            decimal resto = 0;
            if (Convert.ToDecimal(somadevalores) - (Convert.ToDecimal(model.Valor)) != 0)
            {
                resto = Convert.ToDecimal(somadevalores) - Convert.ToDecimal(model.Valor);
                parcelas[0].Valor = parcelas[0].Valor - resto;
            }
            return Ok(parcelas);
        }

        [HttpGet]
        [Route("dashboardFinanceiro")]
        [Authorize]
        public IActionResult DashboardFinanceiro(int ano)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name); // get usuario logado 

            var dashboardFinanceiro = new DashboardFinanceiroAnoModel();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                var sql = $@" select  Tipo, Mes, Situacao, sum(TotalVencimento),  sum(TotalAcerto), sum(TotalAberto), 
                            from Financeiro
                            inner join FinanceiroParcela on FinanceiroParcela.IdFinanceiro = Financeiro.IdFinanceiro
                            where Situacao = 'Baixado' || Situacao = 'Aberto'
                            and YEAR(FinanceiroParcela.DataVencimento) = '{ano}'
                            group  by Situacao ";

                var lista = conn.Query<DashboardAtendimentoModel>(sql).ToList();

                return Ok();
            }
        }

    }
}

