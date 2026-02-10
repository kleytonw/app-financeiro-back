using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        public int? IdConsultor { get; set; }
        public int? IdCliente { get; set; }
        public int? IdAfiliado { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string TipoUsuario { get; set; }

        public int? IdEmpresa { get; set; }
        public string NomeCliente { get; set; }
        public string NomeConsultor { get; set; }

        public int? IdUnidadeAtendimento { get; set; }

        public string NomeUnidadeAtendimento { get; set; }
        public int? IdProfissionalSaude { get; set; }
        public string NomeProfissionalSaude { get; set; }
        public string Situacao { get; set; }
    }

    public class PesquisarUsuarioRequest
    {
        public string Chave { get; set; }
        public string Valor { get; set; }
    }

    public class UsuarioEcomerceModel
    {
        public int IdUsuarioEcomerce { get; set; }
        public string LoginCPF { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public int IdEmpresa { get; set; }
    }

    public class ObterUsuarioEcomerceModel
    {
        public int IdUsuarioEcomerce { get; set; }
        public string LoginCPF { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Sexo { get; set; }
        public int IdEmpresa { get; set; }
        public string Celular { get; set; }
        public string Cep { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Referencia { get; set; }
    }

    public class AlterarSenhaModel
    {
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
        public string ConfirmarNovaSenha { get; set; }
   
    }

    // 4130039840


}