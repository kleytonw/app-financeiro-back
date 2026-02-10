using ERP.Infra;
using ERP.Models;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using System.Data.Entity;
using System.Linq;

namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ColaboradorController : ControllerBase
    {
        protected Context context;

        public ColaboradorController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var result = context.Colaborador.Include(x => x.Pessoa).AsQueryable();

            if (usuarioLogado.TipoUsuario == "Administrador")
            {
                result = result.Include(x => x.Pessoa);
            }
            else
            {
                result = result.Include(x => x.Pessoa).Include(x => x.Pessoa).Where(x => x.IdPessoa == usuarioLogado.IdPessoa);

            }

            result.Select(
                    m => new
                    {
                        m.IdPessoa,
                        m.Pessoa.Nome,
                        m.Pessoa.RazaoSocial,
                        m.Pessoa.CpfCnpj,
                        m.Situacao
                    });

            return Ok(
            result.Select(
                    m => new
                    {
                        m.IdPessoa,
                        m.Pessoa.Nome,
                        m.Pessoa.RazaoSocial,
                        m.Pessoa.CpfCnpj,
                        m.Situacao
                    }));
        }

        [HttpPost]
        [Route("Salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ColaboradorRequest model)
        {
            if (model.IdPessoa > 0)
            {
                var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);

                pessoa.Alterar(
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
                    User.Identity.Name
                );

                context.Update(pessoa);

            }
            else
            {
                var pessoa = new Pessoa(
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
                    User.Identity.Name
                );
                context.Add(pessoa);

                var colaborador = new Domain.Entidades.Colaborador(
                    pessoa,
                    User.Identity.Name
                );
                context.Add(colaborador);

            }

            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var colaborador = context.Colaborador.FirstOrDefault(x => x.IdPessoa == id);
            if (colaborador == null)
                return BadRequest("Colaborador não encontrado.");

            colaborador.Excluir(User.Identity.Name);
            context.Update(colaborador);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var colaborador = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            if (colaborador == null)
                return BadRequest("Colaborador não encontrado.");

            return Ok(new ColaboradorResponse()
            {
                IdPessoa = colaborador.IdPessoa,
                Nome = colaborador.Nome,
                RazaoSocial = colaborador.RazaoSocial,
                Telefone1 = colaborador.Telefone1,
                Telefone2 = colaborador.Telefone2,
                Email = colaborador.Email,
                CpfCnpj = colaborador.CpfCnpj,
                Cep = colaborador.Cep,
                Sexo = colaborador.Sexo,
                Estado = colaborador.Estado,
                Cidade = colaborador.Cidade,
                Bairro = colaborador.Bairro,
                Logradouro = colaborador.Logradouro,
                Numero = colaborador.Numero,
                Complemento = colaborador.Complemento,
                Referencia = colaborador.Referencia,
                DataNascimento = colaborador.DataNascimento,
                Mae = colaborador.Mae,
                Pai = colaborador.Pai,
                TipoPessoa = colaborador.TipoPessoa,
                InscricaoEstadual = colaborador.InscricaoEstadual,
                InscricaoMunicipal = colaborador.InscricaoMunicipal,
                Situacao = colaborador.Situacao

            });

        }
    }
}
