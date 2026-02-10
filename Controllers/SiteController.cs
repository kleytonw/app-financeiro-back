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
using System.Net.Mail;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Net.Mime;

namespace ERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SiteController : ControllerBase
    {
        System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
        SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

        private IConfiguration _config;
        protected Context context;
        public SiteController(Context context, IConfiguration config)
        {
            _config = config;
            this.context = context;
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
                var cpfCnpjExistente = context.Empresa.Any(x => x.CpfCnpj == model.CpfCnpj);

                if (cpfCnpjExistente)
                {
                    return BadRequest("Já existe uma empresa cadastrada com este CPF/CNPJ.");
                }

                empresa = new Empresa(model.Nome, model.TipoPessoa, model.RazaoSocial, model.CpfCnpj, context.GrupoEmpresa.FirstOrDefault(x => x.IdGrupoEmpresa == model.IdGrupoEmpresa), context.Regiao.FirstOrDefault(x => x.IdRegiao == model.IdRegiao), context.RamoAtividade.FirstOrDefault(x => x.IdRamoAtividade == model.IdRamoAtividade), model.Telefone1, model.Telefone2, model.Email, model.Cep, model.Logradouro, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado, model.Referencia, model.InscricaoEstadual, model.InscricaoMunicipal, "empresaNova");
                empresa.SetUsuarioInlcusaoEmpresaNova();
                context.Empresa.Add(empresa);
                EnviarEmail(model.Nome, model.Email, model.RazaoSocial);
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("ativarEmpresa")]
        public IActionResult AtivarEmpresa(int idEmpresa)
        {
            var empresa = context.Empresa.FirstOrDefault(x => x.IdEmpresa == idEmpresa);
            empresa.AtivarEmpresa(User.Identity.Name);

            context.Update(empresa);
            context.SaveChanges();

            return Ok();
        }

        private void EnviarEmail(string nome, string email, string razaoSocial)
        {
            string subject = nome + "Solicitação de cadastro";
            string templateHtml = $@"
                        <h2>Bem vindo à Só Varejo! - Seu Cadastro Está em Análise!</h2>
                        <p>Olá {nome} e {razaoSocial}!</p>
                        <p>Seja bem-vindo à <strong>Só Varejo</strong>.</p>
                        <p>
                            Estamos muito felizes em tê-lo conosco. Queremos informar que seu cadastro foi recebido com sucesso e está atualmente em processo de análise. 
                            Em breve, um de nossos analistas irá revisar as informações e ativar sua conta.
                        </p>
                        <p>
                            Assim que seu cadastro for aprovado, você receberá um e-mail de confirmação com todos os detalhes para começar a usar seu aplicativo
                            de forma rápida e segura.
                        </p>
                        <p>
                            Fique tranquilo(a), estamos cuidando de tudo para garantir a melhor experiência possível. 
                            Se tiver alguma dúvida ou precisar de mais informações, nossa equipe de suporte está à disposição para ajudar.
                        </p>
                        <p>Obrigado por escolher o <strong>Só Varejo</strong>.</p>
                        <p>Atenciosamente,</p>
                        <p><strong>Equipe Só Varejo</strong></p>";


            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", "Solicitação de cadastro!");
            mailMsg.Subject = subject;
            mailMsg.To.Add(new MailAddress(email, "Solicitação de cadastro!"));
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
    }
}
