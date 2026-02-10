using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using Microsoft.EntityFrameworkCore;
using ERP_API.Models.BI;
using System.Linq;
using Dapper;
using System.Collections.Generic;
using System;
using EPPlus.Core.Extensions;
using ERP_API.Service.BI2Service;
using ERP_API.Models.BI2.Filtros;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class BI2Controller : ControllerBase
    {
        protected Context context;


        public IRelatorioBIService _relatorioBIService;   
        public BI2Controller(Context context, IRelatorioBIService relatorioBIService)
        {
            this.context = context;
            _relatorioBIService = relatorioBIService;
        }

        [HttpPost("faturamento-mensal")]
        public IActionResult GetFaturamentoMensal([FromBody] FiltroBIModel model)
        {
            var lista = _relatorioBIService.ObterFaturamentoMensal(model);
            return Ok(lista);
        }

        [HttpPost("margem-bruta")]
        public IActionResult GetMargemBruta([FromBody] FiltroBIModel model)
        {
            var lista = _relatorioBIService.ObterMargemBrutaMensal(model);
            return Ok(lista);
        }

        [HttpPost("ticket-medio")]
        public IActionResult ObterTicketMedio([FromBody] FiltroBIModel model)
        {
            var lista = _relatorioBIService.ObterTicketMedio(model);
            return Ok(lista);
        }

        [HttpPost("top-produtos")]
        public IActionResult postTopProdutos([FromBody] FiltroBIModel model)
        {
            var lista = _relatorioBIService.TopProdutosMaisVendidos(model);
            return Ok(lista);
        }

        [HttpPost("devolucoes")]
        public IActionResult ObterDevolucoes([FromBody] FiltroBIModel model)
        {
            var lista = _relatorioBIService.ObterDevolucoes(model);
            return Ok(lista);
        }

        [HttpPost("movimentacoes-por-tipo")]
        public IActionResult ObterMovimentacaoPorTipo([FromBody] FiltroBIModel model)
        {
            var lista = _relatorioBIService.ObterMovimentacaoPorTipo(model);
            return Ok(lista);
        }

        [HttpPost("volume-itens")]
        public IActionResult ObterVolumeItens([FromBody] FiltroBIModel model)
        {
            var lista = _relatorioBIService.ObterVolumeItens(model);
            return Ok(lista);
        }

        [HttpPost("mov-periodo")]
        public IActionResult ObterMovimentacaoPeriodo([FromBody] FiltroBIMovModel model)
        {
            var lista = _relatorioBIService.ObterMovimentacaoPeriodo(model);
            return Ok(lista);
        }

        [HttpPost("top-categorias")]
        public IActionResult ObterTopCategoria([FromBody] FiltroBIMovModel model)
        {
            var lista = _relatorioBIService.ObterTopCategoria(model);
            return Ok(lista);
        }

        [HttpPost("faturamento-por-cliente")]
        public IActionResult FaturamentoCliente([FromBody] FiltroBIMovModel model)
        {
            var lista = _relatorioBIService.FaturamentoCliente(model);
            return Ok(lista);
        }

        [HttpPost("indicador-promocional")]
        public IActionResult ObterPromocional([FromBody] FiltroBIModel model)
        {
            var lista = _relatorioBIService.ObterPromocional(model);
            return Ok(lista);
        }
    }
}