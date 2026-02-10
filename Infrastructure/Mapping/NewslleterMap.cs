using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Models;
using ERP_API.Domain.Entidades;
using ERP.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class NewsletterMap : BaseModelMap<Newsletter>
    {
        public override void Configure(EntityTypeBuilder<Newsletter> builder)
        {
            builder.HasKey(c => c.IdNewsletter);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);

            builder.Property(c => c.Email);
            builder.Property(c => c.Data);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}
