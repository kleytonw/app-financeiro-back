using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Reporting.WinForms;
using System.Reflection;
using ERP_API.Models.Relatorio;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using ERP_API.Models.BI;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dapper;
using System.Linq;


namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RelatorioController : ControllerBase
    {
        protected Context context;
        public RelatorioController(Context context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("relatorioConciliacaoVenda")]
        [AllowAnonymous]
        public IActionResult RelatorioConciliacaoVenda([FromBody] FiltroTeste model)
        {

            // 1. Load the RDLC report from embedded resource
            var assembly = Assembly.GetExecutingAssembly();
            var rdlcStream = assembly.GetManifestResourceStream("ERP_API.Report.RelatorioConciliacaoVenda.rdlc");
            if (rdlcStream == null)
                return NotFound("Report not found.");

            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);
            if (unidade == null)
                return BadRequest("Unidade não encontrada");

            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            if (unidade == null)
                return BadRequest("Unidade não encontrada");

            ICollection<RelatorioConciliacaoVendaModel> lista = new List<RelatorioConciliacaoVendaModel>(); 
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@" 
                        select Empresa.Nome as Empresa, DescricaoProduto, QuantidadeParcela, NomeOperadora, Bandeira, MeioPagamento, SUM(ValorBruto) ValorBruto, sum(Taxa) TotalTaxa, sum(ValorLiquido) TotalLiquido
                        From Transacao
                        INNER JOIN Empresa ON Empresa.IdEmpresa = Transacao.IdEmpresa
                        WHERE MONTH(Transacao.DataMovimentacao) = '{model.Mes}'
                        and YEAR(Transacao.DataMOvimentacao) = '{model.Ano}'
                        and Empresa.IdEmpresa = '{model.IdEmpresa}'
                        and Transacao.IdUnidade = '{model.IdUnidade}'
                        Group by Empresa.Nome, DescricaoProduto, QuantidadeParcela, NomeOperadora, Bandeira, MeioPagamento
                        ORDER BY Empresa.Nome, DescricaoProduto, QuantidadeParcela, NomeOperadora, Bandeira, MeioPagamento ";

                lista = conn.Query<RelatorioConciliacaoVendaModel>(sqlBI).ToList();
            }  

            // 3. Prepare report
            var report = new LocalReport();
            report.LoadReportDefinition(rdlcStream);
            report.DataSources.Add(new ReportDataSource("dsConciliacaoVenda", lista));

            report.Refresh();

            decimal totalTaxa = lista.Sum(x => x.TotalTaxa) ?? 0;
            decimal totalLiquido = lista.Sum(x=>x.ValorBruto) ?? 0;

            decimal totalMedia = (totalTaxa / totalLiquido)* 100;

            // parametros
            ReportParameter[] parametros = new ReportParameter[] {
                new ReportParameter("usuario","admin"),
                  new ReportParameter("empresa",empresa.Nome),
                  new ReportParameter("unidade",unidade.Nome),
                  new ReportParameter("mes",$"{model.Mes}"),
                  new ReportParameter("ano",$"{model.Ano}"),
                  new ReportParameter("TotalTarifa",$"{lista.Sum(x=>x.TotalTaxa)}"),
                  new ReportParameter("TotalBruto",$"{lista.Sum(x=>x.ValorBruto)}"),
                  new ReportParameter("TotalLiquido",$"{lista.Sum(x=>x.TotalLiquido)}"),
                   new ReportParameter("TotalMedia",$"{totalMedia}")
            };

            report.SetParameters(parametros);



            string deviceInfo =
           "<DeviceInfo>" +
           "<OutputFormat>PDF</OutputFormat>" +
           "<PageWidth>29.7cm</PageWidth>" +
           "<PageHeight>20cm</PageHeight>" +
           "<MarginTop>0.5cm</MarginTop>" +
           "<MarginLeft>0.5cm</MarginLeft>" +
           "<MarginRight>0.5cm</MarginRight>" +
           "<MarginBottom>0.5cm</MarginBottom>" +
           "</DeviceInfo>";

            string mimeType = "";
            string encoding = "";
            string filenameExtension = "";
            string[] streams = null;
            Warning[] warnings = null;

            // 4. Render to PDF
            byte[] result = report.Render("pdf", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

            var returnString = result;

            var a = File(returnString, "application/pdf");

            return Ok(a);
        }


    }

    public class FiltroTeste
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public int IdEmpresa { get; set;  }
        public int IdUnidade { get; set;  }
        
    }
}
