using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;

namespace ERP_API.Infrastructure.Mapping
{
    public class ClienteParametrosMap : BaseModelMap<ClienteParametros>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ClienteParametros> builder)
        {
            builder.HasKey(c => c.IdClienteParametros);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);
            builder.Property(c => c.Chave);
            builder.Property(c => c.Valor);
            base.Configure(builder);
        }
    }
}
