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
    public class UnidadeParametroMap : BaseModelMap<UnidadeParametro>
    {
        public override void Configure(EntityTypeBuilder<UnidadeParametro> builder)
        {
            builder.HasKey(c => c.IdUnidadeParametro);
            builder.HasOne(c => c.Unidade)
                .WithMany()
                .HasForeignKey(c => c.IdUnidade);
            builder.HasOne(c => c.Operadora)
                .WithMany()
                .HasForeignKey(c => c.IdOperadora);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);
            builder.Property(c => c.Chave);
            builder.Property(c => c.Valor);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}