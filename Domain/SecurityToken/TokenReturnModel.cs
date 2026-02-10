using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models.SecurityToken
{
    public class TokenReturnModel
    {
        public User User { get; set; }
        public string Token { get; set; }
        public string TipoUsuario { get; set; }
        public int? IdCliente { get; set; }

        public int? IdERP { get; set; }
        public string PrimerioAcesso { get; set; }
        public string Situacao { get; set; }
    }

    public class TokenReturAppModel
    { 
        public string Token { get; set; } 
        // public DateTime Expiration { get; set; }
    }
}
