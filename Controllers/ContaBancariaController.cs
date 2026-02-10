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
    public class ContaBancariaController : ControllerBase
    {
        protected Context context;
        private readonly ITecnospeedService _tecnospeedService;

        public ContaBancariaController(Context context, ITecnospeedService tecnospeedSerivce)
        {
            this.context = context;
            this._tecnospeedService = tecnospeedSerivce;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            if (usuarioLogado.TipoUsuario == "Administrador" || usuarioLogado.TipoUsuario == "ERP")
            {

                var result = context.ContaBancaria
                    .Select(m => new
                    {
                        m.IdContaBancaria,
                        m.Banco.NomeBanco,
                        m.Agencia,
                        m.DigitoAgencia,
                        m.Conta,
                        m.DigitoConta,
                        m.Saldo,
                        m.DataDoSaldo,
                        m.IdUnidade,
                        m.CodigoSistema,
                        m.HashDaConta,
                        m.Unidade.Nome,
                        m.Operadora.NomeOperadora,
                        m.Situacao
                    }).ToList();

                return Ok(result);
            }

            else
            {
                var result = context.ContaBancaria.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
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

        [HttpGet]
        [Route("listarPorUnidadeEEmpresa")]
        public IActionResult ListarPorUnidadeEEmpresa(int idEmpresa, int idUnidade)
        {

                var result = context.ContaBancaria.Where(x => x.IdEmpresa == idEmpresa && x.IdUnidade == idUnidade)
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

        [HttpGet]
        [Route("pesquisar")]
        public IActionResult Pesquisar(int idEmpresa, int idUnidade)
        {
            var query = context.ContaBancaria.Where(x => x.IdEmpresa == idEmpresa && x.IdUnidade == idUnidade).Select( m => new
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

            return Ok(query);
        }



        [HttpPost]
        [Route("salvar")]
        public async Task<IActionResult> Salvar([FromBody] ContaBancariaRequest model)
        {
            var parceiroParametro = context.ParceiroParametro.Include(x => x.ParceiroSistema).Where(x => x.ParceiroSistema.NomeParceiroSistema == "TecnoSpeed");

            var cnpjsh = parceiroParametro.FirstOrDefault(x => x.Chave == "cnpjsh").Valor ?? throw new Exception("Chave cnpjsh não encontrada");
            var tokensh = parceiroParametro.FirstOrDefault(x => x.Chave == "tokensh").Valor ?? throw new Exception("Chave tokensh não encontrada");
            var url = parceiroParametro.FirstOrDefault(x => x.Chave == "UrlBase").Valor ?? throw new Exception("Chave UrlBase não encontrada");

            Empresa empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);

            Unidade unidade = context.Unidade.FirstOrDefault(x => x.IdUnidade == model.IdUnidade);
            Operadora operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == model.IdOperadora);
            Banco banco = context.Banco.FirstOrDefault(x => x.IdBanco == model.IdBanco);
            ContaBancaria contaBancaria;
            if (model.IdContaBancaria > 0)
            {
                contaBancaria = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == model.IdContaBancaria);
                contaBancaria.Alterar(banco, model.Agencia, model.DigitoAgencia, model.Conta, model.DigitoConta, model.CodigoSistema, unidade, empresa, operadora, User.Identity.Name);

                 AtualizarContaResponseModel retorno;
                 try
                 {
                      retorno = await AtualizarConta(contaBancaria, unidade, cnpjsh, tokensh, url);
                 }
                 catch (Exception ex)
                 {
                     return BadRequest($"Erro ao atualizar conta: {ex.Message}");
                 }
                context.Update(contaBancaria);
            }
            else
            {
                contaBancaria = new ContaBancaria(
                    banco,
                    model.Agencia,
                    model.DigitoAgencia,
                    model.Conta,
                    model.DigitoConta,
                    model.CodigoSistema,
                    unidade,
                    empresa,
                    operadora,
                    User.Identity.Name
                );

                CadastrarContaListResponseModel retorno;
                try
                {
                    if (contaBancaria.Banco.PossuiIntegracaoTecnospeed)
                    {
                        retorno = await CadastrarConta(contaBancaria, unidade, cnpjsh, tokensh, url);
                        contaBancaria.SetHashConta(retorno.accounts.First().AccountHash);
                    }
                } catch (Exception ex) 
                {
                    return BadRequest($"Erro ao cadastrar conta: {ex.Message}");
                }

               
                context.ContaBancaria.Add(contaBancaria);
            }

            context.SaveChanges();
            return Ok();
        }

        private async Task<CadastrarContaListResponseModel> CadastrarConta(ContaBancaria contaBancaria, Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var retorno = new CadastrarContaListResponseModel();
            try
            {
                var request = new CadastrarContaRequestListModel();
                request.Accounts = new List<CadastrarContaRequestModel>();
                request.Accounts.Add(new CadastrarContaRequestModel()
                {
                    bankCode = contaBancaria.Banco.CodigoBancoTecnoSpeed,
                    agency = contaBancaria.Agencia,
                    agencyDigit = contaBancaria.DigitoAgencia,
                    accountNumber = contaBancaria.Conta,
                    accountNumberDigit = contaBancaria.DigitoConta,
                    accountDac = contaBancaria.DigitoConta,
                    ddaActivated = true
                });

                    retorno = await _tecnospeedService.CadastrarContaTecnospeed(request, unidade, cnpjsh, tokensh, url);
                 
                return retorno;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<AtualizarContaResponseModel> AtualizarConta(ContaBancaria contaBancaria, Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var retorno = new AtualizarContaResponseModel();
            try
            {
                var request = new AtualizarContaRequestModel()
                {
                    BankCode = contaBancaria.Banco.CodigoBancoTecnoSpeed,
                    Agency = contaBancaria.Agencia,
                    AgencyDigit = contaBancaria.DigitoAgencia,
                    AccountNumber = contaBancaria.Conta,
                    AccountNumberDigit = contaBancaria.DigitoConta,
                    AccountDac = contaBancaria.DigitoConta
                };

                retorno = await _tecnospeedService.AtualizarContaTecnospeed(request, unidade, contaBancaria.HashDaConta, cnpjsh, tokensh, url);
                return retorno;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public async Task<IActionResult> Excluir(int id)
        {
            var parceiroParametro = context.ParceiroParametro.Include(x => x.ParceiroSistema).Where(x => x.ParceiroSistema.NomeParceiroSistema == "TecnoSpeed");

            var cnpjsh = parceiroParametro.FirstOrDefault(x => x.Chave == "cnpjsh").Valor ?? throw new Exception("Chave cnpjsh não encontrada");
            var tokensh = parceiroParametro.FirstOrDefault(x => x.Chave == "tokensh").Valor ?? throw new Exception("Chave tokensh não encontrada");
            var url = parceiroParametro.FirstOrDefault(x => x.Chave == "UrlBase").Valor ?? throw new Exception("Chave UrlBase não encontrada");

            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            Empresa empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == usuarioLogado.IdEmpresa);

            var contaBancaria = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == id);
            var unidade = context.Unidade.First(x => x.IdUnidade == contaBancaria.IdUnidade);
            var retorno = await DeletarConta(contaBancaria, unidade, cnpjsh, tokensh, url);
            contaBancaria.Excluir(User.Identity.Name);

            context.Update(contaBancaria);
            context.SaveChanges();
            return Ok();
        }

        private async Task<DeletarContaResponseModel> DeletarConta(ContaBancaria contaBancaria, Unidade unidade, string cnpjsh, string tokensh, string url)
        {
            var retorno = new DeletarContaResponseModel();
            try
            {
                var request = new DeletarContaRequestModel();
                if (!string.IsNullOrEmpty(contaBancaria.HashDaConta))
                {
                    request.AccountHash.Add(contaBancaria.HashDaConta);
                }
                else
                {
                    throw new ArgumentException("HashDaConta não pode ser nulo ou vazio.");
                }

                retorno = await _tecnospeedService.DeletarContaTecnospeed(request, unidade, cnpjsh, tokensh, url);
                return retorno;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("obterPorUnidade")]
        public IActionResult ObterPorUnidade([FromQuery] int idUnidade, [FromQuery] int idOperadora)
        {
            var contratoOperadora = context.ContratoOperadora.FirstOrDefault(x => x.IdUnidade == idUnidade && x.IdOperadora == idOperadora && x.Situacao == "Ativo");
            if(contratoOperadora == null)
                return BadRequest("Contrato de Operadora não cadastrado");
            var contaRecebimento = context.ContaBancaria.Include(x => x.Banco).FirstOrDefault(x => x.IdContaBancaria == contratoOperadora.IdContaRecebimento && x.Situacao == "Ativo");
            var contaGravame = context.ContaBancaria.Include(x => x.Banco).FirstOrDefault(x => x.IdContaBancaria == contratoOperadora.IdContaGravame && x.Situacao == "Ativo");
            if (contaRecebimento == null)
                return BadRequest("Conta Bancaria de Recebimento não encontrado ");

            return Ok(new
            {
                ContaRecebimento = contaRecebimento != null ? new ContaBancariaResponse
                {
                    IdContaBancaria = contaRecebimento.IdContaBancaria,
                    NomeBanco = contaRecebimento.Banco.NomeBanco,
                    IdBanco = contaRecebimento.IdBanco,
                    Agencia = contaRecebimento.Agencia,
                    DigitoAgencia = contaRecebimento.DigitoAgencia,
                    Conta = contaRecebimento.Conta,
                    DigitoConta = contaRecebimento.DigitoConta,
                    IdUnidade = contaRecebimento.IdUnidade,
                    CodigoSistema = contaRecebimento.CodigoSistema,
                    IdEmpresa = contaRecebimento.IdEmpresa,
                    IdOperadora = contaRecebimento.IdOperadora,
                    Situacao = contaRecebimento.Situacao
                } : null,

                ContaGravame = contaGravame != null ? new ContaBancariaResponse
                {
                    IdContaBancaria = contaGravame.IdContaBancaria,
                    NomeBanco = contaGravame.Banco.NomeBanco,
                    IdBanco = contaGravame.IdBanco,
                    Agencia = contaGravame.Agencia,
                    DigitoAgencia = contaGravame.DigitoAgencia,
                    Conta = contaGravame.Conta,
                    DigitoConta = contaGravame.DigitoConta,
                    IdUnidade = contaGravame.IdUnidade,
                    CodigoSistema = contaGravame.CodigoSistema,
                    IdEmpresa = contaGravame.IdEmpresa,
                    IdOperadora = contaGravame.IdOperadora,
                    Situacao = contaGravame.Situacao
                } : null
            });

        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var contaBancaria = context.ContaBancaria.FirstOrDefault(x => x.IdContaBancaria == id);
            if (contaBancaria == null)
                return BadRequest("Conta Bancaria não encontrado ");

            return Ok(new ContaBancariaResponse()
            {
                IdContaBancaria = contaBancaria.IdContaBancaria,
                IdBanco = contaBancaria.IdBanco,
                Agencia = contaBancaria.Agencia,
                DigitoAgencia = contaBancaria.DigitoAgencia,
                Conta = contaBancaria.Conta,
                DigitoConta = contaBancaria.DigitoConta,
                IdUnidade = contaBancaria.IdUnidade,
                CodigoSistema = contaBancaria.CodigoSistema,
                IdEmpresa = contaBancaria.IdEmpresa,
                IdOperadora = contaBancaria.IdOperadora,
                Situacao = contaBancaria.Situacao
            });
        }
    }
}
