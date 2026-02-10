using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ERP.Infra;
using System;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;
using ERP.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using MySqlX.XDevAPI.Common;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UsuarioController : ControllerBase
    {

        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        private IConfiguration _config;
        protected Context context;

        public UsuarioController(Context context,
            IConfiguration config)
        {
            _config = config;
            this.context = context;
        }

        [HttpGet]
        [Route("listarUsuarios")]
        public IActionResult ListarUsuarios()
        {
            var result = context.Usuario
                  .Select(m => new
                  {
                      m.IdUsuario,
                      m.Nome,
                      m.Login,
                      m.TipoUsuario,
                      m.Situacao
                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("pesquisarUsuario")]
        public IActionResult PesquisarUsuario([FromBody] PesquisarUsuarioRequest model)
        {
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            var query = context.Usuario.AsQueryable();  

            if (model.Chave == "Nome")
            {
                query = query.Where(x => x.Nome == model.Valor);
            }
            else if (model.Chave == "TipoUsuario")
            {
                query = query.Where(x => x.TipoUsuario == model.Valor);
            }
            else if (model.Chave == "Login")
            {
                query = query.Where(x => x.Login ==  model.Valor);
            }
            else
            {
                return BadRequest("Chave informada incorreta");
            }

            var result = query.Select(
                m => new
                {
                    m.IdUsuario,
                    m.Nome,
                    m.Login,
                    m.TipoUsuario,
                    m.Situacao
                });

            return Ok(result);
        }

        [HttpGet]
        [Route("listarEmpresa")]
        public IActionResult ListarEmpresa()
        {
            var result = context.Empresa
                  .Select(m => new
                  {
                      m.IdEmpresa,
                      m.Nome,
                     
                  }).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("listarConsultor")]
        public IActionResult ListarConsultor()
        {
            var result = context.Consultor.Include(x => x.Pessoa)
                  .Select(m => new
                  {
                      m.IdPessoa,
                      m.Pessoa.Nome,

                  }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("alterar")]
        [Authorize]
        public IActionResult Alterar([FromBody] UsuarioModel model)
        {
            Consultor consultor;
            Cliente cliente;
            Usuario usuario;
            Afiliado afiliado;

                consultor = context.Consultor.FirstOrDefault(e => e.IdPessoa == model.IdConsultor);
                cliente = context.Cliente.FirstOrDefault(p => p.IdPessoa == model.IdCliente);
                usuario = context.Usuario.FirstOrDefault(x => x.IdUsuario == model.IdUsuario);
                afiliado = context.Afiliado.FirstOrDefault(a => a.IdPessoa == model.IdAfiliado);
            usuario.Alterar(
                    model.Login,
                    cliente,
                    consultor,
                    afiliado,
                    model.Nome,
                    model.Email,
                    model.TipoUsuario,
                    User.Identity.Name);

                context.Update(usuario);
                context.SaveChanges();
                return Ok();

        }


        [HttpPost]
        [Route("salvarUsuario")]
        [Authorize]
        public IActionResult SalvarUsuario([FromBody] UsuarioModel model)
        {
            var consultor = context.Consultor.FirstOrDefault(x => x.IdPessoa == model.IdConsultor);
            var usuarioLogado = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);
            var cliente = context.Cliente.FirstOrDefault(x => x.IdPessoa == model.IdCliente);
            var afiliado = context.Afiliado.FirstOrDefault(x => x.IdPessoa == model.IdAfiliado);
            var res = context.Usuario.FirstOrDefault(x => x.Login == model.Login);

            if (res != null)
                return BadRequest("Login já cadastrado");
            if (cliente == null && model.TipoUsuario == "Cliente")
                return BadRequest("Empresa não encontrada.");
            if (consultor == null && model.TipoUsuario == "Consultor")
                return BadRequest("Consultor não encontrada.");


            var usuario = new Usuario(
                login: model.Login,
                cliente : cliente,
                consultor: consultor,
                afiliado: afiliado,
                nome: model.Nome,
                email: model.Email,
                tipoUsuario: model.TipoUsuario,
                senha: model.Senha,
                usuarioInclusao: "admin");

            context.Usuario.Add(usuario);
            EnviarEmail(model);
            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("obterUsuario")]
        [Authorize]
        public IActionResult ObterUsuario(int idUsuario)
        {
            var usuario = context.Usuario.Include(p => p.Cliente).FirstOrDefault(x => x.IdUsuario == idUsuario);
            var consultor = context.Usuario.Include(p => p.Pessoa).FirstOrDefault(x => x.IdUsuario == idUsuario);

            if (usuario == null)
                return BadRequest("Usuário não encontrado ");

            var model = new UsuarioModel();
            model.TipoUsuario = usuario.TipoUsuario;
            model.NomeCliente = usuario.Cliente?.Pessoa.Nome; 
            model.NomeConsultor = consultor.Pessoa?.Nome; 
            model.IdEmpresa = usuario.IdEmpresa;
            model.IdUsuario = usuario.IdUsuario;
            model.Login = usuario.Login;
            model.Email = usuario.Email;
            model.Nome = usuario.Nome;
            model.Situacao = usuario.Situacao;

            return Ok(model);
        }


        [HttpGet]
        [Route("desativarUsuario")]
        [Authorize]
        public IActionResult DesativarUsuario(int idUsuario)
        {
            var usuario = context.Usuario.FirstOrDefault(x => x.IdUsuario == idUsuario);
            if (usuario == null)
                return BadRequest("Usuário não encontrado");

            usuario.Inativar(User.Identity.Name);

            context.Usuario.Update(usuario);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("ativarUsuario")]
        [Authorize]
        public IActionResult AtivarUsuario(int idUsuario)
        {
            var usuario = context.Usuario.FirstOrDefault(x => x.IdUsuario == idUsuario);
            if (usuario == null)
                return BadRequest("Usuário não encontrado");

            usuario.Ativar(User.Identity.Name);

            context.Usuario.Update(usuario);
            context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("resetarSenha")]
        [AllowAnonymous]
        public IActionResult ResetarSenha(int id)
        {
            var usuario = context.Usuario.FirstOrDefault(x => x.IdUsuario == id);

            if (usuario == null)
                return BadRequest("Usuário não encontrado");

            #region RandonSenha
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < 6; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }
            string novasenha = sb.ToString();
            #endregion

            usuario.PrimeiroAcesso = "S";
            usuario.Senha = novasenha;

            context.Update(usuario);
            context.SaveChanges();

            string subject = usuario.Nome + "Senha Resetada";
            string templateHtml = $"<h2> Resete de Senha </h2> <h3> Prezado úsuario, sua senha provisória para alteração é:</h3><h1>{novasenha}</h1>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Resetar Senha");
            mailMsg.Subject = subject;
            mailMsg.To.Add(new MailAddress(usuario.Email, "Senha resetada!"));
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
            return Ok();
        }

        [HttpGet]
        [Route("recuperarSenha")]
        [AllowAnonymous]
        public IActionResult RecuperarSenha(string email)
        {
            var usuario = context.Usuario.FirstOrDefault(x => x.Email == email);

            if (usuario == null)
                return BadRequest("Usuário não encontrado");

            #region RandonSenha
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < 6; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }
            string novasenha = sb.ToString();
            #endregion

            usuario.PrimeiroAcesso = "S";
            usuario.Senha = novasenha;

            context.Update(usuario);
            context.SaveChanges();

            string subject = usuario.Nome + "Senha Recuperada";
            string templateHtml = $"<h2> Recuperação de Senha </h2> <h3> Prezado úsuario, sua senha provisória para alteração é:</h3><h1>{novasenha}</h1>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Recuperar Senha");
            mailMsg.Subject = subject;
            mailMsg.To.Add(new MailAddress(usuario.Email, "Senha resetada!"));
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
            return Ok();
        }

        [HttpGet]
        [Route("recuperarSenhaLogin")]
        [AllowAnonymous]
        public IActionResult RecuperarSenhaLogin(string login)
        {
            var usuario = context.Usuario.FirstOrDefault(x => x.Login == login);

            if (usuario == null)
                return BadRequest("Usuário não encontrado");

            #region RandonSenha
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < 6; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }
            string novasenha = sb.ToString();
            #endregion

            usuario.PrimeiroAcesso = "S";
            usuario.Senha = novasenha;

            context.Update(usuario);
            context.SaveChanges();

            string subject = usuario.Nome + "Senha Recuperada";
            string templateHtml = $"<h2> Recuperação de Senha </h2> <h3> Prezado úsuario, sua senha provisória para alteração é:</h3><h1>{novasenha}</h1>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Recuperar Senha");
            mailMsg.Subject = subject;
            mailMsg.To.Add(new MailAddress(usuario.Email, "Senha resetada!"));
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
            return Ok();
        }

        [HttpGet]
        [Route("enviarEmail")]
        [AllowAnonymous]
        public IActionResult EnviarEmail([FromBody] UsuarioModel model)
        {
            //var usuario = context.Usuario.FirstOrDefault(x => x.IdUsuario == id);

            if (model == null)
                return BadRequest("Usuário não encontrado");

            //#region RandonSenha
            //const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //StringBuilder sb = new StringBuilder();
            // Random rnd = new Random();

            // for (int i = 0; i < 6; i++)
            // {
            //     int index = rnd.Next(chars.Length);
            //     sb.Append(chars[index]);
            // }
            // string novasenha = sb.ToString();
            //  #endregion

            //usuario.PrimeiroAcesso = "S";
            //  usuario.Senha = novasenha;

            // context.Update(usuario);
            // context.SaveChanges();

            string subject = $"Concicard - Confirmação de Cadastro - {model.Nome}";

            string templateHtml = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                    </head>
                    <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px;'>
                        <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
        
                            <div style='background-color: #2563eb; padding: 30px; text-align: center;'>
                                <h1 style='color: #ffffff; margin: 0; font-size: 24px;'>Bem-vindo à Concicard!</h1>
                            </div>
        
                            <div style='padding: 40px 30px;'>
                                <h2 style='color: #333333; margin-top: 0;'>Olá, {model.Nome}!</h2>
            
                                <p style='color: #666666; font-size: 16px; line-height: 1.6;'>
                                    Seu cadastro foi realizado com sucesso. Abaixo estão suas credenciais de acesso:
                                </p>
            
                                <div style='background-color: #f8f9fa; border-left: 4px solid #2563eb; padding: 20px; margin: 25px 0; border-radius: 4px;'>
                                    <p style='margin: 0 0 10px 0; color: #333333;'>
                                        <strong>Login:</strong> {model.Login}
                                    </p>
                                    <p style='margin: 0; color: #333333;'>
                                        <strong>Senha:</strong> {model.Senha}
                                    </p>
                                </div>
            
                                <p style='color: #666666; font-size: 14px; line-height: 1.6;'>
                                    Recomendamos que altere sua senha no primeiro acesso.
                                </p>
            
                                <div style='text-align: center; margin: 35px 0;'>
                                    <a href='https://app.concicard.com.br/' 
                                       style='background-color: #2563eb; color: #ffffff; padding: 14px 40px; text-decoration: none; border-radius: 6px; font-weight: bold; font-size: 16px; display: inline-block;'>
                                        Acessar o Sistema
                                    </a>
                                </div>
            
                                <p style='color: #999999; font-size: 12px; text-align: center; margin-top: 30px;'>
                                    Se você não solicitou este cadastro, por favor ignore este e-mail.
                                </p>
                            </div>
        
                            <div style='background-color: #f8f9fa; padding: 20px; text-align: center; border-top: 1px solid #eeeeee;'>
                                <p style='color: #999999; font-size: 12px; margin: 0;'>
                                    © 2025 Concicard - Todos os direitos reservados
                                </p>
                            </div>
                        </div>
                    </body>
                    </html>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Concicard");
            mailMsg.Subject = subject;
            mailMsg.To.Add(new MailAddress(model.Email, model.Nome));
            mailMsg.IsBodyHtml = true;
            mailMsg.Body = templateHtml;

            smtpClientSendGrid.Credentials = credentialsSendGrid;

            try
            {
                smtpClientSendGrid.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao enviar e-mail: {ex.Message}", ex);
            }
            return Ok();
        }

        [HttpGet]
        [Route("verificarLoginDuplicado")]
        [Authorize]
        public IActionResult VerificarLoginDuplicado(string login)
        {
            var res = context.Usuario.FirstOrDefault(x => x.Login == login);
            if (res == null)
                return Ok(true);
            else
                return Ok(false);
        }



        [HttpPost]
        [Route("alterarSenha")]
        [Authorize]
        public IActionResult AlterarSenha([FromBody] AlterarSenhaModel model)
        {
            var usuario = context.Usuario.FirstOrDefault(x => x.Login == User.Identity.Name);

            if (model.SenhaAtual != usuario.Senha)
                return BadRequest("Senha atual incorreta ");

            if(model.NovaSenha!=model.ConfirmarNovaSenha)
                return BadRequest("As senhas não coencidem!");


            usuario.AlterarSenha(model.NovaSenha);
            

            context.Update(usuario);
            context.SaveChanges();
            
            return Ok();
        }

        [NonAction]
        public void AlterarSenha()
        {
            var usuario = context.Usuario.FirstOrDefault();
            usuario.PrimeiroAcesso = "N";
            context.Update(usuario);
            context.SaveChanges();
        }

    }
}
