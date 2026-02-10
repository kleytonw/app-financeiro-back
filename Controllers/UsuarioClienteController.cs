using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Data.Entity;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class UsuarioClienteController : ControllerBase
    {
        protected Context context;

        public UsuarioClienteController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar(int idUsuario)
        {
            var result = context.UsuarioCliente.Include(x => x.Usuario).Include(x => x.Cliente)
                .Where(x => x.IdUsuario == idUsuario).Select(m => new
                {
                    m.IdUsuarioCliente,
                    NomeCliente = m.Cliente.Pessoa.Nome,
                    m.IdCliente,
                    m.Usuario.Nome,
                    m.IdUsuario,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }


        [HttpGet]
        [Route("listarAtivos")]
        public IActionResult ListarAtivos()
        {
            var result = context.UsuarioCliente.Include(x => x.Usuario).Include(x => x.Cliente)
                .Select(m => new
                {
                    m.IdUsuarioCliente,
                    NomeCliente = m.Cliente.Pessoa.Nome,
                    m.IdCliente,
                    m.Usuario.Nome,
                    m.IdUsuario,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] UsuarioClienteRequest model)
        {
            var usuario = context.Usuario.FirstOrDefault(x => x.IdUsuario == model.IdUsuario);
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);

            if(model.IdUsuarioCliente > 0)
            {
                var usuarioCliente = context.UsuarioCliente.FirstOrDefault(x => x.IdUsuarioCliente == model.IdUsuarioCliente);

                usuarioCliente.Alterar(cliente, usuario, User.Identity.Name);
                context.Update(usuarioCliente);
            }
            else
            {
                var usuarioCliente = new UsuarioCliente(cliente, usuario, User.Identity.Name);
                context.Add(usuarioCliente);
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var usuarioCliente = context.UsuarioCliente.FirstOrDefault(x => x.IdUsuarioCliente == id);
            if (usuarioCliente == null)
                return BadRequest("Cliente não encontrado!");

            context.Remove(usuarioCliente);
            context.SaveChanges();
            return Ok();
            
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var usuarioCliente = context.UsuarioCliente.Include(x => x.Cliente).Include(x => x.Usuario).FirstOrDefault(x => x.IdUsuarioCliente == id);
            if (usuarioCliente == null)
                return BadRequest("Cliente não encontrado!");

            return Ok(new UsuarioClienteResponse()
            {
                IdUsuarioCliente = usuarioCliente.IdUsuarioCliente,
                IdCliente = usuarioCliente.IdCliente,
                NomeCliente = usuarioCliente.Cliente.Pessoa.Nome,
                IdUsuario = usuarioCliente.IdUsuario,
                NomeUsuario = usuarioCliente.Usuario.Nome,
                Situacao = usuarioCliente.Situacao,
            });
        }
    }
}
