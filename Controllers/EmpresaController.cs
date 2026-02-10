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
using System.Data.Entity;
using ERP_API.Models;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class EmpresaController : ControllerBase
    {
        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        private IConfiguration _config;
        protected Context context;
        public EmpresaController(Context context, IConfiguration config)
        {
            _config = config;
            this.context = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            if (usuarioLogado.TipoUsuario == "Administrador")
            {
                var result = context.Empresa
                      .Select(m => new
                      {
                          m.IdEmpresa,
                          m.Nome,
                          m.TipoPessoa,
                          m.RazaoSocial,
                          m.CpfCnpj,
                          m.Telefone1,
                          m.Telefone2,
                          m.Email,
                          m.Cep,
                          m.Logradouro,
                          m.Numero,
                          m.Complemento,
                          m.Bairro,
                          m.Cidade,
                          m.Estado,
                          m.Referencia,
                          m.InscricaoEstadual,
                          m.InscricaoMunicipal,
                          m.Situacao
                      }).ToList();
                return Ok(result);
            }
            else
            {
                var result = context.Empresa.Where(x => x.IdEmpresa == usuarioLogado.IdEmpresa)
                    .Select(m => new
                    {
                        m.IdEmpresa,
                        m.Nome,
                        m.TipoPessoa,
                        m.RazaoSocial,
                        m.CpfCnpj,
                        m.Telefone1,
                        m.Telefone2,
                        m.Email,
                        m.Cep,
                        m.Logradouro,
                        m.Numero,
                        m.Complemento,
                        m.Bairro,
                        m.Cidade,
                        m.Estado,
                        m.Referencia,
                        m.InscricaoEstadual,
                        m.InscricaoMunicipal,
                        m.Situacao
                    }).ToList();
                return Ok(result);
            }
        }


        [HttpGet]
        [Route("lista")]
        public IActionResult Lista()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

                var result = context.Empresa
                      .Select(m => new
                      {
                          m.IdEmpresa,
                          m.Nome,
                          m.TipoPessoa,
                          m.RazaoSocial,
                          m.CpfCnpj,
                          m.Telefone1,
                          m.Telefone2,
                          m.Email,
                          m.Cep,
                          m.Logradouro,
                          m.Numero,
                          m.Complemento,
                          m.Bairro,
                          m.Cidade,
                          m.Estado,
                          m.Referencia,
                          m.InscricaoEstadual,
                          m.InscricaoMunicipal,
                          m.Situacao
                      }).ToList();
                return Ok(result);
        }



        [HttpGet]
        [Route("listarAtivas")]
        public IActionResult listarAtivas()
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            if (usuarioLogado.TipoUsuario == "Administrador")
            {

                var result = context.Empresa
                  .Select(m => new
                  {
                      m.IdEmpresa,
                      m.Nome,
                      m.TipoPessoa,
                      m.RazaoSocial,
                      m.CpfCnpj,
                      m.Telefone1,
                      m.Telefone2,
                      m.Email,
                      m.Cep,
                      m.Logradouro,
                      m.Numero,
                      m.Complemento,
                      m.Bairro,
                      m.Cidade,
                      m.Estado,
                      m.Referencia,
                      m.InscricaoEstadual,
                      m.InscricaoMunicipal,
                      m.Situacao
                  }).Where(x => x.Situacao == "Ativo").ToList();
                return Ok(result);
            }
            else
            {
                var result = context.Empresa
                 .Select(m => new
                 {
                     m.IdEmpresa,
                     m.Nome,
                     m.TipoPessoa,
                     m.RazaoSocial,
                     m.CpfCnpj,
                     m.Telefone1,
                     m.Telefone2,
                     m.Email,
                     m.Cep,
                     m.Logradouro,
                     m.Numero,
                     m.Complemento,
                     m.Bairro,
                     m.Cidade,
                     m.Estado,
                     m.Referencia,
                     m.InscricaoEstadual,
                     m.InscricaoMunicipal,
                     m.Situacao
                 }).Where(x => x.Situacao == "Ativo" && x.IdEmpresa == usuarioLogado.IdEmpresa).ToList();
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("listarRegiao")]
        public IActionResult ListarRegiao()
        {
            var result = context.Regiao
                  .Select(m => new
                  {
                      m.IdRegiao,
                      m.NomeRegiao,
                      
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("salvar")]
        [Authorize]
        public IActionResult Salvar([FromBody] EmpresaRequest model)
        {

            Empresa empresa;
            if (model.IdEmpresa > 0)
            {
                empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == model.IdEmpresa);
                empresa.Alterar(
                    model.Nome,
                    model.TipoPessoa,
                    model.RazaoSocial,
                    model.CpfCnpj,
                    context.GrupoEmpresa.FirstOrDefault(x => x.IdGrupoEmpresa == model.IdGrupoEmpresa),
                    context.Regiao.FirstOrDefault(x => x.IdRegiao == model.IdRegiao),
                    context.RamoAtividade.FirstOrDefault(x => x.IdRamoAtividade == model.IdRamoAtividade),
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

                context.Update(empresa);
            }
            else
            {
                empresa = new Empresa(model.Nome, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, context.GrupoEmpresa.FirstOrDefault(x => x.IdGrupoEmpresa == model.IdGrupoEmpresa), context.Regiao.FirstOrDefault(x => x.IdRegiao == model.IdRegiao), context.RamoAtividade.FirstOrDefault(x => x.IdRamoAtividade == model.IdRamoAtividade), model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, User.Identity.Name);
                context.Empresa.Add(empresa);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("pesquisar")]
        public IActionResult Pesquisar([FromBody] PesquisarRequestModel model)
        {
            var result = context.Empresa.AsQueryable();
            switch (model.Chave)
            {
                case "Nome":
                    result = result.Where(x => x.Nome.Contains(model.Valor.ToUpper()));
                    break;
                case "RazaoSocial":
                    result = result.Where(x => x.RazaoSocial.Contains(model.Valor.ToUpper()));
                    break;
                case "CpfCnpj":
                    result = result.Where(x => x.CpfCnpj == model.Valor);
                    break;
                default:
                    // code block
                    break;
            }

            return Ok(result.Select(m => new
            {
                m.IdEmpresa,
                m.Nome,
                m.RazaoSocial,
                m.CpfCnpj,
                m.Situacao
            }).Take(500).ToList());
        }

        //[HttpGet]
        //[Route("ativarEmpresa")]
        //public IActionResult AtivarEmpresa(int idEmpresa)
        //{
        //    var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
        //    empresa.AtivarEmpresa(User.Identity.Name);

        //    const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    StringBuilder sb = new StringBuilder();
        //    Random rnd = new Random();

        //    for (int i = 0; i < 6; i++)
        //    {
        //        int index = rnd.Next(chars.Length);
        //        sb.Append(chars[index]);
        //    }
        //    string novasenha = sb.ToString();

        //    context.Update(empresa);
        //    var usuario = new Usuario(
        //        empresa.CpfCnpj,
        //        empresa,
        //        null,
        //        empresa.Nome,
        //        empresa.Email,
        //        "Empresa",
        //        novasenha,
        //        User.Identity.Name
        //        );
        //    context.Usuario.Add(usuario);
        //    EnviarEmailAtivacao(empresa.Nome, empresa.CpfCnpj, novasenha, empresa.Email);
        //    context.SaveChanges();

        //    return Ok();
        //}

        [HttpGet]
        [Route("desativarEmpresa")]
        public IActionResult DesativarEmpresa(int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            var usuarios = context.Usuario.Where(x => x.IdEmpresa == idEmpresa).ToList();

            usuarios.ForEach(usuario => usuario.Inativar(User.Identity.Name));
            empresa.DesativarEmpresa(User.Identity.Name);

            context.Usuario.UpdateRange(usuarios);
            context.Update(empresa);
            EnviarEmailDesativacao(empresa.Nome, empresa.Email);
            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("reativarEmpresa")]
        public IActionResult ReativarEmpresa(int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            var usuarios = context.Usuario.Where(x => x.IdEmpresa == idEmpresa).ToList();

            usuarios.ForEach(usuario => usuario.Reativar(User.Identity.Name));
            empresa.AtivarEmpresa(User.Identity.Name);

            context.Usuario.UpdateRange(usuarios);
            context.Update(empresa);
            EnviarEmailReativacao(empresa.Nome, empresa.Email);
            context.SaveChanges();

            return Ok();
        }


        [NonAction]
        public void EnviarEmailAtivacao(string nome, string login, string senha, string email)
        {
            string subject = nome + " Confirmação de Usuário";
            string templateHtml = $@"
                        <h1>Bem vindo a Só Varejo</h1>
                        <h2>Olá {nome}!</h2>

                        <p>É com grande alegria que recebemos você em nossa comunidade de clientes satisfeitos.</p>

                        <p>A partir de agora, você pode aproveitar todos os produtos e serviços disponíveis.</p>

                        <h3>Como começar?</h3>

                        <p>Faça login no aplicativo com seus dados de acesso.</p>

                        <p><strong>Usuário:</strong> {login}</p>
                        <p><strong>Senha temporária:</strong> {senha}</p>

                        <p><a href='https://www.sovarejo.com.br'>Clique aqui para acessar.</a></p>

                        <p>Equipe Só Varejo</p>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Confirmação de Cadastro");
            mailMsg.Subject = subject;
            mailMsg.To.Add(new MailAddress(email, "Usuário Confirmado!"));
            string html = templateHtml;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));


            smtpClientSendGrid.Credentials = credentialsSendGrid;

            try
            {
                smtpClientSendGrid.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
            return;
        }

        public void EnviarEmailDesativacao(string nome, string email)
        {
            string subject = nome + " Conta desativada em Só Varejo";
            string templateHtml = $@"
                <h1>Informação Importante sobre seu Acesso</h1>
                <h2>Olá {nome},</h2>

                <p>Gostaríamos de informar que seu acesso à plataforma <strong>Só Varejo</strong> foi desativado.</p>

                <p>Caso tenha alguma dúvida ou precise de mais informações, entre em contato com nossa equipe de suporte.</p>

                <h3>O que fazer agora?</h3>

                <p>Se você acredita que esta desativação foi um engano ou deseja reativar seu acesso, por favor, entre em contato conosco.</p>

                <p><a href='https://www.sovarejo.com.br/contato'>Clique aqui para falar com o suporte.</a></p>

                <p>Agradecemos pela sua participação.</p>

                <p>Equipe Só Varejo</p>";


            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Conta desativada em Só Varejo");
            mailMsg.Subject = subject;
            mailMsg.To.Add(new MailAddress(email, "Conta desativada!"));
            string html = templateHtml;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));


            smtpClientSendGrid.Credentials = credentialsSendGrid;

            try
            {
                smtpClientSendGrid.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
        }


        public void EnviarEmailReativacao(string nome, string email)
        {
            string subject = nome + " Conta reativada em Só Varejo";
            string templateHtml = $@"
                <h1>Seu Acesso foi Reativado!</h1>
                <h2>Olá {nome},</h2>

                <p>Temos boas notícias! Seu acesso à plataforma <strong>Só Varejo</strong> foi reativado com sucesso.</p>

                <p>Agora você pode continuar aproveitando nossos produtos e serviços normalmente.</p>

                <h3>Como acessar?</h3>

                <p>Basta fazer login na plataforma utilizando suas credenciais habituais.</p>

                <p><a href='https://www.sovarejo.com.br'>Clique aqui para acessar.</a></p>

                <p>Se precisar de qualquer suporte, nossa equipe está à disposição.</p>

                <p>Bem-vindo de volta!</p>

                <p>Equipe Só Varejo</p>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Conta reativada em Só Varejo");
            mailMsg.Subject = subject;
            mailMsg.To.Add(new MailAddress(email, "Conta reativada!"));
            string html = templateHtml;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            smtpClientSendGrid.Credentials = credentialsSendGrid;

            try
            {
                smtpClientSendGrid.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == id);
            empresa.Excluir(User.Identity.Name);

            context.Update(empresa);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var empresa = context.Empresa.Include(x => x.GrupoEmpresa).FirstOrDefault(x => x.IdEmpresa == id);
            if (empresa == null)
                return BadRequest("Empresa não encontrada ");

            return Ok(new EmpresaResponse()
            {
                IdEmpresa = empresa.IdEmpresa,
                Nome = empresa.Nome,
                TipoPessoa = empresa.TipoPessoa,
                RazaoSocial = empresa.RazaoSocial,
                CpfCnpj = empresa.CpfCnpj,
                IdGrupoEmpresa = empresa.IdGrupoEmpresa,
                IdRegiao = empresa.IdRegiao,
                //NomeRegiao = empresa.Regiao.NomeRegiao,
                IdRamoAtividade = empresa.IdRamoAtividade,
                Telefone1 = empresa.Telefone1,
                Telefone2 = empresa.Telefone2,
                Email = empresa.Email,
                Cep = empresa.Cep,
                Logradouro = empresa.Logradouro,
                Numero = empresa.Numero,
                Complemento = empresa.Complemento,
                Bairro = empresa.Bairro,
                Cidade = empresa.Cidade,
                Estado = empresa.Estado,
                Referencia = empresa.Referencia,
                InscricaoEstadual = empresa.InscricaoEstadual,
                InscricaoMunicipal = empresa.InscricaoMunicipal,
                Situacao = empresa.Situacao
            });
        }


    }
}
