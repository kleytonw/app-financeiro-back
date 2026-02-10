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
    public class SetorProdutoController : ControllerBase
    {
        protected Context context;
        public SetorProdutoController(Context context)
        {
            this.context = context;
        }



        [HttpPost]
        [Route("salvarSetorProduto")]
        public IActionResult SalvarSetorProduto([FromBody] SetorRequest model)
        {

            var produto = context.Produto.FirstOrDefault(x => x.IdProduto == model.IdProduto);
            var setor = context.Setor.FirstOrDefault(x => x.IdSetor == model.IdSetor);



            var setorProduto = new SetorProduto(setor, produto, User.Identity.Name);
            context.SetorProduto.Add(setorProduto);

            context.SaveChanges();
            return Ok();
        }
    }
}
