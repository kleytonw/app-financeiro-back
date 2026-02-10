using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;
using ERP.Models;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using System.Data.Entity;

namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RegiaoConsultorController : ControllerBase
    {
        protected Context context;
        public RegiaoConsultorController(Context context)
        {
            this.context = context;
        }



        [HttpPost]
        [Route("salvarRegiaoProduto")]
        public IActionResult SalvarSetorProduto([FromBody] RegiaoRequest model)
        {

            var consultor = context.Consultor.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
            var regiao = context.Regiao.FirstOrDefault(x => x.IdRegiao == model.IdRegiao);



            var regiaoConsultor = new RegiaoConsultor(regiao, consultor, User.Identity.Name);
            context.RegiaoConsultor.Add(regiaoConsultor);

            context.SaveChanges();
            return Ok();
        }
    }
}
