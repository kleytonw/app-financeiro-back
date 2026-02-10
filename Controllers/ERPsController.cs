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

    public class ERPsController : ControllerBase  
    {
            protected Context context;
            public ERPsController(Context context)
            {
                this.context = context;
            }
            [HttpGet]
            [Route("listar")]
            public IActionResult Listar()
            {
                var result = context.ERPs.Include(x => x.Fornecedor.Pessoa).Select(c => new
                {
                    c.IdERPs,
                    c.Nome,
                    NomeFornecedor = c.Fornecedor.Pessoa.Nome,
                    c.IdFornecedor,
                    c.Situacao
                }).ToList();
                return Ok(result);
            }
            [HttpGet]
            [Route("listarAtivos")]
            public IActionResult ListarAtivos()
            {
                var result = context.ERPs.Include(x => x.Fornecedor.Pessoa).Where(x => x.Situacao == "Ativo")
                    .Select(c => new
                    {
                        c.IdERPs,
                        c.Nome,
                        NomeFornecedor = c.Fornecedor.Pessoa.Nome,
                        c.IdFornecedor,
                        c.Situacao
                    }).ToList();
                return Ok(result);
            }

            [HttpPost]
            [Route("salvar")]
            public IActionResult Salvar([FromBody] ERPsRequest model)
            {
                if (model == null)
                    return BadRequest("Dados inválidos.");
                var fornecedor = context.Fornecedor.FirstOrDefault(x => x.IdPessoa == model.IdFornecedor);
                if (fornecedor == null)
                    return NotFound("Fornecedor não encontrado.");
                if (model.IdERPs > 0)
                {
                    var erp = context.ERPs.FirstOrDefault(x => x.IdERPs == model.IdERPs);
                    if (erp == null)
                        return NotFound("ERP não encontrado.");
                    erp.Alterar(model.Nome, fornecedor, User.Identity.Name);

                }
                else
                {
                    var erp = new ERPs(model.Nome, fornecedor, User.Identity.Name);
                    context.ERPs.Add(erp);


                }
                context.SaveChanges();
                return Ok();
            }

            [HttpGet]
            [Route("excluir")]
            public IActionResult Excluir(int id)
            {
                var erp = context.ERPs.FirstOrDefault(x => x.IdERPs == id);
                if (erp == null)
                    return NotFound("ERP não encontrado.");
                erp.Excluir(User.Identity.Name);
                context.SaveChanges();
                return Ok();
            }
            
            
            [HttpGet]
            [Route("obter")]
            public IActionResult Obter(int id)
            {
                var erp = context.ERPs.FirstOrDefault(x => x.IdERPs == id);
                if (erp == null)
                    return NotFound("ERP não encontrado.");
                
                 return Ok(new ERPsResponse
                {
                    IdERPs = erp.IdERPs,
                    Nome = erp.Nome,
                    IdFornecedor = erp.IdFornecedor,
                    Situacao = erp.Situacao
                });
            }
    }
}
