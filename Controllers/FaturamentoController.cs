using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using ERP_API.Service.Parceiros.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using ERP.Models;
using Microsoft.Extensions.Configuration;
using ERP_API.Service.Parceiros;
using System.Net.Mail;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class FaturamentoController : ControllerBase
    {
        private IConfiguration _config;
        protected Context context;
        protected IConciliadoraService conciliadoraService;
        protected IUniqueService _uniqueService;

        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        public FaturamentoController(Context context, IConciliadoraService conciliadoraService, IUniqueService uniqueService, IConfiguration config)
        {
            _config = config;
            this.context = context;
            this.conciliadoraService = conciliadoraService;
            _uniqueService = uniqueService;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar()
           {
            try
            {
                var result = context.Faturamento
                                      .Include(x => x.Cliente)
                                      .ThenInclude(c => c.Pessoa)
                                      .Select(m => new
                                      {
                                          m.IdFaturamento,
                                          m.IdCliente,
                                          m.IdFinanceiro,
                                          NomeCliente = m.Cliente != null && m.Cliente.Pessoa != null
                                              ? m.Cliente.Pessoa.Nome
                                              : null,
                                          m.NumeroVendas,
                                          m.TotalVendas,
                                          m.Mes,
                                          m.Ano,
                                          m.ValorMensalidade,
                                          m.Situacao
                                      }).ToList();

                var retorno = new
                {
                    Result = result,
                    TotalReceber = result.Sum(x => x.ValorMensalidade),
                };

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                // Log o erro real
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("pesquisar")]
        [Authorize]
        public IActionResult Pesquisar([FromBody] PesquisarFaturamentoRequestModel model)
        {
            var query = context.Faturamento.Include(x => x.Cliente)
                                       .ThenInclude(c => c.Pessoa).AsQueryable();
            if (model.IdCliente.HasValue)
            {
                query = query.Where(x => x.IdCliente == model.IdCliente.Value);
            }
            if (!string.IsNullOrEmpty(model.Nome))
            {
                query = query.Where(x => x.Cliente.Pessoa.Nome.Contains(model.Nome));
            }
            if (model.Mes.HasValue)
            {
                query = query.Where(x => x.Mes == model.Mes.Value);
            }
            if (model.Ano.HasValue)
            {
                query = query.Where(x => x.Ano == model.Ano.Value);
            }
            var result = query.Select(m => new
            {
                m.IdFaturamento,
                m.IdCliente,
                m.IdFinanceiro,
                NomeCliente = m.Cliente.Pessoa.Nome,
                m.NumeroVendas,
                m.TotalVendas,
                m.Mes,
                m.Ano,
                m.ValorMensalidade,
                m.Situacao
            });

            var retorno = new
            {
                Result = result,
                TotalReceber = result.Sum(x => x.ValorMensalidade),
            };

            return Ok(retorno);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public async Task<IActionResult> Salvar([FromBody] SalvarFaturamento model)
        {
            try
            {
                var clientes = context.Cliente
                    .Include(x => x.Pessoa)
                    .Where(x => x.Situacao == "Ativo")
                    .ToList();

                var planos = context.Plano.ToList();
                var planoConta = context.PlanoConta.FirstOrDefault(x => x.Descricao == "Contrato");

                if (planoConta == null)
                    return BadRequest("Plano de conta 'Contrato' não encontrado.");

                var dataInicio = new DateTime(model.Ano, model.Mes, 1);
                var dataFim = dataInicio.AddMonths(1).AddDays(-1);

                // Calcula a data de vencimento: dia 10 do próximo mês
                var dataVencimento = dataInicio.AddMonths(1).AddDays(14);

                // Verifica se a data de vencimento não está vencida
                if (dataVencimento.Date < DateTime.Now.Date)
                    return BadRequest("A data de vencimento para este período já passou.");

                foreach (var cliente in clientes)
                {
                    try
                    {
                        // Pula se NÃO tiver identificador OU NÃO tiver ApiKey
                        if (string.IsNullOrEmpty(cliente.IdentificadorConciliadora) ||
                            string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                            continue;

                        var (success, errorMessage, data) = await conciliadoraService.Vendas(
                            cliente.IdentificadorConciliadora, dataInicio, dataFim,
                            cliente.ApiKeyConciliadora, null, null, null, null, null, null);

                        // Verifica se a chamada foi bem sucedida e se há dados
                        if (!success || data?.Value == null)
                            continue;

                        var vendas = data.Value;
                        var numeroVendas = vendas.Count;

                        // Pula se não tiver vendas
                        if (numeroVendas == 0)
                            continue;

                        var totalVendas = vendas.Sum(x => x.ValorBruto);

                        var plano = planos.FirstOrDefault(x =>
                            numeroVendas >= x.QuantidadeVendasInicial &&
                            numeroVendas <= x.QuantidadeVendasFinal);

                        var valorMensalidade = plano?.Valor ?? 0;

                        // 1. Cria o Financeiro
                        var financeiro = new Financeiro(
                            cliente.Pessoa,
                            "Contas a Receber",
                            User.Identity.Name);

                        var financeiroParcela = new FinanceiroParcela(
                            1,
                            dataVencimento,
                            valorMensalidade,
                            $"Mensalidade {model.Mes:00}/{model.Ano}",
                            planoConta,
                            User.Identity.Name);

                        financeiro.AddParcela(financeiroParcela);
                        context.Financeiro.Add(financeiro);
                        context.SaveChanges();

                        // 2. Cria o Faturamento e associa o Financeiro
                        var faturamento = new Faturamento(
                            cliente, numeroVendas, totalVendas,
                            model.Mes, model.Ano, valorMensalidade, User.Identity.Name);

                        faturamento.AssociarFinanceiro(financeiro, User.Identity.Name);
                        context.Faturamento.Add(faturamento);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Erro ao processar cliente " + cliente.Pessoa.Nome + ": " + ex.Message);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao salvar faturamento: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("enviarEmail")]
        [Authorize]
        public async Task<IActionResult> EnviarEmail([FromBody] List<int> idFinanceiros)
        {
            try
            {
                foreach (var idFinanceiro in idFinanceiros)
                {
                    // Busca o financeiro com a primeira parcela em aberto
                    var financeiro = context.Financeiro
                        .Include(f => f.Pessoa)
                        .Include(f => f.Parcelas)
                        .ThenInclude(p => p.PlanoConta)
                        .FirstOrDefault(x => x.IdFinanceiro == idFinanceiro);

                    if (financeiro == null)
                        return BadRequest("Financeiro não encontrado");

                    // Busca a primeira parcela em aberto
                    var financeiroParcela = financeiro.Parcelas
                        .Where(x => x.Situacao == "Aberto")
                        .OrderBy(x => x.DataVencimento)
                        .FirstOrDefault();

                    if (financeiroParcela == null)
                        return BadRequest("Não há parcelas em aberto para este financeiro");

                    var pessoa = financeiro.Pessoa;
                    if (pessoa == null)
                        return BadRequest("Não foi possível recuperar dados da pessoa");

                    // Validações
                    if (financeiroParcela.Situacao == "Baixado")
                        return BadRequest("Boleto já foi baixado");

                    if (financeiroParcela.IdentificadorBoletoUnique == 0 || financeiroParcela.IdentificadorBoletoUnique == null)
                        return BadRequest("Boleto sem identificador");

                    if (string.IsNullOrWhiteSpace(pessoa.Email))
                        return BadRequest("Cliente não possui e-mail cadastrado");

                    // Busca informações do faturamento relacionado
                    var faturamento = context.Faturamento
                        .Include(f => f.Cliente)
                        .ThenInclude(c => c.Pessoa)
                        .FirstOrDefault(f => f.IdFinanceiro == idFinanceiro);

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
                    string subject = $"Boleto Concicard - Mensalidade {(faturamento != null ? $"{faturamento.Mes:00}/{faturamento.Ano}" : "")}";
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
        .stats-box {{
            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
            border-radius: 8px;
            padding: 15px;
            margin: 15px 0;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>💳 Mensalidade Concicard</h1>
            <p>Gestão Inteligente de Cartões</p>
        </div>

        <div class='content'>
            <p>Prezado(a) <strong>{pessoa.Nome}</strong>,</p>
            <p>Segue abaixo o boleto referente à sua mensalidade:</p>

            {(faturamento != null ? $@"
            <div class='stats-box'>
                <h3 style='margin-top: 0; color: #667eea;'>📊 Resumo do Período</h3>
                <div class='info-row'>
                    <span class='info-label'>Período de Referência:</span>
                    <span class='info-value'>{faturamento.Mes:00}/{faturamento.Ano}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Número de Vendas:</span>
                    <span class='info-value'>{faturamento.NumeroVendas:N0}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Total de Vendas:</span>
                    <span class='info-value'>R$ {faturamento.TotalVendas:N2}</span>
                </div>
            </div>" : "")}

            <div class='info-box'>
                <h3 style='margin-top: 0; color: #667eea;'>📋 Informações do Boleto</h3>
                <div class='info-row'>
                    <span class='info-label'>Número da Fatura:</span>
                    <span class='info-value'>#{financeiroParcela.IdFinanceiroParcela}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Descrição:</span>
                    <span class='info-value'>{financeiroParcela.Observacao ?? "Mensalidade"}</span>
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
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao enviar email: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("gerarBoleto")]
        [Authorize]
        public async Task<IActionResult> GerarBoleto([FromBody] List<int> idFinanceiros)
        {
            foreach (var idFinanceiro in idFinanceiros)
            {
                var financeiro = context.Financeiro.Include(x => x.Pessoa).FirstOrDefault(x => x.IdFinanceiro == idFinanceiro);
                if (financeiro == null)
                    return BadRequest("Financeiro não encontrado.");

                var financeiroParcela = context.FinanceiroParcela.FirstOrDefault(x => x.IdFinanceiro == idFinanceiro);
                if (financeiroParcela == null)
                    return BadRequest("Parcela não encontrada.");
                if (financeiroParcela.IdentificadorBoletoUnique != null)
                    continue;

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
                        valor = financeiroParcela.ValorVencimento
                    }
                     },
                    telefoneSacado = financeiro.Pessoa.Telefone1,
                    valor = financeiroParcela.ValorVencimento,
                    valorDesconto = financeiroParcela.ValorDesconto ?? 0,
                    valorJuros = financeiroParcela.ValorAcrescimo ?? 0,
                    valorMulta = financeiroParcela.ValorAcrescimo ?? 0
                };

                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(cobrancaRequest, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                var responseuniqueService = await _uniqueService.CriarCobrancaAsync(cobrancaRequest, token, _config["unique:url"]);

                financeiroParcela.SetIdentificadorBoletoUnique(responseuniqueService.Data.IdTransacao);
                context.FinanceiroParcela.Update(financeiroParcela);
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var faturamento = context.Faturamento.FirstOrDefault(x => x.IdFaturamento == id);
            if (faturamento == null)
                return BadRequest("Faturamento não encontrado.");
            faturamento.Excluir(User.Identity.Name);
            context.Faturamento.Update(faturamento);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var faturamento = context.Faturamento.FirstOrDefault(x => x.IdFaturamento == id);
            if (faturamento == null)
                return BadRequest("Faturamento não encontrado.");

            return Ok(new FaturamentoResponse
            {
                IdFaturamento = faturamento.IdFaturamento,
                IdCliente = faturamento.IdCliente,
                NumeroVendas = faturamento.NumeroVendas,
                TotalVendas = faturamento.TotalVendas,
                Mes = faturamento.Mes,
                Ano = faturamento.Ano,
                ValorMensalidade = faturamento.ValorMensalidade,
                Situacao = faturamento.Situacao
            });
        }

        [HttpPost]
        [Route("salvar-anonimo")]
        [Authorize]
        public async Task<IActionResult> SalvarAnonimo()
        {
            // Obtém o mês e ano vigente (atual)
            var mesVigente = DateTime.Now.Month;
            var anoVigente = DateTime.Now.Year;

            // Busca todos os faturamentos do mês e ano vigente
            var faturamentos = context.Faturamento
                .Include(x => x.Cliente)
                .ThenInclude(c => c.Pessoa)
                .Where(x => x.Mes == mesVigente && x.Ano == anoVigente && x.Situacao == "Ativo")
                .ToList();

            if (faturamentos == null || !faturamentos.Any())
                return BadRequest("Nenhum faturamento encontrado para o mês/ano vigente.");

            // Busca o plano de conta padrão
            var planoConta = context.PlanoConta.FirstOrDefault(x => x.Descricao == "Contrato");
            if (planoConta == null)
                return BadRequest("Plano de conta 'Contrato' não encontrado.");

            var resultados = new List<object>();

            // Para cada faturamento, gera o financeiro e o boleto
            foreach (var faturamento in faturamentos)
            {
                try
                {
                    var cliente = faturamento.Cliente.Pessoa;
                    if (cliente == null)
                        continue;

                    // Cria o Financeiro
                    var financeiro = new Financeiro(
                        cliente,
                        "Contas a Receber",
                        "Anonimo");

                    // Define a data de vencimento (primeiro dia do próximo mês)
                    var dataVencimento = new DateTime(anoVigente, mesVigente, 1).AddMonths(1).AddDays(4).AddHours(3);

                    // Adiciona a parcela
                    financeiro.AddParcela(new FinanceiroParcela(
                        0,
                        dataVencimento,
                        faturamento.ValorMensalidade,
                        $"Mensalidade {mesVigente:00}/{anoVigente}",
                        planoConta,
                        "Anonimo"));

                    // Adiciona o financeiro ao contexto
                    context.Financeiro.Add(financeiro);
                    context.SaveChanges();

                    // Busca a parcela recém-criada
                    var financeiroParcela = context.FinanceiroParcela
                        .Include(x => x.Financeiro)
                        .ThenInclude(x => x.Pessoa)
                        .FirstOrDefault(x => x.IdFinanceiro == financeiro.IdFinanceiro && x.Situacao == "Aberto");

                    if (financeiroParcela == null)
                        continue;

                    // Gera o token de acesso para o serviço Unique
                    var token = await _uniqueService.GerarAccessTokenAsync(
                        _config["unique:login"],
                        _config["unique:password"],
                        _config["unique:url"]);

                    // Cria a requisição de cobrança
                    var cobrancaRequest = new CobrancaRequest
                    {
                        nomeSacado = financeiro.Nome,
                        bairroSacado = cliente.Bairro,
                        boletoMensagem1 = "",
                        boletoMensagem2 = "",
                        boletoMensagem3 = "",
                        boletoMensagem4 = "",
                        cepSacado = cliente.Cep,
                        cidadeSacado = cliente.Cidade,
                        cpfCnpjSacado = cliente.CpfCnpj,
                        dataVencimento = financeiroParcela.DataVencimento,
                        emailSacado = cliente.Email,
                        enderecoSacado = cliente.Logradouro,
                        identificador = financeiroParcela.IdFinanceiroParcela.ToString(),
                        numeroPedido = financeiroParcela.IdFinanceiroParcela.ToString(),
                        numeroSacado = cliente.Numero,
                        splitsValores = new List<SplitValor>
                        {
                            new SplitValor
                            {
                                cobrarTarifa = true,
                                agencia = "000001",
                                conta = "000024",
                                valor = financeiroParcela.ValorVencimento
                            }
                        },
                        telefoneSacado = cliente.Telefone1,
                        valor = financeiroParcela.ValorVencimento,
                        valorDesconto = financeiroParcela.ValorDesconto ?? 0,
                        valorJuros = financeiroParcela.ValorAcrescimo ?? 0,
                        valorMulta = financeiroParcela.ValorAcrescimo ?? 0
                    };

                    // Cria a cobrança no serviço Unique
                    var responseuniqueService = await _uniqueService.CriarCobrancaAsync(
                        cobrancaRequest,
                        token,
                        _config["unique:url"]);

                    // Atualiza a parcela com o identificador do boleto
                    financeiroParcela.SetIdentificadorBoletoUnique(responseuniqueService.Data.IdTransacao);
                    context.FinanceiroParcela.Update(financeiroParcela);
                    context.SaveChanges();

                    // Adiciona o resultado
                    resultados.Add(new
                    {
                        IdCliente = cliente.IdPessoa,
                        NomeCliente = cliente.Nome,
                        IdFinanceiro = financeiro.IdFinanceiro,
                        IdFinanceiroParcela = financeiroParcela.IdFinanceiroParcela,
                        IdentificadorBoleto = financeiroParcela.IdentificadorBoletoUnique,
                        ValorMensalidade = faturamento.ValorMensalidade
                    });
                }
                catch (Exception ex)
                {
                    // Log do erro e continua para o próximo cliente
                    resultados.Add(new
                    {
                        IdCliente = faturamento.IdCliente,
                        NomeCliente = faturamento.Cliente?.Pessoa?.Nome ?? "Desconhecido",
                        Erro = ex.Message
                    });
                }
            }

            return Ok(new
            {
                MesVigente = mesVigente,
                AnoVigente = anoVigente,
                TotalProcessado = faturamentos.Count,
                Resultados = resultados
            });
        }
    }
}
