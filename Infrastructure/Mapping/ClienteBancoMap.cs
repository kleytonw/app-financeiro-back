using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;


namespace ERP_API.Infrastructure.Mapping
{
    public class ClienteBancoMap : BaseModelMap<ClienteBanco>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ClienteBanco> builder)
        {
            builder.HasKey(c => c.IdClienteBanco);
            builder.HasOne(c => c.Banco)
                .WithMany()
                .HasForeignKey(c => c.IdBanco);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}
