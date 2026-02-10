using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ServicoNfse : BaseModel
    {
        public int IdServicoNfse { get; private set; }
        public string Codigo { get; private set; }
        public string CodigoNBS { get; private set; }
        public string Nome { get; private set; }
        public decimal AliquotaISS { get; private set; }
        public string DescricaoServico { get; private set; }

        public ServicoNfse() { }

        public ServicoNfse(string codigo, string codigoNBS, string nome, decimal aliquotaISS, string descricaoServico, string usuarioInclusao)
        {
            Codigo = codigo;
            CodigoNBS = codigoNBS;
            Nome = nome;
            AliquotaISS = aliquotaISS;
            DescricaoServico = descricaoServico;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string codigo, string codigoNBS, string nome, decimal aliquotaISS, string descricaoServico, string usuarioAlteracao)
        {
            Codigo = codigo;
            CodigoNBS = codigoNBS;
            Nome = nome;
            AliquotaISS = aliquotaISS;
            DescricaoServico = descricaoServico;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (string.IsNullOrWhiteSpace(Codigo))
                throw new Exception("O código é obrigatório");

            if (string.IsNullOrWhiteSpace(CodigoNBS))
                throw new Exception("O código NBS é obrigatório");
            if (string.IsNullOrWhiteSpace(Nome))
                throw new Exception("O nome é obrigatório");
            if (AliquotaISS < 0)
                throw new Exception("A alíquota do ISS deve ser maior ou igual a zero");
            if (string.IsNullOrWhiteSpace(DescricaoServico))
                throw new Exception("A descrição do serviço é obrigatória");

        }

    }
}
