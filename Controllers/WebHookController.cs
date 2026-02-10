using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ERP_API.Service.Parceiros;
using System.Linq;
using ERP_API.Domain.Entidades;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class WebHookController : ControllerBase
    {
        private IConfiguration _config;
        protected Context context;

        public WebHookController(IConfiguration config, Context context)
        {
            _config = config;
            this.context = context;
        }

        [HttpPost]
        [Route("ReceberPagamentoUnique")]
        [AllowAnonymous]
        public IActionResult ReceberPagamentoUnique([FromBody] WebHookUniqueRequest data)
        {
            if(data.Evento == "Pagamento")
            {
                MeioPagamento meioPagamento = context.MeioPagamento.FirstOrDefault(x => x.NomeMeioPagamento == "Boleto");
                var financeiroParcela = context.FinanceiroParcela.FirstOrDefault( x => x.IdentificadorBoletoUnique == data.ObjRequest.IdTransacao);
                financeiroParcela.BaixarConta(data.ObjRequest.DataVencimento,
                                              data.ObjRequest.DataPagamento,
                                              null,
                                              "Baixodo pelo webHook.",
                                              0,
                                              0,
                                              data.ObjRequest.ValorPago,
                                              data.ObjRequest.ValorVencimento,
                                              meioPagamento,
                                              "WebHook Unique"
                                              );
                context.FinanceiroParcela.Update(financeiroParcela);
                context.SaveChanges();
               
            }

            return Ok();
        }
    }
}
