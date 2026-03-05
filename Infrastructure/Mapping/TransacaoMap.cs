using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class TransacaoMap : BaseModelMap<Transacao>
    {
        public override void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.HasKey(t => t.IdTransacao);
            builder.HasOne(t => t.Categoria)
                   .WithMany()
                   .HasForeignKey(t => t.IdCategoria);
            builder.HasOne(t => t.Cartao)
                .WithMany()
                .HasForeignKey(t => t.IdCartao);
            builder.HasOne(t => t.Dependente)
                .WithMany()
                .HasForeignKey(t => t.IdDependente);
            builder.Property(t => t.NumeroParcelas);
            builder.Property(t => t.ParcelaAtual);
            builder.Property(t => t.DataCompra);
            builder.Property(t => t.Valor);
            builder.Property(t => t.Descricao);
            base.Configure(builder);
        }
    }
}
