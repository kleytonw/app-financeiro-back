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
using Microsoft.EntityFrameworkCore;
using ERP_API.Models;
using ERP_API.Domain.Entidades;
using System.Reflection.PortableExecutable;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Threading.Tasks;
using ERP_API.Service.Parceiros;
using ERP_API.Service.Parceiros.Interface;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class MinhasContasController : ControllerBase
    {
        protected Context context;
        private readonly ITecnospeedService _tecnospeedService;

        public MinhasContasController(Context context, ITecnospeedService tecnospeedSerivce)
        {
            this.context = context;
            this._tecnospeedService = tecnospeedSerivce;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var result = context.ContaBancaria
                .Select(m => new
                {
                    m.IdContaBancaria,
                    m.Banco.NomeBanco,
                    m.Agencia,
                    m.DigitoAgencia,
                    m.Conta,
                    m.Saldo,
                    m.DataDoSaldo,
                    m.IdUnidade,
                    m.DigitoConta,
                    m.CodigoSistema,
                    m.HashDaConta,
                    m.Unidade.Nome,
                    m.Operadora.NomeOperadora,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }
        

    }
}
