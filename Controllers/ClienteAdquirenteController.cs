using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using ERP_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ClienteAdquirenteController : ControllerBase
    {
        protected Context context;
        protected readonly IBlobStorageService _blobStorageService;
        public ClienteAdquirenteController(Context context, IBlobStorageService blobStorageService)
        {
            this.context = context;
            _blobStorageService = blobStorageService;
        }
        [HttpGet]
        [Route("listar")]
        public IActionResult Listar(int idCliente)
        {
            var result = context.ClienteAdquirente.
                Include(c => c.Cliente.Pessoa)
                .Include(c => c.Operadora)
                .Where(x => x.IdCliente == idCliente)
                .Select(c => new
                {
                    c.IdClienteAdquirente,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdOperadora,
                    NomeAdquirente = c.Operadora.NomeOperadora,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }
        [HttpGet]
        [Route("listarAtivos")]
        public IActionResult ListarAtivos()
        {
            var result = context.ClienteAdquirente.
                 Include(c => c.Cliente.Pessoa)
                .Include(c => c.Operadora)
                .Where(x => x.Situacao == "Ativo")
                .Select(c => new
                {
                    c.IdClienteAdquirente,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdOperadora,
                    NomeAdquirente = c.Operadora.NomeOperadora,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [AllowAnonymous]
        public async Task<IActionResult> Salvar([FromForm] IFormFile arquivo, [FromForm] int idCliente, [FromForm] int idTipoDocumento, [FromForm] int idOperadora, [FromForm] string descricao)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == idCliente);
            if (cliente == null)
                return NotFound("Cliente não encontrado.");
            var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == idOperadora);
            if (operadora == null)
                return NotFound("Operadora não encontrado.");
            var tipoDocumento = context.TipoDocumento.FirstOrDefault(x => x.IdTipoDocumento == idTipoDocumento);
            if (tipoDocumento == null)
                return NotFound("Tipo de documento não encontrado.");

            var responseUpload = await _blobStorageService.UploadAsync(arquivo);

            var clienteAdquirente = new ClienteAdquirente(operadora, cliente, User.Identity.Name);
            context.ClienteAdquirente.Add(clienteAdquirente);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var clienteAdquirente = context.ClienteAdquirente.FirstOrDefault(x => x.IdClienteAdquirente == id);
            if (clienteAdquirente == null)
                return NotFound("Cliente Adquirente não encontrado.");
            clienteAdquirente.Excluir(User.Identity.Name);
            context.ClienteAdquirente.Update(clienteAdquirente);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var clienteAdquirente = context.ClienteAdquirente
                .Include(c => c.Cliente.Pessoa)
                .Include(c => c.Operadora)
                .FirstOrDefault(x => x.IdClienteAdquirente == id);

            if (clienteAdquirente == null)
                return NotFound("Cliente Adquirente não encontrado.");

            return Ok(new ClienteAdquirenteResponse
            {
                IdClienteAdquirente = clienteAdquirente.IdClienteAdquirente,
                IdCliente = clienteAdquirente.IdCliente,
                NomeCliente = clienteAdquirente.Cliente.Pessoa.Nome,
                IdOperadora = clienteAdquirente.IdOperadora,
                NomeOperadora = clienteAdquirente.Operadora.NomeOperadora,
                Situacao = clienteAdquirente.Situacao,
            });
        }
    }
}
