using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ERP.Infra;
using ERP.Models.SecurityToken;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ERP.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        protected Context context;
        public LoginController(Context context,
            IConfiguration config)   // : base(context)
        {
            _config = config;
            this.context = context;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<dynamic>> AuthenticateAsync([FromBody] User model)
        {
            try
            {
                var usuario = context.Usuario.FirstOrDefault(x => x.Login == model.Username && x.Senha == model.Password);
                if (usuario == null)
                    return BadRequest("Usuário ou senha invalidos");
                if (usuario.Situacao != "Ativo")
                {
                    return BadRequest("Usuário inativo! Por favor, entre em contato com o suporte");
                }

                model.Role = usuario.TipoUsuario;
                var token = TokenService.GenerateToken(model);
                model.Password = "";

                var result = new TokenReturnModel()
                {
                    Token = token,
                    User = model,
                    TipoUsuario = usuario.TipoUsuario,
                    Situacao = usuario.Situacao,
                    IdERP = usuario.IdERPs,
                    IdCliente = usuario?.IdPessoa,
                    // IdUnidadeAtendimento = usuario.IdUnidadeAtendimento,
                    // IdProfissioalSaude = usuario.IdFuncionario,
                    PrimerioAcesso = usuario.PrimeiroAcesso
                };

                return result;
            }

            catch(Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}