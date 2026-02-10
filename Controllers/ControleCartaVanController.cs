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
    [ApiExplorerSettings(IgnoreApi  = true)]
    [Authorize]
    public class ControleCartaVanController : ControllerBase
    {
        public Context context;

        public ControleCartaVanController(Context contex)
        {
            this.context = contex;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]

        public IActionResult Listar()
        {
            var result = context.ControleCartaVan
                .Include(x => x.Cliente)
                .Include(x => x.ClienteContaBancaria)
                .Include(x => x.Etapa)
                .Select(
                    m => new
                    {
                        m.IdControleCartaVan,
                        m.IdCliente,
                        NomeCliente = m.Cliente.Pessoa.Nome,
                        m.IdClienteContaBancaria,
                        m.ClienteContaBancaria.Banco.NomeBanco,
                        m.ClienteContaBancaria.Conta,
                        m.ClienteContaBancaria.DigitoConta,
                        m.ClienteContaBancaria.Agencia,
                        m.ClienteContaBancaria.DigitoAgencia,
                        m.IdEtapa,
                        m.Etapa.Nome,
                        m.Status,
                        m.TicketFornecedor,
                        m.Descricao,
                        m.Situacao
                    });
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ControleCartaVanRequest model)
        {
            Cliente cliente;
            ClienteContaBancaria clienteContaBancaria;
            Etapa etapa;

            if(model.IdControleCartaVan > 0)
            {
                var controleCartaVan = context.ControleCartaVan.FirstOrDefault(x => x.IdControleCartaVan == model.IdControleCartaVan);
                cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
                clienteContaBancaria = context.ClienteContaBancaria.FirstOrDefault(x => x.IdClienteContaBancaria == model.IdClienteContaBancaria);
                etapa = context.Etapa.FirstOrDefault(x => x.IdEtapa == model.IdEtapa);


                controleCartaVan.Alterar(cliente, clienteContaBancaria, etapa, model.TicketFornecedor, model.Descricao, User.Identity.Name);
                context.ControleCartaVan.Update(controleCartaVan);
            }
            else
            {
                cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
                clienteContaBancaria = context.ClienteContaBancaria.FirstOrDefault(x => x.IdClienteContaBancaria == model.IdClienteContaBancaria);
                etapa = context.Etapa.FirstOrDefault(x => x.IdEtapa == model.IdEtapa);

                var controleCartaVan = new ControleCartaVan(cliente, clienteContaBancaria, etapa, model.TicketFornecedor, model.Descricao, User.Identity.Name);
                controleCartaVan.AlterarEtapa(etapa);
                context.ControleCartaVan.Add(controleCartaVan);
            }

            context.SaveChanges();

            return Ok();

        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var controleCartaVan = context.ControleCartaVan.FirstOrDefault(x => x.IdControleCartaVan == id);
            if (controleCartaVan == null)
                return BadRequest("Controle de Carta Van não encontrado!");

            controleCartaVan.Excluir(User.Identity.Name);

            context.Update(controleCartaVan);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var controleCartaVan = context.ControleCartaVan.FirstOrDefault(x => x.IdControleCartaVan == id);
            if (controleCartaVan == null)
                return BadRequest("Controle de Carta Van não encontrado!");

            return Ok(new ControleCartaVanResponse()
            {
                IdControleCartaVan = controleCartaVan.IdControleCartaVan,
                IdCliente = controleCartaVan.IdCliente,
                IdClienteContaBancaria = controleCartaVan.IdClienteContaBancaria,
                IdEtapa = controleCartaVan.IdEtapa,
                TicketFornecedor = controleCartaVan.TicketFornecedor,
                Descricao = controleCartaVan.Descricao,
                Situacao = controleCartaVan.Situacao,
            });
        }
    }
}
