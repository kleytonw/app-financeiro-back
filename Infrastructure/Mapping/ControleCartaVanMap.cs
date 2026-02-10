using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ControleCartaVanMap : BaseModelMap<ControleCartaVan>
    {
        public override void Configure(EntityTypeBuilder<ControleCartaVan> builder)
        {
            builder.HasKey(c => c.IdControleCartaVan);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);
            builder.HasOne(c => c.ClienteContaBancaria)
                .WithMany()
                .HasForeignKey(c => c.IdClienteContaBancaria);
            builder.HasOne(c => c.Etapa)
                .WithMany()
                .HasForeignKey(c => c.IdEtapa);
            builder.Property(c => c.Status);
            builder.Property(c => c.TicketFornecedor);
            builder.Property(c => c.Descricao);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}
