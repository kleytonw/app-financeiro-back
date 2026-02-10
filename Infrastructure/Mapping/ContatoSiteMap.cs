using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class ContatoSiteMap : BaseModelMap<ContatoSite>
    {
        public override void Configure(EntityTypeBuilder<ContatoSite> builder)
        {
            builder.HasKey(c => c.IdContatoSite);
            builder.Property(c => c.NomeContato);
            builder.Property(c => c.Telefone);
            builder.Property(c => c.Email);
            builder.Property(c => c.Titulo);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);
            builder.Property(c => c.Mensagem);
            builder.Property(c => c.Data);

            base.Configure(builder);
        }
    }
}