using System;

namespace ERP.Domain.ViewModel
{
    public class RelAgenda
    {
            public int IdAgenda { get; set; }
            public string NomePaciente { get; set; }
            public string Cpf { get; set; }
            public string Email { get; set; }
            public DateTime DataNascimento { get; set; }
            public string Carterinha { get; set; }
            public string NomeUnidadeAtendimento { get; set; }
            public int IdUnidadeAtendimento { get; set; }
            public int IdConvenio { get; set; }
            public string NomeConvenio { get; set; }
            public string Retorno { get; set; }
            public DateTime Data { get; set; }
            public string Situacao { get; set; }
            public string Tipo { get; set; }
            public string SenhaAutorizacao { get; set; }
            public int IdEmpresa { get; set; }
            public int IdAgendaProcedimento { get; set; }
            public int IdProcedimento { get; set; }
            public string NomeEmpresa { get; set; }
            public string AtendimentoRN { get; set; }
            public string CodigoProcedimento { get; set; }
            public string NomeProcedimento { get; set; }
            public string TipoProcedimento { get; set; }
            public decimal ValorEmpresa { get; set; }
            public decimal ValorPrestador { get; set; }
            public decimal ValorProfissional { get; set; }

    }
}
