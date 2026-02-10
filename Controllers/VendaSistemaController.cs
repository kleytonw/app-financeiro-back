using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using ERP_API.Service.Parceiros.Interface;
using ERP_API.Service.Parceiros;
using ERP_API.Service;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using System.Net.Http;
using RestMethod = RestSharp.Method;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Threading;
using Microsoft.Extensions.Azure;
using System.Collections.Generic;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class VendaSistemaController : ControllerBase
    {
        protected Context context;
        protected IConciliadoraService _conciliadoraService;
        private readonly HttpClient _httpClient;
        private IBlobStorageService blobStorageService;
        private readonly string _baseUrl = "https://api.conciliadora.com.br/api/EnvioVendaSistema";
        public VendaSistemaController(Context context, IConciliadoraService conciliadoraService, HttpClient httpClient, IBlobStorageService blobStorageService)
        {
            this.context = context;
            this._conciliadoraService = conciliadoraService;
            this._httpClient = httpClient;
            this.blobStorageService = blobStorageService;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar(int idCliente)
        {
            var result = context.VendaSistema.
                Include(c => c.Cliente.Pessoa)
                .Include(c => c.ERPs)
                .Where(x => x.IdCliente == idCliente)
                .Select(c => new
                {
                    c.IdVendaSistema,
                    c.Data,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdERPs,
                    NomeERP = c.ERPs.Nome,
                    c.Arquivo,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarArquivos")]
        public IActionResult ListarAtivos()
        {
            var result = context.VendaSistema.
                Include(c => c.Cliente.Pessoa)
                .Include(c => c.ERPs)
                .Select(c => new
                {
                    c.IdVendaSistema,
                    c.Data,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdERPs,
                    NomeERP = c.ERPs.Nome,
                    c.Arquivo,
                    c.Situacao
                }).ToList().OrderByDescending(x => x.IdVendaSistema).Take(1000);
            return Ok(result);

        }


        [HttpPost]
        [Route("listarSelecionados")]
        public IActionResult ListarSelecionados([FromBody] PesquisaVendaSistemaRequest model)
        {
           var query = context.VendaSistema.Include(c => c.Cliente.Pessoa)
                .Include(c => c.ERPs).Where(x => x.IdCliente == model.IdCliente).AsQueryable();

            if(model.DataInicio != null && model.DataFim != null)
            {
                query = query.Where(x => x.Data >= model.DataInicio && x.Data <=  model.DataFim);
            }

            var result = query
                .Select(c => new
                {
                    c.IdVendaSistema,
                    c.Data,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdERPs,
                    NomeERP = c.ERPs.Nome,
                    c.Arquivo,
                    c.Situacao
                }).ToList().OrderBy(x => x.IdVendaSistema).Take(1000);

            return Ok(result);

        }

        [HttpPost("enviar")]
        [AllowAnonymous]
        public async Task<IActionResult> EnviarVendaSistema([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo não enviado.");

            var client = new RestClient("https://api.conciliadora.com.br/api/EnvioVendaSistema");
            client.Timeout = -1;

            var request = new RestRequest(RestSharp.Method.POST);
            request.AlwaysMultipartFormData = true;

            request.AddParameter("senha", "HNbtUy9d");
            request.AddParameter("idEmpresa", "516");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            request.AddFile("file", fileBytes, file.FileName, "application/xml");

            IRestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);

            return Ok(response.Content);
        }


        [HttpPost("file2")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadXmlFile(IFormFile xmlFile)
        {
            if (xmlFile == null || xmlFile.Length == 0)
                return BadRequest("Arquivo XML não enviado.");

            // Lê o conteúdo do arquivo para memória
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                await xmlFile.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }

            // Envia para o endpoint da conciliadora
            var conciliadoraResponse = await EnviarParaConciliadora(fileBytes, xmlFile.FileName);

            return Ok(new
            {
                message = "XML recebido e enviado à conciliadora com sucesso!",
                conciliadoraResponse
            });
        }



        [NonAction]
        private async Task<string> EnviarParaConciliadora(byte[] xmlBytes, string fileName)
        {
            var client = new RestClient("https://api.conciliadora.com.br/api/");

            var request = new RestRequest("EnvioVendaSistema", RestSharp.Method.POST);
            request.AlwaysMultipartFormData = true;

            // Adiciona os campos de autenticação/informação
            request.AddParameter("senha", "HNbtUy9d");
            request.AddParameter("idEmpresa", "516");

            // Adiciona o arquivo diretamente do array de bytes
            request.AddFile("file", xmlBytes, fileName, "application/xml");

            var response = await client.ExecuteAsync(request);
            return response.Content;
        }

        [HttpGet]
        [Route("download")]
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                var vendaSistema = context.VendaSistema
                    .FirstOrDefault(x => x.IdVendaSistema == id);

                if (vendaSistema == null || string.IsNullOrEmpty(vendaSistema.NomeArquivo))
                    return NotFound("Arquivo não encontrado.");

                var (stream, contentType) = await blobStorageService.DownloadAsync(vendaSistema.NomeArquivo);

                if (stream == null || stream.Length == 0)
                {
                    return NotFound("Arquivo não encontrado no storage.");
                }

                var extensaoOriginal = Path.GetExtension(vendaSistema.NomeArquivo).ToLowerInvariant();
                var mimeType = ObterMimeType(extensaoOriginal) ?? "application/octet-stream";

                stream.Position = 0;

                // ✅ IMPORTANTE: Definir o Content-Disposition corretamente
                var fileName = Path.GetFileName(vendaSistema.NomeArquivo);
                Response.Headers.Clear();
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                Response.Headers.Add("Content-Type", mimeType);

                return File(stream, mimeType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao baixar arquivo: {ex.Message}");
            }
        }

        // ✅ Melhorar o método ObterMimeType
        private string ObterMimeType(string extensao)
        {
            return extensao.ToLowerInvariant() switch
            {
                ".xml" => "application/xml",
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".txt" => "text/plain",
                ".zip" => "application/zip",
                ".rar" => "application/x-rar-compressed",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };
        }

        [HttpPost]
        [Route("salvar")]
        [AllowAnonymous]
        public async Task<IActionResult> Salvar([FromForm] int idCliente, [FromForm] int idERPs, IFormFile arquivo)
        {
            try
            {
                var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == idCliente);
                if (cliente == null)
                {
                    return NotFound("Cliente não encontrado.");
                }

                var erps = context.ERPs.FirstOrDefault(x => x.IdERPs == idERPs);
                if (erps == null)
                {
                    return NotFound("ERP não encontrado.");
                }

                if (arquivo == null || arquivo.Length == 0)
                {
                    return BadRequest("Nenhum arquivo foi enviado.");
                }

                var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
                if (extensao != ".xml")
                {
                    return BadRequest("Apenas arquivos XML são permitidos.");
                }

                string conteudoXml;
                try
                {
                    using (var reader = new StreamReader(arquivo.OpenReadStream(), Encoding.UTF8))
                    {
                        conteudoXml = await reader.ReadToEndAsync();
                        XDocument.Parse(conteudoXml);
                    }
                }
                catch (XmlException)
                {
                    return BadRequest("O arquivo não contém um XML válido.");
                }
                catch (Exception)
                {
                    return BadRequest("Erro ao processar o arquivo.");
                }


                var senhaConciliadora = context.Cliente
                    .FirstOrDefault(x => x.IdPessoa == idCliente && x.Situacao == "Ativo").SenhaConciliadora;

                if (senhaConciliadora == null || string.IsNullOrEmpty(senhaConciliadora))
                {
                    return BadRequest("Senha não encontrada para este cliente.");
                }

                if (string.IsNullOrEmpty(cliente.IdentificadorConciliadora))
                {
                    return BadRequest("Cliente não possui identificador da conciliadora configurado.");
                }

                var (success, errorMessage, data) = await _conciliadoraService.EnviarVendaSistemaAsync(
                     senhaConciliadora,
                     cliente.IdentificadorConciliadora,
                     arquivo);  

                VendaSistema vendaSistema;

                if (data.Codigo == 0)
                {
                    var salvarArquivoBlob = await blobStorageService.UploadAsync(arquivo);

                    vendaSistema = new VendaSistema(
                        DateTime.Now,
                        cliente,
                        erps,
                        conteudoXml,
                        salvarArquivoBlob,
                        User.Identity?.Name
                    );

                    vendaSistema.SetDadosRespostaEnvio(
                        data.Codigo,
                        data.Mensagem,
                        data.XmlErros[1].Erro,
                        true
                    );

                   
                }
                else
                {
                    var salvarArquivoBlob = await blobStorageService.UploadAsync(arquivo);

                    vendaSistema = new VendaSistema(
                        DateTime.Now,
                        cliente,
                        erps,
                        conteudoXml,
                        salvarArquivoBlob,
                        User.Identity?.Name
                    );

                    vendaSistema.SetDadosRespostaEnvio(
                        data.Codigo,
                        data.Mensagem,
                        data.XmlErros[1].Erro,
                        false
                    );

                    vendaSistema.Situacao = "Erro";
                }

                context.VendaSistema.Add(vendaSistema);
                context.SaveChanges();


                if (data.Codigo == 0)
                {
                    return Ok(new
                    {
                        sucesso = true,
                        codigo = data.Codigo,
                        mensagem = data.Mensagem,
                        idVendaSistema = vendaSistema.IdVendaSistema,
                        xmlTamanho = conteudoXml.Length
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        sucesso = false,
                        codigo = data.Codigo,
                        mensagem = data.Mensagem,
                        xmlErros = data.XmlErros[1].Erro,
                        idVendaSistema = vendaSistema.IdVendaSistema,
                        xmlTamanho = conteudoXml.Length
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Erro no método Salvar: {ex.Message}");
                Console.WriteLine($"[ERROR] StackTrace: {ex.StackTrace}");

                return StatusCode(500, new
                {
                    sucesso = false,
                    mensagem = $"Erro interno: {ex.Message}"
                });
            }
        }


        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var vendaSistema = context.VendaSistema.FirstOrDefault(x => x.IdVendaSistema == id);
            if (vendaSistema == null)
            {
                return NotFound("Venda não encontrada.");
            }
            vendaSistema.Excluir(User.Identity.Name);
            context.VendaSistema.Update(vendaSistema);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obeter(int id)
        {
            var vendaSistema = context.VendaSistema.Include(c => c.Cliente.Pessoa).FirstOrDefault(x => x.IdVendaSistema == id);
            if (vendaSistema == null)
            {
                return NotFound("Venda não encontrada.");
            }
            return Ok(new VendaSistemaResponse
            {
                IdVendaSistema = vendaSistema.IdVendaSistema,
                Data = vendaSistema.Data,
                IdCliente = vendaSistema.IdCliente,
                NomeCliente = vendaSistema.Cliente.Pessoa.Nome,
                IdERPs = vendaSistema.IdERPs,
                NomeERPs = vendaSistema.ERPs.Nome,
                Arquivo = vendaSistema.Arquivo,
                Situacao = vendaSistema.Situacao

            });

        }
    }
}
