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
    public class MovimentacaoDuplicataMap : BaseModelMap<MovimentacaoDuplicata>
    {
        public override void Configure(EntityTypeBuilder<MovimentacaoDuplicata> builder)
        {
            builder.HasKey(c => c.IdMovimentacaoDuplicata);

            builder.Property(c => c.NumeroDuplicata);
            builder.Property(c => c.ValorDuplicata);
            builder.Property(c => c.DataVencimento);

            builder.HasOne(x => x.Movimentacao)
                .WithMany(x=>x.Duplicatas)
                .HasForeignKey(x => x.IdMovimentacao);

            base.Configure(builder);
        }
    }
}
