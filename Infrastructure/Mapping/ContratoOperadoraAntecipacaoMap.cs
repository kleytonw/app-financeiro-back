using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
namespace ERP_API.Infrastructure.Mapping
{
    public class ContratoOperadoraAntecipacaoMap : BaseModelMap<ContratoOperadoraAntecipacao>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ContratoOperadoraAntecipacao> builder)
        {
            builder.HasKey(x => x.IdContratoOperadoraAntecipacao);
            builder.HasOne(x => x.ContratoOperadora)
                .WithMany()
                .HasForeignKey(x => x.IdContratoOperadora);
            builder.HasOne(x => x.Bandeira)
                .WithMany()
                .HasForeignKey(x => x.IdBandeira);
            builder.HasOne(x => x.MeioPagamento)
                .WithMany()
                .HasForeignKey(x => x.IdMeioPagamento);
            builder.Property(x => x.NumeroDias);
            builder.Property(x => x.Valor);
            builder.Property(x => x.Percentual);
            base.Configure(builder);
        }
    }
}