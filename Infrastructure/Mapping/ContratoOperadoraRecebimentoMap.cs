using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;

namespace ERP_API.Infrastructure.Mapping
{
    public class ContratoOperadoraRecebimentoMap : BaseModelMap<ContratoOperadoraRecebimento>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ContratoOperadoraRecebimento> builder)
        {
            builder.HasKey(x => x.IdContratoOperadoraRecebimento);
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
            base.Configure(builder);
        }
    }
}
