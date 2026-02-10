using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empresas.Domain.ViewModel.Relatorios
{
    public class RelAniversariante
    {
        public int IdPaciente { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public string Telefone1 { get; set; }
        public string Email { get; set; }
        public DateTime? DataNascimento { get; set; }
        public int Idade { get; set; }
    }
}