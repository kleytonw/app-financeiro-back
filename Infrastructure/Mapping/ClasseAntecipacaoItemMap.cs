using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ERP_API.Infrastructure.Mapping
{
    public class ClasseAntecipacaoItemMap : BaseModelMap<ClasseAntecipacaoItem>
    {
        public override void Configure(EntityTypeBuilder<ClasseAntecipacaoItem> builder)
        {
            builder.HasKey(x => x.IdClasseAntecipacaoItem);
            builder.HasOne(x => x.ClasseAntecipacao)
                .WithMany()
                .HasForeignKey(x => x.IdClasseAntecipacao);
            builder.HasOne(x => x.Bandeira)
                .WithMany()
                .HasForeignKey(x => x.IdBandeira);
            builder.HasOne(x => x.MeioPagamento)
                .WithMany()
                .HasForeignKey(x => x.IdMeioPagamento);
            builder.Property(x => x.NumeroDias);
            builder.Property(x => x.Valor);
            builder.Property(x => x.Percentual);
            builder.Property(x => x.Situacao);
            base.Configure(builder);
        }
    }
}