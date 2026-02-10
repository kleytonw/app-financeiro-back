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
    public class MovimentacaoItemMap : BaseModelMap<MovimentacaoItem>
    {
        public override void Configure(EntityTypeBuilder<MovimentacaoItem> builder)
        {
            builder.HasKey(c => c.IdMovimentacaoItem);

            builder.Property(c => c.CodigoProd);
            builder.Property(c => c.CodigoEAN);
            builder.Property(c => c.NomeProduto);
            builder.Property(c => c.NCM);
            builder.Property(c => c.CFOP);
            builder.Property(c => c.Unidade);
            builder.Property(c => c.Quantidade);
            builder.Property(c => c.ValorUnitario);
            builder.Property(c => c.SubTotal);
            builder.Property(c => c.IdMovimentacao);

            builder.HasOne(x => x.Movimentacao)
             .WithMany()
             .HasForeignKey(x => x.IdMovimentacao);

            base.Configure(builder);
        }
    }
}