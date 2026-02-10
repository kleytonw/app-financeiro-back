using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class DiagnosticoMap : BaseModelMap<Diagnostico>
    {
        public override void Configure(EntityTypeBuilder<Diagnostico> builder)
        {
            builder.HasKey(c => c.IdDiagnostico);
            builder.Property(c => c.Data);
            builder.Property(c => c.QtdeTransacoes);
            builder.Property(c => c.QtdeVendas);
            builder.Property(c => c.QtdeTransacoesConciliadas);
            builder.Property(c => c.QtdeTransacoesInconsistentes);
            builder.Property(c => c.QtdeTransacoesNaoEncontradas);
            builder.Property(c => c.QtdeVendasConciliadas);
            builder.Property(c => c.QtdeVendasInconsistentes);
            builder.Property(c => c.QtdeVendasNaoEncontradas);
            builder.Property(c => c.StatusDiagnostico);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);
            builder.HasOne(c => c.Unidade)
               .WithMany()
               .HasForeignKey(c => c.IdUnidade);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}
