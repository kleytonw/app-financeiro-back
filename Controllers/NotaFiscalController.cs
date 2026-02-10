using ERP_API.Domain.Entidades;
using ERP_API.Models;
using ERP_API.Models.NotaFiscal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using ERP.Infra;
using System.Linq;
using System.Data.Entity;
using ERP_API.Service.Parceiros.Interface;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.Entity.Infrastructure;

namespace ERP_API.Controllers
{
    public class NotaFiscalController : ControllerBase
    {

        protected Context context;
        protected IUniqueService _uniqueService;
        private IConfiguration _config;

        public NotaFiscalController(Context context, IConfiguration config, IUniqueService uniqueService)
        {
            _config = config;
            this.context = context;
            _uniqueService = uniqueService;
        }

        [HttpPost]
        [Route("criar")]
        [Authorize]
        public async Task<IActionResult> Salvar([FromBody] CriarNotaFiscalRequestModel model)
        {

            try
            {

                var cliente = context.Cliente.Include(x => x.Pessoa).FirstOrDefault(x => x.IdSacadoUnique == model.IdSacadoUnique);
                if (cliente == null)
                {
                    return BadRequest("Cliente não encontrado");
                }


                var token = await _uniqueService.GerarAccessTokenAsync(_config["unique:login"], _config["unique:password"], _config["unique:url"]);
                var resposta = await _uniqueService.CriarNfeAsync(model, token, User.Identity.Name);

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                // ERRO CLÁSSICO DE PROVIDER ASYNC (SEU CASO)
                return StatusCode(500, new
                {
                    erro = "Erro de operação inválida no Entity Framework",
                    detalhe = ex.Message,
                    dica = "Provável uso de EF6 (System.Data.Entity) ou IQueryable sem suporte async",
                    stackTrace = ex.StackTrace
                });
            }
            catch (DbUpdateException ex)
            {
                // ERRO DE BANCO (FK, constraint, etc)
                return StatusCode(500, new
                {
                    erro = "Erro ao salvar no banco de dados",
                    detalhe = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                // QUALQUER OUTRO ERRO
                return StatusCode(500, new
                {
                    erro = "Erro inesperado",
                    detalhe = ex.Message,
                    tipo = ex.GetType().FullName,
                    inner = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost]
        [Route("cancelar")]
        [Authorize]
        public async Task<IActionResult> Cancelar([FromQuery] int idnotafiscal)
        {
            try
            {
                var token = await _uniqueService.GerarAccessTokenAsync(_config["unique:login"], _config["unique:password"], _config["unique:url"]);
                var resposta = await _uniqueService.ExcluirNfeAsync(idnotafiscal,token,"admin");

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                // ERRO CLÁSSICO DE PROVIDER ASYNC (SEU CASO)
                return StatusCode(500, new
                {
                    erro = "Erro de operação inválida no Entity Framework",
                    detalhe = ex.Message,
                    dica = "Provável uso de EF6 (System.Data.Entity) ou IQueryable sem suporte async",
                    stackTrace = ex.StackTrace
                });
            }
            catch (DbUpdateException ex)
            {
                // ERRO DE BANCO (FK, constraint, etc)
                return StatusCode(500, new
                {
                    erro = "Erro ao salvar no banco de dados",
                    detalhe = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                // QUALQUER OUTRO ERRO
                return StatusCode(500, new
                {
                    erro = "Erro inesperado",
                    detalhe = ex.Message,
                    tipo = ex.GetType().FullName,
                    inner = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}
