using ERP.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;
using ERP.Models;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using ERP_API.Service.Parceiros;
using ERP_API.Service.Parceiros.Interface;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Identity.Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class TecnospeedController : ControllerBase
    {
        protected Context context;
        private readonly ITecnospeedService _tecnospeedService;
        public TecnospeedController(Context context, ITecnospeedService tecnospeedSerivce)
        {
            this.context = context;
            _tecnospeedService = tecnospeedSerivce;
        }

        [HttpGet]
        [Route("cadastrarPagador")]
        public async Task<IActionResult> CadastrarPagador()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);

            var parceiroParametro = context.ParceiroParametro.Include(x => x.ParceiroSistema).Where(x => x.ParceiroSistema.NomeParceiroSistema == "TecnoSpeed");

            var cnpjsh = parceiroParametro.FirstOrDefault(x => x.Chave == "cnpjsh").Valor ?? throw new Exception("Chave cnpjsh não encontrada");
            var tokensh = parceiroParametro.FirstOrDefault(x => x.Chave == "tokensh").Valor ?? throw new Exception("Chave tokensh não encontrada");
            var url = parceiroParametro.FirstOrDefault(x => x.Chave == "UrlBase").Valor ?? throw new Exception("Chave UrlBase não encontrada");

            var unidades = context.Unidade
                       .Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
                      .ToList();
            foreach (var unidade in unidades)
            {
               

                var cadastroPagador = new CadastroPagadorRequestModel
                {
                    name = unidade.Nome,
                    email = unidade.Email,
                    cpfCnpj = unidade.CpfCnpj,
                    neighborhood = unidade.Bairro,
                    street = unidade.Logradouro,
                    addressNumber = unidade.Numero,
                    addressComplement = unidade.Complemento,
                    ddaActivated = true, 
                    city = unidade.Cidade,
                    state = unidade.Estado,
                    zipcode = unidade.Cep, 
                };
                CadastroPagadorResponseModel responseCadastro;
                try
                {
                   responseCadastro = await _tecnospeedService.CadastroPagadorTecnospeed(cadastroPagador, cnpjsh, tokensh, url);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Erro ao cadastrar pagador: {ex.Message}");
                }
            }

            return Ok();

        }

        [HttpPost]
        [Route("ReceberExtrato")]
        [AllowAnonymous]
        public async Task<IActionResult> RecebeExtrato([FromBody] RecebeExtratoRedeWebhook model)
        {
            var extrato = context.Extrato.FirstOrDefault(x => x.UniqueId == model.UniqueId);
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == extrato.IdCliente);
           
            var parceiroParametro = context.ParceiroParametro.Include(x => x.ParceiroSistema).Where(x => x.ParceiroSistema.NomeParceiroSistema == "TecnoSpeed");
            var contaBancaria = context.ContaBancaria.FirstOrDefault(x => x.HashDaConta == model.AccountHash);

            var cnpjsh = parceiroParametro.FirstOrDefault(x => x.Chave == "cnpjsh").Valor ?? throw new Exception("Chave cnpjsh não encontrada");
            var tokensh = parceiroParametro.FirstOrDefault(x => x.Chave == "tokensh").Valor ?? throw new Exception("Chave tokensh não encontrada");
            var url = parceiroParametro.FirstOrDefault(x => x.Chave == "UrlBase").Valor ?? throw new Exception("Chave UrlBase não encontrada");

            var recebeExtratoWebhook = new LogWebhookExtratoTecnospeed(
               model.Data,
               model.Happen,
               model.Balance,
               model.UniqueId,
               model.CreatedAt,
               model.AccountHash,
               "admin");

            context.LogWebhookExtratoTecnopeed.Add(recebeExtratoWebhook);
            decimal.TryParse(model.Balance, out decimal balanceDecimal);
            contaBancaria.SetSaldo(balanceDecimal);
            context.SaveChanges();


            //var retornoContaBancaria = await _tecnospeedService.ConsultarExtratoTecnospeed(unidade, extrato, cnpjsh, tokensh, url);

            return Ok();

        }
    }
}

