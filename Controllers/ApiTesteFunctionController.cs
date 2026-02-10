using Azure;
using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ERP_API.Controllers.TesteApFunctionListaModel;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    public class ApiTesteFunctionController : ControllerBase
    {
        [HttpGet("processa")]
        [ProducesResponseType(typeof(Response<TesteApFunctionListaModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProcessaAsync()
        {
            var result = new TesteApFunctionListaModel
            {
                Lista = new List<TesteApFunctionModel>
                {
                    new TesteApFunctionModel
                    {
                        IdEmpresa = "1"
                    },
                     new TesteApFunctionModel
                    {
                        IdEmpresa = "2"
                    },
                    new TesteApFunctionModel
                    {
                        IdEmpresa = "3"
                    }
                }
            };
            return Ok(result);
        }
    }

    public class TesteApFunctionListaModel
    {
        public List<TesteApFunctionModel> Lista { get; set; }
    }
    public class TesteApFunctionModel
    {
        public string IdEmpresa { get; set; }
    }
}
