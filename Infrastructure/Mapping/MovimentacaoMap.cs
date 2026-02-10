using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class MovimentacaoMap : BaseModelMap<Movimentacao>
    {
        public override void Configure(EntityTypeBuilder<Movimentacao> builder)
        {
            builder.HasKey(c => c.IdMovimentacao);

            builder.Property(t => t.IdEmpresa);
            builder.Property(t => t.CodigoUF);
            builder.Property(m => m.CodigoNF);
            builder.Property(m => m.NaturezaOperacao);
            builder.Property(m => m.IndicadorFormaPagamento);
            builder.Property(m => m.Modelo);
            builder.Property(m => m.Serie);
            builder.Property(m => m.NumeroNF);
            builder.Property(m => m.DataHoraEmissao);
            builder.Property(m => m.DataHoraSaiEntrada);

            builder.Property(t => t.EmitenteCNPJ);
            builder.Property(m => m.EmitenteNome);
            builder.Property(m => m.EmitenteFantasia);
            builder.Property(m => m.EmitenteIE);
            builder.Property(m => m.EmitenteIEST);
            builder.Property(m => m.EmitenteCRT);

            builder.Property(t => t.EmitenteLogradouro);
            builder.Property(m => m.EmitenteNumero);
            builder.Property(m => m.EmitenteBairro);
            builder.Property(m => m.EmitenteCodigoMunicipio);
            builder.Property(m => m.EmitenteMunicipio);
            builder.Property(m => m.EmitenteUF);
            builder.Property(m => m.EmitenteCEP);
            builder.Property(m => m.EmitenteCodigoPais);
            builder.Property(m => m.EmitentePais);

            builder.Property(t => t.DestinatarioCNPJ);
            builder.Property(m => m.DestinatarioCPF);
            builder.Property(m => m.DestinatarioNome);
            builder.Property(m => m.DestinatarioEmail);

            builder.Property(t => t.DestinatarioLogradouro);
            builder.Property(m => m.DestinatarioNumero);
            builder.Property(m => m.DestinatarioBairro);
            builder.Property(m => m.DestinatarioCodigoMunicipio);
            builder.Property(m => m.DestinatarioMunicipio);
            builder.Property(m => m.DestinatarioUF);
            builder.Property(m => m.DestinatarioCEP);
            builder.Property(m => m.DestinatarioCodigoPais);
            builder.Property(m => m.DestinatarioPais);

            builder.Property(m => m.Total);
            builder.Property(m => m.ValorICMS);
            builder.Property(m => m.ValorBC);
            builder.Property(m => m.ValorBCST);
            builder.Property(m => m.ValorST);
            builder.Property(m => m.ValorProdutos);
            builder.Property(m => m.ValorFrete);
            builder.Property(m => m.ValorSeguro);
            builder.Property(m => m.ValorDesconto);
            builder.Property(m => m.ValorVLL);
            builder.Property(m => m.ValorIPI);
            builder.Property(m => m.ValorPIS);
            builder.Property(m => m.ValorCofins);
            builder.Property(m => m.ValorOutro);
            builder.Property(m => m.ValorNF);
            builder.Property(m => m.ValorTotalTributos);

            builder.Property(m => m.ChaveAcesso);
            builder.Property(m => m.TipoMovimentacao);


            base.Configure(builder);
        }
    }
}