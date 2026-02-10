using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using System;
using System.Linq; 
using ERP_API.Domain.Entidades;
using System.Data.Entity;
using System.Threading.Tasks; 
using ERP_API.Models.App;
using ERP_API.Service; 
using System.Data;
using ERP_API.Service.Parceiros.Interface;
using System.Net.Http;
using ERP_API.Service.Pluggy.Interface;
using ERP_API.Models.Pluggy;


namespace ERP.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PluggyController : ControllerBase
    {

        protected Context context;
        protected IConciliadoraService _conciliadoraService;
        private readonly HttpClient _httpClient;
        private IBlobStorageService blobStorageService;
        private readonly string _baseUrl = "https://api.conciliadora.com.br/api/EnvioVendaSistema";
        private readonly IPluggyService _pluggy;

        public PluggyController(Context context, IConciliadoraService conciliadoraService, HttpClient httpClient, IBlobStorageService blobStorageService, IPluggyService pluggy)
        {
            this.context = context;
            this._conciliadoraService = conciliadoraService;
            this._httpClient = httpClient;
            this.blobStorageService = blobStorageService;
            _pluggy = pluggy;
        }
         

        [HttpGet("token")]
        public async Task<IActionResult> GetConnectToken() // [FromQuery] string clienteUserId, [FromQuery] string itemId
        {
            var token = await _pluggy.CreateConnectTokenAsync(); // clienteUserId, itemId
            return Ok(new { connectToken = token  });
        }

        [HttpGet("accounts/{itemId}")]
        public async Task<IActionResult> GetAccounts(string itemId)
        {
            var result = await _pluggy.GetAccountsAsync(itemId);
            return Ok(result);
        }

        [HttpPost("CreateItemEmpresarial")]
        public async Task<IActionResult> CreateItemEmpresarialAsync(CreateItemEmpresarialPluggyRequestModel request)
        {
            var result = await _pluggy.CreateItemEmpresarialAsync(request);

            await Task.Delay(2000); // 3 segundos
            var item = await _pluggy.GetItemAsync(result.Id.ToString());
            return Ok(item);
        }

        [HttpGet("conectores")]
        public async Task<IActionResult> GetAllConnectors()
        {
            var result = await _pluggy.GetAllConnectors();
            return Ok(result);
        }

    }
}