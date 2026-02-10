using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP_API.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class MovimentacaoEstoqueMap : BaseModelMap<MovimentacaoEstoque>
    {
        public override void Configure(EntityTypeBuilder<MovimentacaoEstoque> builder)
        {
            builder.HasKey(c => c.IdMovimentacaoEstoque);
            builder.HasOne(c => c.Produto)
                .WithMany()
                .HasForeignKey(c => c.IdProduto);
            builder.Property(c => c.Data);
            builder.Property(c => c.Tipo);
            builder.Property(c => c.Quantidade);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}