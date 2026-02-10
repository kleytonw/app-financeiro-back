using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ClasseRecebimentoItemMap : BaseModelMap<ClasseRecebimentoItem>
    {
        public override void Configure(EntityTypeBuilder<ClasseRecebimentoItem> builder)
        {
            builder.HasKey(x => x.IdClasseRecebimentoItem);
            builder.HasOne(x => x.ClasseRecebimento)
                .WithMany()
                .HasForeignKey(x => x.IdClasseRecebimento);
            builder.HasOne(x => x.Bandeira)
                .WithMany()
                .HasForeignKey(x => x.IdBandeira);
            builder.HasOne(x => x.MeioPagamento)
                .WithMany()
                .HasForeignKey(x => x.IdMeioPagamento);
            builder.Property(x => x.NumeroDias);
            builder.Property(x => x.Situacao);
            base.Configure(builder);
        }
    }
}
