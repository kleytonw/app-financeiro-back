
using ERP.Infra;
using ERP.Models;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ERP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ClienteController : ControllerBase
    {
        protected Context context;

        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        public ClienteController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var result = context.Cliente
                .Include(x => x.Pessoa).Where(x => x.Situacao == "Ativo")
                  .Select(m => new
                  {
                      
                      m.IdPessoa,
                      Nome = m.Pessoa.Nome,
                      m.Pessoa.RazaoSocial,
                      m.Pessoa.CpfCnpj,
                      m.IdentificadorConciliadora,
                      m.Senha,
                      m.SenhaConciliadora,
                      m.ApiKeyConciliadora,
                      m.Status,
                      m.Situacao
                  }).Take(500).ToList().OrderByDescending(x => x.IdPessoa);

            return Ok(result);
        }


        [HttpGet]
        [Route("listar-clientes-usuarios")]
        public IActionResult ListarClientesUsuario()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);


            if (usuarioLogado.TipoUsuario == "Administrador")
            {

                var result = context.Cliente.Include(x => x.Pessoa).Where(x => x.Situacao == "Ativo").Select(m => new
                {
                    m.Pessoa.IdPessoa,
                    m.Pessoa.Nome,
                    m.Pessoa.RazaoSocial,
                    m.Pessoa.CpfCnpj,
                    m.IdentificadorConciliadora,
                    m.Senha,
                    m.Status,
                    m.Situacao
                }).ToList().OrderBy(x => x.Nome);

                return Ok(result);
            }

            else if (usuarioLogado.TipoUsuario == "ERP")
            {

                var result = context.ClienteERP.Include(x => x.Cliente.Pessoa).Where(x => x.IdERPs == usuarioLogado.IdERPs && x.Situacao == "Ativo").Select(m => new
                {
                    m.Cliente.Pessoa.IdPessoa,
                    m.Cliente.Pessoa.Nome,
                    m.Cliente.Pessoa.RazaoSocial,
                    m.Cliente.Pessoa.CpfCnpj,
                    m.Cliente.IdentificadorConciliadora,
                    m.Cliente.Senha,
                    m.Cliente.Situacao
                }).ToList().OrderBy(x => x.Nome);

                return Ok(result);
            }

            else if (usuarioLogado.TipoUsuario == "Cliente" || usuarioLogado.TipoUsuario == "Afiliado")
            {
                var result = context.UsuarioCliente.Include(x => x.Cliente.Pessoa).Include(x => x.Usuario).Where(x => x.IdUsuario == usuarioLogado.IdUsuario && x.Situacao == "Ativo").Select(m => new
                {
                    m.Cliente.Pessoa.IdPessoa,
                    m.Cliente.Pessoa.Nome,
                    m.Cliente.Pessoa.RazaoSocial,
                    m.Cliente.Pessoa.CpfCnpj,
                    m.Cliente.IdentificadorConciliadora,
                    m.Cliente.Senha,
                    m.Cliente.Situacao
                }).ToList().OrderBy(x => x.Nome);

                return Ok(result);
            }

            else
            {
                return BadRequest("Erro ao buscar o usuário!");
            }

            
        }

        [HttpGet]
        [Route("listarPorOrdemAlfabetica")]
        public IActionResult ListarPorOrdemAlfabetica()
        {
            var result = context.Cliente.Where(x => x.Situacao == "Ativo")
                .Select(m => new
                {
                    m.IdPessoa,
                    m.Pessoa.Nome,
                }).ToList().OrderBy(x => x.Nome);
            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var result = context.Cliente.Where(x => x.Situacao == "Ativo").AsQueryable();
            switch (model.Chave)
            {
                case "CpfCnpj":
                    result = result.Where(x => x.Pessoa.CpfCnpj == model.Valor);
                    break;
                case "Nome":
                    result = result.Where(x => x.Pessoa.Nome.Contains(model.Valor.ToUpper()));
                    break;
                case "RazaoSocial":
                    result = result.Where(x => x.Pessoa.RazaoSocial.Contains(model.Valor.ToUpper()));
                    break;
                default:
                    // code block
                    break;
            }

            return Ok(result.Select(m => new
            {
                m.IdPessoa,
                m.Pessoa.Nome,
                m.Pessoa.RazaoSocial,
                m.Pessoa.CpfCnpj,
                m.Pessoa.Situacao,
                m.Senha,
                m.SenhaConciliadora,
                m.ApiKeyConciliadora,
                m.IdentificadorConciliadora,
                m.Status
            }).Take(500).ToList());
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] ClienteRequest model)
        {

            Cliente cliente;
            Pessoa pessoa;
            Colaborador colaborador;
            Afiliado afiliado;

            string senhaAleatoria = GerarSenhaAleatoria();

            if (model.IdPessoa > 0)
            {
                cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
                pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);
                colaborador = context.Colaborador.FirstOrDefault(x => x.IdPessoa == model.IdColaborador);
                afiliado = context.Afiliado.FirstOrDefault(x => x.IdPessoa == model.IdAfiliado);

                pessoa.Alterar(
                    model.Nome,
                    model.Sexo,
                    model.DataNascimento,
                    model.Mae,
                    model.Pai,
                    model.TipoPessoa,
                    model.RazaoSocial,
                    model.CpfCnpj,
                    model.Telefone1,
                    model.Telefone2,
                    model.Email,
                    model.Cep,
                    model.Logradouro,
                    model.Numero,
                    model.Complemento,
                    model.Bairro,
                    model.Cidade,
                    model.Estado,
                    model.Referencia,
                    model.InscricaoEstadual,
                    model.InscricaoMunicipal,
                    User.Identity.Name);
                cliente.Alterar(pessoa, senhaAleatoria, model.NomeResponsavel, model.CelularResponsavel,
                    model.EmailResponsavel, model.NomeContratante, model.CelularContratante, model.EmailContratante, colaborador, afiliado, User.Identity.Name);


                context.Update(cliente);
            }
            else
            {
                var cpfJaCadastrado = context.Pessoa
                .Any(x => x.CpfCnpj == model.CpfCnpj);

                if (cpfJaCadastrado)
                    return BadRequest("O cliente já possui cadastro");

                colaborador = context.Colaborador.FirstOrDefault(x => x.IdPessoa == model.IdColaborador);
                afiliado = context.Afiliado.FirstOrDefault(x => x.IdPessoa == model.IdAfiliado);
                pessoa = new Pessoa(model.Nome, model.Sexo, model.DataNascimento, model.Mae, model.Pai, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, User.Identity.Name);
                cliente = new Cliente(pessoa, senhaAleatoria, model.NomeResponsavel, model.CelularResponsavel,
                    model.EmailResponsavel, model.NomeContratante, model.CelularContratante, model.EmailContratante, colaborador, afiliado, User.Identity.Name);
                context.Cliente.Add(cliente);
            }
            context.SaveChanges();
            return Ok(pessoa.IdPessoa);
        }

        [HttpPost]
        [Route("salvar-tecnicos")]
        [Authorize]
        public IActionResult SalvarTecnicos([FromBody] ClienteTecnicosRequest model)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdPessoa);

            cliente.AdicionarDadosConciliadora(model.IdentificadorConciliadora, model.SenhaConciliadora, model.ApiKeyConciliadora, User.Identity.Name);
            context.Update(cliente);

            var erps = context.ClienteERP.Where(x => x.IdCliente == model.IdPessoa);
            var operadoras = context.ClienteAdquirente.Where(x => x.IdCliente == model.IdPessoa);
            var bancos = context.ClienteBanco.Where(x => x.IdCliente == model.IdPessoa);

            if (erps.Any())
            {
                context.ClienteERP.RemoveRange(erps);
                context.SaveChanges();
            }

            foreach (var erpss in model.Erps)
            {
                var erp = context.ERPs.FirstOrDefault(x => x.IdERPs == erpss);

                var clienteErp = new ClienteERP(cliente, erp, User.Identity.Name);
                context.ClienteERP.Add(clienteErp);
                context.SaveChanges();
            }


            if (operadoras.Any())
            {
                context.ClienteAdquirente.RemoveRange(operadoras);
                context.SaveChanges();
            }

            foreach (var operadorass in model.Operadoras)
            {
                var operadora = context.Operadora.FirstOrDefault(x => x.IdOperadora == operadorass);

                var clienteAdquirente = new ClienteAdquirente(operadora, cliente, User.Identity.Name);
                context.ClienteAdquirente.Add(clienteAdquirente);
                context.SaveChanges();
            }

            if (bancos.Any())
            {
                context.ClienteBanco.RemoveRange(bancos);
                context.SaveChanges();
            }

            foreach (var bancoss in model.Bancos)
            {
                var banco = context.Banco.FirstOrDefault(x => x.IdBanco == bancoss);

                var clienteBanco = new ClienteBanco(banco, cliente, User.Identity.Name);
                context.ClienteBanco.Add(clienteBanco);
                context.SaveChanges();
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("gerar-senha-aleatoria")]
        public IActionResult GerarSenhaAleatoria2(int idCliente)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == idCliente);
            string senha = GerarSenhaAleatoria();
            if (cliente == null)
                return BadRequest("Cliente não encontrado");
            cliente.SalvarSenha(senha, User.Identity.Name);
            context.Update(cliente);
            context.SaveChanges();
            return Ok();
        }

        private string GerarSenhaAleatoria()
        {
            Guid guid = Guid.NewGuid();
            string hash = GenerateSha256Hash(guid.ToString());

            // Você pode retornar o hash completo ou apenas uma parte dele
            return hash.Substring(0, 12); // Primeiros 12 caracteres
                                          // ou retornar o hash completo: return hash;
        }

        private string GenerateSha256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }


        [HttpGet]
        [Route("obter")]
        public IActionResult Obter(int id)
        {
            var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == id);
            var senhaConciliadora = context.Cliente.FirstOrDefault(x => x.IdPessoa == id).SenhaConciliadora;
            var apiKeyConciliadora = context.Cliente.FirstOrDefault(x => x.IdPessoa == id).ApiKeyConciliadora;
            var senha = context.Cliente.FirstOrDefault(x => x.IdPessoa == id)?.Senha;
            var identificadorConciliadora = context.Cliente.FirstOrDefault(x => x.IdPessoa == id).IdentificadorConciliadora;
            var bancos = context.ClienteBanco.Where(x => x.IdCliente == id).Select(x => x.IdBanco).ToList();
            var erps = context.ClienteERP.Where(x => x.IdCliente == id).Select(x => x.IdERPs).ToList();
            var operadoras = context.ClienteAdquirente.Where(x => x.IdCliente == id).Select(x => x.IdOperadora).ToList();
            var contrato = context.Contrato.FirstOrDefault(x => x.IdCliente == id);

            if (cliente == null)
                return BadRequest("Cliente não encontrado ");

            return Ok(new ClienteResponse()
            {
                IdPessoa = cliente.IdPessoa,
                Nome = pessoa.Nome,
                RazaoSocial = pessoa.RazaoSocial,
                Telefone1 = pessoa.Telefone1,
                Telefone2 = pessoa.Telefone2,
                Email = pessoa.Email,
                CpfCnpj = pessoa.CpfCnpj,
                Cep = pessoa.Cep,
                Sexo = pessoa.Sexo,
                Estado = pessoa.Estado,
                Cidade = pessoa.Cidade,
                Bairro = pessoa.Bairro,
                Logradouro = pessoa.Logradouro,
                Numero = pessoa.Numero,
                Complemento = pessoa.Complemento,
                Referencia = pessoa.Referencia,
                DataNascimento = pessoa.DataNascimento,
                Mae = pessoa.Mae,
                Pai = pessoa.Pai,
                TipoPessoa = pessoa.TipoPessoa,
                InscricaoEstadual = pessoa.InscricaoEstadual,
                IdentificadorConciliadora = identificadorConciliadora,
                InscricaoMunicipal = pessoa.InscricaoMunicipal,
                Senha = senha,
                SenhaConciliadora = senhaConciliadora,
                ApiKeyConciliadora = apiKeyConciliadora,
                IdColaborador = cliente.IdColaborador,
                NomeResponsavel = cliente.NomeResponsavel,
                CelularResponsavel = cliente.CelularResponsavel,
                EmailResponsavel = cliente.EmailResponsavel,
                NomeContratante = cliente.NomeContratante,
                CelularContratante = cliente.CelularContratante,
                EmailContratante = cliente.EmailContratante,
                Bancos = bancos,
                Erps = erps,
                Operadoras = operadoras,
                IdContrato = contrato?.IdContrato ?? 0,
                IdFinanceiro = contrato?.IdFinanceiro ?? 0,
                IdCliente = contrato?.IdCliente ?? 0,
                IdVendedor = contrato?.IdVendedor ?? 0,
                DataInicio = contrato?.DataInicio,
                DataTermino = contrato?.DataTermino,
                IdAfiliado = cliente?.IdAfiliado ?? 0,
                ValorMensalidade = contrato?.ValorMensalidade ?? 0,
                LinkContrato = contrato?.LinkContrato ?? "",
                DataPrimeiraMensalidade = contrato?.DataPrimeiraMensalidade,
                Descricao = contrato?.Descricao ?? "",
                IdPlano = contrato?.IdPlano ?? 0,
                ValorAdesao = contrato?.ValorAdesao ?? 0,
                DataAdesao = contrato?.DataAdesao,
                ContratoAdesao = contrato?.ContratoAdesao,
                ValorTotal = contrato?.ValorTotal ?? 0,
                NumeroParcelas = contrato?.NumeroParcelas ?? 0,
                ResponsavelNome = contrato?.ResponsavelNome ?? "",
                ResponsavelCpf = contrato?.ResponsavelCpf ?? "",
                ResponsavelCargo = contrato?.ResponsavelCargo ?? "",
                ResponsavelEmail = contrato?.ResponsavelEmail ?? "",
                ResponsavelTelefone = contrato?.ResponsavelTelefone ?? "",
                ResponsavelCelular = contrato?.ResponsavelCelular ?? "",
                Situacao = cliente.Situacao

            });
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var pessoa = context.Pessoa.FirstOrDefault(x => x.IdPessoa == id);
            var cliente = context.Cliente.FirstOrDefault(p => p.IdPessoa == id);

            pessoa.Excluir(User.Identity.Name);
            cliente.Excluir(User.Identity.Name);

            context.Update(pessoa);
            context.Update(cliente);
            context.SaveChanges();
            return Ok();
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
        [Route("enviar-extratos-mes")]
        [Authorize]
        public async Task<IActionResult> EnviarExtratosDoMesPorEmail()
        {
            try
            {
                // Se não informado, usa o mês e ano atual
                var mesSelecionado =  DateTime.Now.Month;
                var anoSelecionado =  DateTime.Now.Year;

                // Busca todos os extratos do mês apenas de clientes ativos
                var extratosMes = context.Extrato
                    .Include(e => e.ClienteContaBancaria)
                    .Include(e => e.Cliente.Pessoa)
                    .Where(e => e.DataLancamento.Month == mesSelecionado &&
                                e.DataLancamento.Year == anoSelecionado &&
                                e.Situacao == "Ativo" &&
                                e.Cliente.Situacao == "Ativo")
                    .OrderBy(e => e.IdCliente)
                    .ThenBy(e => e.DataLancamento)
                    .ToList();

                if (!extratosMes.Any())
                    return Ok(new { mensagem = "Não há extratos para o período selecionado." });

                // Agrupa extratos por cliente
                var extratosPorCliente = extratosMes.GroupBy(e => e.IdCliente);

                int emailsEnviados = 0;
                int erros = 0;
                var listaErros = new List<string>();

                // Envia e-mail para cada cliente
                foreach (var grupo in extratosPorCliente)
                {
                    var cliente = context.Cliente
                        .Include(c => c.Pessoa)
                        .FirstOrDefault(c => c.IdPessoa == grupo.Key);

                    if (cliente == null || string.IsNullOrEmpty(cliente.Pessoa.Email))
                    {
                        listaErros.Add($"Cliente ID {grupo.Key}: E-mail não cadastrado");
                        erros++;
                        continue;
                    }

                    try
                    {
                        await EnviarEmailExtrato(cliente, grupo.ToList(), mesSelecionado, anoSelecionado);
                        emailsEnviados++;
                    }
                    catch (Exception ex)
                    {
                        listaErros.Add($"Cliente {cliente.Pessoa.Nome}: {ex.Message}");
                        erros++;
                    }
                }

                return Ok(new
                {
                    mensagem = "Processo concluído",
                    emailsEnviados = emailsEnviados,
                    erros = erros,
                    detalhesErros = listaErros,
                    mes = mesSelecionado,
                    ano = anoSelecionado
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao enviar extratos: {ex.Message}");
            }
        }

        private async Task EnviarEmailExtrato(Cliente cliente, List<Extrato> extratos, int mes, int ano)
        {
            var nomeMes = new DateTime(ano, mes, 1).ToString("MMMM", new System.Globalization.CultureInfo("pt-BR"));
            var subject = $"CONCICARD - Extrato Mensal - {nomeMes}/{ano}";

            var totalCredito = extratos.Where(e => e.Valor > 0).Sum(e => e.Valor);
            var totalDebito = extratos.Where(e => e.Valor < 0).Sum(e => e.Valor);
            var saldoFinal = totalCredito + totalDebito;

            var linhasExtrato = string.Join("", extratos.Select(e => $@"
                <tr style='border-bottom: 1px solid #e0e0e0;'>
                    <td style='padding: 12px; text-align: center;'>{e.DataLancamento:dd/MM/yyyy}</td>
                    <td style='padding: 12px;'>{e.Descricao}</td>
                    <td style='padding: 12px;'>{(!string.IsNullOrEmpty(e.Pagador) ? e.Pagador : "-")}</td>
                    <td style='padding: 12px; text-align: right; color: {(e.Valor >= 0 ? "#2e7d32" : "#c62828")}; font-weight: bold;'>
                        {(e.Valor >= 0 ? "+" : "")}{e.Valor:C2}
                    </td>
                </tr>"));

            var templateHtml = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 800px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #1e88e5; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                        .content {{ background-color: #f9f9f9; padding: 20px; border: 1px solid #ddd; }}
                        .info-box {{ background-color: white; padding: 15px; margin: 15px 0; border-radius: 5px; border-left: 4px solid #1e88e5; }}
                        .totais {{ display: flex; justify-content: space-around; margin: 20px 0; }}
                        .total-item {{ background-color: white; padding: 15px; border-radius: 5px; text-align: center; flex: 1; margin: 0 10px; }}
                        .total-label {{ font-size: 12px; color: #666; text-transform: uppercase; }}
                        .total-valor {{ font-size: 24px; font-weight: bold; margin-top: 5px; }}
                        .credito {{ color: #2e7d32; }}
                        .debito {{ color: #c62828; }}
                        .saldo {{ color: #1e88e5; }}
                        table {{ width: 100%; border-collapse: collapse; background-color: white; margin: 20px 0; }}
                        th {{ background-color: #1e88e5; color: white; padding: 12px; text-align: left; }}
                        td {{ padding: 12px; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>Extrato Mensal</h2>
                            <p style='margin: 5px 0; font-size: 18px;'>{nomeMes.ToUpper()} / {ano}</p>
                        </div>
                        <div class='content'>
                            <div class='info-box'>
                                <strong>Cliente:</strong> {cliente.Pessoa.Nome}<br/>
                                <strong>{(cliente.Pessoa.TipoPessoa == "PJ" ? "CNPJ" : "CPF")}:</strong> {cliente.Pessoa.CpfCnpj}<br/>
                                {(!string.IsNullOrEmpty(cliente.Pessoa.RazaoSocial) ? $"<strong>Razão Social:</strong> {cliente.Pessoa.RazaoSocial}<br/>" : "")}
                            </div>

                            <div class='totais'>
                                <div class='total-item'>
                                    <div class='total-label'>Créditos</div>
                                    <div class='total-valor credito'>+{totalCredito:C2}</div>
                                </div>
                                <div class='total-item'>
                                    <div class='total-label'>Débitos</div>
                                    <div class='total-valor debito'>{totalDebito:C2}</div>
                                </div>
                                <div class='total-item'>
                                    <div class='total-label'>Saldo Final</div>
                                    <div class='total-valor saldo'>{saldoFinal:C2}</div>
                                </div>
                            </div>

                            <h3 style='color: #1e88e5; margin-top: 30px;'>Lançamentos do Período</h3>
                            <table>xxxx
                                <thead>
                                    <tr>
                                        <th style='text-align: center; width: 120px;'>Data</th>
                                        <th>Descrição</th>
                                        <th style='width: 200px;'>Pagador</th>
                                        <th style='text-align: right; width: 120px;'>Valor</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {linhasExtrato}
                                </tbody>
                            </table>

                            <div style='margin-top: 20px; padding: 15px; background-color: #fff3e0; border-radius: 5px;'>
                                <strong>Total de lançamentos:</strong> {extratos.Count}
                            </div>
                        </div>
                        <div class='footer'>
                            <p>Equipe Concicard - Sistema de Gestão</p>
                            <p>Este é um e-mail automático, por favor não responda.</p>
                            <p>Em caso de dúvidas, entre em contato com nossa equipe de suporte.</p>
                        </div>
                    </div>
                </body>
                </html>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard - Sistema");
            //mailMsg.To.Add(new MailAddress(cliente.Pessoa.Email));
            mailMsg.To.Add("renato@genialsoft.com.br");
            mailMsg.To.Add(new MailAddress(cliente.Pessoa.Email));
            mailMsg.Subject = subject;
            mailMsg.IsBodyHtml = true;
            mailMsg.Body = templateHtml;

            smtpClientSendGrid.Credentials = credentialsSendGrid;
            await smtpClientSendGrid.SendMailAsync(mailMsg);
        }

        [HttpGet]
        [Route("ativar")]
        [Authorize]
        public IActionResult Ativar(int idPessoa)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == idPessoa);
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            cliente.Ativar(User.Identity.Name);
            context.Update(cliente);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("inativar")]
        [Authorize]
        public IActionResult Inativar(int idPessoa)
        {
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == idPessoa);
            if (cliente == null)
                return BadRequest("Cliente não encontrado");

            cliente.Inativar(User.Identity.Name);
            context.Update(cliente);
            context.SaveChanges();
            return Ok();
        }



    }
}

