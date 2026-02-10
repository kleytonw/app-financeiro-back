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
    public class ClienteERPController : ControllerBase
    {
        protected Context Context;

        public ClienteERPController(Context context)
        {
            this.Context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar(int idCliente)
        {
            var result = Context.ClienteERP.Include(c => c.Cliente.Pessoa).Include(c => c.ERPs)
                .Where(x => x.IdCliente == idCliente).Select(c => new
                {
                    c.IdClienteERP,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdERPs,
                    NomeERP = c.ERPs.Nome,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarAtivos")]
        public IActionResult ListarAtivos()
        {
            var result = Context.ClienteERP.Include(c => c.Cliente.Pessoa).Include(c => c.ERPs)
                .Where(x => x.Situacao == "Ativo")
                .Select(c => new
                {
                    c.IdClienteERP,
                    c.IdCliente,
                    c.Cliente.Pessoa.Nome,
                    c.IdERPs,
                    NomeERP = c.ERPs.Nome,
                    c.Situacao
                }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ERPClienteRequest model)
        {
            if (model == null)
                return BadRequest("Dados inválidos.");
            var cliente = Context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
            if (cliente == null)
                return BadRequest("Cliente não encontrado.");
            var erp = Context.ERPs.FirstOrDefault(x => x.IdERPs == model.IdERPs);
            if (erp == null)
                return BadRequest("ERP não encontrado.");
            if (model.IdClienteERP > 0)
            {
                var erpCliente = Context.ClienteERP.FirstOrDefault(x => x.IdClienteERP == model.IdClienteERP);
                if (erpCliente == null)
                    return BadRequest("ERP Cliente não encontrado.");
                erpCliente.Alterar(cliente, erp, User.Identity.Name);
            }
            else
            {
                var erpCliente = new ClienteERP(cliente, erp, User.Identity.Name);
                Context.ClienteERP.Add(erpCliente);
            }
            Context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var erpCliente = Context.ClienteERP.FirstOrDefault(x => x.IdClienteERP == id);
            if (erpCliente == null)
                return BadRequest("ERP Cliente não encontrado.");
            erpCliente.Excluir(User.Identity.Name);
            Context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var erpCliente = Context.ClienteERP.Include(c => c.Cliente.Pessoa).Include(c => c.ERPs)
                .FirstOrDefault(x => x.IdClienteERP == id);
            if (erpCliente == null)
                return BadRequest("ERP Cliente não encontrado.");
            return Ok(new ERPClienteResponse
            {
                IdClienteERP = erpCliente.IdClienteERP,
                IdCliente = erpCliente.IdCliente,
                NomeCliente = erpCliente.Cliente.Pessoa.Nome,
                IdERPs = erpCliente.IdERPs,
                NomeERP = erpCliente.ERPs.Nome,
                Situacao = erpCliente.Situacao
            });

        }
    }
}
