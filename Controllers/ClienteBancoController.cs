using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ClienteBancoController : ControllerBase
    {
        protected Context context;
        public ClienteBancoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar(int idCliente)
        {
            var result = context.ClienteBanco
                .Where(x => x.IdCliente == idCliente)
                .Select(c => new
                {
                    c.IdClienteBanco,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdBanco,
                    c.Banco.NomeBanco,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ClienteBancoRequest request)
        {
            Banco banco = context.Banco.FirstOrDefault(b => b.IdBanco == request.IdBanco);
            Cliente cliente = context.Cliente.FirstOrDefault(c => c.IdPessoa == request.IdCliente);
            if (request.IdClienteBanco == 0)
            {
                var clienteBanco = new ClienteBanco(banco, cliente, User.Identity.Name);
                context.ClienteBanco.Add(clienteBanco);
            }
            else
            {
                var clienteBancoExistente = context.ClienteBanco.FirstOrDefault(c => c.IdClienteBanco == request.IdClienteBanco);
                if (clienteBancoExistente == null)
                    return NotFound("ClienteBanco não encontrado.");

                clienteBancoExistente.Alterar(banco, cliente, User.Identity.Name);
                context.ClienteBanco.Update(clienteBancoExistente);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var clienteBanco = context.ClienteBanco.FirstOrDefault(x => x.IdClienteBanco == id);
            if (clienteBanco == null)
                return BadRequest("ClienteBanco não encontrado");

            clienteBanco.Excluir(User.Identity.Name);
            context.ClienteBanco.Update(clienteBanco);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var clienteBanco = context.ClienteBanco.Include(x => x.Cliente).Include(x => x.Banco).FirstOrDefault(x => x.IdClienteBanco == id);
            if (clienteBanco == null)
                return NotFound("ClienteBanco não encontrado.");

            return Ok(new ClienteBancoResponse
            {
                IdClienteBanco = clienteBanco.IdClienteBanco,
                IdCliente = clienteBanco.IdCliente,
                NomeCliente = clienteBanco.Cliente.Pessoa.Nome,
                IdBanco = clienteBanco.IdBanco,
                NomeBanco = clienteBanco.Banco.NomeBanco,   
            });
        }
    }
}
