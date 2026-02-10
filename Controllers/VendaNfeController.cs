using ERP.Infra;
using ERP_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Threading.Tasks;
using ERP_API.Domain.Entidades;
using ERP_API.Service;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.AspNetCore.Authorization;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class VendaNfeController : ControllerBase
    {
        protected Context context;
        protected IBlobStorageService blobStorageService;

        public VendaNfeController(Context context, IBlobStorageService blobStorageService)
        {
            this.context = context;
            this.blobStorageService = blobStorageService;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var uasrioLogador = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            if (uasrioLogador == null)
                return Unauthorized("Usuário não encontrado");
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == uasrioLogador.IdEmpresa);
            var vendas = context.VendaNfe
                .Include(x => x.Cliente.Pessoa)
                .Select(c => new
                {
                    c.IdVendaNfe,
                    c.IdCliente,
                    c.Senha,
                    c.DataVenda,
                    c.Modelo,
                    c.Arquivo,
                    c.Cliente.Pessoa.Nome,
                    c.Situacao
                }).ToList();
            return Ok(vendas);
        }

        [HttpGet]
        [Route("listarAtivos")]
        public IActionResult ListarAtivos(int idCliente)
        {
            var vendas = context.VendaNfe.Where(x => x.IdCliente == idCliente && x.Situacao == "Ativo")
                .Include(x => x.Cliente.Pessoa)
                .Select(c => new
                {
                    c.IdVendaNfe,
                    c.IdCliente,
                    c.Senha,
                    c.DataVenda,
                    c.Modelo,
                    c.Arquivo,
                    c.Cliente.Pessoa.Nome,
                    c.Situacao,
                }).ToList();
            return Ok(vendas);
        }

        [HttpPost]
        [Route("salvar")]
        [AllowAnonymous]
        public async Task<IActionResult> Salvar([FromForm] int idCliente, [FromForm] string senha, [FromForm] DateTime dataVenda, [FromForm] int modelo, IFormFile arquivo)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == idCliente);
            var senhaNfe = context.VendaNfe.FirstOrDefault(x => x.IdCliente == idCliente)?.Senha;
            if (cliente == null)
                return BadRequest("Cliente não encontrado");
            if (senhaNfe != senha)
                return BadRequest("Senha inválida!");

            // Verifica a extensão do arquivo
            var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
            if (extensao != ".xml")
            {
                return BadRequest("Apenas arquivos XML são permitidos.");
            }

            // ✅ LER ARQUIVO COMO STRING (SEM ARQUIVO TEMPORÁRIO)
            string nomeArquivoXml;
            try
            {
                using (var reader = new StreamReader(arquivo.OpenReadStream(), Encoding.UTF8))
                {
                    nomeArquivoXml = await reader.ReadToEndAsync();
                    // Verifica se o conteúdo é XML válido
                    XDocument.Parse(nomeArquivoXml);
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

            nomeArquivoXml = await blobStorageService.UploadAsync(arquivo);

            var vendaNfe = new VendaNfe(cliente, senha, dataVenda, modelo, nomeArquivoXml, User.Identity.Name);
            context.VendaNfe.Add(vendaNfe);
            context.SaveChanges();

      

            return Ok();
        }

        [HttpGet]
        [Route("baixarArquivo")]
        public async Task<IActionResult> BaixarArquivo(int id)
        {
            try
            {
                // 1. Buscar documento no banco
                var vendaNfe = context.VendaNfe
                    .Include(c => c.Cliente.Pessoa)
                    .FirstOrDefault(c => c.IdVendaNfe == id);

                if (vendaNfe == null)
                {
                    return NotFound("Documento não encontrado.");
                }

                // 2. Baixar arquivo do Blob Storage
                var (stream, contentType) = await blobStorageService.DownloadAsync(vendaNfe.Arquivo);

                if (stream == null || stream.Length == 0)
                {
                    return NotFound("Arquivo não encontrado no storage.");
                }

                var extensaoOriginal = Path.GetExtension(vendaNfe.Arquivo);

                // 4. Definir Content-Type correto
                var mimeType = ObterMimeType(extensaoOriginal) ?? contentType ?? "application/octet-stream";

                // 5. Garantir posição inicial do stream
                stream.Position = 0;

                var nomeArquivo = vendaNfe.Arquivo; // Nome original do GUID

                // Limpar Response Headers primeiro (para evitar duplicatas)
                Response.Headers.Remove("Content-Disposition");

                // Adicionar header manualmente
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{nomeArquivo}\"");


                // 7. Retornar arquivo
                return File(stream, mimeType, vendaNfe.Arquivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao baixar arquivo: {ex.Message}");
            }
        }

        private string ObterMimeType(string extensao)
        {
            var mimeTypes = new Dictionary<string, string>
            {
                { ".pdf", "application/pdf" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".gif", "image/gif" },
                { ".doc", "application/msword" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".txt", "text/plain" },
                { ".zip", "application/zip" },
                { ".rar", "application/x-rar-compressed" }
            };

            return mimeTypes.TryGetValue(extensao.ToLower(), out var mimeType) ? mimeType : null;
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarVendaNfeRequest model)
        {
            if(model.DataInicio > model.DataFinal)
                return BadRequest("Data inicial deve ser menor que a data final");
            if (model.DataInicio == default || model.DataFinal == default)
                return BadRequest("Data inicial e data final são obrigatórias");
            var query = context.VendaNfe.Include(x => x.Cliente.Pessoa).Where(x => x.DataVenda >=  model.DataInicio && x.DataVenda <= model.DataFinal).AsQueryable();

            if(model.OpcaoBusca == "Nome")
            {
                query = query.Where(x => x.Cliente.Pessoa.Nome.Contains(model.NomeCliente));
            }
            else if (model.OpcaoBusca == "CpfCnpj")
            {
                query = query.Where(x => x.Cliente.Pessoa.CpfCnpj.Contains(model.NomeCliente));
            }

            var resultado = query.Select(c => new
            {
                c.IdVendaNfe,
                c.IdCliente,
                c.Senha,
                c.DataVenda,
                c.Modelo,
                c.Arquivo,
                c.Cliente.Pessoa.Nome,
                c.Situacao
            }).ToList();

            return Ok(resultado);
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var vendaNfe = context.VendaNfe.FirstOrDefault(x => x.IdVendaNfe == id);
            if (vendaNfe == null)
                return NotFound("Venda NFe não encontrada");
            vendaNfe.Excluir(User.Identity.Name);
            context.VendaNfe.Update(vendaNfe);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var vendaNfe = context.VendaNfe
                .Include(x => x.Cliente.Pessoa)
                .FirstOrDefault(x => x.IdVendaNfe == id);
            if (vendaNfe == null)
                return NotFound("Venda NFe não encontrada");
             return Ok(new VendaNfeResponse()
            {
                IdVendaNfe = vendaNfe.IdVendaNfe,
                IdCliente = vendaNfe.IdCliente,
                Senha = vendaNfe.Senha,
                DataVenda = vendaNfe.DataVenda,
                Modelo = vendaNfe.Modelo,
                Arquivo = vendaNfe.Arquivo,
                Situacao = vendaNfe.Situacao
            });
        }
    }
}
