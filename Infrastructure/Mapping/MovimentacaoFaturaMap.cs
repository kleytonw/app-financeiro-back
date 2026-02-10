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
    public class MovimentacaoFaturaMap : BaseModelMap<MovimentacaoFatura>
    {
        public override void Configure(EntityTypeBuilder<MovimentacaoFatura> builder)
        {
            builder.HasKey(c => c.IdMovimentacaoFatura);

            builder.Property(c => c.ValorOriginal);
            builder.Property(c => c.ValorDesconto);
            builder.Property(c => c.ValorLiquido);

            builder.HasOne(x => x.Movimentacao)
                .WithMany()
                .HasForeignKey(x => x.IdMovimentacao);

            base.Configure(builder);
        }
    }
}
