using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using Microsoft.EntityFrameworkCore;
using ERP_API.Models.BI;
using System.Linq;
using Dapper;
using System.Collections.Generic;
using System;
using ERP_API.Models.DashboardTransacao;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class DashboardTransacaoController : ControllerBase
    {
        protected Context context;
        public DashboardTransacaoController(Context context)
        {
            this.context = context;
        }


        /// <summary>
        /// Total de transações por bandeira
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("indicador1")]
        [AllowAnonymous]
        public IActionResult DashboardTransacoesBandeira([FromBody] FiltroDashboardTransacaoRequestModel model)
        {
            var result = new List<DashboardTransacoesBandeiraModel>();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select Transacao.IdUnidade, Transacao.Bandeira,  Transacao.NomeOperadora, 
	                    Sum(ValorBruto) TotalBruto, 
	                    Sum(ValorLiquido) TotalLiquido, 
	                    Sum(Despesa) TotalDespesas, 
	                    Count(*) as QuantidadeTransacoes 
                        from Transacao 
                        where Transacao.IdUnidade = '{model.IdUnidade}' and Transacao.IdOperadora = '{model.IdOperadora}' ";
                sqlBI += $@" and CAST(Transacao.DataMovimentacao as date)  BETWEEN Convert(date, '{Convert.ToDateTime(model.DataInicial).ToString("yyyy-MM-dd")}', 23)  AND Convert(date, '{Convert.ToDateTime(model.DataFinal).ToString("yyyy-MM-dd")}', 23) 
                        Group by Transacao.IdUnidade, Transacao.Bandeira, Transacao.NomeOperadora
                        ORDER BY Transacao.IdUnidade, Transacao.Bandeira, Transacao.NomeOperadora ";

                result = conn.Query<DashboardTransacoesBandeiraModel>(sqlBI).ToList();
            }
            return Ok(result);
        }

        /// <summary>
        /// Total de transações por meio de pagamento 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("indicador2")]
        [AllowAnonymous]
        public IActionResult DashboardTransacoesMeioPagamento([FromBody] FiltroDashboardTransacaoRequestModel model)
        {
            var result = new List<DashboardTransacoesMeioPagamentoModel>();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select Transacao.IdUnidade, Transacao.MeioPagamento,  Transacao.NomeOperadora, 
	                    Sum(ValorBruto) TotalBruto, 
	                    Sum(ValorLiquido) TotalLiquido, 
	                    Sum(Despesa) TotalDespesas, 
	                    Count(*) as QuantidadeTransacoes 
                        from Transacao 
                        where Transacao.IdUnidade = '{model.IdUnidade}' and Transacao.IdOperadora = '{model.IdOperadora}' ";
                sqlBI += $@" and CAST(Transacao.DataMovimentacao as date)  BETWEEN Convert(date, '{Convert.ToDateTime(model.DataInicial).ToString("yyyy-MM-dd")}', 23)  AND Convert(date, '{Convert.ToDateTime(model.DataFinal).ToString("yyyy-MM-dd")}', 23) 
                        Group by Transacao.IdUnidade, Transacao.MeioPagamento, Transacao.NomeOperadora
                        ORDER BY Transacao.IdUnidade, Transacao.MeioPagamento, Transacao.NomeOperadora ";

                result = conn.Query<DashboardTransacoesMeioPagamentoModel>(sqlBI).ToList();
            }
            return Ok(result);
        }

        /// <summary>
        /// Total de transações por terminal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("indicador3")]
        [AllowAnonymous]
        public IActionResult DashboardTransacoesTerminal([FromBody] FiltroDashboardTransacaoRequestModel model)
        {
            var result = new List<DashboardTransacoesTerminalModel>();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select Transacao.IdUnidade, Transacao.Terminal,  Transacao.NomeOperadora, 
	                    Sum(ValorBruto) TotalBruto, 
	                    Sum(ValorLiquido) TotalLiquido, 
	                    Sum(Despesa) TotalDespesas, 
	                    Count(*) as QuantidadeTransacoes 
                        from Transacao 
                        where Transacao.IdUnidade = '{model.IdUnidade}' and Transacao.IdOperadora = '{model.IdOperadora}' ";
                sqlBI += $@" and CAST(Transacao.DataMovimentacao as date)  BETWEEN Convert(date, '{Convert.ToDateTime(model.DataInicial).ToString("yyyy-MM-dd")}', 23)  AND Convert(date, '{Convert.ToDateTime(model.DataFinal).ToString("yyyy-MM-dd")}', 23) 
                        Group by Transacao.IdUnidade, Transacao.Terminal, Transacao.NomeOperadora
                        ORDER BY Transacao.IdUnidade, Transacao.Terminal, Transacao.NomeOperadora ";

                result = conn.Query<DashboardTransacoesTerminalModel>(sqlBI).ToList();
            }
            return Ok(result);
        }


        /// <summary>
        /// Total de transações 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("indicador4")]
        [AllowAnonymous]
        public IActionResult DashboardTransacoesTotalizadores([FromBody] FiltroDashboardTransacaoRequestModel model)
        {
            var result = new DashboardTransacoesTotalizados();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@" select
	                    Sum(ValorBruto) TotalBruto, 
	                    Sum(ValorLiquido) TotalLiquido, 
	                    Sum(Despesa) TotalDespesas, 
	                    Count(*) as QuantidadeTransacoes 
                        from Transacao 
                        where Transacao.IdUnidade = '{model.IdUnidade}' and Transacao.IdOperadora = '{model.IdOperadora}' ";
                sqlBI += $@" and CAST(Transacao.DataMovimentacao as date)  BETWEEN Convert(date, '{Convert.ToDateTime(model.DataInicial).ToString("yyyy-MM-dd")}', 23)  AND Convert(date, '{Convert.ToDateTime(model.DataFinal).ToString("yyyy-MM-dd")}', 23)  ";

                result = conn.Query<DashboardTransacoesTotalizados>(sqlBI).FirstOrDefault();
            }
            return Ok(result);
        }


        /// <summary>
        /// Total de transações 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("indicador5")]
        [AllowAnonymous]
        public IActionResult DashboardTransacoesProduto([FromBody] FiltroDashboardTransacaoRequestModel model)
        {
            var result = new List<DashboardTransacoesProdutoModel>();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select Transacao.IdUnidade, Transacao.DescricaoProduto as Produto,  Transacao.NomeOperadora, 
	                    Sum(ValorBruto) TotalBruto, 
	                    Sum(ValorLiquido) TotalLiquido, 
	                    Sum(Despesa) TotalDespesas, 
	                    Count(*) as QuantidadeTransacoes 
                        from Transacao 
                        where Transacao.IdUnidade = '{model.IdUnidade}' and Transacao.IdOperadora = '{model.IdOperadora}' ";
                        sqlBI += $@" and CAST(Transacao.DataMovimentacao as date)  BETWEEN Convert(date, '{Convert.ToDateTime(model.DataInicial).ToString("yyyy-MM-dd")}', 23)  AND Convert(date, '{Convert.ToDateTime(model.DataFinal).ToString("yyyy-MM-dd")}', 23) 
                        Group by Transacao.IdUnidade, Transacao.DescricaoProduto, Transacao.NomeOperadora
                        ORDER BY Transacao.IdUnidade, Transacao.DescricaoProduto, Transacao.NomeOperadora ";

                result = conn.Query<DashboardTransacoesProdutoModel>(sqlBI).ToList();
            }
            return Ok(result);
        }



    }
}