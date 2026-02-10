using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class ContratoOperadoraTaxaMap : BaseModelMap<ContratoOperadoraTaxa>
    {
        public override void Configure(EntityTypeBuilder<ContratoOperadoraTaxa> builder)
        {
            builder.HasKey(c => c.IdContratoOperadoraTaxa);
            builder.HasOne(c => c.ContratoOperadora)
                .WithMany()
                .HasForeignKey(c => c.IdContratoOperadora);
            builder.HasOne(c => c.MeioPagamento)
                .WithMany()
                .HasForeignKey(c => c.IdMeioPagamento);
            builder.HasOne(c => c.Bandeira)
                .WithMany()
                .HasForeignKey(c => c.IdBandeira);
            builder.Property(c => c.Taxa);
            builder.Property(c => c.Valor);
            builder.Property(c => c.ParcelaInicio);
            builder.Property(c => c.ParcelaFim);
            builder.Property(c => c.Tipo);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);
            builder.HasOne(c => c.Unidade)
                .WithMany()
                .HasForeignKey(c => c.IdUnidade);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}
