using ERP.Infra;
using ERP.Models.SecurityToken;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UsuarioController(Context context) : ControllerBase
    {
        [HttpPost]
        [Route("cadastrar")]
        [AllowAnonymous]
        public IActionResult Cadastrar([FromBody] UsuarioRequest model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email))
                    return BadRequest("Email é obrigatório");

                if (string.IsNullOrEmpty(model.Senha))
                    return BadRequest("Senha é obrigatória");

                var usuarioExistente = context.Usuario.FirstOrDefault(x => x.Email == model.Email && x.Situacao == "Ativo");
                if (usuarioExistente != null)
                    return BadRequest("Email já cadastrado");

                var senhaHash = HashSenha(model.Senha);
                var usuario = new Usuario(model.Nome, model.Email, senhaHash, model.Telefone, model.Email);

                context.Usuario.Add(usuario);
                context.SaveChanges();

                var token = GerarToken(usuario);

                return Ok(new LoginResponse
                {
                    IdUsuario = usuario.IdUsuario,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UsuarioLoginRequest model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Senha))
                    return BadRequest("Email e senha são obrigatórios");

                var senhaHash = HashSenha(model.Senha);
                var usuario = context.Usuario.FirstOrDefault(x =>
                    x.Email == model.Email &&
                    x.Senha == senhaHash &&
                    x.Situacao == "Ativo");

                if (usuario == null)
                    return BadRequest("Email ou senha inválidos");

                var token = GerarToken(usuario);

                return Ok(new LoginResponse
                {
                    IdUsuario = usuario.IdUsuario,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("google")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginGoogle([FromBody] UsuarioGoogleRequest model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.GoogleToken))
                    return BadRequest("Token do Google é obrigatório");

                var payload = await GoogleJsonWebSignature.ValidateAsync(model.GoogleToken);

                if (payload == null)
                    return BadRequest("Token do Google inválido");

                var usuario = context.Usuario.FirstOrDefault(x =>
                    (x.GoogleId == payload.Subject || x.Email == payload.Email) &&
                    x.Situacao == "Ativo");

                if (usuario == null)
                {
                    usuario = Usuario.CriarComGoogle(
                        payload.Name,
                        payload.Email,
                        payload.Subject,
                        payload.Picture
                    );

                    context.Usuario.Add(usuario);
                    context.SaveChanges();
                }
                else if (string.IsNullOrEmpty(usuario.GoogleId))
                {
                    usuario.VincularGoogle(payload.Subject, payload.Email);
                    context.Update(usuario);
                    context.SaveChanges();
                }

                var token = GerarToken(usuario);

                return Ok(new LoginResponse
                {
                    IdUsuario = usuario.IdUsuario,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Token = token
                });
            }
            catch (InvalidJwtException)
            {
                return BadRequest("Token do Google inválido");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        public IActionResult Listar()
        {
            var result = context.Usuario.Where(x => x.Situacao == "Ativo")
                .Select(m => new UsuarioResponse
                {
                    IdUsuario = m.IdUsuario,
                    Nome = m.Nome,
                    Email = m.Email,
                    Telefone = m.Telefone,
                    Foto = m.Foto,
                    EmailConfirmado = m.EmailConfirmado,
                    PossuiGoogle = !string.IsNullOrEmpty(m.GoogleId),
                    Situacao = m.Situacao
                }).Take(500).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("obter")]
        [Authorize]
        public IActionResult Obter(int id)
        {
            var usuario = context.Usuario.FirstOrDefault(x => x.IdUsuario == id && x.Situacao == "Ativo");
            if (usuario == null)
                return BadRequest("Usuário não encontrado");

            return Ok(new UsuarioResponse
            {
                IdUsuario = usuario.IdUsuario,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                Foto = usuario.Foto,
                EmailConfirmado = usuario.EmailConfirmado,
                PossuiGoogle = !string.IsNullOrEmpty(usuario.GoogleId),
                Situacao = usuario.Situacao
            });
        }

        [HttpGet]
        [Route("perfil")]
        [Authorize]
        public IActionResult ObterPerfil()
        {
            var email = User.Identity?.Name;
            var usuario = context.Usuario.FirstOrDefault(x => x.Email == email && x.Situacao == "Ativo");

            if (usuario == null)
                return BadRequest("Usuário não encontrado");

            return Ok(new UsuarioResponse
            {
                IdUsuario = usuario.IdUsuario,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                Foto = usuario.Foto,
                EmailConfirmado = usuario.EmailConfirmado,
                PossuiGoogle = !string.IsNullOrEmpty(usuario.GoogleId),
                Situacao = usuario.Situacao
            });
        }

        [HttpPost]
        [Route("alterar")]
        [Authorize]
        public IActionResult Alterar([FromBody] UsuarioRequest model)
        {
            try
            {
                var usuario = context.Usuario.FirstOrDefault(x => x.IdUsuario == model.IdUsuario && x.Situacao == "Ativo");
                if (usuario == null)
                    return BadRequest("Usuário não encontrado");

                usuario.Alterar(model.Nome, model.Telefone, model.Foto, User.Identity?.Name);

                context.Update(usuario);
                context.SaveChanges();

                return Ok(new UsuarioResponse
                {
                    IdUsuario = usuario.IdUsuario,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Telefone = usuario.Telefone,
                    Foto = usuario.Foto,
                    EmailConfirmado = usuario.EmailConfirmado,
                    PossuiGoogle = !string.IsNullOrEmpty(usuario.GoogleId),
                    Situacao = usuario.Situacao
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("alterar-senha")]
        [Authorize]
        public IActionResult AlterarSenha([FromBody] AlterarSenhaRequest model)
        {
            try
            {
                var email = User.Identity?.Name;
                var usuario = context.Usuario.FirstOrDefault(x => x.Email == email && x.Situacao == "Ativo");

                if (usuario == null)
                    return BadRequest("Usuário não encontrado");

                var senhaAtualHash = HashSenha(model.SenhaAtual);
                if (usuario.Senha != senhaAtualHash)
                    return BadRequest("Senha atual incorreta");

                var novaSenhaHash = HashSenha(model.NovaSenha);
                usuario.AlterarSenha(novaSenhaHash, email);

                context.Update(usuario);
                context.SaveChanges();

                return Ok("Senha alterada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("excluir")]
        [Authorize]
        public IActionResult Excluir(int id)
        {
            var usuario = context.Usuario.FirstOrDefault(x => x.IdUsuario == id);
            if (usuario == null)
                return BadRequest("Usuário não encontrado");

            usuario.Excluir(User.Identity?.Name);

            context.Update(usuario);
            context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("vincular-google")]
        [Authorize]
        public async Task<IActionResult> VincularGoogle([FromBody] UsuarioGoogleRequest model)
        {
            try
            {
                var email = User.Identity?.Name;
                var usuario = context.Usuario.FirstOrDefault(x => x.Email == email && x.Situacao == "Ativo");

                if (usuario == null)
                    return BadRequest("Usuário não encontrado");

                var payload = await GoogleJsonWebSignature.ValidateAsync(model.GoogleToken);

                if (payload == null)
                    return BadRequest("Token do Google inválido");

                var googleJaVinculado = context.Usuario.Any(x => x.GoogleId == payload.Subject && x.IdUsuario != usuario.IdUsuario);
                if (googleJaVinculado)
                    return BadRequest("Esta conta Google já está vinculada a outro usuário");

                usuario.VincularGoogle(payload.Subject, email);
                context.Update(usuario);
                context.SaveChanges();

                return Ok("Conta Google vinculada com sucesso");
            }
            catch (InvalidJwtException)
            {
                return BadRequest("Token do Google inválido");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static string HashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

        private static string GerarToken(Usuario usuario)
        {
            var user = new User
            {
                Id = usuario.IdUsuario,
                Username = usuario.Email,
                Role = "Usuario"
            };
            return TokenService.GenerateToken(user);
        }
    }
}
