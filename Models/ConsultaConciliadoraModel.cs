using System;

namespace ERP_API.Models
{
    public class ConsultaConciliadoraModel
    {
        public int IdCliente {  get; set; }
        public DateTime DataInicio {  get; set; }
        public DateTime DataFim { get; set; }
        public int? Top { get; set; }
        public int? Skip { get; set; }
        public int? Adquirente { get; set; }
        public int? Produto { get; set; }
        public string Nsu {  get; set; }
        public int? Modalidade { get; set; }
        public string Email { get; set; }
    }


    public class ClientePessoaDto
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public string IdentificadorConciliadora { get; set; }
    }

    public class VendaConciliadaResumoDto
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public int Quantidade { get; set; }
        public decimal Total { get; set; }
    }


}
