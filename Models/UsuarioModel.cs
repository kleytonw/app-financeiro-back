namespace ERP_API.Models
{
    public class UsuarioRequest
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string Foto { get; set; }
    }

    public class UsuarioResponse
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Foto { get; set; }
        public bool EmailConfirmado { get; set; }
        public bool PossuiGoogle { get; set; }
        public string Situacao { get; set; }
    }

    public class UsuarioLoginRequest
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class UsuarioGoogleRequest
    {
        public string GoogleToken { get; set; }
    }

    public class AlterarSenhaRequest
    {
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
    }

    public class LoginResponse
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
