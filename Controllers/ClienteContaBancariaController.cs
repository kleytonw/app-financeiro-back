using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using ERP_API.Service.Pluggy.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ClienteContaBancariaController : ControllerBase
    {
        public Context context { get; set; }
        public IPluggyService _pluggyService { get; set; }

        public ClienteContaBancariaController(Context context, IPluggyService pluggyService)
        {
            this.context = context;
            _pluggyService = pluggyService;
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar()
        {
            var result = context.ClienteContaBancaria
                .Include(x => x.Cliente)
                .Include(x => x.Banco)
                .Select(m => new
                {
                    m.IdClienteContaBancaria,
                    m.IdCliente,
                    NomeCliente = m.Cliente.Pessoa.Nome,
                    m.IdBanco,
                    m.Banco.NomeBanco,
                    m.Agencia,
                    m.Conta,
                    m.DigitoConta,
                    m.DigitoAgencia,
                    m.Descricao,
                    m.Saldo,
                    m.DataDoSaldo,
                    m.Situacao,
                    m.AccountIdOpenFinance
                }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("listarPorCliente")]
        [Authorize]
        public IActionResult ListarPorCliente(int idCliente)
        {
            
            if(idCliente == 0)
                return BadRequest("O id do cliente é obrigatório");

            var result = context.ClienteContaBancaria
              .Include(x => x.Cliente)
              .Include(x => x.Banco)
              .Where(x => x.IdCliente == idCliente && x.Situacao == "Ativo")
              .Select(m => new
              {
                  m.IdClienteContaBancaria,
                  NomeCliente = m.Cliente.Pessoa.Nome,
                  m.Banco.NomeBanco,
                  m.Agencia,
                  m.DigitoAgencia,
                  m.DigitoConta,
                  m.Tipo,
                  m.SubTipo,
                  m.Conta,
                  m.UrlAtivacaoConta
              }).ToList();

            return Ok(result);
        }
        
        [HttpGet]
        [Route("pesquisar")]
        [Authorize]
        public IActionResult Pesquisar(int idCliente)
        {
           
                var result = context.ClienteContaBancaria.Include(x=>x.Cliente.Pessoa).Where(x => x.IdCliente == idCliente && x.Situacao == "Ativo")
                    .Select(m => new
                    {
                        m.IdClienteContaBancaria,
                        m.IdCliente,
                        NomeCliente = m.Cliente.Pessoa.Nome,
                        m.IdBanco,
                        NomeBanco = m.Nome,
                        m.Agencia,
                        m.Conta,
                        m.DigitoConta,
                        m.DigitoAgencia,
                        m.Descricao,
                        m.Saldo,
                        m.DataDoSaldo,
                        m.Situacao,
                        m.Tipo,
                        m.SubTipo,
                        m.UrlAtivacaoConta
                    }).ToList();

                return Ok(result); 
        }

        [HttpGet]
        [Route("listarContaBancariasAdmninistrador")]
        [Authorize]
        public IActionResult ListarContaBancariaAdministrador()
        {

            var result = context.ClienteContaBancaria.Include(x => x.Cliente.Pessoa).Where(x => x.Situacao == "Ativo")
                .Select(m => new
                {
                    m.IdClienteContaBancaria,
                    m.IdCliente,
                    NomeCliente = m.Cliente.Pessoa.Nome,
                    m.IdBanco,
                    NomeBanco = m.Nome,
                    m.Agencia,
                    m.Conta,
                    m.DigitoConta,
                    m.DigitoAgencia,
                    m.Descricao,
                    m.Saldo,
                    m.DataDoSaldo,
                    m.Situacao,
                    m.Tipo,
                    m.SubTipo,
                    m.UrlAtivacaoConta
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public async Task<IActionResult> Salvar([FromBody] ClienteContaBancariaRequest model)
        {
            Cliente cliente;
            Banco banco;

            if(model.IdClienteContaBancaria > 0)
            {
                var clienteContaBancaria = context.ClienteContaBancaria.FirstOrDefault(x => x.IdClienteContaBancaria == model.IdClienteContaBancaria);

                cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
                banco = context.Banco.FirstOrDefault(x => x.IdBanco ==  model.IdBanco);

                clienteContaBancaria.Alterar(cliente, banco, model.Agencia, model.Conta, model.DigitoConta, model.DigitoAgencia, model.Descricao, User.Identity.Name, model.AccountIdOpenFinance, model.ItemIdOpenFinance);
                context.ClienteContaBancaria.Update(clienteContaBancaria);
            }
            else
            {
                cliente = context.Cliente.Include(x=>x.Pessoa).FirstOrDefault(x => x.IdPessoa == model.IdCliente);
                banco = context.Banco.FirstOrDefault(x => x.IdBanco == model.IdBanco);

                var clienteContaBancaria = new ClienteContaBancaria(cliente, banco, model.Agencia, model.Conta, model.DigitoConta, model.DigitoAgencia, model.Descricao, User.Identity.Name, model.AccountIdOpenFinance, model.ItemIdOpenFinance);

                var pluggyItem = await _pluggyService.CreateItemEmpresarialAsync(new Models.Pluggy.CreateItemEmpresarialPluggyRequestModel()
                {
                    clientUserId = clienteContaBancaria.IdentificadorConta.ToString(),
                    connectorId = (int)banco.IdOpenFinance,
                    parameters = new Models.Pluggy.PluggyParametersEmpresarial
                    {
                        cpf = model.CPF,
                        cnpj = model.CNPJ,
                    }
                });

                var consultaSpecificItem = await _pluggyService.GetItemAsync(pluggyItem.Id.ToString());
                clienteContaBancaria.ItemIdOpenFinance = pluggyItem.Id.ToString();
                clienteContaBancaria.UrlAtivacaoConta = consultaSpecificItem?.Parameter?.Data;
                context.ClienteContaBancaria.Add(clienteContaBancaria);
            }
            context.SaveChanges();
            
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var clienteContaBancaria = context.ClienteContaBancaria.FirstOrDefault(x => x.IdClienteContaBancaria == id);
            if (clienteContaBancaria == null)
                return BadRequest("Cliente não encontrado");

            clienteContaBancaria.Excluir(User.Identity.Name);
            context.ClienteContaBancaria.Update(clienteContaBancaria);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {

            var clienteContaBancaria = context.ClienteContaBancaria.FirstOrDefault(x => x.IdClienteContaBancaria == id);
            if (clienteContaBancaria == null)
                return BadRequest("Cliente não encontrado");

            return Ok(new ClienteContaBancariaResponse()
            {
                IdClienteContaBancaria = clienteContaBancaria.IdClienteContaBancaria,
                IdCliente = clienteContaBancaria.IdCliente,
                IdBanco = clienteContaBancaria.IdBanco,
                Agencia = clienteContaBancaria.Agencia,
                Conta = clienteContaBancaria.Conta,
                DigitoConta = clienteContaBancaria.DigitoConta,
                DigitoAgencia = clienteContaBancaria.DigitoAgencia,
                Descricao = clienteContaBancaria.Descricao,
                Saldo = clienteContaBancaria.Saldo,
                DataDoSaldo = clienteContaBancaria.DataDoSaldo,
                Situacao = clienteContaBancaria.Situacao,
                AccountIdOpenFinance = clienteContaBancaria.AccountIdOpenFinance,
                ItemIdOpenFinance = clienteContaBancaria.ItemIdOpenFinance

            });
        }

    }
}
