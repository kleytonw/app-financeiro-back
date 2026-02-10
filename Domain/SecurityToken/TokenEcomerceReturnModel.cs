using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models.SecurityToken
{
    public class TokenEcomerceReturnModel
    {
        public int IdUsuarioEcomerce { get; set; }
        public string LoginCPF { get; set; }
        public string Email { get; set; }
        public int? IdEmpresa { get; set; }
        public string Token { get; set; }
    }
}
