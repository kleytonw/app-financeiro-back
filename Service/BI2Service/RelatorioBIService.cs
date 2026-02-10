using ERP.Infra;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dapper;
using System.Linq;
using ERP_API.Models.BI2;
using ERP_API.Models.BI2.Filtros; 

namespace ERP_API.Service.BI2Service
{
    public class RelatorioBIService : IRelatorioBIService
    {
        public Context _context;
        public RelatorioBIService(Context context)
        {

            _context = context;
        }

        public List<RelatorioDevolucaoModel> ObterDevolucoes(FiltroBIModel model)
        {
            ICollection<RelatorioDevolucaoModel> lista = new List<RelatorioDevolucaoModel>();

            using (IDbConnection conn = _context.Database.GetDbConnection())
            {
                string sqlBI = @"
                    SELECT 
                        YEAR(DataMovimentacao) AS Ano,
                        MONTH(DataMovimentacao) AS Mes,
                        SUM(CASE WHEN TipoMovimentacao = 'VENDA' THEN ValorTotal ELSE 0 END) AS TotalVendas,
                        SUM(CASE WHEN TipoMovimentacao = 'DEVOLUCAO_VENDA' THEN ValorTotal ELSE 0 END) AS TotalDevolucoes,
                        CASE 
                            WHEN SUM(CASE WHEN TipoMovimentacao = 'VENDA' THEN ValorTotal ELSE 0 END) > 0 THEN
                                ROUND(
                                    (SUM(CASE WHEN TipoMovimentacao = 'DEVOLUCAO_VENDA' THEN ValorTotal ELSE 0 END) * 100.0) /
                                    SUM(CASE WHEN TipoMovimentacao = 'VENDA' THEN ValorTotal ELSE 0 END), 2
                                )
                            ELSE 0 
                        END AS PercentualDevolucao
                    FROM MovimentacaoDiaria
                    WHERE 
                        (@IdCliente IS NULL OR IdCliente = @IdCliente)
                        AND DataMovimentacao BETWEEN @DataInicial AND @DataFinal
                    GROUP BY 
                        YEAR(DataMovimentacao),
                        MONTH(DataMovimentacao)
                    ORDER BY 
                        Ano, Mes;";

                lista = conn.Query<RelatorioDevolucaoModel>(sqlBI, new
                {
                    model.IdCliente,
                    model.DataInicial,
                    model.DataFinal
                }).ToList();
            }

            return (List<RelatorioDevolucaoModel>)lista;

        }

        public List<RelatorioFaturamentoMensalModel> ObterFaturamentoMensal(FiltroBIModel model)
        {
            ICollection<RelatorioFaturamentoMensalModel> lista = new List<RelatorioFaturamentoMensalModel>();
            using (IDbConnection conn = _context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                    SELECT 
                        YEAR(DataMovimentacao) AS Ano,
                        MONTH(DataMovimentacao) AS Mes,
                        FORMAT(SUM(ValorTotal), 'N2') AS FaturamentoTotal
                    FROM MovimentacaoDiaria
                    WHERE TipoMovimentacao = 'VENDA'
                    AND (@IdCliente IS NULL OR IdCliente = @IdCliente)
                    AND DataMovimentacao BETWEEN @DataInicial AND @DataFinal
                    GROUP BY YEAR(DataMovimentacao), MONTH(DataMovimentacao)
                    ORDER BY Ano, Mes";

                lista = conn.Query<RelatorioFaturamentoMensalModel>(sqlBI, new
                {
                    model.IdCliente,
                    model.DataInicial,
                    model.DataFinal
                }).ToList();
            }

            return (List<RelatorioFaturamentoMensalModel>)lista;
        }

        public List<RelatorioMargemBrutaModel> ObterMargemBrutaMensal(FiltroBIModel model)
        {
            ICollection<RelatorioMargemBrutaModel> lista = new List<RelatorioMargemBrutaModel>();
            using (IDbConnection conn = _context.Database.GetDbConnection())
            {
                string sqlBI = @"
                SELECT 
                    YEAR(DataMovimentacao) AS Ano,
                    MONTH(DataMovimentacao) AS Mes,
                    SUM(ValorTotal) AS TotalVendas,
                    SUM(CMV_Total) AS TotalCmv,
                    CASE 
                        WHEN SUM(ValorTotal) > 0 THEN 
                            ROUND(((SUM(ValorTotal) - SUM(CMV_Total)) / SUM(ValorTotal)) * 100, 2)
                        ELSE 0 
                    END AS MargemBrutaPercentual
                FROM MovimentacaoDiaria
                WHERE TipoMovimentacao = 'VENDA'
                AND (@IdCliente IS NULL OR IdCliente = @IdCliente)
                AND DataMovimentacao BETWEEN @DataInicial AND @DataFinal
                GROUP BY YEAR(DataMovimentacao), MONTH(DataMovimentacao)
                ORDER BY Ano, Mes";

                lista = conn.Query<RelatorioMargemBrutaModel>(sqlBI, new
                {
                    model.IdCliente,
                    model.DataInicial,
                    model.DataFinal
                }).ToList();
            }

            return (List<RelatorioMargemBrutaModel>)lista;
        }

        public List<RelatorioMovimentacaoPeriodoModel> ObterMovimentacaoPeriodo(FiltroBIMovModel model)
        {
            ICollection<RelatorioMovimentacaoPeriodoModel> lista = new List<RelatorioMovimentacaoPeriodoModel>();

            using (IDbConnection conn = _context.Database.GetDbConnection())
            {
                const string sqlBI = @"
                    IF (@Granularidade = 'D')
                    BEGIN
                        SELECT 
                            YEAR(DataMovimentacao) AS Ano,
                            MONTH(DataMovimentacao) AS Mes,
                            DAY(DataMovimentacao) AS Dia,
                            CONVERT(date, DataMovimentacao) AS PeriodoRef,
                            SUM(ValorTotal) AS ValorTotal
                        FROM MovimentacaoDiaria
                        WHERE 
                            (@IdCliente IS NULL OR IdCliente = @IdCliente)
                            AND DataMovimentacao BETWEEN @DataInicial AND @DataFinal
                            AND (@SomenteVendas = 0 OR TipoMovimentacao = 'VENDA')
                        GROUP BY 
                            CONVERT(date, DataMovimentacao),
                            YEAR(DataMovimentacao),
                            MONTH(DataMovimentacao),
                            DAY(DataMovimentacao)
                        ORDER BY 
                            PeriodoRef;
                    END
                    ELSE
                    BEGIN
                        SELECT 
                            YEAR(DataMovimentacao) AS Ano,
                            MONTH(DataMovimentacao) AS Mes,
                            NULL AS Dia,
                            DATEFROMPARTS(YEAR(DataMovimentacao), MONTH(DataMovimentacao), 1) AS PeriodoRef,
                            SUM(ValorTotal) AS ValorTotal
                        FROM MovimentacaoDiaria
                        WHERE 
                            (@IdCliente IS NULL OR IdCliente = @IdCliente)
                            AND DataMovimentacao BETWEEN @DataInicial AND @DataFinal
                            AND (@SomenteVendas = 0 OR TipoMovimentacao = 'VENDA')
                        GROUP BY 
                            YEAR(DataMovimentacao),
                            MONTH(DataMovimentacao)
                        ORDER BY 
                            Ano, Mes;
                    END";

                lista = conn.Query<RelatorioMovimentacaoPeriodoModel>(
                    sqlBI,
                    new
                    {
                        model.IdCliente,
                        model.DataInicial,
                        model.DataFinal,
                        model.Granularidade,     // "D" ou "M" model.Granularidade
                        SomenteVendas = true     // ou false para todas as movimentações
                    }
                ).ToList();
            }
            return (List<RelatorioMovimentacaoPeriodoModel>)lista;
        }

        public List<RelatorioMovimentacaoTipoModel> ObterMovimentacaoPorTipo(FiltroBIModel model)
        {
            ICollection<RelatorioMovimentacaoTipoModel> lista = new List<RelatorioMovimentacaoTipoModel>();

            using (IDbConnection conn = _context.Database.GetDbConnection())
            {
                string sqlBI = @"
                SELECT 
                    TipoMovimentacao,
                    SUM(ValorTotal) AS ValorTotal,
                    COUNT(*) AS QtdeMovimentacoes,
                    ROUND(
                        (SUM(ValorTotal) * 100.0) / NULLIF(SUM(SUM(ValorTotal)) OVER(), 0),
                        2
                    ) AS Percentual
                FROM MovimentacaoDiaria
                WHERE 
                    (@IdCliente IS NULL OR IdCliente = @IdCliente)
                    AND DataMovimentacao BETWEEN @DataInicial AND @DataFinal
                GROUP BY 
                    TipoMovimentacao
                ORDER BY 
                    ValorTotal DESC;";

                lista = conn.Query<RelatorioMovimentacaoTipoModel>(sqlBI, new
                {
                    model.IdCliente,
                    model.DataInicial,
                    model.DataFinal
                }).ToList();


            }
            return (List<RelatorioMovimentacaoTipoModel>)lista;

        }

        public List<RelatorioTicketMedioModel> ObterTicketMedio(FiltroBIModel model)
        {
            ICollection<RelatorioTicketMedioModel> lista = new List<RelatorioTicketMedioModel>();
            using (IDbConnection conn = _context.Database.GetDbConnection())
            {
                string sqlBI = @"
                WITH VendasUnicas AS
                    (
                        SELECT 
                            IdCliente,
                            NotaFiscal,
                            YEAR(DataMovimentacao) AS Ano,
                            MONTH(DataMovimentacao) AS Mes,
                            SUM(ValorTotal) AS ValorNota
                        FROM MovimentacaoDiaria
                        WHERE 
                            TipoMovimentacao = 'VENDA'
                            AND NotaFiscal IS NOT NULL
                            AND (@IdCliente IS NULL OR IdCliente = @IdCliente)
                            AND DataMovimentacao BETWEEN @DataInicial AND @DataFinal
                        GROUP BY 
                            IdCliente,
                            NotaFiscal,
                            YEAR(DataMovimentacao),
                            MONTH(DataMovimentacao)
                    )
                    SELECT 
                        Ano,
                        Mes,
                        COUNT(DISTINCT NotaFiscal) AS QtdeTransacoes,
                        SUM(ValorNota) AS TotalVendas,
                        CASE 
                            WHEN COUNT(DISTINCT NotaFiscal) > 0 THEN 
                                ROUND(SUM(ValorNota) / COUNT(DISTINCT NotaFiscal), 2)
                            ELSE 
                                0 
                        END AS TicketMedio
                    FROM VendasUnicas
                    GROUP BY 
                        Ano,
                        Mes
                    ORDER BY 
                        Ano,
                        Mes";

                lista = conn.Query<RelatorioTicketMedioModel>(sqlBI, new
                {
                    model.IdCliente,
                    model.DataInicial,
                    model.DataFinal
                }).ToList();
            }

            return (List<RelatorioTicketMedioModel>)lista;
        }

        public List<RelatorioVolumeItensModel> ObterVolumeItens(FiltroBIModel model)
        {
            ICollection<RelatorioVolumeItensModel> lista = new List<RelatorioVolumeItensModel>();

            using (IDbConnection conn = _context.Database.GetDbConnection())
            {
                string sqlBI = @"
        SELECT 
            YEAR(DataMovimentacao) AS Ano,
            MONTH(DataMovimentacao) AS Mes,
            TipoMovimentacao,
            SUM(Quantidade) AS QtdeTotal
        FROM MovimentacaoDiaria
        WHERE 
            (@IdCliente IS NULL OR IdCliente = @IdCliente)
            AND DataMovimentacao BETWEEN @DataInicial AND @DataFinal
        GROUP BY 
            YEAR(DataMovimentacao),
            MONTH(DataMovimentacao),
            TipoMovimentacao
        ORDER BY 
            Ano, Mes, TipoMovimentacao;";

                lista = conn.Query<RelatorioVolumeItensModel>(sqlBI, new
                {
                    model.IdCliente,
                    model.DataInicial,
                    model.DataFinal
                }).ToList();
            }
            return (List<RelatorioVolumeItensModel>)lista;
        }

        public List<RelatorioTopProdutoModel> TopProdutosMaisVendidos(FiltroBIModel model)
        {
            ICollection<RelatorioTopProdutoModel> lista = new List<RelatorioTopProdutoModel>();

            using (IDbConnection conn = _context.Database.GetDbConnection())
            {
                string sqlBI = @"
                    SELECT TOP 10
                    Produto,
                    SUM(Quantidade) AS QtdeVendida,
                    SUM(ValorTotal) AS ValorTotal
                    FROM MovimentacaoDiaria
                    WHERE 
                    TipoMovimentacao = 'VENDA'
                    AND (@IdCliente IS NULL OR IdCliente = @IdCliente)
                    AND DataMovimentacao BETWEEN @DataInicial AND @DataFinal
                    GROUP BY Produto
                    ORDER BY SUM(ValorTotal) DESC;";

                lista = conn.Query<RelatorioTopProdutoModel>(sqlBI, new
                {
                    model.IdCliente,
                    model.DataInicial,
                    model.DataFinal
                }).ToList();

            }
            return (List<RelatorioTopProdutoModel>)lista;
        }


        public List<RelatorioTopCategoriaModel> ObterTopCategoria(FiltroBIMovModel model)
        {

            ICollection<RelatorioTopCategoriaModel> lista = new List<RelatorioTopCategoriaModel>();

            using (IDbConnection conn = _context.Database.GetDbConnection())
            {
                string sqlBI = @"
                SELECT TOP 10
                    Categoria,
                    SUM(ValorTotal) AS ValorTotal,
                    SUM(Quantidade) AS QtdeVendida,
                    ROUND(
                        (SUM(ValorTotal) * 100.0) / NULLIF(SUM(SUM(ValorTotal)) OVER(), 0),
                        2
                    ) AS Percentual
                FROM MovimentacaoDiaria
                WHERE 
                    TipoMovimentacao = 'VENDA'
                    AND (@IdCliente IS NULL OR IdCliente = @IdCliente)
                    AND DataMovimentacao BETWEEN @DataInicial AND @DataFinal
                GROUP BY 
                    Categoria
                ORDER BY 
                    ValorTotal DESC;";

                lista = conn.Query<RelatorioTopCategoriaModel>(sqlBI, new
                {
                    model.IdCliente,
                    model.DataInicial,
                    model.DataFinal
                }).ToList();


            }
            return (List<RelatorioTopCategoriaModel>)lista;
        }

        public List<RelatorioFaturamentoClienteModel> FaturamentoCliente(FiltroBIMovModel model)
        {
            ICollection<RelatorioFaturamentoClienteModel> lista = new List<RelatorioFaturamentoClienteModel>();

            using (IDbConnection conn = _context.Database.GetDbConnection())
            {
                string sqlBI = @"
                    SELECT 
                    p.Nome AS Cliente,
                    SUM(m.ValorTotal) AS ValorTotal,
                    SUM(m.Quantidade) AS QtdeVendida,
                    ROUND(
                        (SUM(m.ValorTotal) * 100.0) / NULLIF(SUM(SUM(m.ValorTotal)) OVER(), 0),
                        2
                    ) AS Percentual
                FROM MovimentacaoDiaria m
                INNER JOIN Cliente c ON c.IdPessoa = m.IdCliente
                INNER JOIN Pessoa p on p.IdPessoa = c.IdPessoa
                WHERE 
                    m.TipoMovimentacao = 'VENDA'
                    AND m.DataMovimentacao BETWEEN @DataInicial AND @DataFinal
                GROUP BY 
                    p.Nome
                ORDER BY 
                    ValorTotal DESC";

                lista = conn.Query<RelatorioFaturamentoClienteModel>(sqlBI, new
                {
                    model.DataInicial,
                    model.DataFinal
                }).ToList();
            }
            return (List<RelatorioFaturamentoClienteModel>)lista;
        }

        public RelatorioPromocionalModel ObterPromocional(FiltroBIModel model)
        {
            RelatorioPromocionalModel indicador = new();

            using (IDbConnection conn = _context.Database.GetDbConnection())
            {
                        string sqlBI = @"
                WITH Vendas AS (
                    SELECT 
                        SUM(CASE WHEN Promocao = 1 THEN ValorTotal ELSE 0 END) AS ValorPromocional,
                        SUM(ValorTotal) AS ValorTotal
                    FROM MovimentacaoDiaria
                    WHERE 
                        TipoMovimentacao = 'VENDA'
                        AND (@IdCliente IS NULL OR IdCliente = @IdCliente)
                        AND DataMovimentacao BETWEEN @DataInicial AND @DataFinal
                )
                SELECT 
                    ValorPromocional,
                    ValorTotal,
                    ROUND(
                        CASE WHEN ValorTotal > 0 THEN (ValorPromocional / ValorTotal) * 100 ELSE 0 END,
                        2
                    ) AS PercentualPromocional
                FROM Vendas;";

                        indicador = conn.QueryFirstOrDefault<RelatorioPromocionalModel>(sqlBI, new
                        {
                            model.IdCliente,
                            model.DataInicial,
                            model.DataFinal
                        });
            }
            return indicador;
        }
    }
}
