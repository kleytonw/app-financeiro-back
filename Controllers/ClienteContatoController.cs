using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ERP_API.Models;
using ERP_API.Domain.Entidades;
using Microsoft.CodeAnalysis.Operations;
using System;

namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ClienteContatoController : ControllerBase
    {
        protected Context context;

        public ClienteContatoController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar(int id)
        {
            var result = context.ClienteContato.Where(x => x.IdCliente == id)
                .Select(m => new
                {
                    m.IdClienteContato,
                    m.IdCliente,
                    m.Nome,
                    m.DataNascimento,
                    m.Telefone,
                    m.Email,
                    m.Cargo,
                    m.Observacao,
                    m.Situacao
                }).Take(500).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarAniversariantesMes")]
        public IActionResult ListarAnioversariantesMes()
        {
            var result = context.ClienteContato.Where(x => x.DataNascimento.Month == DateTime.Now.Month)
                .Select(m => new
                {
                    m.IdClienteContato,
                    m.IdCliente,
                    m.Nome,
                    m.DataNascimento,
                    m.Telefone,
                    m.Email,
                    m.Cargo,
                    m.Observacao,
                    m.Situacao
                }).Take(500).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ClienteContatoRequest model)
        {
            if (model.IdClienteContato > 0)
            {
                var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
                var clienteContato = context.ClienteContato.FirstOrDefault(x => x.IdClienteContato == model.IdClienteContato);

                clienteContato.Alterar(cliente,
                                       model.Nome,
                                       model.DataNascimento,
                                       model.Email,
                                       model.Telefone,
                                       model.Cargo,
                                       model.Observacao,
                                       User.Identity.Name);
                context.Update(clienteContato);
            }
            else
            {
                var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
                var clienteContato = new ClienteContato(cliente,
                                                        model.Nome,
                                                        model.DataNascimento,
                                                        model.Email,
                                                        model.Telefone,
                                                        model.Cargo,
                                                        model.Observacao,
                                                        User.Identity.Name);
                context.Add(clienteContato);
            }

            context.SaveChanges();
            return Ok();

        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var clienteContato = context.ClienteContato.FirstOrDefault(x => x.IdClienteContato == id);
            if (clienteContato == null)
                return BadRequest("Contato do cliente não encontrado!");

            return Ok(new ClienteContatoResponse()
            {
                IdClienteContato = clienteContato.IdClienteContato,
                IdCliente = clienteContato.IdCliente,
                Nome = clienteContato.Nome,
                DataNascimento = clienteContato.DataNascimento,
                Telefone = clienteContato.Telefone,
                Email = clienteContato.Email,
                Cargo = clienteContato.Cargo,
                Observacao = clienteContato.Observacao,
                Situacao = clienteContato.Situacao
            });
        }

        [HttpGet]
        [Route("deletar")]
        public IActionResult Deletar(int id)
        {
            var clienteContato = context.ClienteContato.FirstOrDefault(x => x.IdClienteContato == id);
            clienteContato.Excluir(User.Identity.Name);

            context.Remove(clienteContato);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var clienteContato = context.ClienteContato.FirstOrDefault(x => x.IdClienteContato == id);
            if (clienteContato == null)
                return BadRequest("Contato do cliente não encontrado!");
            clienteContato.Excluir(User.Identity.Name);
            context.Update(clienteContato);
            context.SaveChanges();
            return Ok();
        }
    }
}