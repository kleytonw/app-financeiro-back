using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ControleCartaVanHistoricoMap: BaseModelMap<ControleCartaVanHistorico>
    {
        public override void Configure(EntityTypeBuilder<ControleCartaVanHistorico> builder)
        {
            builder.HasKey(c => c.IdControleCartaVanHistorico);
            builder.HasOne(c => c.ControleCartaVan)
                .WithMany()
                .HasForeignKey(c => c.IdControleCartaVan);
            builder.HasOne(c => c.Etapa)
                .WithMany()
                .HasForeignKey(c => c.IdEtapa);
            builder.Property(c => c.Data);
            builder.Property(c => c.Descricao);
            builder.Property(c => c.Guid);
            builder.Property(c => c.EnviarEmail);
            builder.Property(c => c.Email);
            builder.Property(c => c.Assunto);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}
