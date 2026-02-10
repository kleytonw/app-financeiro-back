using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using System;
using ERP_API.Domain.Entidades;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;


namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ImportarController : ControllerBase
    {
        protected Context context;
        public ImportarController(Context context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("ImportarBIFaturamentoPeriodoMensalReais")]
        public IActionResult ImportarBIFaturamentoPeriodoMensal([FromForm] IFormFile excelFile, [FromForm] int idUnidade, [FromForm] int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdEmpresa == idEmpresa && x.IdUnidade == idUnidade);


            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("Arquivo não encontrado.");
            }

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    var faturamentoList = new List<BIFaturamentoPeriodoReais>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var descricao = worksheet.Cells[row, 1].Text;
                            var ano = worksheet.Cells[row, 2].Text;
                            var janeiro = ParseCellToDecimal(worksheet.Cells[row, 3].Text);
                            var fevereiro = ParseCellToDecimal(worksheet.Cells[row, 4].Text);
                            var marco = ParseCellToDecimal(worksheet.Cells[row, 5].Text);
                            var abril = ParseCellToDecimal(worksheet.Cells[row, 6].Text);
                            var maio = ParseCellToDecimal(worksheet.Cells[row, 7].Text);
                            var junho = ParseCellToDecimal(worksheet.Cells[row, 8].Text);
                            var julho = ParseCellToDecimal(worksheet.Cells[row, 9].Text);
                            var agosto = ParseCellToDecimal(worksheet.Cells[row, 10].Text);
                            var setembro = ParseCellToDecimal(worksheet.Cells[row, 11].Text);
                            var outubro = ParseCellToDecimal(worksheet.Cells[row, 12].Text);
                            var novembro = ParseCellToDecimal(worksheet.Cells[row, 13].Text);
                            var dezembro = ParseCellToDecimal(worksheet.Cells[row, 14].Text);

                            var faturamento = new BIFaturamentoPeriodoReais(empresa,
                                unidade,
                                descricao,
                                ano,
                                janeiro,
                                fevereiro,
                                marco,
                                abril,
                                maio,
                                junho,
                                julho,
                                agosto,
                                setembro,
                                outubro,
                                novembro,
                                dezembro, User.Identity.Name);

                            faturamentoList.Add(faturamento);
                        }

                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.BIFaturamentoPeriodoReais.AddRange(faturamentoList);
                    context.SaveChanges();

                }


            }

            return Ok();
        }

        [HttpPost]
        [Route("ImportarBIFaturamentoPeriodoMensalPorcentagem")]
        public IActionResult ImportarBIFaturamentoPeriodoMensalPorcentagem([FromForm] IFormFile excelFile, [FromForm] int idUnidade, [FromForm] int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdEmpresa == idEmpresa && x.IdUnidade == idUnidade);


            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("Arquivo não encontrado.");
            }

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    var faturamentoList = new List<BIFaturamentoPeriodoPorcentagem>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var descricao = worksheet.Cells[row, 1].Text;
                            var ano = worksheet.Cells[row, 2].Text;
                            var janeiro = ParseCellToDecimal(worksheet.Cells[row, 3].Text);
                            var fevereiro = ParseCellToDecimal(worksheet.Cells[row, 4].Text);
                            var marco = ParseCellToDecimal(worksheet.Cells[row, 5].Text);
                            var abril = ParseCellToDecimal(worksheet.Cells[row, 6].Text);
                            var maio = ParseCellToDecimal(worksheet.Cells[row, 7].Text);
                            var junho = ParseCellToDecimal(worksheet.Cells[row, 8].Text);
                            var julho = ParseCellToDecimal(worksheet.Cells[row, 9].Text);
                            var agosto = ParseCellToDecimal(worksheet.Cells[row, 10].Text);
                            var setembro = ParseCellToDecimal(worksheet.Cells[row, 11].Text);
                            var outubro = ParseCellToDecimal(worksheet.Cells[row, 12].Text);
                            var novembro = ParseCellToDecimal(worksheet.Cells[row, 13].Text);
                            var dezembro = ParseCellToDecimal(worksheet.Cells[row, 14].Text);

                            var faturamento = new BIFaturamentoPeriodoPorcentagem(empresa,
                                unidade,
                                descricao,
                                ano,
                                janeiro,
                                fevereiro,
                                marco,
                                abril,
                                maio,
                                junho,
                                julho,
                                agosto,
                                setembro,
                                outubro,
                                novembro,
                                dezembro, User.Identity.Name);

                            faturamentoList.Add(faturamento);
                        }

                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.BIFaturamentoPeriodoPorcentagem.AddRange(faturamentoList);
                    context.SaveChanges();

                }


            }

            return Ok();
        }


        [HttpPost]
        [Route("ImportarBILucroBrutoPeriodoReais")]
        public IActionResult ImportarBILucroBrutoPeriodoReais([FromForm] IFormFile excelFile, [FromForm] int idUnidade, [FromForm] int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdEmpresa == idEmpresa && x.IdUnidade == idUnidade);


            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("Arquivo não encontrado.");
            }

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    var faturamentoList = new List<BILucroBrutoPeriodoReais>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var descricao = worksheet.Cells[row, 1].Text;
                            var ano = worksheet.Cells[row, 2].Text;
                            var janeiro = ParseCellToDecimal(worksheet.Cells[row, 3].Text);
                            var fevereiro = ParseCellToDecimal(worksheet.Cells[row, 4].Text);
                            var marco = ParseCellToDecimal(worksheet.Cells[row, 5].Text);
                            var abril = ParseCellToDecimal(worksheet.Cells[row, 6].Text);
                            var maio = ParseCellToDecimal(worksheet.Cells[row, 7].Text);
                            var junho = ParseCellToDecimal(worksheet.Cells[row, 8].Text);
                            var julho = ParseCellToDecimal(worksheet.Cells[row, 9].Text);
                            var agosto = ParseCellToDecimal(worksheet.Cells[row, 10].Text);
                            var setembro = ParseCellToDecimal(worksheet.Cells[row, 11].Text);
                            var outubro = ParseCellToDecimal(worksheet.Cells[row, 12].Text);
                            var novembro = ParseCellToDecimal(worksheet.Cells[row, 13].Text);
                            var dezembro = ParseCellToDecimal(worksheet.Cells[row, 14].Text);

                            var faturamento = new BILucroBrutoPeriodoReais(empresa,
                                unidade,
                                descricao,
                                ano,
                                janeiro,
                                fevereiro,
                                marco,
                                abril,
                                maio,
                                junho,
                                julho,
                                agosto,
                                setembro,
                                outubro,
                                novembro,
                                dezembro, User.Identity.Name);

                            faturamentoList.Add(faturamento);
                        }

                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.BILucroBrutoPeriodoReais.AddRange(faturamentoList);
                    context.SaveChanges();

                }


            }

            return Ok();
        }

        [HttpPost]
        [Route("ImportarBILucroBrutoPeriodoPorcentagem")]
        public IActionResult ImportarBILucroBrutoPeriodoPorcentagem([FromForm] IFormFile excelFile, [FromForm] int idUnidade, [FromForm] int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdEmpresa == idEmpresa && x.IdUnidade == idUnidade);


            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("Arquivo não encontrado.");
            }

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    var faturamentoList = new List<BILucroBrutoPeriodoPorcentagem>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var descricao = worksheet.Cells[row, 1].Text;
                            var ano = worksheet.Cells[row, 2].Text;
                            var janeiro = ParseCellToDecimal(worksheet.Cells[row, 3].Text);
                            var fevereiro = ParseCellToDecimal(worksheet.Cells[row, 4].Text);
                            var marco = ParseCellToDecimal(worksheet.Cells[row, 5].Text);
                            var abril = ParseCellToDecimal(worksheet.Cells[row, 6].Text);
                            var maio = ParseCellToDecimal(worksheet.Cells[row, 7].Text);
                            var junho = ParseCellToDecimal(worksheet.Cells[row, 8].Text);
                            var julho = ParseCellToDecimal(worksheet.Cells[row, 9].Text);
                            var agosto = ParseCellToDecimal(worksheet.Cells[row, 10].Text);
                            var setembro = ParseCellToDecimal(worksheet.Cells[row, 11].Text);
                            var outubro = ParseCellToDecimal(worksheet.Cells[row, 12].Text);
                            var novembro = ParseCellToDecimal(worksheet.Cells[row, 13].Text);
                            var dezembro = ParseCellToDecimal(worksheet.Cells[row, 14].Text);

                            var faturamento = new BILucroBrutoPeriodoPorcentagem(empresa,
                                unidade,
                                descricao,
                                ano,
                                janeiro,
                                fevereiro,
                                marco,
                                abril,
                                maio,
                                junho,
                                julho,
                                agosto,
                                setembro,
                                outubro,
                                novembro,
                                dezembro, User.Identity.Name);

                            faturamentoList.Add(faturamento);
                        }

                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.BILucroBrutoPeriodoPorcentagem.AddRange(faturamentoList);
                    context.SaveChanges();

                }


            }

            return Ok();
        }

        [HttpPost]
        [Route("ImportarBINumeroClientes")]
        public IActionResult ImportarBI([FromForm] IFormFile excelFile, [FromForm] int idUnidade, [FromForm] int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdEmpresa == idEmpresa && x.IdUnidade == idUnidade);


            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("Arquivo não encontrado.");
            }

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    var faturamentoList = new List<BINumeroClientes>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var descricao = worksheet.Cells[row, 1].Text;
                            var ano = worksheet.Cells[row, 2].Text;
                            var janeiro = ParseCellToInt(worksheet.Cells[row, 3].Text);
                            var fevereiro = ParseCellToInt(worksheet.Cells[row, 4].Text);
                            var marco = ParseCellToInt(worksheet.Cells[row, 5].Text);
                            var abril = ParseCellToInt(worksheet.Cells[row, 6].Text);
                            var maio = ParseCellToInt(worksheet.Cells[row, 7].Text);
                            var junho = ParseCellToInt(worksheet.Cells[row, 8].Text);
                            var julho = ParseCellToInt(worksheet.Cells[row, 9].Text);
                            var agosto = ParseCellToInt(worksheet.Cells[row, 10].Text);
                            var setembro = ParseCellToInt(worksheet.Cells[row, 11].Text);
                            var outubro = ParseCellToInt(worksheet.Cells[row, 12].Text);
                            var novembro = ParseCellToInt(worksheet.Cells[row, 13].Text);
                            var dezembro = ParseCellToInt(worksheet.Cells[row, 14].Text);

                            var faturamento = new BINumeroClientes(empresa,
                                unidade,
                                descricao,
                                ano,
                                janeiro,
                                fevereiro,
                                marco,
                                abril,
                                maio,
                                junho,
                                julho,
                                agosto,
                                setembro,
                                outubro,
                                novembro,
                                dezembro, User.Identity.Name);

                            faturamentoList.Add(faturamento);
                        }

                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.BINumeroClientes.AddRange(faturamentoList);
                    context.SaveChanges();

                }


            }

            return Ok();
        }

        [HttpPost]
        [Route("ImportarBITicketMedio")]
        public IActionResult ImportarBITicketMedio([FromForm] IFormFile excelFile, [FromForm] int idUnidade, [FromForm] int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdEmpresa == idEmpresa && x.IdUnidade == idUnidade);


            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("Arquivo não encontrado.");
            }

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    var faturamentoList = new List<BITicketMedio>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var descricao = worksheet.Cells[row, 1].Text;
                            var ano = worksheet.Cells[row, 2].Text;
                            var janeiro = ParseCellToDecimal(worksheet.Cells[row, 3].Text);
                            var fevereiro = ParseCellToDecimal(worksheet.Cells[row, 4].Text);
                            var marco = ParseCellToDecimal(worksheet.Cells[row, 5].Text);
                            var abril = ParseCellToDecimal(worksheet.Cells[row, 6].Text);
                            var maio = ParseCellToDecimal(worksheet.Cells[row, 7].Text);
                            var junho = ParseCellToDecimal(worksheet.Cells[row, 8].Text);
                            var julho = ParseCellToDecimal(worksheet.Cells[row, 9].Text);
                            var agosto = ParseCellToDecimal(worksheet.Cells[row, 10].Text);
                            var setembro = ParseCellToDecimal(worksheet.Cells[row, 11].Text);
                            var outubro = ParseCellToDecimal(worksheet.Cells[row, 12].Text);
                            var novembro = ParseCellToDecimal(worksheet.Cells[row, 13].Text);
                            var dezembro = ParseCellToDecimal(worksheet.Cells[row, 14].Text);

                            var faturamento = new BITicketMedio(empresa,
                                unidade,
                                descricao,
                                ano,
                                janeiro,
                                fevereiro,
                                marco,
                                abril,
                                maio,
                                junho,
                                julho,
                                agosto,
                                setembro,
                                outubro,
                                novembro,
                                dezembro, User.Identity.Name);

                            faturamentoList.Add(faturamento);
                        }

                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.BITicketMedio.AddRange(faturamentoList);
                    context.SaveChanges();

                }


            }

            return Ok();
        }


        [HttpPost]
        [Route("ImportarBITicketPorSetor")]
        public IActionResult ImportarBITicketPorSetor([FromForm] IFormFile excelFile, [FromForm] int idUnidade, [FromForm] int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdEmpresa == idEmpresa && x.IdUnidade == idUnidade);


            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("Arquivo não encontrado.");
            }

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    var faturamentoList = new List<BITicketPorSetor>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var descricao = worksheet.Cells[row, 1].Text;
                            var ano = worksheet.Cells[row, 2].Text;
                            var janeiro = ParseCellToDecimal(worksheet.Cells[row, 3].Text);
                            var fevereiro = ParseCellToDecimal(worksheet.Cells[row, 4].Text);
                            var marco = ParseCellToDecimal(worksheet.Cells[row, 5].Text);
                            var abril = ParseCellToDecimal(worksheet.Cells[row, 6].Text);
                            var maio = ParseCellToDecimal(worksheet.Cells[row, 7].Text);
                            var junho = ParseCellToDecimal(worksheet.Cells[row, 8].Text);
                            var julho = ParseCellToDecimal(worksheet.Cells[row, 9].Text);
                            var agosto = ParseCellToDecimal(worksheet.Cells[row, 10].Text);
                            var setembro = ParseCellToDecimal(worksheet.Cells[row, 11].Text);
                            var outubro = ParseCellToDecimal(worksheet.Cells[row, 12].Text);
                            var novembro = ParseCellToDecimal(worksheet.Cells[row, 13].Text);
                            var dezembro = ParseCellToDecimal(worksheet.Cells[row, 14].Text);

                            var faturamento = new BITicketPorSetor(empresa,
                                unidade,
                                descricao,
                                ano,
                                janeiro,
                                fevereiro,
                                marco,
                                abril,
                                maio,
                                junho,
                                julho,
                                agosto,
                                setembro,
                                outubro,
                                novembro,
                                dezembro, User.Identity.Name);

                            faturamentoList.Add(faturamento);
                        }

                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.BITicketPorSetor.AddRange(faturamentoList);
                    context.SaveChanges();

                }


            }

            return Ok();
        }

        [HttpPost]
        [Route("ImportarBITicketMedioPorSetor")]
        public IActionResult ImportarBITicketMedioPorSetor([FromForm] IFormFile excelFile, [FromForm] int idUnidade, [FromForm] int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdEmpresa == idEmpresa && x.IdUnidade == idUnidade);


            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("Arquivo não encontrado.");
            }

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    var faturamentoList = new List<BITicketMedioPorSetor>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var descricao = worksheet.Cells[row, 1].Text;
                            var ano = worksheet.Cells[row, 2].Text;
                            var janeiro = ParseCellToDecimal(worksheet.Cells[row, 3].Text);
                            var fevereiro = ParseCellToDecimal(worksheet.Cells[row, 4].Text);
                            var marco = ParseCellToDecimal(worksheet.Cells[row, 5].Text);
                            var abril = ParseCellToDecimal(worksheet.Cells[row, 6].Text);
                            var maio = ParseCellToDecimal(worksheet.Cells[row, 7].Text);
                            var junho = ParseCellToDecimal(worksheet.Cells[row, 8].Text);
                            var julho = ParseCellToDecimal(worksheet.Cells[row, 9].Text);
                            var agosto = ParseCellToDecimal(worksheet.Cells[row, 10].Text);
                            var setembro = ParseCellToDecimal(worksheet.Cells[row, 11].Text);
                            var outubro = ParseCellToDecimal(worksheet.Cells[row, 12].Text);
                            var novembro = ParseCellToDecimal(worksheet.Cells[row, 13].Text);
                            var dezembro = ParseCellToDecimal(worksheet.Cells[row, 14].Text);

                            var faturamento = new BITicketMedioPorSetor(empresa,
                                unidade,
                                descricao,
                                ano,
                                janeiro,
                                fevereiro,
                                marco,
                                abril,
                                maio,
                                junho,
                                julho,
                                agosto,
                                setembro,
                                outubro,
                                novembro,
                                dezembro, User.Identity.Name);

                            faturamentoList.Add(faturamento);
                        }

                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.BITicketMedioPorSetor.AddRange(faturamentoList);
                    context.SaveChanges();

                }


            }

            return Ok();
        }

        [HttpPost]
        [Route("ImportarBIMargemPorcentagem")]
        public IActionResult ImportarBIMargemPorcentagem([FromForm] IFormFile excelFile, [FromForm] int idUnidade, [FromForm] int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            var unidade = context.Unidade.FirstOrDefault(x => x.IdEmpresa == idEmpresa && x.IdUnidade == idUnidade);


            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("Arquivo não encontrado.");
            }

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    var faturamentoList = new List<BIMargemPorcentagem>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var descricao = worksheet.Cells[row, 1].Text;
                            var ano = worksheet.Cells[row, 2].Text;
                            var janeiro = ParseCellToDecimal(worksheet.Cells[row, 3].Text);
                            var fevereiro = ParseCellToDecimal(worksheet.Cells[row, 4].Text);
                            var marco = ParseCellToDecimal(worksheet.Cells[row, 5].Text);
                            var abril = ParseCellToDecimal(worksheet.Cells[row, 6].Text);
                            var maio = ParseCellToDecimal(worksheet.Cells[row, 7].Text);
                            var junho = ParseCellToDecimal(worksheet.Cells[row, 8].Text);
                            var julho = ParseCellToDecimal(worksheet.Cells[row, 9].Text);
                            var agosto = ParseCellToDecimal(worksheet.Cells[row, 10].Text);
                            var setembro = ParseCellToDecimal(worksheet.Cells[row, 11].Text);
                            var outubro = ParseCellToDecimal(worksheet.Cells[row, 12].Text);
                            var novembro = ParseCellToDecimal(worksheet.Cells[row, 13].Text);
                            var dezembro = ParseCellToDecimal(worksheet.Cells[row, 14].Text);

                            var faturamento = new BIMargemPorcentagem(empresa,
                                unidade,
                                descricao,
                                ano,
                                janeiro,
                                fevereiro,
                                marco,
                                abril,
                                maio,
                                junho,
                                julho,
                                agosto,
                                setembro,
                                outubro,
                                novembro,
                                dezembro, User.Identity.Name);

                            faturamentoList.Add(faturamento);
                        }

                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na linha {row}: {ex.Message}", ex);
                        }
                    }

                    context.BIMargemPorcentagem.AddRange(faturamentoList);
                    context.SaveChanges();

                }


            }

            return Ok();
        }



        [NonAction]
        public decimal ParseCellToDecimal(string cellText)
        {

            if (string.IsNullOrEmpty(cellText))
            {
                return 0m;
            }

            string valorFormatado = Regex.Replace(cellText, @"[^0-9.,.-]", "");

            if (valorFormatado.Contains(",") && valorFormatado.Contains("."))
            {
                valorFormatado = valorFormatado.Replace(".", "");
                valorFormatado = valorFormatado.Replace(",", ".");
            }
            else if (valorFormatado.Contains(","))
            {
                valorFormatado = valorFormatado.Replace(",", ".");
            }

            else if( valorFormatado.Contains("."))
            {
                valorFormatado = valorFormatado.Replace(".", "");
            }

            if (decimal.TryParse(valorFormatado, NumberStyles.Any, CultureInfo.InvariantCulture, out var valorConvertido))
            {
                return valorConvertido;
            }

            return 0m;
        }

        [NonAction]
        public int ParseCellToInt(string cellText)
        {
            if (string.IsNullOrEmpty(cellText))
            {
                return 0;
            }

            // Substitui o ponto e converte para inteiro
            string valorFormatado = cellText.Replace(".", "").Split(',')[0];

            // Usa TryParse para evitar exceções
            if (int.TryParse(valorFormatado, NumberStyles.Integer, CultureInfo.InvariantCulture, out int valorConvertido))
            {
                return valorConvertido;
            }

            return 0;
        }
    }
}
      
