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
    public class TabelaPrecoItemMap : BaseModelMap<TabelaPrecoItem>
    {
        public override void Configure(EntityTypeBuilder<TabelaPrecoItem> builder)
        {
            builder.HasKey(c => c.IdTabelaPrecoItem);
            builder.HasOne(c => c.TabelaPreco)
                .WithMany()
                .HasForeignKey(c => c.IdTabelaPreco);
            builder.HasOne(c => c.Produto)
                .WithMany()
                .HasForeignKey(c => c.IdProduto);
            builder.Property(c => c.ValorVenda);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}

