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
    public class ClienteParametrosController : ControllerBase
    {
        protected Context context;

        public ClienteParametrosController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar(int idCliente)
        {
            var result = context.ClienteParametros.Include(c => c.Cliente.Pessoa)
                .Where(x => x.IdCliente == idCliente)
                .Select(c => new
                {
                    c.IdClienteParametros,
                    c.IdCliente,
                    c.Chave,
                    c.Valor,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarAtivos")]
        public IActionResult ListarAtivos(int idCliente)
        {
            var result = context.ClienteParametros.Include(c => c.Cliente.Pessoa)
                .Where(x => x.Situacao == "Ativo")
                .Select(c => new
                {
                    c.IdClienteParametros,
                    c.IdCliente,
                    c.Chave,
                    c.Valor,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ClienteParametrosRequest model)
        {
            if (model == null)
                return BadRequest("Dados inválidos.");
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
            if (cliente == null)
                return NotFound("Cliente não encontrado.");
            if (model.IdClienteParametros > 0)
            {
                var clienteParametro = context.ClienteParametros.FirstOrDefault(x => x.IdClienteParametros == model.IdClienteParametros);
                if (clienteParametro == null)
                    return NotFound("Parâmetro não encontrado.");
                clienteParametro.Alterar(cliente, model.Chave, model.Valor, User.Identity.Name);
                context.ClienteParametros.Update(clienteParametro);
            }
            else
            {
                var clienteParametro = new ClienteParametros(cliente, model.Chave, model.Valor, User.Identity.Name);
                context.ClienteParametros.Add(clienteParametro);

            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var clienteParametro = context.ClienteParametros.FirstOrDefault(x => x.IdClienteParametros == id);
            if (clienteParametro == null)
                return NotFound("Parâmetro não encontrado.");
            clienteParametro.Excluir(User.Identity.Name);
            context.ClienteParametros.Update(clienteParametro);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var clienteParametro = context.ClienteParametros.Include(c => c.Cliente.Pessoa).FirstOrDefault(x => x.IdClienteParametros == id);
            if (clienteParametro == null)
                return NotFound("Parâmetro não encontrado.");

            return Ok(new ClienteParametrosResponse
            {
                IdClienteParametros = clienteParametro.IdClienteParametros,
                IdCliente = clienteParametro.IdCliente,
                NomeCliente = clienteParametro.Cliente.Pessoa.Nome,
                Chave = clienteParametro.Chave,
                Valor = clienteParametro.Valor,
                Situacao = clienteParametro.Situacao
            });
        }
    }
}
