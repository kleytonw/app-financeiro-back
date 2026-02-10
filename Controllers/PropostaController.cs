using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using System.Data.Entity;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class PropostaController : ControllerBase
    {
        private Context context;

        public PropostaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var propostas = context.Proposta
                .Select(p => new
                {
                    p.IdProposta,
                    p.Sexo,
                    p.DataNascimento,
                    p.Mae,
                    p.Pai,
                    p.Nome,
                    p.TipoPessoa,
                    p.RazaoSocial,
                    p.CpfCnpj,
                    p.Telefone1,
                    p.Telefone2,
                    p.Email,
                    p.Cep,
                    p.Logradouro,
                    p.Numero,
                    p.Complemento,
                    p.Bairro,
                    p.Cidade,
                    p.Estado,
                    p.Referencia,
                    p.InscricaoEstadual,
                    p.InscricaoMunicipal,
                    p.IdPlano,
                    NomePlano = p.Plano.Nome,
                    p.IdVendedor,
                    NomeVendedor = p.Vendedor.Pessoa.Nome,
                    p.DataInicio,
                    p.DataTermino,
                    p.StatusProposta,
                    p.Situacao
                }).ToList();
            return Ok(propostas);

        }

        [HttpGet]
        [Route("listarAtivos")]
        public IActionResult ListarAtivos()
        {
            var propostasAtivas = context.Proposta
                .Where(p => p.Situacao == "Ativo")
                .Select(p => new
                {
                    p.IdProposta,
                    p.Sexo,
                    p.DataNascimento,
                    p.Mae,
                    p.Pai,
                    p.Nome,
                    p.TipoPessoa,
                    p.RazaoSocial,
                    p.CpfCnpj,
                    p.Telefone1,
                    p.Telefone2,
                    p.Email,
                    p.Cep,
                    p.Logradouro,
                    p.Numero,
                    p.Complemento,
                    p.Bairro,
                    p.Cidade,
                    p.Estado,
                    p.Referencia,
                    p.InscricaoEstadual,
                    p.InscricaoMunicipal,
                    p.IdPlano,
                    NomePlano = p.Plano.Nome,
                    p.IdVendedor,
                    NomeVendedor = p.Vendedor.Pessoa.Nome,
                    p.DataInicio,
                    p.DataTermino,
                    p.StatusProposta,
                    p.Situacao
                }).ToList();
            return Ok(propostasAtivas);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar(PropostaRequest model)
        {
            if (model == null)
                return BadRequest("Dados inválidos");
            var proposta = new Proposta(
                    model.Nome,
                    model.Sexo,
                    model.DataNascimento,
                    model.Mae,
                    model.Pai,
                    model.TipoPessoa,
                    model.RazaoSocial,
                    model.CpfCnpj,
                    model.Telefone1,
                    model.Telefone2,
                    model.Email,
                    model.Cep,
                    model.Logradouro,
                    model.Numero,
                    model.Complemento,
                    model.Bairro,
                    model.Cidade,
                    model.Estado,
                    model.Referencia,
                    model.InscricaoEstadual,
                    model.InscricaoMunicipal,
                    context.Plano.FirstOrDefault(x => x.IdPlano == model.IdPlano),
                    context.Vendedor.FirstOrDefault(x => x.IdPessoa  == model.IdVendedor),
                    model.DataInicio,
                    model.DataTermino,
                    model.StatusProposta,
                    User.Identity.Name
            );
            context.Proposta.Add(proposta);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        public IActionResult Excluir(int id)
        {
            var proposta = context.Proposta.FirstOrDefault(x => x.IdProposta == id);
            if (proposta == null)
                return NotFound("Proposta não encontrada");
            proposta.Excluir(User.Identity.Name);
            context.Proposta.Update(proposta);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var proposta = context.Proposta
                .Include(x => x.Plano)
                .Include(x => x.Vendedor.Pessoa)
                .FirstOrDefault(x => x.IdProposta == id);
            if (proposta == null)
                return NotFound("Proposta não encontrada");
            return Ok(new PropostaResponse()
            {
                IdProposta = proposta.IdProposta,
                Nome = proposta.Nome,
                TipoPessoa = proposta.TipoPessoa,
                RazaoSocial = proposta.RazaoSocial,
                CpfCnpj = proposta.CpfCnpj,
                Telefone1 = proposta.Telefone1,
                Telefone2 = proposta.Telefone2,
                Email = proposta.Email,
                Cep = proposta.Cep,
                Logradouro = proposta.Logradouro,
                Numero = proposta.Numero,
                Complemento = proposta.Complemento,
                Bairro = proposta.Bairro,
                Cidade = proposta.Cidade,
                Estado = proposta.Estado,
                Referencia = proposta.Referencia,
                InscricaoEstadual = proposta.InscricaoEstadual,
                InscricaoMunicipal = proposta.InscricaoMunicipal,
                IdPlano = proposta.IdPlano,
                NomePlano = proposta.Plano.Nome,
                IdVendedor = proposta.IdVendedor,
                NomeVendedor = proposta.Vendedor.Pessoa.Nome,
                DataInicio = proposta.DataInicio,
                DataTermino = proposta.DataTermino,
                StatusProposta = proposta.StatusProposta
            });
        }
    }
}