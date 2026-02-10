using System;

namespace ERP.Models
{
    public class BaseViewModel
    {

        public string UsuarioInclusao { get; set; }
        public string UsuarioAlteracao { get; set; }
        public string UsuarioExclusao { get; set; }

        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataExclusao { get; set; }

        public string Situacao { get; set; }



    }
}
