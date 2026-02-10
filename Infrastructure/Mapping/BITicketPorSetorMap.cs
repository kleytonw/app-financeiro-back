using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Hangfire.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ERP_API.Infrastructure.Mapping
{
    public class BITicketPorSetorMap : BaseModelMap<BITicketPorSetor>
    {
        public override void Configure(EntityTypeBuilder<BITicketPorSetor> builder)
        {
            builder.HasKey(c => c.IdTicketPorSetor);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);
            builder.HasOne(c => c.Unidade)
                .WithMany()
                .HasForeignKey(c => c.IdUnidade);
            builder.Property(c => c.Descricao);
            builder.Property(c => c.Ano);
            builder.Property(c => c.Janeiro);
            builder.Property(c => c.Fevereiro);
            builder.Property(c => c.Marco);
            builder.Property(c => c.Abril);
            builder.Property(c => c.Maio);
            builder.Property(c => c.Junho);
            builder.Property(c => c.Julho);
            builder.Property(c => c.Agosto);
            builder.Property(c => c.Setembro);
            builder.Property(c => c.Outubro);
            builder.Property(c => c.Novembro);
            builder.Property(c => c.Dezembro);
            builder.Property(c => c.UsuarioInclusao);
            base.Configure(builder);
        }
    }
}
