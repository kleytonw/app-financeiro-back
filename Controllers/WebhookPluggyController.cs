using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using ERP_API.Service;
using ERP_API.Service.Parceiros.Interface;
using ERP_API.Service.Pluggy.Interface;
using Hangfire.Logging.LogProviders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [Authorize]
    public class WebhookPluggyController : ControllerBase
    {
        protected Context context;
        protected IWebhookPluggyService _webhookPluggyService;
        private IPluggyService _pluggyService;
        public WebhookPluggyController(Context context, IWebhookPluggyService webhookPluggyService, IPluggyService pluggyService)
        {
            this.context = context;
            _webhookPluggyService = webhookPluggyService;
            _pluggyService = pluggyService;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.WebHookPluggy.
                Select(
                m => new 
                {
                   m.IdWebHookPluggy,
                   m.Data,
                   m.ObjJson
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] LogWebhookPluggyPesquisarModel model)
        {
            var query = context.WebHookPluggy.AsQueryable();
             
            var result = query.Where(x => x.Data >= model.DataInicio && x.Data <= model.DataFim).Select(
                m => new 
                {
                   m.IdWebHookPluggy,
                   m.Data,
                   m.ObjJson
                }).Take(100).OrderByDescending(x => x.Data).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("criar-log-webhook-pluggy")]
        [AllowAnonymous]
        public async Task<IActionResult> CriarLogWebHookAsync([FromBody] object data)
        {
            await _webhookPluggyService.CriarLogWebHookAsync(new CriarLogWebHookPluggyModel()
            {
                ObjJson = data
            });

            var result = JsonSerializer.Deserialize<PluggyItemUpdatedEventDto>(
                    data.ToString(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    });

            if(result.Event== "item/updated")
            {
               await _pluggyService.UpdateItemAsync(result.ItemId);
            }


            if(result.Event== "item/created")
            {
                var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == Convert.ToInt32(result.ClientUserId));
                if (cliente == null)
                    throw new Exception("Cliente não encontrado");

                var contas = await _pluggyService.GetAccountsAsync(result.ItemId);

                List<ClienteContaBancaria> clienteContaBancarias = new List<ClienteContaBancaria>();
                foreach (var item in contas.Results)
                {
                    var clienteContaBancaria = new ClienteContaBancaria(cliente, null, null, item.Number, null, null, item.BankData?.TransferNumber, "admin", item.Id.ToString(), item.ItemId.ToString());
                    clienteContaBancaria.Nome = item.Name;
                    clienteContaBancaria.Tipo = item.Type;
                    clienteContaBancaria.SubTipo = item.Subtype;
                    clienteContaBancaria.SetSaldo(item.Balance, DateTime.Now, "admin");

                    clienteContaBancarias.Add(clienteContaBancaria);
                }

                context.ClienteContaBancaria.AddRange(clienteContaBancarias);
                await  context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
