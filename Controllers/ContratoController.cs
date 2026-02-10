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
using ERP_API.Service.Parceiros;
using ERP_API.Service.Parceiros.Interface;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MySqlX.XDevAPI;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.CodeDom;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ContratoController : ControllerBase
    {
        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        private IConfiguration _config;
        protected Context context;
        protected IUniqueService _uniqueService;

        public ContratoController(Context context, IUniqueService uniqueService, IConfiguration config)
        {
            _config = config;
            this.context = context;
            _uniqueService = uniqueService;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Contrato.Include(x => x.Cliente).Include(x => x.Plano)
                .Select(m => new {
                    m.IdContrato,
                    m.IdCliente,
                    m.IdVendedor,
                    m.Cliente.Nome,
                    m.DataInicio,
                    m.DataTermino,
                    m.ValorMensalidade,
                    m.LinkContrato,
                    m.Descricao,
                    NomePlano  = m.Plano.Nome,
                    m.ValorAdesao,
                    m.DataAdesao,
                    m.ContratoAdesao,
                    m.ValorTotal,
                    m.ResponsavelNome,
                    m.ResponsavelCpf,
                    m.ResponsavelCargo,
                    m.ResponsavelEmail,
                    m.ResponsavelTelefone,
                    m.ResponsavelCelular,
                    m.Situacao
                }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("listarCliente")]
        public IActionResult ListarCliente()
        {
            var result = context.Cliente
                .Select(m => new {
                    m.IdPessoa,
                    m.Pessoa.Nome
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Nome))
            {
                var result = context.Contrato
               .Include(x => x.Cliente)
               .Where(m => m.DataInicio.Date >= model.DataInicio.Date && m.DataTermino.Date <= model.DataTermino.Date)
               .Select(m => new
               {
                   m.IdContrato,
                   m.IdCliente,
                   m.IdVendedor,
                   m.Cliente.Nome,
                   m.DataInicio,
                   m.DataTermino,
                   m.ValorMensalidade,
                   m.LinkContrato,
                   m.Descricao,
                   NomePlano = m.Plano.Nome,
                   m.ValorAdesao,
                   m.DataAdesao,
                   m.ContratoAdesao,
                   m.ValorTotal,
                   m.ResponsavelNome,
                   m.ResponsavelCpf,
                   m.ResponsavelCargo,
                   m.ResponsavelEmail,
                   m.ResponsavelTelefone,
                   m.ResponsavelCelular,
                   m.Situacao
               }).ToList();

                return Ok(result);

            }
            else if (model.DataInicio == DateTime.MinValue)
            {
                var result = context.Contrato
               .Include(x => x.Cliente)
               .Where(m => m.Cliente.Nome.Contains(model.Nome) && m.DataTermino.Date <= model.DataTermino.Date)
               .Select(m => new
               {
                   m.IdContrato,
                   m.IdCliente,
                   m.IdVendedor,
                   m.Cliente.Nome,
                   m.DataInicio,
                   m.DataTermino,
                   m.ValorMensalidade,
                   m.LinkContrato,
                   m.Descricao,
                   NomePlano = m.Plano.Nome,
                   m.ValorAdesao,
                   m.DataAdesao,
                   m.ContratoAdesao,
                   m.ValorTotal,
                   m.ResponsavelNome,
                   m.ResponsavelCpf,
                   m.ResponsavelCargo,
                   m.ResponsavelEmail,
                   m.ResponsavelTelefone,
                   m.ResponsavelCelular,
                   m.Situacao
               }).ToList();

                return Ok(result);

            }
            else if (model.DataTermino == DateTime.MinValue)
            {
                var result = context.Contrato
                .Include(x => x.Cliente)
                .Where(m => m.Cliente.Nome.Contains(model.Nome) && m.DataInicio.Date >= model.DataInicio.Date)
                .Select(m => new
                {
                    m.IdContrato,
                    m.IdCliente,
                    m.IdVendedor,
                    m.Cliente.Nome,
                    m.DataInicio,
                    m.DataTermino,
                    m.ValorMensalidade,
                    m.LinkContrato,
                    m.Descricao,
                    NomePlano = m.Plano.Nome,
                    m.ValorAdesao,
                    m.DataAdesao,
                    m.ContratoAdesao,
                    m.ValorTotal,
                    m.ResponsavelNome,
                    m.ResponsavelCpf,
                    m.ResponsavelCargo,
                    m.ResponsavelEmail,
                    m.ResponsavelTelefone,
                    m.ResponsavelCelular,
                    m.Situacao
                }).ToList();

                return Ok(result);

            }
            else
            {

                var result = context.Contrato
                    .Include(x => x.Cliente)
                    .Where(m => m.Cliente.Nome.Contains(model.Nome) && m.DataInicio.Date >= model.DataInicio.Date && m.DataTermino.Date <= model.DataTermino.Date)
                    .Select(m => new
                    {
                        m.IdContrato,
                        m.IdCliente,
                        m.IdVendedor,
                        m.Cliente.Nome,
                        m.DataInicio,
                        m.DataTermino,
                        m.ValorMensalidade,
                        m.LinkContrato,
                        m.Descricao,
                        NomePlano = m.Plano.Nome,
                        m.ValorAdesao,
                        m.DataAdesao,
                        m.ContratoAdesao,
                        m.ValorTotal,
                        m.ResponsavelNome,
                        m.ResponsavelCpf,
                        m.ResponsavelCargo,
                        m.ResponsavelEmail,
                        m.ResponsavelTelefone,
                        m.ResponsavelCelular,
                        m.Situacao
                    }).ToList();

                return Ok(result);
            }

        }

        [HttpGet]
        [Route("contabilizar")]
        public IActionResult Contabilizar()
        {
            var dataHoje = DateTime.Now.Date;

            var quantidadeAtivos = context.Contrato
                .Where(c => c.DataTermino >= dataHoje)
                .Count(); 

            var quantidadeInativos = context.Contrato
                .Where(c => c.DataTermino < dataHoje) 
                .Count();

            var resultado = new
            {
                Ativos = quantidadeAtivos,
                Inativos = quantidadeInativos
            };

            return Ok(resultado);
        }

        [HttpPost]
        [Route("criarConta")]
        [AllowAnonymous]
        public async Task<IActionResult> CriarConta([FromBody] CriarContaRequestModel model)
        {
            const string USUARIO_SISTEMA = "Anônimo";
            Pessoa pessoa = null;
            Cliente cliente = null;
            Usuario usuario = null;
            Afiliado afiliado = null;
            Usuario usuarioAfiliado = null;
            UsuarioCliente usuarioClienteAfiliado = null;
            string novasenha = null;
            IActionResult resultadoBoleto = null;

            try
            {
                if (model == null)
                    return BadRequest("Dados inválidos.");

                var clienteExiste = context.Cliente.Include(x => x.Pessoa).FirstOrDefault(x => x.Pessoa.CpfCnpj == model.CpfCnpj);

                if (clienteExiste != null)
                    return BadRequest("O cliente já possui cadastro dentro do sistema. Contate o administrador do sistema.");

                pessoa = new Pessoa(model.Nome, model.Sexo, model.DataNascimento, model.Mae, model.Pai, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, USUARIO_SISTEMA);
                context.Add(pessoa);
                
                if(model.IdAfiliado > 0)
                {
                    afiliado = context.Afiliado.FirstOrDefault(x => x.IdPessoa == model.IdAfiliado);
                    if (afiliado == null)
                        return BadRequest("Afiliado não encontrado.");

                    usuarioAfiliado = context.Usuario.FirstOrDefault(x => x.IdPessoa == afiliado.IdPessoa);
                    if (usuarioAfiliado == null)
                        return BadRequest("Afiliado não encontrado.");

                }


                cliente = new Cliente(pessoa, afiliado);
                context.Add(cliente);
                context.SaveChanges();

                // Enviar e-mail de notificação
                await EnviarEmailNovoCliente(pessoa, cliente);

                Contrato contrato;
                Plano plano;
                Financeiro financeiro;

                var planoConta = context.PlanoConta.FirstOrDefault(x => x.Descricao == "Contrato");

                plano = context.Plano.FirstOrDefault(x => x.IdPlano == model.IdPlano);

                if (plano == null)
                    return BadRequest("Plano não encontrado.");

                financeiro = new Financeiro(
                  pessoa,
                  "Contas a Receber",
                  USUARIO_SISTEMA);

                var dataVencimento = DateTime.SpecifyKind(
                                    DateTime.Now.Date.AddDays(1).AddHours(3),
                                    DateTimeKind.Unspecified
                                );

                var financeiroParcela = new FinanceiroParcela(
                             0,
                             dataVencimento,
                             plano.ValorAdesao,
                             plano.Descricao,
                             planoConta,
                             USUARIO_SISTEMA);

                financeiro.AddParcela(financeiroParcela);

                var dataInicio = DateTime.Now.Date;
                var dataTermino = DateTime.Now.Date.AddMonths(12);
                contrato = new Contrato(
                    pessoa,
                    dataInicio,
                    dataTermino,
                    plano.Valor,
                    plano.Descricao,
                    plano,
                    plano.ValorAdesao,
                    DateTime.Now.Date,
                    true,
                    plano.Valor,
                    financeiro,
                    USUARIO_SISTEMA
                );

                #region RandonSenha
                const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                StringBuilder sb = new StringBuilder();
                Random rnd = new Random();

                for (int i = 0; i < 6; i++)
                {
                    int index = rnd.Next(chars.Length);
                    sb.Append(chars[index]);
                }
                novasenha = sb.ToString();
                #endregion

                // Usar o cliente recém-criado ao invés de buscar novamente
                usuario = new Usuario(pessoa.CpfCnpj, cliente, null, null, pessoa.Nome, pessoa.Email, "Cliente", novasenha, USUARIO_SISTEMA);

                // 1. Salvar usuário e contrato primeiro
                context.Usuario.Add(usuario);
                context.Contrato.Add(contrato);
                context.SaveChanges();

                var usuarioCliente = new UsuarioCliente(cliente, usuario, USUARIO_SISTEMA);

                if (usuarioAfiliado != null)
                {
                    usuarioClienteAfiliado = new UsuarioCliente(cliente, usuarioAfiliado, USUARIO_SISTEMA);
                    context.UsuarioCliente.Add(usuarioClienteAfiliado);
                }

                
                context.UsuarioCliente.Add(usuarioCliente);
                context.SaveChanges();

                // 2. Gerar boleto
                resultadoBoleto = await GerarBoleto(financeiro.IdFinanceiro, financeiroParcela.IdFinanceiroParcela);

                // Verificar se a geração do boleto falhou
                if (resultadoBoleto is BadRequestObjectResult)
                    return resultadoBoleto;

                // 3. Enviar email apenas se tudo funcionou
                string subject = "Bem-vindo(a) - Suas credenciais de acesso";
                string templateHtml = $@"
<!DOCTYPE html>
<html lang='pt-BR'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Confirmação de Cadastro</title>
</head>
<body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;'>
    <table width='100%' cellpadding='0' cellspacing='0' style='background-color: #f4f4f4; padding: 20px;'>
        <tr>
            <td align='center'>
                <table width='600' cellpadding='0' cellspacing='0' style='background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
                    <!-- Header -->
                    <tr>
                        <td style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 40px 30px; text-align: center;'>
                            <h1 style='color: #ffffff; margin: 0; font-size: 28px; font-weight: 600;'>Bem-vindo(a)!</h1>
                            <p style='color: #ffffff; margin: 10px 0 0 0; font-size: 16px; opacity: 0.9;'>Seu cadastro foi realizado com sucesso</p>
                        </td>
                    </tr>

                    <!-- Body -->
                    <tr>
                        <td style='padding: 40px 30px;'>
                            <p style='color: #333333; font-size: 16px; line-height: 1.6; margin: 0 0 20px 0;'>
                                Olá <strong>{pessoa.Nome}</strong>,
                            </p>
                            <p style='color: #666666; font-size: 14px; line-height: 1.6; margin: 0 0 30px 0;'>
                                Seu cadastro foi confirmado! Abaixo estão suas credenciais de acesso ao sistema:
                            </p>

                            <!-- Credentials Box -->
                            <table width='100%' cellpadding='0' cellspacing='0' style='background-color: #f8f9fa; border-radius: 6px; border: 1px solid #e9ecef; margin: 0 0 30px 0;'>
                                <tr>
                                    <td style='padding: 25px;'>
                                        <table width='100%' cellpadding='0' cellspacing='0'>
                                            <tr>
                                                <td style='padding-bottom: 15px;'>
                                                    <p style='margin: 0; color: #666666; font-size: 12px; text-transform: uppercase; letter-spacing: 0.5px;'>Login</p>
                                                    <p style='margin: 5px 0 0 0; color: #333333; font-size: 18px; font-weight: 600;'>{usuario.Login}</p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style='border-top: 1px solid #dee2e6; padding-top: 15px;'>
                                                    <p style='margin: 0; color: #666666; font-size: 12px; text-transform: uppercase; letter-spacing: 0.5px;'>Senha</p>
                                                    <p style='margin: 5px 0 0 0; color: #667eea; font-size: 24px; font-weight: 700; letter-spacing: 2px;'>{novasenha}</p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                            <!-- Security Notice -->
                            <table width='100%' cellpadding='0' cellspacing='0' style='background-color: #fff3cd; border-left: 4px solid #ffc107; border-radius: 4px; margin: 0 0 30px 0;'>
                                <tr>
                                    <td style='padding: 15px 20px;'>
                                        <p style='margin: 0; color: #856404; font-size: 13px; line-height: 1.5;'>
                                            <strong>⚠️ Importante:</strong> Por questões de segurança, recomendamos que você altere sua senha no primeiro acesso.
                                        </p>
                                    </td>
                                </tr>
                            </table>

                            <p style='color: #666666; font-size: 14px; line-height: 1.6; margin: 0;'>
                                Se você tiver alguma dúvida ou precisar de ajuda, não hesite em entrar em contato conosco.
                            </p>
                        </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                        <td style='background-color: #f8f9fa; padding: 30px; text-align: center; border-top: 1px solid #e9ecef;'>
                            <p style='margin: 0 0 10px 0; color: #666666; font-size: 14px; font-weight: 600;'>Gerenciar SC</p>
                            <p style='margin: 0; color: #999999; font-size: 12px;'>
                                Este é um email automático, por favor não responda.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";

                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Gerenciar SC");
                mailMsg.Subject = subject;
                //mailMsg.To.Add(new MailAddress(pessoa.Email, pessoa.Nome));
                mailMsg.To.Add(new MailAddress("kleytonwillian@hotmail.com", "Teste"));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(templateHtml, null, MediaTypeNames.Text.Html));

                smtpClientSendGrid.Credentials = credentialsSendGrid;
                smtpClientSendGrid.EnableSsl = true;

                try
                {
                    smtpClientSendGrid.Send(mailMsg);
                }
                catch (Exception ex)
                {
                    // Email falhou mas usuário já foi criado - log do erro
                    System.Console.WriteLine($"Erro ao enviar email: {ex.Message}");
                }

                // Retornar o identificador do boleto Unique para o frontend
                var parcelaAtualizada = context.FinanceiroParcela
                    .FirstOrDefault(x => x.IdFinanceiroParcela == financeiroParcela.IdFinanceiroParcela);

                return Ok(parcelaAtualizada?.IdentificadorBoletoUnique ?? 0);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    erro = "Erro ao criar conta",
                    mensagem = ex.Message,
                    detalhes = ex.InnerException?.Message
                });
            }
        }

        private async Task<IActionResult> GerarBoleto(int idFinanceiro, int idFinanceiroParcela)
        {
            try
            {
                var financeiro = context.Financeiro.AsNoTracking().Include(x => x.Parcelas).Include(x => x.Pessoa).FirstOrDefault(x => x.IdFinanceiro == idFinanceiro);
                if (financeiro == null)
                    return BadRequest("Não foi possível recuperar dados do financeiro.");

                var financeiroParcela = context.FinanceiroParcela.Include(x => x.Financeiro)
                    .Include(x => x.Financeiro.Pessoa).FirstOrDefault(x => x.IdFinanceiroParcela == idFinanceiroParcela && x.IdFinanceiro == idFinanceiro && x.Situacao == "Aberto");

                if (financeiroParcela == null)
                    return BadRequest("Não foi possível recuperar a parcela.");
                if (financeiroParcela.IdentificadorBoletoUnique != 0)
                    return BadRequest("Boleto já foi gerado para esta parcela.");
                if (financeiroParcela.DataVencimento.Date < DateTime.Now.Date)
                    return BadRequest("Não é possivel gerar um boleto com a data vencida");

                var token = await _uniqueService.GerarAccessTokenAsync(_config["unique:login"], _config["unique:password"], _config["unique:url"]);


                var cobrancaRequest = new CobrancaRequest
                {
                    nomeSacado = financeiro.Nome,
                    bairroSacado = financeiro.Pessoa.Bairro,
                    boletoMensagem1 = "",
                    boletoMensagem2 = "",
                    boletoMensagem3 = "",
                    boletoMensagem4 = "",
                    cepSacado = financeiro.Pessoa.Cep,
                    cidadeSacado = financeiro.Pessoa.Cidade,
                    cpfCnpjSacado = financeiro.Pessoa.CpfCnpj,
                    dataVencimento = financeiroParcela.DataVencimento,
                    emailSacado = financeiro.Pessoa.Email,
                    enderecoSacado = financeiro.Pessoa.Logradouro,
                    identificador = financeiroParcela.IdFinanceiroParcela.ToString(),
                    numeroPedido = financeiroParcela.IdFinanceiroParcela.ToString(),
                    numeroSacado = financeiro.Pessoa.Numero,
                    splitsValores = new List<SplitValor>
                {
                    new SplitValor
                {
                    cobrarTarifa = true,
                    agencia = "000001",
                    conta = "000024",
                    valor = financeiroParcela.ValorVencimento
                }
                 },
                    telefoneSacado = financeiro.Pessoa.Telefone1,
                    valor = financeiroParcela.ValorVencimento,
                    valorDesconto = financeiroParcela.ValorDesconto ?? 0,
                    valorJuros = financeiroParcela.ValorAcrescimo ?? 0,
                    valorMulta = financeiroParcela.ValorAcrescimo ?? 0
                };

                var responseuniqueService = await _uniqueService.CriarCobrancaAsync(cobrancaRequest, token, _config["unique:url"]);

                financeiroParcela.SetIdentificadorBoletoUnique(responseuniqueService.Data.IdTransacao);
                context.FinanceiroParcela.Update(financeiroParcela);
                context.SaveChanges();

                return Ok(financeiroParcela.IdentificadorBoletoUnique);
        }
            catch (Exception ex)
            {
                // Log do erro para debug
                System.Console.WriteLine($"Erro ao gerar boleto: {ex.Message}");
                System.Console.WriteLine($"StackTrace: {ex.StackTrace}");

                // Se for erro do serviço Unique, retorna detalhes
                if (ex.InnerException != null)
                {
                    System.Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                    return BadRequest(new
                    {
                        erro = "Erro ao gerar boleto",
                        mensagem = ex.Message,
                        detalhes = ex.InnerException.Message
                    });
                }

                    return BadRequest(new
                    {
                        erro = "Erro ao gerar boleto",
                        mensagem = ex.Message
                    });
            }
        }

        private async Task EnviarEmailNovoCliente(Pessoa pessoa, Cliente cliente)
        {
            var subject = $"CONCICARD - Novo Cliente Cadastrado - {pessoa.Nome}";

            var tipoPessoaTexto = pessoa.TipoPessoa == "PJ" ? "Pessoa Jurídica" : "Pessoa Física";
            var documento = pessoa.TipoPessoa == "PJ" ? "CNPJ" : "CPF";

            var templateHtml = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #1e88e5; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                        .content {{ background-color: #f9f9f9; padding: 20px; border: 1px solid #ddd; }}
                        .info-row {{ margin: 10px 0; padding: 10px; background-color: white; border-left: 4px solid #1e88e5; }}
                        .label {{ font-weight: bold; color: #1e88e5; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                        .status-badge {{ background-color: #fff3e0; color: #e65100; padding: 5px 15px; border-radius: 20px; display: inline-block; margin: 10px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>Novo Cliente Cadastrado</h2>
                        </div>
                        <div class='content'>
                            <p><span class='status-badge'>EM ANÁLISE</span></p>
                            <p>Um novo cliente realizou cadastro através do formulário anônimo e aguarda análise.</p>

                            <div class='info-row'>
                                <span class='label'>Tipo:</span> {tipoPessoaTexto}
                            </div>

                            <div class='info-row'>
                                <span class='label'>Nome:</span> {pessoa.Nome}
                            </div>

                            {(!string.IsNullOrEmpty(pessoa.RazaoSocial) && pessoa.TipoPessoa == "PJ" ? $@"
                            <div class='info-row'>
                                <span class='label'>Razão Social:</span> {pessoa.RazaoSocial}
                            </div>" : "")}

                            <div class='info-row'>
                                <span class='label'>{documento}:</span> {pessoa.CpfCnpj}
                            </div>

                            <div class='info-row'>
                                <span class='label'>E-mail:</span> {pessoa.Email}
                            </div>

                            <div class='info-row'>
                                <span class='label'>Telefone:</span> {pessoa.Telefone1}
                            </div>

                            <div class='info-row'>
                                <span class='label'>Endereço:</span> {pessoa.Logradouro}, {pessoa.Numero}{(!string.IsNullOrEmpty(pessoa.Complemento) ? " - " + pessoa.Complemento : "")}
                            </div>

                            <div class='info-row'>
                                <span class='label'>Bairro:</span> {pessoa.Bairro}
                            </div>

                            <div class='info-row'>
                                <span class='label'>Cidade/Estado:</span> {pessoa.Cidade}/{pessoa.Estado}
                            </div>

                            <div class='info-row'>
                                <span class='label'>CEP:</span> {pessoa.Cep}
                            </div>

                            {(!string.IsNullOrEmpty(pessoa.InscricaoEstadual) ? $@"
                            <div class='info-row'>
                                <span class='label'>Inscrição Estadual:</span> {pessoa.InscricaoEstadual}
                            </div>" : "")}

                            {(!string.IsNullOrEmpty(pessoa.InscricaoMunicipal) ? $@"
                            <div class='info-row'>
                                <span class='label'>Inscrição Municipal:</span> {pessoa.InscricaoMunicipal}
                            </div>" : "")}

                            <p style='margin-top: 20px;'><strong>Ação necessária:</strong> Acesse o sistema para analisar e aprovar o cadastro.</p>
                        </div>
                        <div class='footer'>
                            <p>Equipe Concicard - Sistema de Gestão</p>
                            <p>Este é um e-mail automático, por favor não responda.</p>
                        </div>
                    </div>
                </body>
                </html>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Sistema");
            mailMsg.To.Add(new MailAddress("atendimento@concicard.com.br"));
            mailMsg.To.Add(new MailAddress("kleytonwillian@gmail.com"));
            mailMsg.Subject = subject;
            mailMsg.IsBodyHtml = true;
            mailMsg.Body = templateHtml;

            smtpClientSendGrid.Credentials = credentialsSendGrid;
            await smtpClientSendGrid.SendMailAsync(mailMsg);
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult Salvar([FromBody] ContratoRequest model)
        {
            Pessoa cliente;
            Contrato contrato;
            Plano plano;
            Financeiro financeiro;
            Vendedor vendedor;
            cliente = context.Pessoa.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
            vendedor = context.Vendedor.FirstOrDefault(x => x.IdPessoa == model.IdVendedor);
            var planoConta = context.PlanoConta.FirstOrDefault(x => x.IdPlanoConta == model.IdPlanoConta);

            if (model.DataPrimeiraMensalidade.Day != 5 && model.DataPrimeiraMensalidade.Day != 10 && model.DataPrimeiraMensalidade.Day != 15)
                return BadRequest("As datas para a mensalidade só podem ser os dias 5, 10 e 15");
            if (model.DataInicio >= model.DataTermino)
                return BadRequest("A data de início não pode ser maior que a data de termino");
            if(model.DataAdesao.HasValue && model.DataAdesao >= model.DataInicio)
                return BadRequest("A data de adesão não pode ser maior que a data de início");

            financeiro = new Financeiro(
                cliente,
                "Contas a Receber",
                User.Identity.Name);
            if (model.ContratoAdesao == true && model.DataAdesao.HasValue)
            {
                financeiro.AddParcela(new FinanceiroParcela(
                     0,
                     model.DataAdesao.GetValueOrDefault(),
                     model.ValorAdesao,
                     model.Descricao,
                     planoConta,
                     User.Identity.Name));
            }

            DateTime dataMensalidade = new DateTime();

            for (int i = 1; i <= model.NumeroParcelas; i++)
            {

              if (i == 1)
              {
                 dataMensalidade = new DateTime(model.DataPrimeiraMensalidade.Year, model.DataPrimeiraMensalidade.Month, model.DataPrimeiraMensalidade.Day);
              }
              else
              {
                  dataMensalidade = dataMensalidade.AddMonths(1);
              }

               financeiro.AddParcela( new FinanceiroParcela(
                    i,
                    dataMensalidade,
                    model.ValorMensalidade,
                    model.Descricao,
                    planoConta,
                    User.Identity.Name));
            }


            plano = context.Plano.FirstOrDefault(x => x.IdPlano == model.IdPlano);
                contrato = new Contrato(
                    cliente,
                    model.DataInicio,
                    model.DataTermino,
                    model.ValorMensalidade,
                    model.LinkContrato,
                    model.Descricao,
                    plano,
                    model.ValorAdesao,
                    model.DataAdesao, 
                    model.ContratoAdesao, 
                    model.ValorTotal,
                    financeiro,
                    model.NumeroParcelas,
                    model.DataPrimeiraMensalidade,
                    vendedor,
                    model.ResponsavelNome,
                    model.ResponsavelCpf,
                    model.ResponsavelCargo,
                    model.ResponsavelEmail,
                    model.ResponsavelTelefone,
                    model.ResponsavelCelular,
                    User.Identity.Name
                );

                context.Contrato.Add(contrato);
            

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var contrato = context.Contrato.FirstOrDefault(x => x.IdContrato == id);
            contrato.Excluir(User.Identity.Name);

            context.Update(contrato);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var contrato = context.Contrato.Include(x => x.Cliente).FirstOrDefault(x => x.IdContrato == id);
            if (contrato == null)
                return BadRequest("Contrato não encontrado ");

            return Ok(new ContratoResponse()
            {
                IdContrato = contrato.IdContrato,
                IdCliente = contrato.IdCliente,
                ClienteRazaoSocial = contrato.Cliente.RazaoSocial,
                ClienteCpfCnpj = contrato.Cliente.CpfCnpj,
                ClienteLogradouro = contrato.Cliente.Logradouro,
                ClienteNumero = contrato.Cliente.Numero,
                ClienteBairro = contrato.Cliente.Bairro,
                ClienteCidade = contrato.Cliente.Cidade,
                ClienteEstado = contrato.Cliente.Estado,
                DataInicio = contrato.DataInicio.Date,
                DataTermino = contrato.DataTermino.Date ,
                Descricao = contrato.Descricao,
                ValorMensalidade = contrato.ValorMensalidade,
                IdPlano = contrato.IdPlano,
                ValorAdesao = contrato.ValorAdesao,
                DataAdesao = contrato.DataAdesao,
                ContratoAdesao = contrato.ContratoAdesao,
                ValorTotal = contrato.ValorTotal,
                NumeroParcelas = contrato.NumeroParcelas,
                DataPrimeiraMensalidade = contrato.DataPrimeiraMensalidade,
                IdVendedor = contrato.IdVendedor,
                LinkContrato = contrato.LinkContrato,
                ResponsavelNome = contrato.ResponsavelNome,
                ResponsavelCpf = contrato.ResponsavelCpf,
                ResponsavelCargo = contrato.ResponsavelCargo,
                ResponsavelEmail = contrato.ResponsavelEmail,
                ResponsavelTelefone = contrato.ResponsavelTelefone,
                ResponsavelCelular = contrato.ResponsavelCelular,
                DataAssinatura = contrato.DataInclusao,
                Situacao = contrato.Situacao
            });
        }
    }
}
