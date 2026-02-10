using Dapper;
using ERP.Infra;
using ERP.Models;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using ERP_API.Service.Parceiros;
using ERP_API.Service.Parceiros.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ConciliadoraController : ControllerBase
    {

        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        protected Context context;
        protected IConciliadoraService conciliadoraService;

        public ConciliadoraController(Context context, IConciliadoraService conciliadoraService)
        {
            this.context = context;
            this.conciliadoraService = conciliadoraService;
        }

        [HttpGet]
        [Route("adquirentes")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAdquirentes(int idCliente, int top, int skip)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == idCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await conciliadoraService.ListaAdquirenteConciliadoraResponse1(cliente.ApiKeyConciliadora, top, skip);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpPost]
        [Route("vendas-canceladas")]
        [AllowAnonymous]
        public async Task<ActionResult> GetVendasCanceladas([FromBody] ConsultaConciliadoraModel model)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await conciliadoraService.VendasCanceladas(cliente.IdentificadorConciliadora, model.DataInicio, model.DataFim, cliente.ApiKeyConciliadora, model.Top, model.Skip, model.Adquirente, model.Produto, model.Nsu);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpPost]
        [Route("conciliacao-sistema")]
        [AllowAnonymous]
        public async Task<ActionResult> GetConciliacaoSistema([FromBody] ConsultaConciliadoraModel model)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await conciliadoraService.ConciliacaoSistema(cliente.IdentificadorConciliadora, model.DataInicio, model.DataFim, cliente.ApiKeyConciliadora, model.Top, model.Skip, model.Adquirente, model.Produto, model.Nsu);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpPost]
        [Route("pagamentos")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPagamentos([FromBody] ConsultaConciliadoraModel model)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");


            var (success, errorMessage, data) = await conciliadoraService.PagamentosRecebidos(cliente.IdentificadorConciliadora, model.DataInicio, model.DataFim, cliente.ApiKeyConciliadora, model.Top, model.Skip, model.Adquirente, model.Produto, model.Nsu);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpPost]
        [Route("previsao-pagamento")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPrevisaoPagamento([FromBody] ConsultaConciliadoraModel model)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await conciliadoraService.PrevisaoPagamento(cliente.IdentificadorConciliadora, model.DataInicio, model.DataFim, cliente.ApiKeyConciliadora, model.Top, model.Skip, model.Adquirente, model.Produto, model.Nsu);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpPost]
        [Route("status-processamento")]
        [AllowAnonymous]
        public async Task<ActionResult> GetStatusProcessamento([FromQuery] StatusProcessamentoConciladoraRequest model)
        {
            var (success, errorMessage, data) = await conciliadoraService.StatusProcessamento(model);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpPost]
        [Route("vendas")]
        [AllowAnonymous]
        public async Task<ActionResult> GetVendas([FromBody] ConsultaConciliadoraModel model)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await conciliadoraService.Vendas(cliente.IdentificadorConciliadora, model.DataInicio, model.DataFim, cliente.ApiKeyConciliadora, model.Top, model.Skip, model.Adquirente, model.Produto, model.Nsu, model.Modalidade);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpPost]
        [Route("vendas-conciliadas")]
        [AllowAnonymous]
        public async Task<ActionResult> GetVendasConciliadas([FromBody] ConsultaConciliadoraModel model)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await conciliadoraService.VendasConciliadas(cliente.IdentificadorConciliadora, model.DataInicio, model.DataFim, cliente.ApiKeyConciliadora, model.Top, model.Skip, model.Adquirente, model.Produto, model.Nsu, model.Modalidade);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpPost]
        [Route("vendas-nao-enviadas")]
        [AllowAnonymous]
        public async Task<ActionResult> GetVendasNaoEnviadas([FromBody] ConsultaConciliadoraModel model)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await conciliadoraService.VendasNaoEnviadas(cliente.IdentificadorConciliadora, model.DataInicio, model.DataFim, cliente.ApiKeyConciliadora, model.Top, model.Skip, model.Adquirente, model.Produto, model.Nsu, model.Modalidade);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpGet]
        [Route("meio-captura")]
        [AllowAnonymous]
        public async Task<ActionResult> GetMeioCaptura(int idCliente, int top, int skip)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == idCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await conciliadoraService.MeioCaptura(cliente.ApiKeyConciliadora, top, skip);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpGet]
        [Route("modalidades")]
        [AllowAnonymous]
        public async Task<ActionResult> GetModalidades(int idCliente, int top, int skip)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == idCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await conciliadoraService.Modalidade(cliente.ApiKeyConciliadora, top, skip);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpGet]
        [Route("produtos")]
        [AllowAnonymous]
        public async Task<ActionResult> GetProdutos(int idCliente, int top, int skip)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == idCliente.ToString());
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            if (string.IsNullOrEmpty(cliente.ApiKeyConciliadora))
                return BadRequest("Cliente não possui Api Key");

            var (success, errorMessage, data) = await conciliadoraService.Produto(cliente.ApiKeyConciliadora, top, skip);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpPost]
        [Route("enviar-venda")]
        [AllowAnonymous]
        public async Task<ActionResult> EnviarVenda(string senha, string idEmpresa, IFormFile arquivoXml)
        {
            var (success, errorMessage, data) = await conciliadoraService.EnviarVendaSistemaAsync(senha, idEmpresa, arquivoXml);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(data);
        }

        [HttpPost]
        [Route("previaEmail")]
        public async Task<IActionResult> PreviaEmail([FromBody] ConsultaConciliadoraModel model)
        {
            try
            {
                // Validação
                if (model == null || string.IsNullOrEmpty(model.Email))
                    return BadRequest(new { message = "Dados inválidos." });

                var cliente = context.Cliente
                    .Include(c => c.Pessoa)
                    .FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());

                var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == cliente.IdPessoa);

                if (cliente == null)
                    return NotFound(new { message = "Cliente não encontrado." });

                // Obter dados
                var (success, errorMessage, data) = await conciliadoraService.Vendas(
                    cliente.IdentificadorConciliadora,
                    model.DataInicio.Date,
                    model.DataFim.Date,
                    cliente.ApiKeyConciliadora,
                    model.Top,
                    model.Skip,
                    model.Adquirente,
                    model.Produto,
                    model.Nsu,
                    model.Modalidade
                );

                if (!success || data?.Value == null)
                    return BadRequest(new { message = errorMessage ?? "Erro ao obter dados de vendas." });

                var vendas = data.Value;

                // Processar dados
                var emailData = ProcessarDadosRelatorio(vendas, pessoa, model.DataInicio);

                // Construir e enviar email
                var emailHtml = ConstruirEmailHtml(emailData);
                await EnviarEmailAsync(model.Email, emailData.Subject, emailHtml);

                return Ok(new { message = "Email de prévia enviado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao enviar email: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("previaEmailPagamentoBanco")]
        public async Task<IActionResult> PreviaEmailPagamentoBanco([FromBody] ConsultaConciliadoraModel model)
        {
            try
            {
                // Validação
                if (model == null || string.IsNullOrEmpty(model.Email))
                    return BadRequest(new { message = "Dados inválidos." });

                var cliente = context.Cliente
                    .Include(c => c.Pessoa)
                    .FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());

                var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == cliente.IdPessoa);

                if (cliente == null)
                    return NotFound(new { message = "Cliente não encontrado." });

                // Executar consulta SQL
                var dadosConciliacao = context.ConciliacaoBancaria
                    .Where(x => x.IdCliente == cliente.IdPessoa &&
                                x.DataPagamento >= model.DataInicio &&
                                x.DataPagamento <= model.DataFim)
                    .GroupBy(x => new { x.Status, x.Adquirente })
                    .Select(g => new
                    {
                        Nome = pessoa.Nome,
                        Status = g.Key.Status,
                        Adquirente = g.Key.Adquirente,
                        Quantidade = g.Count(),
                        Total = g.Sum(x => x.Valor)
                    })
                    .OrderBy(x => x.Status)
                    .ToList();

                if (!dadosConciliacao.Any())
                    return BadRequest(new { message = "Não há dados de conciliação bancária para o período informado." });

                // Processar dados para o email
                var emailData = ProcessarDadosConciliacaoBancaria(dadosConciliacao, pessoa, model.DataInicio);

                // Construir email
                var emailHtml = ConstruirEmailConciliacaoBancaria(emailData);

                // Enviar email apenas para o email fornecido (prévia)
                await EnviarEmailAsync(model.Email, emailData.Subject, emailHtml);

                return Ok(new { message = "Email de prévia enviado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao enviar email: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("enviarEmailPagamentoBanco")]
        public async Task<IActionResult> EnviarEmailPagamentoBanco([FromBody] ConsultaConciliadoraModel model)
        {
            try
            {
                // Validação
                if (model == null)
                    return BadRequest(new { message = "Dados inválidos." });

                var cliente = context.Cliente
                    .Include(c => c.Pessoa)
                    .FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());

                var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == cliente.IdPessoa);

                if (cliente == null)
                    return NotFound(new { message = "Cliente não encontrado." });

                if (string.IsNullOrEmpty(cliente.Pessoa?.Email))
                    return BadRequest(new { message = "Cliente não possui email cadastrado." });

                // Executar consulta SQL
                var dadosConciliacao = context.ConciliacaoBancaria
                    .Where(x => x.IdCliente == cliente.IdPessoa &&
                                x.DataPagamento >= model.DataInicio &&
                                x.DataPagamento <= model.DataFim)
                    .GroupBy(x => new { x.Status, x.Adquirente })
                    .Select(g => new
                    {
                        Nome = pessoa.Nome,
                        Status = g.Key.Status,
                        Adquirente = g.Key.Adquirente,
                        Quantidade = g.Count(),
                        Total = g.Sum(x => x.Valor)
                    })
                    .OrderBy(x => x.Status)
                    .ToList();

                if (!dadosConciliacao.Any())
                    return BadRequest(new { message = "Não há dados de conciliação bancária para o período informado." });

                // Processar dados para o email
                var emailData = ProcessarDadosConciliacaoBancaria(dadosConciliacao, pessoa, model.DataInicio);

                // Construir email
                var emailHtml = ConstruirEmailConciliacaoBancaria(emailData);

                // Lista de destinatários
                var destinatarios = new List<string>
                {
                    cliente.Pessoa.Email.Trim(),
                    "kleytonwillian@gmail.com",
                    "renato@genialsoft.com.br"
                };

                // Enviar emails
                await EnviarEmailAsync(destinatarios, emailData.Subject, emailHtml);

                return Ok(new { message = "Emails enviados com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao enviar email: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("previaEmailErpOperadora")]
        public async Task<IActionResult> PreviaEmailErpOperadora([FromBody] ConsultaConciliadoraModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Email))
                    return BadRequest(new { message = "Dados inválidos." });

                using (IDbConnection conn = context.Database.GetDbConnection())
                {
                    // Buscar cliente e pessoa
                    var clienteQuery = @"
                SELECT c.*, p.IdPessoa, p.Nome 
                FROM Cliente c
                INNER JOIN Pessoa p ON p.IdPessoa = c.IdPessoa
                WHERE c.IdentificadorConciliadora = @IdCliente";

                    var cliente = conn.QueryFirstOrDefault<ClientePessoaDto>(clienteQuery,
                        new { IdCliente = model.IdCliente.ToString() });

                    if (cliente == null)
                        return BadRequest(new { message = "Cliente não encontrado." });

                    // Consulta de vendas conciliadas agrupadas
                    var vendasQuery = @"
                SELECT 
                    @IdPessoa as IdPessoa,
                    @Nome as Nome,
                    Status,
                    COUNT(*) as Quantidade,
                    SUM(TotalVenda) as Total
                FROM VendasConciliadas
                WHERE IdentificadorConciliadora = @IdentificadorConciliadora
                  AND DataVenda >= @DataInicio
                  AND DataVenda <= @DataFim
                GROUP BY Status
                ORDER BY Status";

                    var vendasConciliadas = conn.Query<VendaConciliadaResumoDto>(vendasQuery, new
                    {
                        IdPessoa = cliente.IdPessoa,
                        Nome = cliente.Nome,
                        IdentificadorConciliadora = cliente.IdentificadorConciliadora,
                        DataInicio = model.DataInicio,
                        DataFim = model.DataFim
                    }).ToList();

                    if (!vendasConciliadas.Any())
                        return BadRequest(new { message = "Não há dados de vendas conciliadas para o período informado." });

                    // Criar objeto pessoa para os métodos existentes
                    var pessoa = new Pessoa { IdPessoa = cliente.IdPessoa, Nome = cliente.Nome };

                    var emailData = ProcessarDadosVendasConciliadas(vendasConciliadas, pessoa, model.DataInicio);
                    var emailHtml = ConstruirEmailVendasConciliadas(emailData);

                    await EnviarEmailAsync(model.Email, emailData.Subject, emailHtml);

                    return Ok(new { message = "Email de prévia enviado com sucesso!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao enviar email: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("enviarEmailErpOperadora")]
        public async Task<IActionResult> EnviarEmailErpOperadora([FromBody] ConsultaConciliadoraModel model)
        {
            try
            {
                // Validação
                if (model == null)
                    return BadRequest(new { message = "Dados inválidos." });

                var cliente = context.Cliente
                    .Include(c => c.Pessoa)
                    .FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());

                var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == cliente.IdPessoa);

                if (cliente == null)
                    return NotFound(new { message = "Cliente não encontrado." });

                if (string.IsNullOrEmpty(cliente.Pessoa?.Email))
                    return BadRequest(new { message = "Cliente não possui email cadastrado." });

                // Executar consulta SQL
                var vendasConciliadas = context.VendasConciliadas
                    .Where(x => x.IdentificadorConciliadora == cliente.IdentificadorConciliadora &&
                                x.DataVenda >= model.DataInicio &&
                                x.DataVenda <= model.DataFim)
                    .GroupBy(x => x.Status)
                    .Select(g => new
                    {
                        IdPessoa = pessoa.IdPessoa,
                        Nome = pessoa.Nome,
                        Status = g.Key,
                        Quantidade = g.Count(),
                        Total = g.Sum(x => x.TotalVenda)
                    })
                    .OrderBy(x => x.Status)
                    .ToList();

                if (!vendasConciliadas.Any())
                    return BadRequest(new { message = "Não há dados de vendas conciliadas para o período informado." });

                // Processar dados para o email
                var emailData = ProcessarDadosVendasConciliadas(vendasConciliadas, pessoa, model.DataInicio);

                // Construir email
                var emailHtml = ConstruirEmailVendasConciliadas(emailData);

                // Lista de destinatários
                var destinatarios = new List<string>
                {
                    cliente.Pessoa.Email.Trim(),
                    "kleytonwillian@gmail.com",
                    "renato@genialsoft.com.br"
                };

                // Enviar emails
                await EnviarEmailAsync(destinatarios, emailData.Subject, emailHtml);

                return Ok(new { message = "Emails enviados com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao enviar email: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("enviarEmail")]
        public async Task<IActionResult> EnviarEmail([FromBody] ConsultaConciliadoraModel model)
        {
            try
            {
                // Validação
                if (model == null)
                    return BadRequest(new { message = "Dados inválidos." });

                var cliente = context.Cliente
                    .Include(c => c.Pessoa)
                    .FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());
                var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == cliente.IdPessoa);

                if (cliente == null)
                    return NotFound(new { message = "Cliente não encontrado." });

                if (string.IsNullOrEmpty(cliente.Pessoa?.Email))
                    return BadRequest(new { message = "Cliente não possui email cadastrado." });

                // Obter dados
                var (success, errorMessage, data) = await conciliadoraService.Vendas(
                    cliente.IdentificadorConciliadora,
                    model.DataInicio,
                    model.DataFim,
                    cliente.ApiKeyConciliadora,
                    model.Top,
                    model.Skip,
                    model.Adquirente,
                    model.Produto,
                    model.Nsu,
                    model.Modalidade
                );

                if (!success || data?.Value == null)
                    return BadRequest(new { message = errorMessage ?? "Erro ao obter dados de vendas." });

                var vendas = data.Value;

                // Processar dados
                var emailData = ProcessarDadosRelatorio(vendas, pessoa, model.DataInicio);

                // Construir email
                var emailHtml = ConstruirEmailHtml(emailData);

                // Lista de destinatários
                var destinatarios = new List<string>
        {
            cliente.Pessoa.Email.Trim(),
            "kleytonwillian@gmail.com",
            "renato@genialsoft.com.br"
        };

                // Enviar emails
                await EnviarEmailAsync(destinatarios, emailData.Subject, emailHtml);

                return Ok(new { message = "Emails enviados com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao enviar email: {ex.Message}" });
            }
        }

        // Métodos auxiliares privados

        private EmailRelatorioData ProcessarDadosRelatorio(IEnumerable<dynamic> vendas, Pessoa pessoa, DateTime dataInicio)
        {
            var cultura = CultureInfo.GetCultureInfo("pt-BR");
            var mesExtensoDe = dataInicio.ToString("MMMM 'de' yyyy", cultura);

            // Agrupamento por adquirente
            var agrupamento = vendas
                .GroupBy(p => new { Adquirente = p.Adquirente })
                .Select(g => new AdquirenteResumo
                {
                    Adquirente = g.Key.Adquirente,
                    ValorBrutoTotal = g.Sum(p => (decimal)p.ValorBruto),
                    ValorLiquidoTotal = g.Sum(p => (decimal)p.ValorLiquido),
                    ValorTaxaTotal = g.Sum(p => (decimal)p.ValorBruto - (decimal)p.ValorLiquido),
                    QuantidadeTransacoes = g.Count(),
                    TaxaMediaPonderada = g.Sum(p => (decimal)p.ValorBruto * (decimal)p.PercentualTaxa) / g.Sum(p => (decimal)p.ValorBruto),
                    TicketMedio = g.Average(p => (decimal)p.ValorBruto)
                })
                .ToList();

            // Totalizadores gerais
            var totalBruto = agrupamento.Sum(a => a.ValorBrutoTotal);
            var totalLiquido = agrupamento.Sum(a => a.ValorLiquidoTotal);
            var totalTaxa = agrupamento.Sum(a => a.ValorTaxaTotal);
            var totalQuantidade = agrupamento.Sum(a => a.QuantidadeTransacoes);
            var taxaMediaGeral = totalBruto > 0 ? (totalTaxa / totalBruto) : 0;
            var ticketMedioGeral = totalQuantidade > 0 ? (totalBruto / totalQuantidade) : 0;

            return new EmailRelatorioData
            {
                NomeCliente = pessoa.Nome,
                MesAno = mesExtensoDe,
                Subject = $"Relatório de {mesExtensoDe} Concicard - {pessoa.Nome}",
                Adquirentes = agrupamento,
                TotalBruto = totalBruto,
                TotalLiquido = totalLiquido,
                TotalTaxa = totalTaxa,
                TaxaMediaGeral = taxaMediaGeral,
                QuantidadeTotal = totalQuantidade,
                TicketMedioGeral = ticketMedioGeral,
                QuantidadeVendasERP = totalQuantidade,
                TotalVendasERP = totalBruto
            };
        }

        private string ConstruirEmailHtml(EmailRelatorioData data)
        {
            string templateHtml = GetEmailTemplate();

            // Substituir placeholders do cabeçalho
            templateHtml = templateHtml.Replace("{NOME_CLIENTE}", data.NomeCliente);
            templateHtml = templateHtml.Replace("{MES_ANO}", data.MesAno);

            // Construir resumo geral
            var resumoGeralHtml = $@"
        <div class='summary-grid-geral'>
            <div class='summary-item'>
                <div class='summary-label'>VALOR BRUTO</div>
                <div class='summary-value bruto'>R$ {data.TotalBruto:N2}</div>
            </div>
            <div class='summary-item'>
                <div class='summary-label'>VALOR LÍQUIDO</div>
                <div class='summary-value liquido'>R$ {data.TotalLiquido:N2}</div>
            </div>
            <div class='summary-item'>
                <div class='summary-label'>VALOR PAGO EM TAXA</div>
                <div class='summary-value taxa'>R$ {data.TotalTaxa:N2}</div>
            </div>
            <div class='summary-item'>
                <div class='summary-label'>TAXA MÉDIA</div>
                <div class='summary-value taxa-percent'>{data.TaxaMediaGeral:P2}</div>
            </div>
            <div class='summary-item'>
                <div class='summary-label'>QUANTIDADE DE VENDAS</div>
                <div class='summary-value quantidade'>{data.QuantidadeTotal:N0}</div>
            </div>
            <div class='summary-item'>
                <div class='summary-label'>TICKET MÉDIO</div>
                <div class='summary-value ticket'>R$ {data.TicketMedioGeral:N2}</div>
            </div>
        </div>";

            templateHtml = templateHtml.Replace("{RESUMO_GERAL}", resumoGeralHtml);

            // Construir resumo de vendas ERP
            var resumoVendasERPHtml = $@"
        <div class='erp-summary-box'>
            <div class='erp-summary-title'>🏪 Resumo de Vendas ERP</div>
            <div class='erp-summary-grid'>
                <div class='erp-summary-item'>
                    <div class='erp-summary-label'>QUANTIDADE DE VENDAS ERP</div>
                    <div class='erp-summary-value quantidade-erp'>{data.QuantidadeVendasERP:N0}</div>
                </div>
                <div class='erp-summary-item'>
                    <div class='erp-summary-label'>TOTAL DE VENDAS</div>
                    <div class='erp-summary-value total-erp'>R$ {data.TotalVendasERP:N2}</div>
                </div>
            </div>
        </div>";

            templateHtml = templateHtml.Replace("{RESUMO_VENDAS_ERP}", resumoVendasERPHtml);

            // Construir HTML dos adquirentes
            StringBuilder adquirentesHtml = new StringBuilder();

            foreach (var adquirente in data.Adquirentes)
            {
                string adquirenteSection = $@"
            <div class='adquirente-section'>
                <div class='adquirente-header'>
                    <div class='adquirente-name'>{adquirente.Adquirente}</div>
                </div>
                <div class='metrics-grid'>
                    <div class='metric-item'>
                        <div class='metric-label'>Valor Bruto</div>
                        <div class='metric-value bruto'>R$ {adquirente.ValorBrutoTotal:N2}</div>
                    </div>
                    <div class='metric-item'>
                        <div class='metric-label'>Valor Líquido</div>
                        <div class='metric-value liquido'>R$ {adquirente.ValorLiquidoTotal:N2}</div>
                    </div>
                    <div class='metric-item'>
                        <div class='metric-label'>Taxa Média</div>
                        <div class='metric-value taxa'>{adquirente.TaxaMediaPonderada:P2}</div>
                    </div>
                    <div class='metric-item'>
                        <div class='metric-label'>Quantidade</div>
                        <div class='metric-value quantidade'>{adquirente.QuantidadeTransacoes:N0}</div>
                    </div>
                    <div class='metric-item'>
                        <div class='metric-label'>Ticket Médio</div>
                        <div class='metric-value ticket'>R$ {adquirente.TicketMedio:N2}</div>
                    </div>
                    <div class='metric-item'>
                        <div class='metric-label'>Valor em Taxa</div>
                        <div class='metric-value taxa-valor'>R$ {adquirente.ValorTaxaTotal:N2}</div>
                    </div>
                </div>
            </div>";

                adquirentesHtml.AppendLine(adquirenteSection);
            }

            templateHtml = templateHtml.Replace("{ADQUIRENTES_LOOP}", adquirentesHtml.ToString());

            return templateHtml;
        }

        private async Task EnviarEmailAsync(string destinatario, string assunto, string corpoHtml)
        {
            await EnviarEmailAsync(new List<string> { destinatario }, assunto, corpoHtml);
        }

        private async Task EnviarEmailAsync(List<string> destinatarios, string assunto, string corpoHtml)
        {
            using (MailMessage mailMsg = new MailMessage())
            {
                mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Relatórios");
                mailMsg.Subject = assunto;
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = corpoHtml;

                foreach (var email in destinatarios)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                        mailMsg.To.Add(new MailAddress(email.Trim()));
                }

                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
                    corpoHtml, null, MediaTypeNames.Text.Html));

                smtpClientSendGrid.Credentials = credentialsSendGrid;

                try
                {
                    await smtpClientSendGrid.SendMailAsync(mailMsg);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao enviar email: {ex.Message}", ex);
                }
            }
        }

        private EmailConciliacaoBancariaData ProcessarDadosConciliacaoBancaria(dynamic dadosConciliacao, Pessoa pessoa, DateTime dataInicio)
        {
            var cultura = CultureInfo.GetCultureInfo("pt-BR");
            var mesExtensoDe = dataInicio.ToString("MMMM 'de' yyyy", cultura);

            // Agrupamento por Status e Adquirente
            var agrupamento = new List<ConciliacaoBancariaResumo>();

            foreach (var item in dadosConciliacao)
            {
                agrupamento.Add(new ConciliacaoBancariaResumo
                {
                    Status = item.Status,
                    Adquirente = item.Adquirente,
                    Quantidade = item.Quantidade,
                    Total = item.Total
                });
            }

            // Agrupamento por Status
            var agrupadoPorStatus = agrupamento
                .GroupBy(x => x.Status)
                .Select(g => new StatusResumo
                {
                    Status = g.Key,
                    Quantidade = g.Sum(x => x.Quantidade),
                    Total = g.Sum(x => x.Total),
                    Detalhes = g.ToList()
                })
                .OrderBy(x => x.Status)
                .ToList();

            // Totalizadores gerais
            var totalGeral = agrupamento.Sum(a => a.Total);
            var quantidadeTotal = agrupamento.Sum(a => a.Quantidade);

            return new EmailConciliacaoBancariaData
            {
                NomeCliente = pessoa.Nome,
                MesAno = mesExtensoDe,
                Subject = $"Relatório de Pagamentos Bancários - {mesExtensoDe} - {pessoa.Nome}",
                StatusResumo = agrupadoPorStatus,
                TotalGeral = totalGeral,
                QuantidadeTotal = quantidadeTotal
            };
        }

        private string ConstruirEmailConciliacaoBancaria(EmailConciliacaoBancariaData data)
        {
            string templateHtml = GetEmailTemplateConciliacaoBancaria();

            // Substituir placeholders do cabeçalho
            templateHtml = templateHtml.Replace("{NOME_CLIENTE}", data.NomeCliente);
            templateHtml = templateHtml.Replace("{MES_ANO}", data.MesAno);

            // Construir resumo geral
            var resumoGeralHtml = $@"
        <div class='summary-grid-geral'>
            <div class='summary-item'>
                <div class='summary-label'>TOTAL GERAL</div>
                <div class='summary-value bruto'>R$ {data.TotalGeral:N2}</div>
            </div>
            <div class='summary-item'>
                <div class='summary-label'>QUANTIDADE TOTAL</div>
                <div class='summary-value quantidade'>{data.QuantidadeTotal:N0}</div>
            </div>
        </div>";

            templateHtml = templateHtml.Replace("{RESUMO_GERAL}", resumoGeralHtml);

            // Construir HTML dos status
            StringBuilder statusHtml = new StringBuilder();

            foreach (var statusItem in data.StatusResumo)
            {
                statusHtml.AppendLine($@"
            <div class='status-section'>
                <div class='status-header'>
                    <div class='status-name'>Status: {statusItem.Status}</div>
                    <div class='status-totals'>
                        <span class='status-total-label'>Total:</span>
                        <span class='status-total-value'>R$ {statusItem.Total:N2}</span> |
                        <span class='status-total-label'>Quantidade:</span>
                        <span class='status-total-value'>{statusItem.Quantidade:N0}</span>
                    </div>
                </div>
                <div class='adquirentes-grid'>");

                foreach (var detalhe in statusItem.Detalhes)
                {
                    statusHtml.AppendLine($@"
                    <div class='adquirente-item'>
                        <div class='adquirente-title'>{detalhe.Adquirente}</div>
                        <div class='adquirente-metrics'>
                            <div class='metric-small'>
                                <div class='metric-small-label'>Quantidade</div>
                                <div class='metric-small-value quantidade'>{detalhe.Quantidade:N0}</div>
                            </div>
                            <div class='metric-small'>
                                <div class='metric-small-label'>Total</div>
                                <div class='metric-small-value total'>R$ {detalhe.Total:N2}</div>
                            </div>
                        </div>
                    </div>");
                }

                statusHtml.AppendLine(@"
                </div>
            </div>");
            }

            templateHtml = templateHtml.Replace("{STATUS_LOOP}", statusHtml.ToString());

            return templateHtml;
        }

        private EmailVendasConciliadasData ProcessarDadosVendasConciliadas(dynamic vendasConciliadas, Pessoa pessoa, DateTime dataInicio)
        {
            var cultura = CultureInfo.GetCultureInfo("pt-BR");
            var mesExtensoDe = dataInicio.ToString("MMMM 'de' yyyy", cultura);

            // Criar lista de resumo por status
            var resumoPorStatus = new List<VendasConciliadasResumo>();

            foreach (var item in vendasConciliadas)
            {
                resumoPorStatus.Add(new VendasConciliadasResumo
                {
                    IdPessoa = item.IdPessoa,
                    Nome = item.Nome,
                    Status = item.Status,
                    Quantidade = item.Quantidade,
                    Total = item.Total
                });
            }

            // Totalizadores gerais
            var totalGeral = resumoPorStatus.Sum(a => a.Total);
            var quantidadeTotal = resumoPorStatus.Sum(a => a.Quantidade);

            return new EmailVendasConciliadasData
            {
                NomeCliente = pessoa.Nome,
                MesAno = mesExtensoDe,
                Subject = $"Relatório de Vendas ERP Operadora - {mesExtensoDe} - {pessoa.Nome}",
                VendasResumo = resumoPorStatus,
                TotalGeral = totalGeral,
                QuantidadeTotal = quantidadeTotal
            };
        }

        private string ConstruirEmailVendasConciliadas(EmailVendasConciliadasData data)
        {
            string templateHtml = GetEmailTemplateVendasConciliadas();

            // Substituir placeholders do cabeçalho
            templateHtml = templateHtml.Replace("{NOME_CLIENTE}", data.NomeCliente);
            templateHtml = templateHtml.Replace("{MES_ANO}", data.MesAno);

            // Construir resumo geral
            var resumoGeralHtml = $@"
        <div class='summary-grid-geral'>
            <div class='summary-item'>
                <div class='summary-label'>TOTAL GERAL</div>
                <div class='summary-value bruto'>R$ {data.TotalGeral:N2}</div>
            </div>
            <div class='summary-item'>
                <div class='summary-label'>QUANTIDADE TOTAL</div>
                <div class='summary-value quantidade'>{data.QuantidadeTotal:N0}</div>
            </div>
        </div>";

            templateHtml = templateHtml.Replace("{RESUMO_GERAL}", resumoGeralHtml);

            // Construir HTML dos status
            StringBuilder statusHtml = new StringBuilder();

            foreach (var venda in data.VendasResumo)
            {
                statusHtml.AppendLine($@"
            <div class='status-item'>
                <div class='status-item-header'>
                    <div class='status-item-name'>Status: {venda.Status}</div>
                </div>
                <div class='status-item-metrics'>
                    <div class='metric-small'>
                        <div class='metric-small-label'>Quantidade</div>
                        <div class='metric-small-value quantidade'>{venda.Quantidade:N0}</div>
                    </div>
                    <div class='metric-small'>
                        <div class='metric-small-label'>Total</div>
                        <div class='metric-small-value total'>R$ {venda.Total:N2}</div>
                    </div>
                </div>
            </div>");
            }

            templateHtml = templateHtml.Replace("{VENDAS_LOOP}", statusHtml.ToString());

            return templateHtml;
        }

        private string GetEmailTemplateVendasConciliadas()
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
                    max-width: 800px;
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
                .summary-grid-geral {
                    display: grid;
                    grid-template-columns: repeat(2, 1fr);
                    gap: 15px;
                    margin-bottom: 20px;
                }
                .summary-item {
                    background: white;
                    padding: 20px;
                    border-radius: 6px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.05);
                    text-align: center;
                }
                .summary-label {
                    font-size: 11px;
                    color: #666;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                    margin-bottom: 8px;
                    font-weight: 600;
                }
                .summary-value {
                    font-size: 26px;
                    font-weight: bold;
                    color: #333;
                }
                .summary-value.bruto {
                    color: #4CAF50;
                }
                .summary-value.quantidade {
                    color: #9C27B0;
                }
                .vendas-grid {
                    display: grid;
                    grid-template-columns: repeat(2, 1fr);
                    gap: 15px;
                }
                .status-item {
                    background: #fafafa;
                    border-radius: 8px;
                    padding: 20px;
                    border-left: 4px solid #667eea;
                }
                .status-item-header {
                    margin-bottom: 15px;
                    padding-bottom: 10px;
                    border-bottom: 2px solid #e0e0e0;
                }
                .status-item-name {
                    font-size: 18px;
                    font-weight: 700;
                    color: #333;
                }
                .status-item-metrics {
                    display: grid;
                    grid-template-columns: repeat(2, 1fr);
                    gap: 10px;
                }
                .metric-small {
                    text-align: center;
                    background: white;
                    padding: 12px;
                    border-radius: 6px;
                }
                .metric-small-label {
                    font-size: 10px;
                    color: #666;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                    margin-bottom: 5px;
                    font-weight: 600;
                }
                .metric-small-value {
                    font-size: 18px;
                    font-weight: bold;
                }
                .metric-small-value.quantidade {
                    color: #9C27B0;
                }
                .metric-small-value.total {
                    color: #4CAF50;
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
                @media only screen and (max-width: 600px) {
                    .summary-grid-geral {
                        grid-template-columns: 1fr;
                    }
                    .vendas-grid {
                        grid-template-columns: 1fr;
                    }
                }
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Relatório de Vendas ERP Operadora</h1>
                    <p>{NOME_CLIENTE} - {MES_ANO}</p>
                </div>
                <div class='content'>
                    <div class='greeting'>
                        Prezado(a) {NOME_CLIENTE},<br>
                        Segue abaixo o relatório detalhado das vendas conciliadas no ERP.
                    </div>
                    <div class='summary-box'>
                        <div class='summary-title'>📊 Resumo Geral</div>
                        {RESUMO_GERAL}
                    </div>
                    <div class='vendas-grid'>
                        {VENDAS_LOOP}
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

        private string GetEmailTemplateConciliacaoBancaria()
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
                    max-width: 800px;
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
                .summary-grid-geral {
                    display: grid;
                    grid-template-columns: repeat(2, 1fr);
                    gap: 15px;
                    margin-bottom: 20px;
                }
                .summary-item {
                    background: white;
                    padding: 20px;
                    border-radius: 6px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.05);
                    text-align: center;
                }
                .summary-label {
                    font-size: 11px;
                    color: #666;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                    margin-bottom: 8px;
                    font-weight: 600;
                }
                .summary-value {
                    font-size: 26px;
                    font-weight: bold;
                    color: #333;
                }
                .summary-value.bruto {
                    color: #4CAF50;
                }
                .summary-value.quantidade {
                    color: #9C27B0;
                }
                .status-section {
                    margin-bottom: 30px;
                    background: #fafafa;
                    border-radius: 8px;
                    padding: 20px;
                    border-left: 4px solid #667eea;
                }
                .status-header {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    margin-bottom: 20px;
                    padding-bottom: 15px;
                    border-bottom: 2px solid #e0e0e0;
                }
                .status-name {
                    font-size: 22px;
                    font-weight: 700;
                    color: #333;
                }
                .status-totals {
                    font-size: 14px;
                    color: #666;
                }
                .status-total-label {
                    font-weight: 600;
                    color: #555;
                }
                .status-total-value {
                    font-weight: 700;
                    color: #667eea;
                    font-size: 16px;
                }
                .adquirentes-grid {
                    display: grid;
                    grid-template-columns: repeat(2, 1fr);
                    gap: 15px;
                }
                .adquirente-item {
                    background: white;
                    padding: 15px;
                    border-radius: 6px;
                    box-shadow: 0 2px 8px rgba(0,0,0,0.05);
                }
                .adquirente-title {
                    font-size: 16px;
                    font-weight: 600;
                    color: #333;
                    margin-bottom: 12px;
                    padding-bottom: 8px;
                    border-bottom: 1px solid #e0e0e0;
                }
                .adquirente-metrics {
                    display: grid;
                    grid-template-columns: repeat(2, 1fr);
                    gap: 10px;
                }
                .metric-small {
                    text-align: center;
                }
                .metric-small-label {
                    font-size: 10px;
                    color: #666;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                    margin-bottom: 5px;
                    font-weight: 600;
                }
                .metric-small-value {
                    font-size: 16px;
                    font-weight: bold;
                }
                .metric-small-value.quantidade {
                    color: #9C27B0;
                }
                .metric-small-value.total {
                    color: #4CAF50;
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
                @media only screen and (max-width: 600px) {
                    .summary-grid-geral {
                        grid-template-columns: 1fr;
                    }
                    .adquirentes-grid {
                        grid-template-columns: 1fr;
                    }
                    .status-header {
                        flex-direction: column;
                        align-items: flex-start;
                    }
                    .status-totals {
                        margin-top: 10px;
                    }
                }
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Relatório de Pagamentos Bancários</h1>
                    <p>{NOME_CLIENTE} - {MES_ANO}</p>
                </div>
                <div class='content'>
                    <div class='greeting'>
                        Prezado(a) {NOME_CLIENTE},<br>
                        Segue abaixo o relatório detalhado dos pagamentos bancários.
                    </div>
                    <div class='summary-box'>
                        <div class='summary-title'>📊 Resumo Geral</div>
                        {RESUMO_GERAL}
                    </div>
                    <div class='status-container'>
                        {STATUS_LOOP}
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
                    max-width: 800px;
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
                .summary-grid-geral {
                    display: grid;
                    grid-template-columns: repeat(3, 1fr);
                    gap: 15px;
                    margin-bottom: 20px;
                }
                .summary-item {
                    background: white;
                    padding: 15px;
                    border-radius: 6px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.05);
                    text-align: center;
                }
                .summary-label {
                    font-size: 11px;
                    color: #666;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                    margin-bottom: 8px;
                    font-weight: 600;
                }
                .summary-value {
                    font-size: 22px;
                    font-weight: bold;
                    color: #333;
                }
                .summary-value.bruto {
                    color: #4CAF50;
                }
                .summary-value.liquido {
                    color: #2196F3;
                }
                .summary-value.taxa {
                    color: #f44336;
                }
                .summary-value.taxa-percent {
                    color: #FF9800;
                }
                .summary-value.quantidade {
                    color: #9C27B0;
                }
                .summary-value.ticket {
                    color: #00BCD4;
                }
                .erp-summary-box {
                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                    border-radius: 8px;
                    padding: 25px;
                    margin-bottom: 30px;
                    box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
                }
                .erp-summary-title {
                    font-size: 20px;
                    font-weight: 700;
                    color: white;
                    margin-bottom: 20px;
                    text-align: center;
                }
                .erp-summary-grid {
                    display: grid;
                    grid-template-columns: repeat(2, 1fr);
                    gap: 20px;
                }
                .erp-summary-item {
                    background: white;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 4px 10px rgba(0,0,0,0.1);
                    text-align: center;
                }
                .erp-summary-label {
                    font-size: 12px;
                    color: #666;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                    margin-bottom: 10px;
                    font-weight: 700;
                }
                .erp-summary-value {
                    font-size: 28px;
                    font-weight: bold;
                    margin-top: 5px;
                }
                .erp-summary-value.quantidade-erp {
                    color: #667eea;
                }
                .erp-summary-value.total-erp {
                    color: #4CAF50;
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
                    font-size: 11px;
                    color: #666;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                    margin-bottom: 8px;
                    font-weight: 600;
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
                .metric-value.quantidade {
                    color: #9C27B0;
                }
                .metric-value.ticket {
                    color: #00BCD4;
                }
                .metric-value.taxa-valor {
                    color: #f44336;
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
                @media only screen and (max-width: 600px) {
                    .summary-grid-geral {
                        grid-template-columns: repeat(2, 1fr);
                    }
                    .metrics-grid {
                        grid-template-columns: repeat(2, 1fr);
                    }
                    .erp-summary-grid {
                        grid-template-columns: 1fr;
                    }
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
                        {RESUMO_GERAL}
                    </div>
                    {RESUMO_VENDAS_ERP}
                    <div class='adquirentes-container'>
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

