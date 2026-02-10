using ERP.Infra;
using ERP_API.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ERP_API.Models;
using System.Threading.Tasks;
using ERP_API.Service.Parceiros;
using System;
using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entidades;
using ERP_API.Service.Parceiros.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]" )]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ParceiroSistemaController : ControllerBase
    {
        protected Context context;
        private readonly ITecnospeedService _tecnospeedService;
        public ParceiroSistemaController(Context context, ITecnospeedService tecnospeedService)
        {
            this.context = context;
            _tecnospeedService = tecnospeedService;
        }

        [HttpGet]
        [Route("listar")]
        public ActionResult Listar()
        {
            var result = context.ParceiroSistema
                .Select(m => new
                {
                    m.IdParceiroSistema,
                    m.NomeParceiroSistema,
                    m.Observacao,
                    m.Situacao,
                }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ParceiroSistemaRequest model)
        {
            ParceiroSistema parceiroSistema;
            if(model.IdParceiroSistema > 0)
            {
               parceiroSistema = context.ParceiroSistema.FirstOrDefault(x => x.IdParceiroSistema == model.IdParceiroSistema);
               parceiroSistema.Alterar(model.NomeParceiroSistema, model.Observacao, User.Identity.Name);

                context.Update(parceiroSistema);

            } else 
            {
               parceiroSistema = new ParceiroSistema(model.NomeParceiroSistema, model.Observacao, User.Identity.Name);
                context.ParceiroSistema.Add(parceiroSistema);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var parceiroSistema = context.ParceiroSistema.FirstOrDefault(x => x.IdParceiroSistema == id);
            parceiroSistema.Excluir(User.Identity.Name);

            context.Update(parceiroSistema);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var parceiroSistema = context.ParceiroSistema.FirstOrDefault(x => x.IdParceiroSistema ==id);
            if (parceiroSistema == null)
                return BadRequest("Parceiro do sistema não encontrado");

            return Ok(new ParceiroSistemaResponse()
            {
                IdParceiroSistema = parceiroSistema.IdParceiroSistema,
                NomeParceiroSistema = parceiroSistema.NomeParceiroSistema,
                Observacao = parceiroSistema.Observacao,
                Situacao = parceiroSistema.Situacao,
            });
        }

        [HttpGet]
        [Route("criarNotificacao")]
        [Authorize]

        public async Task<IActionResult> CriarNotificacaoTecnospeed(int idUnidade)
        {

            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == idUnidade);

            var parceiroParametro = context.ParceiroParametro.Include(x => x.ParceiroSistema).Where(x => x.ParceiroSistema.NomeParceiroSistema == "TecnoSpeed");
            var parceiroSistema = context.ParceiroSistema.FirstOrDefault(x => x.NomeParceiroSistema == "TecnoSpeed");

            var cnpjsh = parceiroParametro.FirstOrDefault(x => x.Chave == "cnpjsh").Valor ?? throw new Exception("Chave cnpjsh não encontrada");
            var tokensh = parceiroParametro.FirstOrDefault(x => x.Chave == "tokensh").Valor ?? throw new Exception("Chave tokensh não encontrada");
            var url = parceiroParametro.FirstOrDefault(x => x.Chave == "UrlBase").Valor ?? throw new Exception("Chave UrlBase não encontrada");
            var emailsh = parceiroParametro.FirstOrDefault(x => x.Chave == "Emailsh").Valor ?? throw new Exception("Chave Emailsh não encontrada");
            var copiaEmailsh = parceiroParametro.FirstOrDefault(x => x.Chave == "CopiaEmailsh").Valor ?? throw new Exception("Chave CopiaEmailsh não encontrada");
            var urlWebhook = parceiroParametro.FirstOrDefault(x => x.Chave == "UrlWebhook").Valor ?? throw new Exception("Chave CopiaEmailsh não encontrada");
            var mobilesh = parceiroParametro.FirstOrDefault(x => x.Chave == "Mobilesh").Valor ?? throw new Exception("Chave Mobilesh não encontrada");
            var retorno = new CriarNotificacaoTecnospeedResponseModel();
            try
            {
                var request = new CriarNotificacaoTecnospeedRequestModel
                {
                    Type = "webhook",
                    Email = emailsh,
                    Cc = copiaEmailsh,
                    Url = urlWebhook,
                    Mobile = mobilesh,
                    Happen = new List<string>
                    {
                        "STATEMENT"
                    }

                };

                retorno = await _tecnospeedService.CriarNotificacaoTecnospeed(request, unidade, cnpjsh, tokensh, url);
                unidade.SetUniqueId(retorno.UniqueId);
                context.Unidade.Update(unidade);
                context.SaveChanges();

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        [Route("recebeExtratoRede")]
        [AllowAnonymous]
        public async Task<IActionResult> RecebeExtratoRede([FromBody] RecebeExtratoRedeWebhook model)
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
               User?.Identity?.Name);

            context.LogWebhookExtratoTecnopeed.Add(recebeExtratoWebhook);
            decimal.TryParse(model.Balance, out decimal balanceDecimal);
            contaBancaria.SetSaldo(balanceDecimal);
            await context.SaveChangesAsync();

           // var retornoContaBancaria = await _tecnospeedService.ConsultarExtratoTecnospeed(unidade, extrato, cnpjsh, tokensh, url);
            await context.SaveChangesAsync();

            return Ok();

        }

        [HttpGet]
        [Route("listarNotificacao")]
        [Authorize]

        public async Task<IActionResult> ListarNotificacaoTecnospeed(int idUnidade)
        {

            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == idUnidade);

            var parceiroParametro = context.ParceiroParametro.Include(x => x.ParceiroSistema).Where(x => x.ParceiroSistema.NomeParceiroSistema == "TecnoSpeed");
            var parceiroSistema = context.ParceiroSistema.FirstOrDefault(x => x.NomeParceiroSistema == "TecnoSpeed");

            var cnpjsh = parceiroParametro.FirstOrDefault(x => x.Chave == "cnpjsh").Valor ?? throw new Exception("Chave cnpjsh não encontrada");
            var tokensh = parceiroParametro.FirstOrDefault(x => x.Chave == "tokensh").Valor ?? throw new Exception("Chave tokensh não encontrada");
            var url = parceiroParametro.FirstOrDefault(x => x.Chave == "UrlBase").Valor ?? throw new Exception("Chave UrlBase não encontrada");
            var retorno = new ListarNotificacaoTecnospeedResponseModel();
            try
            { 
                retorno = await _tecnospeedService.ListarNotificacaoTecnospeed(unidade, cnpjsh, tokensh, url);
                retorno.IdUnidade = idUnidade;
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet]
        [Route("deletarNotificacao")]
        [Authorize]

        public async Task<IActionResult> DeletarNotificacaoTecnospeed(int idUnidade)
        {

            var unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == idUnidade);

            var parceiroParametro = context.ParceiroParametro.Include(x => x.ParceiroSistema).Where(x => x.ParceiroSistema.NomeParceiroSistema == "TecnoSpeed");
            var parceiroSistema = context.ParceiroSistema.FirstOrDefault(x => x.NomeParceiroSistema == "TecnoSpeed");

            var cnpjsh = parceiroParametro.FirstOrDefault(x => x.Chave == "cnpjsh").Valor ?? throw new Exception("Chave cnpjsh não encontrada");
            var tokensh = parceiroParametro.FirstOrDefault(x => x.Chave == "tokensh").Valor ?? throw new Exception("Chave tokensh não encontrada");
            var url = parceiroParametro.FirstOrDefault(x => x.Chave == "UrlBase").Valor ?? throw new Exception("Chave UrlBase não encontrada");
            var retorno = new DeletarNotificacaoTecnospeedResponseModel();
            try
            {
                retorno = await _tecnospeedService.DeletarNotificacaoTecnospeed(unidade, cnpjsh, tokensh, url);

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        [Route("bilhetagem")]
        [Authorize]

        public async Task<IActionResult> Bilhetagem([FromBody] BilhetagemTecnospeedRequestModel model)
        {
            var parceiroParametro = context.ParceiroParametro.Include(x => x.ParceiroSistema).Where(x => x.ParceiroSistema.NomeParceiroSistema == "TecnoSpeed");

            var cnpjsh = parceiroParametro.FirstOrDefault(x => x.Chave == "cnpjsh").Valor ?? throw new Exception("Chave cnpjsh não encontrada");
            var tokensh = parceiroParametro.FirstOrDefault(x => x.Chave == "tokensh").Valor ?? throw new Exception("Chave tokensh não encontrada");
            var url = parceiroParametro.FirstOrDefault(x => x.Chave == "UrlBase").Valor ?? throw new Exception("Chave UrlBase não encontrada");

            var retorno = new BilhetagemTecnospeedResponseModel();
            try
            {
                var request = new BilhetagemTecnospeedRequestModel
                {
                    DataInicial = model.DataInicial,
                    DataFinal = model.DataFinal,
                };

                retorno = await _tecnospeedService.BilhetagemTecnospeed(request, cnpjsh, tokensh, url);
                return Ok(retorno);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        [HttpGet]
        [Route("bilhetagem-mock")]
        public IActionResult SimularBilhetagem()
        {
            var mockResponse = new BilhetagemTecnospeedResponseModel
            {
                Status = "Sucesso",
                Message = "Bilhetagem simulada com sucesso.",
                TotalPayments = 5,
                TotalVan = 2,
                Payers = new List<Payer>
        {
            new Payer
            {
                CpfCnpj = "12345678900",
                QtdePayments = 3,
                QtdeVan = 1,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        UniqueId = "ABC123",
                        AccountHash = "HASH1",
                        CreatedAt = DateTime.Now.AddMinutes(-30)
                    },
                    new Payment
                    {
                        UniqueId = "DEF456",
                        AccountHash = "HASH2",
                        CreatedAt = DateTime.Now.AddMinutes(-15)
                    },
                    new Payment
                    {
                        UniqueId = "GHI789",
                        AccountHash = "HASH3",
                        CreatedAt = DateTime.Now
                    }
                }
            },
            new Payer
            {
                CpfCnpj = "98765432100",
                QtdePayments = 2,
                QtdeVan = 1,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        UniqueId = "JKL012",
                        AccountHash = "HASH4",
                        CreatedAt = DateTime.Now.AddMinutes(-45)
                    },
                    new Payment
                    {
                        UniqueId = "MNO345",
                        AccountHash = "HASH5",
                        CreatedAt = DateTime.Now.AddMinutes(-20)
                    }
                }
            }
        }
            };

            return Ok(mockResponse);
        }



    }
}
