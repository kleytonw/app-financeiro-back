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
    public class ContratoOperadoraMap : BaseModelMap<ContratoOperadora>
    {
        public override void Configure(EntityTypeBuilder<ContratoOperadora> builder)
        {
            builder.HasKey(c => c.IdContratoOperadora);
            builder.HasOne(c => c.Operadora)
                .WithMany()
                .HasForeignKey(c => c.IdOperadora);
            builder.Property(c => c.DataInicio);
            builder.Property(c => c.DataTermino);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);
            builder.HasOne(c => c.Unidade)
                .WithMany()
                .HasForeignKey(c => c.IdUnidade);
            builder.HasOne(c => c.ContaRecebimento)
            .WithMany()
            .HasForeignKey(c => c.IdContaRecebimento);
            builder.HasOne(c => c.ContaGravame)
                 .WithMany()
                 .HasForeignKey(c => c.IdContaGravame);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}
