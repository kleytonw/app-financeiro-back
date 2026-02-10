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
using EPPlus.Core.Extensions;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class BIController : ControllerBase
    {
        protected Context context;
        public BIController(Context context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("FaturamentoMensal")]
        public IActionResult FaturamentoMensal([FromBody] BIAnoRequestModel model)
        { 
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            var result = new BIFaturamentoMensalResponseModel();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select *
                        From BIFaturamentoMensal 
                        where BIFaturamentoMensal.Ano = '{model.Ano}'
                        AND BIFaturamentoMensal.IdEmpresa='{model.IdEmpresa}' ";

                result = conn.Query<BIFaturamentoMensalResponseModel>(sqlBI).FirstOrDefault();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("FaturamentoPeriodoReais")]
        public IActionResult FaturamentoPeridoReais([FromBody] BIFaturamentoPeriodoReaisRequest model)
        {
            var result = new BIFaturamentoMensalResponseModel();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select *
                        From BIFaturamentoPeriodoReais 
                        where BIFaturamentoPeriodoReais.Ano = '{model.Ano}'
                        AND BIFaturamentoPeriodoReais.IdCliente = {model.IdCliente}";

                result.ListaFaturamento = conn.Query<BIFaturamentoListaMensalResponseModel>(sqlBI).ToList();
                result.TotalJaneiro = result.ListaFaturamento.Sum(x => x.Janeiro);
                result.TotalFevereiro = result.ListaFaturamento.Sum(x => x.Fevereiro);
                result.TotalMarco = result.ListaFaturamento.Sum(x => x.Marco);
                result.TotalAbril = result.ListaFaturamento.Sum(x => x.Abril);
                result.TotalMaio = result.ListaFaturamento.Sum(x => x.Maio);
                result.TotalJunho = result.ListaFaturamento.Sum(x => x.Junho);
                result.TotalJulho = result.ListaFaturamento.Sum(x => x.Julho);
                result.TotalAgosto = result.ListaFaturamento.Sum(x => x.Agosto);
                result.TotalSetembro = result.ListaFaturamento.Sum(x => x.Setembro);
                result.TotalOutubro = result.ListaFaturamento.Sum(x => x.Outubro);
                result.TotalNovembro = result.ListaFaturamento.Sum(x => x.Novembro);
                result.TotalDezembro = result.ListaFaturamento.Sum(x => x.Dezembro);
                result.TotalGeral = result.ListaFaturamento.Sum(x => x.Janeiro + x.Fevereiro + x.Marco + x.Abril + x.Maio + x.Junho + x.Julho + x.Agosto + x.Setembro + x.Outubro + x.Novembro + x.Dezembro);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("FaturamentoPeriodoPorcentagem")]
        public IActionResult FaturamentoPeridoPorcentagem([FromBody] BIFaturamentoPeriodoPorcentagemRequest model)
        {
            var result = new BIFaturamentoMensalResponseModel();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select *
                        From BIFaturamentoPeriodoPorcentagem
                        where BIFaturamentoPeriodoPorcentagem.Ano = '{model.Ano}'
                        AND BIFaturamentoPeriodoPorcentagem.IdCliente = {model.IdCliente}";

                result.ListaFaturamento = conn.Query<BIFaturamentoListaMensalResponseModel>(sqlBI).ToList();
                result.TotalJaneiro = result.ListaFaturamento.Sum(x => x.Janeiro);
                result.TotalFevereiro = result.ListaFaturamento.Sum(x => x.Fevereiro);
                result.TotalMarco = result.ListaFaturamento.Sum(x => x.Marco);
                result.TotalAbril = result.ListaFaturamento.Sum(x => x.Abril);
                result.TotalMaio = result.ListaFaturamento.Sum(x => x.Maio);
                result.TotalJunho = result.ListaFaturamento.Sum(x => x.Junho);
                result.TotalJulho = result.ListaFaturamento.Sum(x => x.Julho);
                result.TotalAgosto = result.ListaFaturamento.Sum(x => x.Agosto);
                result.TotalSetembro = result.ListaFaturamento.Sum(x => x.Setembro);
                result.TotalOutubro = result.ListaFaturamento.Sum(x => x.Outubro);
                result.TotalNovembro = result.ListaFaturamento.Sum(x => x.Novembro);
                result.TotalDezembro = result.ListaFaturamento.Sum(x => x.Dezembro);
                result.TotalGeral = result.ListaFaturamento.Sum(x => x.Janeiro + x.Fevereiro + x.Marco + x.Abril + x.Maio + x.Junho + x.Julho + x.Agosto + x.Setembro + x.Outubro + x.Novembro + x.Dezembro);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("LucroBrutoPeriodoReais")]
        public IActionResult LucroBrutoPeriodoReais([FromBody] BILucroBrutoPeriodoReaisRequest model)
        {
            var result = new BIFaturamentoMensalResponseModel();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select *
                        From BILucroBrutoPeriodoReais 
                        where BILucroBrutoPeriodoReais.Ano = '{model.Ano}'
                        AND BILucroBrutoPeriodoReais.IdCliente = {model.IdCliente}";

                result.ListaFaturamento = conn.Query<BIFaturamentoListaMensalResponseModel>(sqlBI).ToList();
                result.TotalJaneiro = result.ListaFaturamento.Sum(x => x.Janeiro);
                result.TotalFevereiro = result.ListaFaturamento.Sum(x => x.Fevereiro);
                result.TotalMarco = result.ListaFaturamento.Sum(x => x.Marco);
                result.TotalAbril = result.ListaFaturamento.Sum(x => x.Abril);
                result.TotalMaio = result.ListaFaturamento.Sum(x => x.Maio);
                result.TotalJunho = result.ListaFaturamento.Sum(x => x.Junho);
                result.TotalJulho = result.ListaFaturamento.Sum(x => x.Julho);
                result.TotalAgosto = result.ListaFaturamento.Sum(x => x.Agosto);
                result.TotalSetembro = result.ListaFaturamento.Sum(x => x.Setembro);
                result.TotalOutubro = result.ListaFaturamento.Sum(x => x.Outubro);
                result.TotalNovembro = result.ListaFaturamento.Sum(x => x.Novembro);
                result.TotalDezembro = result.ListaFaturamento.Sum(x => x.Dezembro);
                result.TotalGeral = result.ListaFaturamento.Sum(x => x.Janeiro + x.Fevereiro + x.Marco + x.Abril + x.Maio + x.Junho + x.Julho + x.Agosto + x.Setembro + x.Outubro + x.Novembro + x.Dezembro);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("LucroBrutoPeriodoPorcentagem")]
        public IActionResult LucroBrutoPeriodoPorcentagem([FromBody] BILucroBrutoPeriodoPorcentagemRequest model)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa && x.IdUnidade == model.IdUnidade);
            var result = new BIFaturamentoMensalResponseModel();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select *
                        From BILucroBrutoPeriodoPorcentagem 
                        where BILucroBrutoPeriodoPorcentagem.Ano = '{model.Ano}'
                        AND BILucroBrutoPeriodoPorcentagem.IdCliente = {model.IdCliente}";

                result.ListaFaturamento = conn.Query<BIFaturamentoListaMensalResponseModel>(sqlBI).ToList();
                result.TotalJaneiro = result.ListaFaturamento.Sum(x => x.Janeiro);
                result.TotalFevereiro = result.ListaFaturamento.Sum(x => x.Fevereiro);
                result.TotalMarco = result.ListaFaturamento.Sum(x => x.Marco);
                result.TotalAbril = result.ListaFaturamento.Sum(x => x.Abril);
                result.TotalMaio = result.ListaFaturamento.Sum(x => x.Maio);
                result.TotalJunho = result.ListaFaturamento.Sum(x => x.Junho);
                result.TotalJulho = result.ListaFaturamento.Sum(x => x.Julho);
                result.TotalAgosto = result.ListaFaturamento.Sum(x => x.Agosto);
                result.TotalSetembro = result.ListaFaturamento.Sum(x => x.Setembro);
                result.TotalOutubro = result.ListaFaturamento.Sum(x => x.Outubro);
                result.TotalNovembro = result.ListaFaturamento.Sum(x => x.Novembro);
                result.TotalDezembro = result.ListaFaturamento.Sum(x => x.Dezembro);
                result.TotalGeral = result.ListaFaturamento.Sum(x => x.Janeiro + x.Fevereiro + x.Marco + x.Abril + x.Maio + x.Junho + x.Julho + x.Agosto + x.Setembro + x.Outubro + x.Novembro + x.Dezembro);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("NumeroClientes")]
        public IActionResult NumeroClientes([FromBody] BINumeroClientesRequest model)
        {
            var result = new BIFaturamentoMensalResponseModel();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select *
                        From BINumeroClientes 
                        where BINumeroClientes.Ano = '{model.Ano}'
                        AND BINumeroClientes.IdCliente = {model.IdCliente}";

                result.ListaFaturamento = conn.Query<BIFaturamentoListaMensalResponseModel>(sqlBI).ToList();
                result.TotalJaneiro = result.ListaFaturamento.Sum(x => x.Janeiro);
                result.TotalFevereiro = result.ListaFaturamento.Sum(x => x.Fevereiro);
                result.TotalMarco = result.ListaFaturamento.Sum(x => x.Marco);
                result.TotalAbril = result.ListaFaturamento.Sum(x => x.Abril);
                result.TotalMaio = result.ListaFaturamento.Sum(x => x.Maio);
                result.TotalJunho = result.ListaFaturamento.Sum(x => x.Junho);
                result.TotalJulho = result.ListaFaturamento.Sum(x => x.Julho);
                result.TotalAgosto = result.ListaFaturamento.Sum(x => x.Agosto);
                result.TotalSetembro = result.ListaFaturamento.Sum(x => x.Setembro);
                result.TotalOutubro = result.ListaFaturamento.Sum(x => x.Outubro);
                result.TotalNovembro = result.ListaFaturamento.Sum(x => x.Novembro);
                result.TotalDezembro = result.ListaFaturamento.Sum(x => x.Dezembro);
                result.TotalGeral = result.ListaFaturamento.Sum(x => x.Janeiro + x.Fevereiro + x.Marco + x.Abril + x.Maio + x.Junho + x.Julho + x.Agosto + x.Setembro + x.Outubro + x.Novembro + x.Dezembro);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("TicketMedio")]
        public IActionResult TicketMedio([FromBody] BITicketMedioRequest model)
        {
            var result = new BIFaturamentoMensalResponseModel();
            var resultFaturamento = new BIFaturamentoPeriodoReaisTotalModel();
            var resultNumeroClientes = new BINumeroClientesTotalModel();

            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBITicketMedio = $@"
                        select *
                        From BITicketMedio 
                        where BITicketMedio.Ano = '{model.Ano}'
                        AND BITicketMedio.IdCliente = {model.IdCliente}";

                string sqlBITotalFaturamentoPeriodo = $@"
                        SELECT 
                            SUM(Janeiro) AS TotalJaneiro,
                            SUM(Fevereiro) AS TotalFevereiro,
                            SUM(Marco) AS TotalMarco,
                            SUM(Abril) AS TotalAbril,
                            SUM(Maio) AS TotalMaio,
                            SUM(Junho) AS TotalJunho,
                            SUM(Julho) AS TotalJulho,
                            SUM(Agosto) AS TotalAgosto,
                            SUM(Setembro) AS TotalSetembro,
                            SUM(Outubro) AS TotalOutubro,
                            SUM(Novembro) AS TotalNovembro,
                            SUM(Dezembro) AS TotalDezembro
                        FROM BIFaturamentoPeriodoReais
                        WHERE BIFaturamentoPeriodoReais.Ano = '{model.Ano}'
                        AND BIFaturamentoPeriodoReais.IdCliente = {model.IdCliente}";

                string sqlBITotalNumeroClientes = $@"
                       SELECT 
                           SUM(Janeiro) AS TotalJaneiro,
                           SUM(Fevereiro) AS TotalFevereiro,
                           SUM(Marco) AS TotalMarco,
                           SUM(Abril) AS TotalAbril,
                           SUM(Maio) AS TotalMaio,
                           SUM(Junho) AS TotalJunho,
                           SUM(Julho) AS TotalJulho,
                           SUM(Agosto) AS TotalAgosto,
                           SUM(Setembro) AS TotalSetembro,
                           SUM(Outubro) AS TotalOutubro,
                           SUM(Novembro) AS TotalNovembro,
                           SUM(Dezembro) AS TotalDezembro
                       FROM BINumeroClientes
                       WHERE BINumeroClientes.Ano = '{model.Ano}'
                       AND BINumeroClientes.IdCliente = {model.IdCliente}";



                result.ListaFaturamento = conn.Query<BIFaturamentoListaMensalResponseModel>(sqlBITicketMedio).ToList();
                resultFaturamento = conn.Query<BIFaturamentoPeriodoReaisTotalModel>(sqlBITotalFaturamentoPeriodo).FirstOrDefault();
                resultNumeroClientes = conn.Query<BINumeroClientesTotalModel>(sqlBITotalNumeroClientes).FirstOrDefault();


                result.TotalJaneiro = resultFaturamento.TotalJaneiro > 0 && resultNumeroClientes.TotalJaneiro > 0
                      ? resultFaturamento.TotalJaneiro / resultNumeroClientes.TotalJaneiro
                      : 0;

                result.TotalFevereiro = resultFaturamento.TotalFevereiro > 0 && resultNumeroClientes.TotalFevereiro > 0
                    ? resultFaturamento.TotalFevereiro / resultNumeroClientes.TotalFevereiro
                    : 0;

                result.TotalMarco = resultFaturamento.TotalMarco > 0 && resultNumeroClientes.TotalMarco > 0
                    ? resultFaturamento.TotalMarco / resultNumeroClientes.TotalMarco
                    : 0;

                result.TotalAbril = resultFaturamento.TotalAbril > 0 && resultNumeroClientes.TotalAbril > 0
                    ? resultFaturamento.TotalAbril / resultNumeroClientes.TotalAbril
                    : 0;

                result.TotalMaio = resultFaturamento.TotalMaio > 0 && resultNumeroClientes.TotalMaio > 0
                    ? resultFaturamento.TotalMaio / resultNumeroClientes.TotalMaio
                    : 0;

                result.TotalJunho = resultFaturamento.TotalJunho > 0 && resultNumeroClientes.TotalJunho > 0
                    ? resultFaturamento.TotalJunho / resultNumeroClientes.TotalJunho
                    : 0;

                result.TotalJulho = resultFaturamento.TotalJulho > 0 && resultNumeroClientes.TotalJulho > 0
                    ? resultFaturamento.TotalJulho / resultNumeroClientes.TotalJulho
                    : 0;

                result.TotalAgosto = resultFaturamento.TotalAgosto > 0 && resultNumeroClientes.TotalAgosto > 0
                    ? resultFaturamento.TotalAgosto / resultNumeroClientes.TotalAgosto
                    : 0;

                result.TotalSetembro = resultFaturamento.TotalSetembro > 0 && resultNumeroClientes.TotalSetembro > 0
                    ? resultFaturamento.TotalSetembro / resultNumeroClientes.TotalSetembro
                    : 0;

                result.TotalOutubro = resultFaturamento.TotalOutubro > 0 && resultNumeroClientes.TotalOutubro > 0
                    ? resultFaturamento.TotalOutubro / resultNumeroClientes.TotalOutubro
                    : 0;

                result.TotalNovembro = resultFaturamento.TotalNovembro > 0 && resultNumeroClientes.TotalNovembro > 0
                    ? resultFaturamento.TotalNovembro / resultNumeroClientes.TotalNovembro
                    : 0;

                result.TotalDezembro = resultFaturamento.TotalDezembro > 0 && resultNumeroClientes.TotalDezembro > 0
                    ? resultFaturamento.TotalDezembro / resultNumeroClientes.TotalDezembro
                    : 0;

                result.TotalGeral = (result.TotalJaneiro + result.TotalFevereiro + result.TotalMarco +
                                     result.TotalAbril + result.TotalMaio + result.TotalJunho +
                                     result.TotalJulho + result.TotalAgosto + result.TotalSetembro +
                                     result.TotalOutubro + result.TotalNovembro + result.TotalDezembro) / 12;
                result.TotalGeral = result.ListaFaturamento.Sum(x => x.Janeiro + x.Fevereiro + x.Marco + x.Abril + x.Maio + x.Junho + x.Julho + x.Agosto + x.Setembro + x.Outubro + x.Novembro + x.Dezembro);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("TicketPorSetor")]
        public IActionResult TicketPorSetor([FromBody] BITicketPorSetorRequest model)
        {
            var result = new BIFaturamentoMensalResponseModel();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select *
                        From BITicketPorSetor 
                        where BITicketPorSetor.Ano = '{model.Ano}'
                        AND BITicketPorSetor.IdCliente = {model.IdCliente}";

                result.ListaFaturamento = conn.Query<BIFaturamentoListaMensalResponseModel>(sqlBI).ToList();
                result.TotalJaneiro = result.ListaFaturamento.Sum(x => x.Janeiro);
                result.TotalFevereiro = result.ListaFaturamento.Sum(x => x.Fevereiro);
                result.TotalMarco = result.ListaFaturamento.Sum(x => x.Marco);
                result.TotalAbril = result.ListaFaturamento.Sum(x => x.Abril);
                result.TotalMaio = result.ListaFaturamento.Sum(x => x.Maio);
                result.TotalJunho = result.ListaFaturamento.Sum(x => x.Junho);
                result.TotalJulho = result.ListaFaturamento.Sum(x => x.Julho);
                result.TotalAgosto = result.ListaFaturamento.Sum(x => x.Agosto);
                result.TotalSetembro = result.ListaFaturamento.Sum(x => x.Setembro);
                result.TotalOutubro = result.ListaFaturamento.Sum(x => x.Outubro);
                result.TotalNovembro = result.ListaFaturamento.Sum(x => x.Novembro);
                result.TotalDezembro = result.ListaFaturamento.Sum(x => x.Dezembro);
                result.TotalGeral = result.ListaFaturamento.Sum(x => x.Janeiro + x.Fevereiro + x.Marco + x.Abril + x.Maio + x.Junho + x.Julho + x.Agosto + x.Setembro + x.Outubro + x.Novembro + x.Dezembro);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("TicketMedioPorSetor")]
        public IActionResult TicketMedioPorSetor([FromBody] BITicketMedioRequest model)
        {
            var result = new BIFaturamentoMensalResponseModel();
            var resultFaturamento = new BIFaturamentoPeriodoReaisTotalModel();
            var resultTicketPorSetor = new BITicketPorSetorTotalModel();

            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBITicketMedio = $@"
                        select *
                        From BITicketMedioPorSetor 
                        where BITicketMedioPorSetor.Ano = '{model.Ano}'
                        AND BITicketMedioPorSetor.IdCliente = {model.IdCliente}";

                string sqlBITotalFaturamentoPeriodo = $@"
                        SELECT 
                            SUM(Janeiro) AS TotalJaneiro,
                            SUM(Fevereiro) AS TotalFevereiro,
                            SUM(Marco) AS TotalMarco,
                            SUM(Abril) AS TotalAbril,
                            SUM(Maio) AS TotalMaio,
                            SUM(Junho) AS TotalJunho,
                            SUM(Julho) AS TotalJulho,
                            SUM(Agosto) AS TotalAgosto,
                            SUM(Setembro) AS TotalSetembro,
                            SUM(Outubro) AS TotalOutubro,
                            SUM(Novembro) AS TotalNovembro,
                            SUM(Dezembro) AS TotalDezembro
                        FROM BIFaturamentoPeriodoReais
                        WHERE BIFaturamentoPeriodoReais.Ano = '{model.Ano}'
                        AND BIFaturamentoPeriodoReais.IdCliente = {model.IdCliente}";

                string sqlBITotalTicketPorSetor = $@"
                       SELECT 
                           SUM(Janeiro) AS TotalJaneiro,
                           SUM(Fevereiro) AS TotalFevereiro,
                           SUM(Marco) AS TotalMarco,
                           SUM(Abril) AS TotalAbril,
                           SUM(Maio) AS TotalMaio,
                           SUM(Junho) AS TotalJunho,
                           SUM(Julho) AS TotalJulho,
                           SUM(Agosto) AS TotalAgosto,
                           SUM(Setembro) AS TotalSetembro,
                           SUM(Outubro) AS TotalOutubro,
                           SUM(Novembro) AS TotalNovembro,
                           SUM(Dezembro) AS TotalDezembro
                       FROM BITicketPorSetor
                       WHERE BITicketPorSetor.Ano = '{model.Ano}'
                       AND BITicketPorSetor.IdCliente = {model.IdCliente}";



                result.ListaFaturamento = conn.Query<BIFaturamentoListaMensalResponseModel>(sqlBITicketMedio).ToList();
                resultFaturamento = conn.Query<BIFaturamentoPeriodoReaisTotalModel>(sqlBITotalFaturamentoPeriodo).FirstOrDefault();
                resultTicketPorSetor = conn.Query<BITicketPorSetorTotalModel>(sqlBITotalTicketPorSetor).FirstOrDefault();


                result.TotalJaneiro = resultFaturamento.TotalJaneiro > 0 && resultTicketPorSetor.TotalJaneiro > 0
                      ? resultFaturamento.TotalJaneiro / resultTicketPorSetor.TotalJaneiro
                      : 0;

                result.TotalFevereiro = resultFaturamento.TotalFevereiro > 0 && resultTicketPorSetor.TotalFevereiro > 0
                    ? resultFaturamento.TotalFevereiro / resultTicketPorSetor.TotalFevereiro
                    : 0;

                result.TotalMarco = resultFaturamento.TotalMarco > 0 && resultTicketPorSetor.TotalMarco > 0
                    ? resultFaturamento.TotalMarco / resultTicketPorSetor.TotalMarco
                    : 0;

                result.TotalAbril = resultFaturamento.TotalAbril > 0 && resultTicketPorSetor.TotalAbril > 0
                    ? resultFaturamento.TotalAbril / resultTicketPorSetor.TotalAbril
                    : 0;

                result.TotalMaio = resultFaturamento.TotalMaio > 0 && resultTicketPorSetor.TotalMaio > 0
                    ? resultFaturamento.TotalMaio / resultTicketPorSetor.TotalMaio
                    : 0;

                result.TotalJunho = resultFaturamento.TotalJunho > 0 && resultTicketPorSetor.TotalJunho > 0
                    ? resultFaturamento.TotalJunho / resultTicketPorSetor.TotalJunho
                    : 0;

                result.TotalJulho = resultFaturamento.TotalJulho > 0 && resultTicketPorSetor.TotalJulho > 0
                    ? resultFaturamento.TotalJulho / resultTicketPorSetor.TotalJulho
                    : 0;

                result.TotalAgosto = resultFaturamento.TotalAgosto > 0 && resultTicketPorSetor.TotalAgosto > 0
                    ? resultFaturamento.TotalAgosto / resultTicketPorSetor.TotalAgosto
                    : 0;

                result.TotalSetembro = resultFaturamento.TotalSetembro > 0 && resultTicketPorSetor.TotalSetembro > 0
                    ? resultFaturamento.TotalSetembro / resultTicketPorSetor.TotalSetembro
                    : 0;

                result.TotalOutubro = resultFaturamento.TotalOutubro > 0 && resultTicketPorSetor.TotalOutubro > 0
                    ? resultFaturamento.TotalOutubro / resultTicketPorSetor.TotalOutubro
                    : 0;

                result.TotalNovembro = resultFaturamento.TotalNovembro > 0 && resultTicketPorSetor.TotalNovembro > 0
                    ? resultFaturamento.TotalNovembro / resultTicketPorSetor.TotalNovembro
                    : 0;

                result.TotalDezembro = resultFaturamento.TotalDezembro > 0 && resultTicketPorSetor.TotalDezembro > 0
                    ? resultFaturamento.TotalDezembro / resultTicketPorSetor.TotalDezembro
                    : 0;

                result.TotalGeral = (result.TotalJaneiro + result.TotalFevereiro + result.TotalMarco +
                                     result.TotalAbril + result.TotalMaio + result.TotalJunho +
                                     result.TotalJulho + result.TotalAgosto + result.TotalSetembro +
                                     result.TotalOutubro + result.TotalNovembro + result.TotalDezembro) / 12;
                result.TotalGeral = result.ListaFaturamento.Sum(x => x.Janeiro + x.Fevereiro + x.Marco + x.Abril + x.Maio + x.Junho + x.Julho + x.Agosto + x.Setembro + x.Outubro + x.Novembro + x.Dezembro);
            }
            return Ok(result);
        }





        [HttpPost]
        [Route("MargemPorcentagem")]
        public IActionResult MargemPorcentagem([FromBody] BIMargemPorcentagemRequest model)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa && x.IdUnidade == model.IdUnidade);
            var result = new BIFaturamentoMensalResponseModel();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select *
                        From BIMargemPorcentagem 
                        where BIMargemPorcentagem.Ano = '{model.Ano}'
                        AND BIMargemPorcentagem.IdEmpresa={model.IdEmpresa}
                        AND BIMargemPorcentagem.IdUnidade = {model.IdUnidade}";

                result.ListaFaturamento = conn.Query<BIFaturamentoListaMensalResponseModel>(sqlBI).ToList();
                result.TotalJaneiro = result.ListaFaturamento.Sum(x => x.Janeiro);
                result.TotalFevereiro = result.ListaFaturamento.Sum(x => x.Fevereiro);
                result.TotalMarco = result.ListaFaturamento.Sum(x => x.Marco);
                result.TotalAbril = result.ListaFaturamento.Sum(x => x.Abril);
                result.TotalMaio = result.ListaFaturamento.Sum(x => x.Maio);
                result.TotalJunho = result.ListaFaturamento.Sum(x => x.Junho);
                result.TotalJulho = result.ListaFaturamento.Sum(x => x.Julho);
                result.TotalAgosto = result.ListaFaturamento.Sum(x => x.Agosto);
                result.TotalSetembro = result.ListaFaturamento.Sum(x => x.Setembro);
                result.TotalOutubro = result.ListaFaturamento.Sum(x => x.Outubro);
                result.TotalNovembro = result.ListaFaturamento.Sum(x => x.Novembro);
                result.TotalDezembro = result.ListaFaturamento.Sum(x => x.Dezembro);
                result.TotalGeral = result.ListaFaturamento.Sum(x => x.Janeiro + x.Fevereiro + x.Marco + x.Abril + x.Maio + x.Junho + x.Julho + x.Agosto + x.Setembro + x.Outubro + x.Novembro + x.Dezembro);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("faturamentoDiario")]
        public IActionResult FaturamentoDiario([FromBody] BIAnoRequestModel model)
        {

            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            var result = new List<BIFaturamentoDiarioResponseModel>();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@" SET LANGUAGE 'Brazilian'
                                select  DAY(DataHoraEmissao) Dia, DATENAME(WEEKDAY,DataHoraEmissao) NomeDia, sum(ValorNF) as Total From Movimentacao 
                                where Movimentacao.IdEmpresa = {model.IdEmpresa} 
                                AND YEAR(DataHoraEmissao)='{model.Ano}'
                                AND MONTH(DataHoraEmissao)='{model.Mes}'
                                AND EmitenteCNPJ ='{empresa.CpfCnpj}'
                                GROUP BY DAY(DataHoraEmissao), DATENAME(WEEKDAY,DataHoraEmissao)
                                order by Dia ";

                result = conn.Query<BIFaturamentoDiarioResponseModel>(sqlBI).ToList();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("faturamentoProduto")]
        public IActionResult FaturamentoProduto([FromBody] BIRequestFaturamentoProdutoModel model)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            var result = new List<BIFaturamentoProdutoResponseModel>();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select *
                        From BIFaturamentoProduto 
                        where BIFaturamentoProduto.Ano = '{model.Ano}'
                        AND BIFaturamentoProduto.IdEmpresa='{model.IdEmpresa}' ";

                result = conn.Query<BIFaturamentoProdutoResponseModel>(sqlBI).ToList();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("faturamentoRegiao")]
        public IActionResult FaturamentoRegiao([FromBody] BIRequestFaturamentoProdutoModel model)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            var result = new List<BIFaturamentoRegiaoResponseModel>();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select *
                        From BIFaturamentoRegiao 
                        where BIFaturamentoRegiao.Ano = '{model.Ano}'
                        AND BIFaturamentoRegiao.IdEmpresa='{model.IdEmpresa}' ";

                result = conn.Query<BIFaturamentoRegiaoResponseModel>(sqlBI).ToList();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("graficoRegiao")]
        public IActionResult GraficoRegiao([FromBody] BIRequestFaturamentoProdutoModel model)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            var result = new List<BIRegiaoTotalResponseModel>();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@" select Regiao, sum(Janeiro+Fevereiro+Marco+Abril+Maio+Junho+Julho+Agosto+Setembro+Outubro+Novembro+Dezembro) AS Total
                        from BIFaturamentoRegiao
                        where BIFaturamentoRegiao.Ano = '{model.Ano}'
                        AND BIFaturamentoRegiao.IdEmpresa='{model.IdEmpresa}' group by Regiao ";

                result = conn.Query<BIRegiaoTotalResponseModel>(sqlBI).ToList();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("faturamentoSetor")]
        public IActionResult FaturamentoSetor([FromBody] BIRequestFaturamentoProdutoModel model)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            var result = new List<BIFaturamentoSetorResponseModel>();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@"
                        select *
                        From BIFaturamentoSetor 
                        where BIFaturamentoSetor.Ano = '{model.Ano}'
                        AND BIFaturamentoSetor.IdEmpresa='{model.IdEmpresa}' ";

                result = conn.Query<BIFaturamentoSetorResponseModel>(sqlBI).ToList();
            }
            return Ok(result);
        }


        [HttpPost]
        [Route("graficoSetor")]
        public IActionResult GraficoSetor([FromBody] BIRequestFaturamentoProdutoModel model)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            var result = new List<BISetorTotalResponseModel>();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@" select Setor, sum(Janeiro+Fevereiro+Marco+Abril+Maio+Junho+Julho+Agosto+Setembro+Outubro+Novembro+Dezembro) AS Total
                        from BIFaturamentoSetor
                        where BIFaturamentoSetor.Ano = '{model.Ano}'
                        AND BIFaturamentoSetor.IdEmpresa='{model.IdEmpresa}' group by Setor ";

                result = conn.Query<BISetorTotalResponseModel>(sqlBI).ToList();
            }
            return Ok(result);
        }


        [HttpPost]
        [Route("faturamentoCliente")]
        public IActionResult FaturamentoCliente([FromBody] BIRequestFaturamentoClienteModel model)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
            var result = new List<BIFaturamentoClienteResponseModel>();
            using (IDbConnection conn = context.Database.GetDbConnection())
            {
                string sqlBI = $@" select Nome,RazaoSocial, CpfCnpj, Telefone, Email, Total 
                                    From BIFaturamentoCliente 
                                    where BIFaturamentoCliente.Ano = '{model.Ano}'
                                    AND BIFaturamentoCliente.IdEmpresa = '{empresa.IdEmpresa}' ORDER BY Total DESC ";

                result = conn.Query<BIFaturamentoClienteResponseModel>(sqlBI).ToList();
            }
            return Ok(result);
        }
         
    }
}