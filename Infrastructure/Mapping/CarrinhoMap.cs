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
    public class CarrinhoMap : BaseModelMap<Carrinho>
    {
        public override void Configure(EntityTypeBuilder<Carrinho> builder)
        {
            builder.HasKey(c => c.IdCarrinho);
            builder.Property(c => c.DataFinal);

            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);


            builder.HasOne(c => c.Vendedor)
                 .WithMany()
                 .HasForeignKey(c => c.IdVendedor);

            builder.Property(c => c.Situacao);

            builder.HasMany(x => x.Itens);
            builder.Property(c => c.Total);


            base.Configure(builder);
        }
    }
}