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
    public class LancamentoItemMap : BaseModelMap<LancamentoItem>
    {
        public override void Configure(EntityTypeBuilder<LancamentoItem> builder)
        {
            builder.HasKey(c => c.IdLancamentoItem);
            builder.HasOne(c => c.Produto)
            .WithMany()
            .HasForeignKey(c => c.IdProduto);
            
            builder.HasOne(c => c.Carrinho)
                .WithMany(c=>c.Itens)
                .HasForeignKey(c => c.IdCarrinho);

            builder.Property(c => c.NomeProduto);
            builder.Property(c => c.Preco);
            builder.Property(c => c.Quantidade);
            builder.Property(c => c.Subtotal);
            builder.Property(c => c.Situacao);


            base.Configure(builder);
        }
    }
}
