using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ERP_API.Infrastructure.Mapping
{
    public class BIMargemPorcentagemMap : BaseModelMap<BIMargemPorcentagem>
    {
        public override void Configure(EntityTypeBuilder<BIMargemPorcentagem> builder)
        {
            builder.HasKey(x => x.IdMargemPorcentagem);
            builder.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.IdEmpresa);
            builder.HasOne(x => x.Unidade)
                .WithMany()
                .HasForeignKey(x => x.IdUnidade);
            builder.Property(x => x.Descricao);
            builder.Property(x => x.Ano);
            builder.Property(x => x.Janeiro);
            builder.Property(x => x.Fevereiro);
            builder.Property(x => x.Marco);
            builder.Property(x => x.Abril);
            builder.Property(x => x.Maio);
            builder.Property(x => x.Junho);
            builder.Property(x => x.Julho);
            builder.Property(x => x.Agosto);
            builder.Property(x => x.Setembro);
            builder.Property(x => x.Outubro);
            builder.Property(x => x.Novembro);
            builder.Property(x => x.Dezembro);
            builder.Property(x => x.Situacao);
            base.Configure(builder);
        }
    }
}
