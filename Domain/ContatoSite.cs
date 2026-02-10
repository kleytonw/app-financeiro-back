using ERP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ERP.Domain
{
    public class ContatoSite : BaseModel
    {
        public int IdContatoSite { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Mensagem { get; set; }
        public ContatoSite() { }
    }      
}
