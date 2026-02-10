using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Service.Parceiros;
using ERP_API.Service.Parceiros.Interface;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders.Physical;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Globalization;
using System.Net.Mime;
using System.Text;
using System.Data.Entity;
using System.Collections.Generic;
using ERP_API.Extensions;


namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ConciliacaoBancariaController: ControllerBase
    {
        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        private  IConciliadoraDashBoardService conciliadoraDashBoardService;
        private  IConciliadoraService conciliadoraService;
        private IConfiguration _config;
        protected Context context { get; set; }
        public ConciliacaoBancariaController(Context context, IConciliadoraDashBoardService conciliadoraService, IConfiguration configuration, IConciliadoraService conciliadoraService1)
        {
            this.context = context;
            this.conciliadoraDashBoardService = conciliadoraService;
            _config = configuration;
            this.conciliadoraService = conciliadoraService1;

        }

        [HttpGet]
        [Route("listar")]
        [Authorize]

        public IActionResult Listar(int idCliente)
        {
            var result = context.ConciliacaoBancaria.Where(x => x.IdCliente ==  idCliente)
                .Select(
                    m => new
                    {
                        m.IdConciliacaoBancaria,
                        m.IdCliente,
                        NomeCliente = m.Cliente.Pessoa.Nome,
                        m.DataPagamento,
                        m.Valor,
                        m.Adquirente,
                        m.ConciliadoManual,
                        m.Status,
                        m.Situacao
                    }).Take(500).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("pesquisar")]
        [Authorize]
        public IActionResult Pesquisar(string identificadorConciliadora, DateTime dataInicio, DateTime dataFim, string status, string adquirente)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == identificadorConciliadora);
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            var result = context.ConciliacaoBancaria.Where(x => x.IdCliente == cliente.IdPessoa &&
                                                                x.DataPagamento.Date >= dataInicio.Date &&
                                                                x.DataPagamento.Date <= dataFim.Date).AsQueryable();

            if (!string.IsNullOrEmpty(adquirente) && adquirente != "null")
            {
                result = result.Where(x => x.Adquirente == adquirente);
            }
            if (!string.IsNullOrEmpty(status) && status != "null")
            {
                result = result.Where(x => x.Status == status);

            } 

            return Ok(new
            {
                Resultado = result.Select(
                    m => new
                    {
                        m.IdConciliacaoBancaria,
                        m.IdCliente,
                        NomeCliente = m.Cliente.Pessoa.Nome,
                        m.DataPagamento,
                        m.Valor,
                        m.ValorConciliacao,
                        m.ConciliadoManual,
                        m.Adquirente,
                        m.Status,
                        m.Situacao
                    }).Take(500).OrderBy(x => x.DataPagamento).ToList(),

                TotalPendentes = result.Count(x => x.Status == "Pendente"),
                TotalConciliados = result.Count(x => x.Status == "Conciliado"),
                TotalNaoConciliados = result.Count(x => x.Status == "Não conciliado"),
                TotalSem = result.Count(x => x.Status == "Sem")
            });

        }

        [HttpPost]
        [Route("detalhar")]
        [Authorize]
        public IActionResult Detalhar([FromBody] DetalharConciliacaoBancariaRequest model)
        {
            var dataInicio = model.DataPagamento.Date;
            var dataFim = model.DataPagamento.Date.AddDays(1);

            var extrato = context.Extrato.Where(x => x.IdCliente == model.IdCliente &&
                                                    x.DataLancamento >= dataInicio &&
                                                    x.DataLancamento < dataFim &&
                                                    x.Valor > 0).AsQueryable();

            var nomeCliente = context.Pessoa.FirstOrDefault(x => x.IdPessoa == model.IdCliente).Nome;

            var dataPagamento = model.DataPagamento.Date;
            bool isSegundaFeira = dataPagamento.DayOfWeek == DayOfWeek.Monday;

            if (model.NomeAdquirente == "Sicredi" && nomeCliente.Contains("HOTEL BRASIL"))
            {
                extrato = extrato.Where(x =>
                    x.IdCliente == model.IdCliente
                    && (isSegundaFeira
                        ? (x.DataLancamento.Date >= dataPagamento.Date.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                        : x.DataLancamento.Date == dataPagamento) &&
                    (x.Descricao.Contains("SICREDI") || x.Descricao.Contains("PIX"))
                    && x.Valor > 0);
            }


            if (model.NomeAdquirente == "Sicredi")
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                     && !x.Descricao.ToUpper().Contains("PAGAMENTO")
                     && !x.Descricao.ToUpper().Contains("LIQUIDACAO ")
                     && !x.Descricao.ToUpper().Contains("CX")
                     && x.Descricao.ToUpper().Contains("SICREDI"));
            }
            else if (model.NomeAdquirente == "Stone")
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                                         && x.Descricao.Contains("Recebimento")
                                         && x.Valor > 0);
            }
            else if (model.NomeAdquirente == "Ticket")
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                                     && (x.Descricao.Contains("TOPAZIO")
                                     || x.Descricao.Contains(model.NomeAdquirente.ToUpper())
                                     || x.Pagador.Contains(model.NomeAdquirente.ToUpper())));
            }
            else if (model.NomeAdquirente == "VR")
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                                     && (x.Descricao.Contains("02535864000133")
                                     || x.Descricao.Contains(model.NomeAdquirente.ToUpper())
                                     || x.Pagador.Contains(model.NomeAdquirente.ToUpper())
                                     || x.Pagador.Contains("TOPAZIO"))
                                     && x.Valor > 0);
            }
            else if (model.NomeAdquirente == "Banese")
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                     && (x.Descricao.ToUpper().Contains("TED") && x.Descricao.ToUpper().Contains("RECEBIDA")));
            }
            else if (model.NomeAdquirente == "Cielo" && nomeCliente.Contains("MERCADO PAGUE MENOS"))
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                                                           && ((x.Descricao.Contains("CIEL") && !x.Descricao.Contains("PIX RECEBIDO"))
                                                           || (x.Pagador.Contains("VENICIO A COSTA MINIMERCADO") && x.Descricao.Contains("PIX RECEBIDO")))
                                                           && x.Valor > 0);
            }
            else if (model.NomeAdquirente == "Cielo" && nomeCliente.Contains("RFC ESPETINHOS"))
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                                                           && (x.Descricao.Contains("CIELO")
                                                           || x.Descricao.Contains("TRANSFERENCIA PIX REM: RF CARNES"))
                                                           && x.Valor > 0);
            }
            else if (model.NomeAdquirente == "Cielo")
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                                                           && ((x.Descricao.Contains("CIELO") || x.Descricao.Contains("CIEL"))
                                                           || x.Descricao.Contains("TRANSFERENCIA PIX REM: RF CARNES"))
                                                           && x.Valor > 0);
            }
            else if (model.NomeAdquirente == "Alelo")
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                                                           && (x.Descricao.Contains("ALELO")
                                                           || x.Descricao.Contains("NAIP")
                                                           || x.Pagador.Contains(model.NomeAdquirente))
                                                           && x.Valor > 0);
            }
            else if (model.NomeAdquirente == "UpBrasil")
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                                                           && (x.Descricao.Contains("UpBrasil".ToUpper()) || x.Pagador.Contains(model.NomeAdquirente) || x.Pagador.Contains("UP BRASIL ADMINISTRACAO"))
                                                           && x.Valor > 0);
            }
            else if (model.NomeAdquirente == "Sodexo")
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                                                           && (x.Descricao.Contains("Sodexo") || x.Pagador.Contains(model.NomeAdquirente) || x.Pagador.Contains("PLUXEE"))
                                                           && x.Valor > 0);
            }
            else
            {
                extrato = extrato.Where(x => x.IdCliente == model.IdCliente
                                                           && x.Descricao.Contains(model.NomeAdquirente.ToUpper())
                                                           && x.Valor > 0);
            }

            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            var (success, errorMessage, data) = conciliadoraService.PagamentosRecebidos(cliente.IdentificadorConciliadora, model.DataPagamento, model.DataPagamento.AddDays(1), cliente.ApiKeyConciliadora, null, null, model.Adquirente, null, null).Result;

            var extratoResult = extrato.Select(x => new
            {
                x.IdExtrato,
                x.Cliente,
                x.Descricao,
                x.Valor,
                x.DataLancamento,
                x.Pagador,
                x.CpfCnpjPagador,
                x.Categoria,
                x.Banco,
                x.MetodoPagamento,
                Tipo = x.Tipo.ToString(),
            }).ToList();

            return Ok(new
            {
                Extratos = extratoResult,
                Pagamentos = data.Value
            });
        }

        [HttpPost]
        [Route("Salvar")]
        [Authorize]
        public async Task<IActionResult> Salvar([FromBody] ConsultaConciliadoraModel model)
        {
            var cliente = context.Cliente
                        .Include(x => x.ERPs)
                        .Include(x => x.Colaborador)
                        .FirstOrDefault(x => x.IdentificadorConciliadora == model.IdCliente.ToString());

            DateTime dataFinal = model.DataFim.AddDays(1);

            if (cliente == null)
            return BadRequest("Cliente não encontrado");

            var (success, errorMessage, data) = await conciliadoraService.PagamentosRecebidos(cliente.IdentificadorConciliadora, model.DataInicio, dataFinal, cliente.ApiKeyConciliadora, model.Top, model.Skip, model.Adquirente, model.Produto, model.Nsu);

            var responser = data.Value;
            DateTime startDate = model.DataInicio;
            DateTime endDate = dataFinal;

            // Agrupamento inicial dos pagamentos existentes
            var agrupamentoPagamentos = responser
                .GroupBy(p => new {
                    Data = p.DataPagamento.Date.DayOfWeek == DayOfWeek.Saturday
                        ? p.DataPagamento.Date.AddDays(2)
                        : p.DataPagamento.Date.DayOfWeek == DayOfWeek.Sunday
                            ? p.DataPagamento.Date.AddDays(1)
                            : p.DataPagamento.Date,
                    Adquirente = p.Adquirente
                })
                .Select(g => new {
                    Data = g.Key.Data,
                    Adquirente = g.Key.Adquirente,
                    ValorLiquidoTotal = g.Sum(p => p.ValorLiquido - (p.OutrasDespesas ?? 0)),
                    QuantidadeTransacoes = g.Count()
                })
                .ToList();

            // Obter todos os adquirentes únicos
            var adquirentes = agrupamentoPagamentos.Select(x => x.Adquirente).Distinct().ToList();

            // Gerar todos os dias úteis no período (excluindo sábados e domingos)
            var diasUteis = new List<DateTime>();
            for (DateTime dia = startDate.Date; dia <= endDate.Date; dia = dia.AddDays(1))
            {
                if (dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasUteis.Add(dia);
                }
            }

            // Criar combinação completa de dias úteis x adquirentes
            var agrupamentoCompleto = new List<dynamic>();
            foreach (var dia in diasUteis)
            {
                foreach (var adquirente in adquirentes)
                {
                    // Buscar se existe pagamento para este dia e adquirente
                    var pagamentoExistente = agrupamentoPagamentos
                        .FirstOrDefault(p => p.Data.Date == dia.Date && p.Adquirente == adquirente);

                    if (pagamentoExistente != null)
                    {
                        // Adicionar com valores reais
                        agrupamentoCompleto.Add(new {
                            Data = pagamentoExistente.Data,
                            Adquirente = pagamentoExistente.Adquirente,
                            ValorLiquidoTotal = (decimal?)(pagamentoExistente.ValorLiquidoTotal ?? 0m),
                            QuantidadeTransacoes = pagamentoExistente.QuantidadeTransacoes
                        });
                    }
                    else
                    {
                        // Adicionar com valores zero
                        agrupamentoCompleto.Add(new {
                            Data = dia,
                            Adquirente = adquirente,
                            ValorLiquidoTotal = (decimal?)0.00m,
                            QuantidadeTransacoes = 0
                        });
                    }
                }
            }

            var agrupamento = agrupamentoCompleto
                .OrderBy(x => x.Data)
                .ThenBy(x => x.Adquirente);

            var conciliacaoBancaria = context.ConciliacaoBancaria
                .Where(e => e.IdCliente == cliente.IdPessoa &&
                            e.DataPagamento >= startDate && e.DataPagamento <= endDate)
                .ToList();

            if (conciliacaoBancaria.Any())
            {
                context.ConciliacaoBancaria.RemoveRange(conciliacaoBancaria);
                await context.SaveChangesAsync();
            }

            int contador = 0;

            foreach (var item in agrupamento)
            {
                try
                {
                    decimal valorDecimal = 0;

                    valorDecimal = item.ValorLiquidoTotal > 0
                        ? item.ValorLiquidoTotal
                        : 0m;

                    var conciliacao = new ConciliacaoBancaria(
                        cliente,
                        item.Data,
                        valorDecimal,
                        item.Adquirente,
                        "Não conciliado",
                        User.Identity.Name
                    );
                    context.ConciliacaoBancaria.Add(conciliacao);
                    contador++;
                }

                catch (Exception ex)
                {
                    // Log de erro
                    Console.WriteLine($"Erro ao processar item {contador} - Data: {item.Data}, Adquirente: {item.Adquirente}. Erro: {ex.Message}");
                }
            }

            context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("conciliarPagamento")]
        [Authorize]
        public IActionResult ConciliarPagamento([FromBody] PesquisarConciliacaoBancariaRequest model)
        {
            try
            {
                var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == model.IdentificadorConciliadora);
                if (cliente == null)
                    return BadRequest("Cliente não encontrado!");

                var conciliacaoBancaria = context.ConciliacaoBancaria
                    .Where(x => x.DataPagamento.Date >= model.DataInicio.Date
                        && x.DataPagamento.Date <= model.DataFim.Date.AddDays(1)
                        && x.IdCliente == cliente.IdPessoa)
                    .ToList();

                if (!conciliacaoBancaria.Any())
                    return Ok("Nenhuma conciliação encontrada no período.");

                // Remove conciliações de sábado e domingo antecipadamente
                var datasConciliacoes = context.ConciliacaoBancaria
                    .Where(x => x.IdCliente == cliente.IdPessoa
                             && x.DataPagamento.Date >= model.DataInicio.Date
                             && x.DataPagamento.Date <= model.DataFim.Date)
                    .Select(x => x.DataPagamento.Date)
                    .ToList();

                var datasFinsDeSemana = datasConciliacoes
                    .Where(d => d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                    .ToList();

                if (datasFinsDeSemana.Any())
                {
                    var conciliacoesFinsDeSemana = context.ConciliacaoBancaria
                        .Where(x => x.IdCliente == cliente.IdPessoa
                                 && datasFinsDeSemana.Contains(x.DataPagamento.Date))
                        .ToList();

                    context.ConciliacaoBancaria.RemoveRange(conciliacoesFinsDeSemana);
                    context.SaveChanges();
                }

                // Recarrega as conciliações após remoção
                conciliacaoBancaria = context.ConciliacaoBancaria
                    .Where(x => x.DataPagamento.Date >= model.DataInicio.Date
                        && x.DataPagamento.Date <= model.DataFim.Date
                        && x.IdCliente == cliente.IdPessoa)
                    .ToList();

                // Calcula a data de início correta: verifica se há segundas-feiras no período
                // Se houver, inclui o sábado anterior à primeira segunda-feira
                DateTime dataInicioParaBusca = model.DataInicio.Date;

                // Procura a primeira segunda-feira no período
                var primeiraSegunda = Enumerable.Range(0, (model.DataFim.Date - model.DataInicio.Date).Days + 1)
                    .Select(offset => model.DataInicio.Date.AddDays(offset))
                    .FirstOrDefault(data => data.DayOfWeek == DayOfWeek.Monday);

                DateTime dataInicio = model.DataInicio.Date.DayOfWeek == DayOfWeek.Sunday
                                                      ? model.DataInicio.Date.AddDays(-1)
                                                      : model.DataInicio.Date;

                // Se houver segunda-feira no período, busca desde o sábado anterior
                if (primeiraSegunda != default(DateTime))
                {
                    dataInicioParaBusca = primeiraSegunda.AddDays(-2); // Sábado anterior
                }

                // Carrega o extrato UMA ÚNICA VEZ fora do loop
                var extratoBase = context.Extrato
                        .Where(e => e.IdCliente == cliente.IdPessoa
                            && e.DataLancamento.Date >= dataInicio
                            && e.DataLancamento.Date <= model.DataFim.Date)
                        .ToList();

                // Carrega o nome do cliente uma única vez
                var nomeCliente = context.Pessoa.FirstOrDefault(x => x.IdPessoa == cliente.IdPessoa)?.Nome ?? "";

                foreach (var item in conciliacaoBancaria)
                {
                    var dataPagamento = item.DataPagamento.Date;
                    bool isSegundaFeira = dataPagamento.DayOfWeek == DayOfWeek.Monday;

                    IEnumerable<Extrato> pesquisaExtrato;

                    if (item.Adquirente == "Sicredi" && nomeCliente.Contains("HOTEL BRASIL"))
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento) &&
                            (x.Descricao.Contains("SICREDI") || x.Descricao.Contains("PIX"))
                            && x.Valor > 0);
                    }

                    else if (item.Adquirente == "Sicredi")
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && (!x.Descricao.ToUpper().Contains("PAGAMENTO")
                            && !x.Descricao.ToUpper().Contains("LIQUIDACAO")
                            && !x.Descricao.ToUpper().Contains("CX")
                            && x.Descricao.ToUpper().Contains("SICREDI"))
                            && x.Valor > 0);
                    }
                    else if (item.Adquirente == "Stone")
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && x.Descricao.Contains("Recebimento")
                            && x.Valor > 0);
                    }
                    else if (item.Adquirente == "Ticket")
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && (x.Descricao.Contains("TOPAZIO")
                                || x.Descricao.Contains(item.Adquirente.ToUpper())
                                || x.Pagador.Contains(item.Adquirente.ToUpper())
                                || x.Pagador.Contains("TOPAZIO"))
                                && x.Valor > 0);
                    }
                    else if (item.Adquirente == "VR")
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && (x.Descricao.Contains("02535864000133")
                                || x.Descricao.Contains(item.Adquirente.ToUpper())
                                || x.Pagador.Contains(item.Adquirente.ToUpper()))
                            && x.Valor > 0);
                    }
                    else if (item.Adquirente == "Banese")
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && x.Descricao.ToUpper().Contains("TED")
                            && x.Descricao.ToUpper().Contains("RECEBIDA")
                            &&  x.Valor > 0);
                    }
                    else if (item.Adquirente == "Cielo" && nomeCliente.Contains("RFC ESPETINHOS"))
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && (x.Descricao.Contains("CIELO")
                                || x.Descricao.Contains("TRANSFERENCIA PIX REM: RF CARNES"))
                            && x.Valor > 0);
                    }
                    else if (item.Adquirente == "Cielo" && nomeCliente.Contains("MERCADO PAGUE MENOS"))
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && ((x.Descricao.Contains("CIEL") && !x.Descricao.Contains("PIX RECEBIDO"))
                                || (x.Pagador.Contains("VENICIO A COSTA MINIMERCADO") && x.Descricao.Contains("PIX RECEBIDO")))
                            && x.Valor > 0);
                    }
                    else if (item.Adquirente == "Cielo")
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && (x.Descricao.Contains("CIELO") || x.Descricao.Contains("CIEL")
                                || x.Descricao.Contains("TRANSFERENCIA PIX REM: RF CARNES"))
                            && x.Valor > 0);
                    }
                    else if (item.Adquirente == "Alelo")
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && (x.Descricao.Contains("ALELO")
                                || x.Descricao.Contains("NAIP")
                                || x.Pagador.Contains(item.Adquirente))
                            && x.Valor > 0);
                    }
                    else if (item.Adquirente == "UpBrasil")
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && (x.Descricao.Contains("UPBRASIL")
                                || x.Pagador.Contains(item.Adquirente)
                                || x.Pagador.Contains("UP BRASIL ADMINISTRACAO"))
                            && x.Valor > 0);
                    }
                    else if (item.Adquirente == "Sodexo")
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && (x.Descricao.Contains("Sodexo")
                                || x.Pagador.Contains(item.Adquirente)
                                || x.Pagador.Contains("PLUXEE"))
                            && x.Valor > 0);
                    }
                    else
                    {
                        pesquisaExtrato = extratoBase.Where(x =>
                            x.IdCliente == item.IdCliente
                            && (isSegundaFeira
                                ? (x.DataLancamento.Date >= dataPagamento.AddDays(-2) && x.DataLancamento.Date <= dataPagamento)
                                : x.DataLancamento.Date == dataPagamento)
                            && x.Descricao.Contains(item.Adquirente.ToUpper())
                            && x.Valor > 0);
                    }

                    var valorPequisaExtrato = pesquisaExtrato.Sum(x => x.Valor);

                    if (Math.Abs(item.Valor - valorPequisaExtrato) <= 0.10m && pesquisaExtrato.Any())
                    {
                        item.SetStatus("Conciliado", User.Identity.Name);
                        item.SetValorConciliacao(valorPequisaExtrato, User.Identity.Name);
                        context.Update(item);
                    }
                    else if (!pesquisaExtrato.Any() && item.Valor == 0)
                    {
                        item.SetStatus("Sem movimentação no período", User.Identity.Name);
                        item.SetValorConciliacao(valorPequisaExtrato, User.Identity.Name);
                        context.Update(item);
                    }
                    else
                    {
                        item.SetStatus("Pendente", User.Identity.Name);
                        item.SetValorConciliacao(valorPequisaExtrato, User.Identity.Name);
                        context.Update(item);
                    }
                }

                context.SaveChanges();
                return Ok(new
                {
                    message = $"{conciliacaoBancaria.Count} conciliações processadas.",
                    count = conciliacaoBancaria.Count
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao processar conciliação: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("conciliacaoManual")]
        [Authorize]
        public IActionResult ConciliacaoManual(int idConciliacaoBancaria)
        {
            var conciliacaoBancaria = context.ConciliacaoBancaria.FirstOrDefault(x => x.IdConciliacaoBancaria == idConciliacaoBancaria);
            if (conciliacaoBancaria == null)
                return BadRequest("Conciliação não encontrada!");

            conciliacaoBancaria.SettConciliadoManual(true, User.Identity.Name);
            context.Update(conciliacaoBancaria);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var conciliacao = context.ConciliacaoBancaria.FirstOrDefault(x => x.IdConciliacaoBancaria == id);
            if (conciliacao == null)
                return BadRequest("Conciliação não encontrada!");

            conciliacao.Excluir(User.Identity.Name);
            context.Update(conciliacao);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var conciliacao = context.ConciliacaoBancaria.FirstOrDefault(x => x.IdConciliacaoBancaria == id);
            if (conciliacao == null)
                return BadRequest("Conciliação não encontrada!");


            return Ok(new ConciliacaoBancariaResponse()
            {
                IdConciliacaoBancaria = conciliacao.IdConciliacaoBancaria,
                IdCliente = conciliacao.IdCliente,
                NomeCliente = conciliacao.Cliente.Pessoa.Nome,
                DataPagamento = conciliacao.DataPagamento,
                Valor = conciliacao.Valor,
                Adquirente = conciliacao.Adquirente,
                Status = conciliacao.Status,
                Situacao = conciliacao.Situacao
            });
        }

        [HttpPost]
        [Route("previaEmail")]
        [Authorize]
        public async Task<IActionResult> PreviaEmail([FromBody] PesquisarConciliacaoBancariaRequest model)
        {
            try
            {
                // Validação
                if (model == null || string.IsNullOrEmpty(model.Email))
                    return BadRequest(new { message = "Dados inválidos ou email não informado." });

                var dataInicio = model.DataInicio;
                var dataFim = model.DataFim;
                var periodoFormatado = $"{dataInicio:dd/MM/yyyy} - {dataFim:dd/MM/yyyy}";

                var cliente = context.Cliente.Include(x => x.Pessoa).FirstOrDefault(x => x.IdentificadorConciliadora == model.IdentificadorConciliadora);

                if (cliente == null)
                    return NotFound(new { message = "Cliente não encontrado." });

                var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == cliente.IdPessoa);

                // Buscar dados da conciliação para este cliente
                var dadosConciliacao = BuscarDadosConciliacao(model);

                string subject = $"[PRÉVIA] Relatório de Conciliação Bancária - {pessoa.Nome} - {periodoFormatado}";

                // Construir o HTML do email
                string templateHtml = GetEmailTemplateConciliacao();

                // Substituir placeholders do cabeçalho
                templateHtml = templateHtml.Replace("{NOME_CLIENTE}", pessoa.Nome);
                templateHtml = templateHtml.Replace("{PERIODO}", periodoFormatado);

                // Substituir estatísticas
                templateHtml = templateHtml.Replace("{TOTAL_PENDENTE}", dadosConciliacao.TotalPendente.ToString());
                templateHtml = templateHtml.Replace("{TOTAL_CONCILIADO}", dadosConciliacao.TotalConciliado.ToString());
                templateHtml = templateHtml.Replace("{TOTAL_NAO_CONCILIADO}", dadosConciliacao.TotalNaoConciliado.ToString());

                // Substituir dados de conciliação bancária
                templateHtml = templateHtml.Replace("{QUANTIDADE_CONCILIACOES}", dadosConciliacao.QuantidadeConciliacoes.ToString("N0"));
                templateHtml = templateHtml.Replace("{VALOR_TOTAL_CONCILIACOES}", dadosConciliacao.ValorTotalConciliacoes.ToString("N2"));

                // Substituir dados de vendas do ERP
                templateHtml = templateHtml.Replace("{QUANTIDADE_VENDAS_ERP}", dadosConciliacao.QuantidadeVendasERP.ToString("N0"));
                templateHtml = templateHtml.Replace("{VALOR_TOTAL_VENDAS_ERP}", dadosConciliacao.ValorTotalVendasERP.ToString("N2"));

                // Construir tabela de transações
                StringBuilder tabelaHtml = new StringBuilder();

                foreach (var item in dadosConciliacao.Transacoes)
                {
                    string statusClass = item.Status switch
                    {
                        "Pendente" => "status-pendente",
                        "Conciliado" => "status-conciliado",
                        "Não conciliado" => "status-nao-conciliado",
                        _ => "status-pendente"
                    };

                    string statusText = item.Status switch
                    {
                        "Pendente" => "🕐 Pendente",
                        "Conciliado" => "✅ Conciliado",
                        "Não conciliado" => "❌ Não Conciliado",
                        _ => "🕐 Pendente"
                    };

                    tabelaHtml.AppendLine($@"
                    <tr>
                        <td class='table-cell'>{item.IdConciliacaoBancaria}</td>
                        <td class='table-cell'>{item.DataPagamento:dd/MM/yyyy}</td>
                        <td class='table-cell valor'>R$ {item.Valor:N2}</td>
                        <td class='table-cell valor'>R$ {item.ValorConciliacao:N2}</td>
                        <td class='table-cell'>{item.Adquirente}</td>
                        <td class='table-cell'><span class='status {statusClass}'>{statusText}</span></td>
                    </tr>");
                }

                templateHtml = templateHtml.Replace("{TABELA_TRANSACOES}", tabelaHtml.ToString());

                // Configurar e enviar email para o email de teste
                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Relatórios");
                mailMsg.Subject = subject;

                mailMsg.To.Add(new MailAddress(model.Email.Trim()));

                mailMsg.IsBodyHtml = true;
                mailMsg.Body = templateHtml;
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
                    templateHtml, null, MediaTypeNames.Text.Html));

                smtpClientSendGrid.Credentials = credentialsSendGrid;

                await smtpClientSendGrid.SendMailAsync(mailMsg);

                return Ok(new { message = "Email de prévia enviado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao enviar email de prévia: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("enviarEmail")]
        [Authorize]
        public async Task<IActionResult> EnviarEmail([FromBody] PesquisarConciliacaoBancariaRequest model)
        {
            try
            {
                var dataInicio = model.DataInicio;
                var dataFim = model.DataFim;
                var periodoFormatado = $"{dataInicio:dd/MM/yyyy} - {dataFim:dd/MM/yyyy}";

                    var cliente = context.Cliente.Include(x => x.Pessoa).FirstOrDefault(x => x.IdentificadorConciliadora == model.IdentificadorConciliadora);

                    var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == cliente.IdPessoa);

                    // Buscar dados da conciliação para este cliente
                    var dadosConciliacao =  BuscarDadosConciliacao(model);

                    string subject = $"Relatório de Conciliação Bancária - {pessoa.Nome} - {periodoFormatado}";

                    // Construir o HTML do email
                    string templateHtml = GetEmailTemplateConciliacao();

                    // Substituir placeholders do cabeçalho
                    templateHtml = templateHtml.Replace("{NOME_CLIENTE}", pessoa.Nome);
                    templateHtml = templateHtml.Replace("{PERIODO}", periodoFormatado);

                    // Substituir estatísticas
                    templateHtml = templateHtml.Replace("{TOTAL_PENDENTE}", dadosConciliacao.TotalPendente.ToString());
                    templateHtml = templateHtml.Replace("{TOTAL_CONCILIADO}", dadosConciliacao.TotalConciliado.ToString());
                    templateHtml = templateHtml.Replace("{TOTAL_NAO_CONCILIADO}", dadosConciliacao.TotalNaoConciliado.ToString());

                    // Substituir dados de conciliação bancária
                    templateHtml = templateHtml.Replace("{QUANTIDADE_CONCILIACOES}", dadosConciliacao.QuantidadeConciliacoes.ToString("N0"));
                    templateHtml = templateHtml.Replace("{VALOR_TOTAL_CONCILIACOES}", dadosConciliacao.ValorTotalConciliacoes.ToString("N2"));

                    // Substituir dados de vendas do ERP
                    templateHtml = templateHtml.Replace("{QUANTIDADE_VENDAS_ERP}", dadosConciliacao.QuantidadeVendasERP.ToString("N0"));
                    templateHtml = templateHtml.Replace("{VALOR_TOTAL_VENDAS_ERP}", dadosConciliacao.ValorTotalVendasERP.ToString("N2"));

                    // Construir tabela de transações
                    StringBuilder tabelaHtml = new StringBuilder();

                    foreach (var item in dadosConciliacao.Transacoes)
                    {
                        string statusClass = item.Status switch
                        {
                            "Pendente" => "status-pendente",
                            "Conciliado" => "status-conciliado",
                            "Não conciliado" => "status-nao-conciliado",
                            _ => "status-pendente"
                        };

                        string statusText = item.Status switch
                        {
                            "Pendente" => "🕐 Pendente",
                            "Conciliado" => "✅ Conciliado",
                            "Não conciliado" => "❌ Não Conciliado",
                            _ => "🕐 Pendente"
                        };

                        tabelaHtml.AppendLine($@"
                    <tr>
                        <td class='table-cell'>{item.IdConciliacaoBancaria}</td>
                        <td class='table-cell'>{item.DataPagamento:dd/MM/yyyy}</td>
                        <td class='table-cell valor'>R$ {item.Valor:N2}</td>
                        <td class='table-cell valor'>R$ {item.ValorConciliacao:N2}</td>
                        <td class='table-cell'>{item.Adquirente}</td>
                        <td class='table-cell'><span class='status {statusClass}'>{statusText}</span></td>
                    </tr>");
                    }

                    templateHtml = templateHtml.Replace("{TABELA_TRANSACOES}", tabelaHtml.ToString());

                    // Configurar e enviar email
                    MailMessage mailMsg = new MailMessage();
                    mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Relatórios");
                    mailMsg.Subject = subject;

                    // Em produção, usar: mailMsg.To.Add(new MailAddress(pessoa.Email.Trim()));
                    mailMsg.To.Add(new MailAddress("kleytonwillian@gmail.com"));
                    mailMsg.To.Add(new MailAddress("renato@genialsoft.com.br"));

                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = templateHtml;
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
                        templateHtml, null, MediaTypeNames.Text.Html));

                    smtpClientSendGrid.Credentials = credentialsSendGrid;

                    await smtpClientSendGrid.SendMailAsync(mailMsg);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao enviar e-mails: {ex.Message}" });
            }
        }

        // Método auxiliar para buscar dados da conciliação
        private  DadosConciliacaoModel BuscarDadosConciliacao(PesquisarConciliacaoBancariaRequest filter)
        {
            // Implementar a lógica para buscar os dados da conciliação
            // Este é um exemplo - ajustar conforme sua estrutura de dados

            var cliente = context.Cliente.FirstOrDefault(x => x.IdentificadorConciliadora == filter.IdentificadorConciliadora);

            var query = context.ConciliacaoBancaria
                .Where(x => x.IdCliente == cliente.IdPessoa);

            if (!DateTime.MinValue.Equals(filter.DataInicio))
                query = query.Where(x => x.DataPagamento >= filter.DataInicio);

            if (!DateTime.MinValue.Equals(filter.DataFim))
                query = query.Where(x => x.DataPagamento <= filter.DataFim);

            var transacoes =  query.ToList();

            // Buscar vendas do ERP
            var vendasERP = context.VendasConciliadas
                .Where(x => x.IdentificadorConciliadora == filter.IdentificadorConciliadora);

            if (!DateTime.MinValue.Equals(filter.DataInicio))
                vendasERP = vendasERP.Where(x => x.DataVenda >= filter.DataInicio);

            if (!DateTime.MinValue.Equals(filter.DataFim))
                vendasERP = vendasERP.Where(x => x.DataVenda <= filter.DataFim);

            var listaVendasERP = vendasERP.ToList();

            return new DadosConciliacaoModel
            {
                TotalPendente = transacoes.Count(x => x.Status == "Pendente"),
                TotalConciliado = transacoes.Count(x => x.Status == "Conciliado"),
                TotalNaoConciliado = transacoes.Count(x => x.Status == "Não conciliado"),
                Transacoes = transacoes,
                QuantidadeConciliacoes = transacoes.Count,
                ValorTotalConciliacoes = transacoes.Sum(x => x.Valor),
                QuantidadeVendasERP = listaVendasERP.Count,
                ValorTotalVendasERP = listaVendasERP.Sum(x => x.TotalVenda)
            };
        }

        private string GetEmailTemplateConciliacao()
        {
            return @"<!DOCTYPE html>
        <html lang='pt-BR'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Relatório de Conciliação Bancária</title>
            <style>
                * {
                    box-sizing: border-box;
                    margin: 0;
                    padding: 0;
                }
                
                body {
                    font-family: 'Segoe UI', Roboto, -apple-system, BlinkMacSystemFont, sans-serif;
                    line-height: 1.6;
                    color: #333;
                    background-color: #f5f7fa;
                }
                
                .email-container {
                    max-width: 800px;
                    margin: 20px auto;
                    background-color: #ffffff;
                    border-radius: 12px;
                    box-shadow: 0 4px 20px rgba(0,0,0,0.1);
                    overflow: hidden;
                }
                
                .header {
                    background: linear-gradient(135deg, #4a90e2 0%, #357abd 100%);
                    color: white;
                    padding: 40px 30px;
                    text-align: center;
                }
                
                .header h1 {
                    font-size: 32px;
                    font-weight: 600;
                    margin-bottom: 10px;
                }
                
                .header p {
                    font-size: 18px;
                    opacity: 0.95;
                }
                
                .content {
                    padding: 40px 30px;
                }
                
                .greeting {
                    font-size: 18px;
                    color: #555;
                    margin-bottom: 30px;
                    padding: 20px;
                    background: #f8f9fa;
                    border-left: 4px solid #4a90e2;
                    border-radius: 8px;
                }
                
                .stats-container {
                    margin: 30px 0;
                }
                
                .stats-title {
                    font-size: 24px;
                    font-weight: 600;
                    color: #333;
                    margin-bottom: 20px;
                    display: flex;
                    align-items: center;
                }
                
                .stats-grid {
                    display: grid;
                    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
                    gap: 20px;
                    margin-bottom: 40px;
                }
                
                .stat-card {
                    padding: 25px 20px;
                    border-radius: 12px;
                    text-align: center;
                    box-shadow: 0 2px 8px rgba(0,0,0,0.05);
                }
                
                .stat-card.pendentes {
                    background: linear-gradient(135deg, #fff5f5 0%, #fed7d7 100%);
                    border-left: 4px solid #f56565;
                }
                
                .stat-card.conciliados {
                    background: linear-gradient(135deg, #f0fff4 0%, #c6f6d5 100%);
                    border-left: 4px solid #48bb78;
                }
                
                .stat-card.nao-conciliados {
                    background: linear-gradient(135deg, #fffbf0 0%, #feebc8 100%);
                    border-left: 4px solid #ed8936;
                }
                
                .stat-icon {
                    font-size: 28px;
                    margin-bottom: 10px;
                }
                
                .stat-label {
                    font-size: 16px;
                    font-weight: 500;
                    margin-bottom: 5px;
                    color: #666;
                }
                
                .stat-value {
                    font-size: 32px;
                    font-weight: bold;
                }
                
                .stat-value.pendentes { color: #f56565; }
                .stat-value.conciliados { color: #48bb78; }
                .stat-value.nao-conciliados { color: #ed8936; }

                .erp-conciliacao-box {
                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                    border-radius: 12px;
                    padding: 30px;
                    margin: 30px 0;
                    box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
                }

                .erp-conciliacao-title {
                    font-size: 24px;
                    font-weight: 700;
                    color: white;
                    margin-bottom: 10px;
                    text-align: center;
                }

                .erp-conciliacao-subtitle {
                    font-size: 14px;
                    color: white;
                    opacity: 0.9;
                    text-align: center;
                    margin-bottom: 25px;
                    font-style: italic;
                }

                .stats-grid-conciliacao {
                    display: grid;
                    grid-template-columns: repeat(3, 1fr);
                    gap: 15px;
                    margin-bottom: 20px;
                }

                .stat-card-conciliacao {
                    background: white;
                    padding: 20px 15px;
                    border-radius: 8px;
                    box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                    text-align: center;
                }

                .stat-card-conciliacao.pendentes {
                    border-top: 4px solid #f56565;
                }

                .stat-card-conciliacao.conciliados {
                    border-top: 4px solid #48bb78;
                }

                .stat-card-conciliacao.nao-conciliados {
                    border-top: 4px solid #ed8936;
                }

                .divider-interno {
                    height: 1px;
                    background: rgba(255, 255, 255, 0.3);
                    margin: 20px 0;
                }

                .erp-conciliacao-grid {
                    display: grid;
                    grid-template-columns: repeat(2, 1fr);
                    gap: 20px;
                }

                .erp-conciliacao-item {
                    background: white;
                    padding: 25px;
                    border-radius: 10px;
                    box-shadow: 0 4px 10px rgba(0,0,0,0.1);
                    text-align: center;
                }

                .erp-conciliacao-label {
                    font-size: 13px;
                    color: #666;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                    margin-bottom: 12px;
                    font-weight: 700;
                }

                .erp-conciliacao-value {
                    font-size: 32px;
                    font-weight: bold;
                    margin-top: 5px;
                }

                .erp-conciliacao-value.quantidade-erp {
                    color: #667eea;
                }

                .erp-conciliacao-value.total-erp {
                    color: #48bb78;
                }

                .erp-vendas-box {
                    background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
                    border-radius: 12px;
                    padding: 30px;
                    margin: 30px 0;
                    box-shadow: 0 4px 15px rgba(245, 87, 108, 0.3);
                }

                .erp-vendas-title {
                    font-size: 24px;
                    font-weight: 700;
                    color: white;
                    margin-bottom: 10px;
                    text-align: center;
                }

                .erp-vendas-subtitle {
                    font-size: 14px;
                    color: white;
                    opacity: 0.9;
                    text-align: center;
                    margin-bottom: 25px;
                    font-style: italic;
                }

                .erp-vendas-grid {
                    display: grid;
                    grid-template-columns: repeat(2, 1fr);
                    gap: 20px;
                }

                .erp-vendas-item {
                    background: white;
                    padding: 25px;
                    border-radius: 10px;
                    box-shadow: 0 4px 10px rgba(0,0,0,0.1);
                    text-align: center;
                }

                .erp-vendas-label {
                    font-size: 13px;
                    color: #666;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                    margin-bottom: 12px;
                    font-weight: 700;
                }

                .erp-vendas-value {
                    font-size: 32px;
                    font-weight: bold;
                    margin-top: 5px;
                }

                .erp-vendas-value.quantidade-vendas {
                    color: #f093fb;
                }

                .erp-vendas-value.total-vendas {
                    color: #f5576c;
                }

                .table-container {
                    margin: 30px 0;
                    overflow-x: auto;
                    border-radius: 12px;
                    box-shadow: 0 4px 12px rgba(0,0,0,0.08);
                }
                
                .table-title {
                    font-size: 24px;
                    font-weight: 600;
                    color: #333;
                    margin-bottom: 20px;
                }
                
                table {
                    width: 100%;
                    border-collapse: collapse;
                    background: white;
                }
                
                .table-header {
                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                    color: white;
                }
                
                .table-header th {
                    padding: 15px 12px;
                    text-align: left;
                    font-weight: 600;
                    font-size: 14px;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                }
                
                .table-cell {
                    padding: 15px 12px;
                    border-bottom: 1px solid #e2e8f0;
                    font-size: 14px;
                }
                
                .table-cell.valor {
                    font-weight: 600;
                    color: #2d3748;
                }
                
                tr:nth-child(even) {
                    background-color: #f8f9fa;
                }
                
                tr:hover {
                    background-color: #e8f4fd;
                }
                
                .status {
                    padding: 6px 12px;
                    border-radius: 20px;
                    font-size: 12px;
                    font-weight: 600;
                    text-align: center;
                    display: inline-block;
                    min-width: 100px;
                }
                
                .status.status-pendente {
                    background: #fff5f5;
                    color: #f56565;
                    border: 1px solid #feb2b2;
                }
                
                .status.status-conciliado {
                    background: #f0fff4;
                    color: #48bb78;
                    border: 1px solid #9ae6b4;
                }
                
                .status.status-nao-conciliado {
                    background: #fffbf0;
                    color: #ed8936;
                    border: 1px solid #f6d05;
                }
                
                .footer {
                    background: linear-gradient(135deg, #f7fafc 0%, #edf2f7 100%);
                    padding: 30px;
                    text-align: center;
                    border-top: 1px solid #e2e8f0;
                }
                
                .footer-text {
                    color: #718096;
                    font-size: 14px;
                    margin-bottom: 10px;
                }
                
                .footer-company {
                    font-weight: 600;
                    color: #4a90e2;
                    font-size: 18px;
                }
                
                .divider {
                    height: 2px;
                    background: linear-gradient(to right, transparent, #e2e8f0, transparent);
                    margin: 30px 0;
                }
                
                @media (max-width: 600px) {
                    .email-container {
                        margin: 10px;
                        border-radius: 8px;
                    }

                    .header {
                        padding: 30px 20px;
                    }

                    .header h1 {
                        font-size: 24px;
                    }

                    .content {
                        padding: 30px 20px;
                    }

                    .stats-grid {
                        grid-template-columns: 1fr;
                        gap: 15px;
                    }

                    .stats-grid-conciliacao {
                        grid-template-columns: 1fr;
                        gap: 12px;
                    }

                    .erp-conciliacao-grid {
                        grid-template-columns: 1fr;
                    }

                    .erp-vendas-grid {
                        grid-template-columns: 1fr;
                    }

                    .table-container {
                        overflow-x: scroll;
                    }

                    .table-header th,
                    .table-cell {
                        padding: 10px 8px;
                        font-size: 12px;
                    }
                }
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='header'>
                    <h1>📊 Conciliação Bancária</h1>
                    <p>Relatório para {NOME_CLIENTE}</p>
                    <p style='font-size: 16px; margin-top: 5px; opacity: 0.9;'>Período: {PERIODO}</p>
                </div>
                
                <div class='content'>
                    <div class='greeting'>
                        <strong>Prezado(a) {NOME_CLIENTE},</strong><br><br>
                        Segue abaixo o relatório detalhado da sua conciliação bancária para o período informado. 
                        Este relatório contém informações sobre o status das suas transações e conciliações.
                    </div>
                    
                    <div class='erp-conciliacao-box'>
                        <div class='erp-conciliacao-title'>💳 Resumo Geral Conciliação Bancária</div>
                        <div class='erp-conciliacao-subtitle'>Pagamentos recebidos das operadoras (Cielo, Stone, Getnet, etc.)</div>
                        <div class='stats-grid-conciliacao'>
                            <div class='stat-card-conciliacao pendentes'>
                                <div class='stat-icon'>🕐</div>
                                <div class='stat-label'>Pendentes</div>
                                <div class='stat-value pendentes'>{TOTAL_PENDENTE}</div>
                            </div>

                            <div class='stat-card-conciliacao conciliados'>
                                <div class='stat-icon'>✅</div>
                                <div class='stat-label'>Conciliados</div>
                                <div class='stat-value conciliados'>{TOTAL_CONCILIADO}</div>
                            </div>

                            <div class='stat-card-conciliacao nao-conciliados'>
                                <div class='stat-icon'>❌</div>
                                <div class='stat-label'>Não Conciliados</div>
                                <div class='stat-value nao-conciliados'>{TOTAL_NAO_CONCILIADO}</div>
                            </div>
                        </div>
                        <div class='divider-interno'></div>
                        <div class='erp-conciliacao-grid'>
                            <div class='erp-conciliacao-item'>
                                <div class='erp-conciliacao-label'>QUANTIDADE DE CONCILIAÇÕES</div>
                                <div class='erp-conciliacao-value quantidade-erp'>{QUANTIDADE_CONCILIACOES}</div>
                            </div>
                            <div class='erp-conciliacao-item'>
                                <div class='erp-conciliacao-label'>VALOR TOTAL</div>
                                <div class='erp-conciliacao-value total-erp'>R$ {VALOR_TOTAL_CONCILIACOES}</div>
                            </div>
                        </div>
                    </div>

                    <div class='divider'></div>

                    <div class='erp-vendas-box'>
                        <div class='erp-vendas-title'>🛒 Resumo Geral das Vendas ERP</div>
                        <div class='erp-vendas-subtitle'>Vendas do caixa do supermercado (produtos)</div>
                        <div class='erp-vendas-grid'>
                            <div class='erp-vendas-item'>
                                <div class='erp-vendas-label'>QUANTIDADE DE VENDAS</div>
                                <div class='erp-vendas-value quantidade-vendas'>{QUANTIDADE_VENDAS_ERP}</div>
                            </div>
                            <div class='erp-vendas-item'>
                                <div class='erp-vendas-label'>VALOR TOTAL DE VENDAS</div>
                                <div class='erp-vendas-value total-vendas'>R$ {VALOR_TOTAL_VENDAS_ERP}</div>
                            </div>
                        </div>
                    </div>

                    <div class='divider'></div>
                    
                    <div class='table-container'>
                        <h2 class='table-title'>📋 Detalhamento das Transações</h2>
                        <table>
                            <thead class='table-header'>
                                <tr>
                                    <th>Código</th>
                                    <th>Data de Pagamento</th>
                                    <th>Valor Pagamento</th>
                                    <th>Valor Extrato</th>
                                    <th>Adquirente</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody>
                                {TABELA_TRANSACOES}
                            </tbody>
                        </table>
                    </div>
                </div>
                
                <div class='footer'>
                    <div class='footer-text'>
                        Este é um e-mail automático gerado pelo sistema Concicard.<br>
                        Em caso de dúvidas, entre em contato conosco.
                    </div>
                    <div class='footer-company'>
                        Concicard - Gestão Inteligente de Conciliação
                    </div>
                </div>
            </div>
        </body>
        </html>";
        }

    }
}
