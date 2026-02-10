using ERP_API.Domain.Entidades;
using ERP.Infrastructure.Mapping;

namespace ERP_API.Infrastructure.Mapping
{
    public class ClienteAdquirenteMap : BaseModelMap<ClienteAdquirente>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ClienteAdquirente> builder)
        {
            builder.HasKey(c => c.IdClienteAdquirente);
            builder.HasOne(c => c.Operadora)
                .WithMany()
                .HasForeignKey(c => c.IdOperadora);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}
