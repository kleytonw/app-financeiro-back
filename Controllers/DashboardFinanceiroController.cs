using Dapper;
using ERP.Infra;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DashboardFinanceiroController : ControllerBase
    {
        protected Context context;
        public DashboardFinanceiroController(Context context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("planoConta")]
        [Authorize]
        public IActionResult PlanoConta([FromBody] DashboardFinanceiroPesquisaRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sql = @"SELECT 
                        PlanoConta.Descricao,
                        SUM(ISNULL(FinanceiroParcela.ValorAcerto, 0)) AS Total
                    FROM FinanceiroParcela
                    LEFT JOIN PlanoConta ON PlanoConta.IdPlanoConta = FinanceiroParcela.IdPlanoConta
                    WHERE FinanceiroParcela.DataVencimento BETWEEN @DataInicio AND @DataFim
                      AND FinanceiroParcela.Situacao = 'Baixado'
                    GROUP BY PlanoConta.Descricao
                    ORDER BY PlanoConta.Descricao";

                var lista = conn.Query<DashboardFinanceiroPlanoContaResponse>(sql, new
                {
                    DataInicio = model.DataInicio.Date,
                    DataFim = model.DataFim.Date
                }).ToList();

                return Ok(lista);
            }
        }

        [HttpPost]
        [Route("custoFixoVariavel")]
        [Authorize]
        public ActionResult CustoFixoVariavel([FromBody] DashboardFinanceiroPesquisaRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sql = $@"
                    SELECT 
                        PlanoConta.Tipo, 
                        SUM(FinanceiroParcela.ValorAcerto) AS Total 
                    FROM FinanceiroParcela
                    LEFT JOIN PlanoConta ON PlanoConta.IdPlanoConta = FinanceiroParcela.IdPlanoConta
                    WHERE FinanceiroParcela.Situacao = 'Baixado'
                      AND FinanceiroParcela.DataVencimento BETWEEN @DataInicio AND @DataFim
                    GROUP BY PlanoConta.Tipo";

                var lista = conn.Query<DashboardFinanceiroCustoFixoVariavelResponse>(sql, new
                {
                    DataInicio = model.DataInicio.Date,
                    DataFim = model.DataFim.Date
                }).ToList();

                return Ok(lista);
            }
        }

        [HttpGet]
        [Route("clientesAtivosInativos")]
        [Authorize]
        public IActionResult ClientesAtivosInativos()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var clientes = context.Cliente.AsQueryable();

            var ativos = clientes.Include(x => x.Pessoa).Count(x => x.Situacao == "Ativo");

            var inativos = clientes.Include(x => x.Pessoa).Count(x => x.Situacao == "Excluido");


            return Ok(new
            {
                Ativos = ativos,
                Inativos = inativos
            });

        }

        [HttpGet]
        [Route("contasPagar")]
        [Authorize]
        public IActionResult ContasPagar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var contasPorMes = context.FinanceiroParcela
                .Include(x => x.Financeiro)
                .Where(x => x.Financeiro.Tipo == "Contas a Pagar" && x.Situacao == "Aberto")
                .GroupBy(x => new { x.DataVencimento.Month, x.DataVencimento.Year })
                .Select(g => new
                {
                    Mes = g.Key.Month,
                    Ano = g.Key.Year,
                    Total = g.Count()
                })
                .OrderBy(x => x.Ano).ThenBy(x => x.Mes)
                .ToList();

            return Ok(contasPorMes);
        }

        [HttpPost]
        [Route("contasPagarReceber")]
        [Authorize]
        public IActionResult ContasPagarReceber([FromBody] DashboardFinanceiroPesquisaRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                    string sql = @"SELECT 
                            FORMAT(FinanceiroParcela.DataVencimento, 'yyyy-MM') AS MesAno,
                            SUM(CASE WHEN Financeiro.Tipo = 'Contas a Pagar' THEN FinanceiroParcela.ValorVencimento ELSE 0 END) AS ContasPagar,
                            SUM(CASE WHEN Financeiro.Tipo = 'Contas a Receber' THEN FinanceiroParcela.ValorVencimento ELSE 0 END) AS ContasReceber
                        FROM FinanceiroParcela
                        INNER JOIN Financeiro ON Financeiro.IdFinanceiro = FinanceiroParcela.IdFinanceiro
                        WHERE CAST(FinanceiroParcela.DataVencimento AS DATE) BETWEEN @DataInicio AND @DataFim
                          AND FinanceiroParcela.Situacao = 'Baixado'
                        GROUP BY FORMAT(FinanceiroParcela.DataVencimento, 'yyyy-MM')
                        ORDER BY MesAno";

                var lista = conn.Query<DashboardFinanceiroContasPagarReceberResponse>(sql, new
                {
                    DataInicio = model.DataInicio.Date,
                    DataFim = model.DataFim.Date
                }).ToList();

                return Ok(lista);
            }
        }
    }
}
