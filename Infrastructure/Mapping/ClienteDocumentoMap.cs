using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;

namespace ERP_API.Infrastructure.Mapping
{
    public class ClienteDocumentoMap : BaseModelMap<ClienteDocumento>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ClienteDocumento> builder)
        {
            builder.HasKey(c => c.IdClienteDocumento);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);
            builder.HasOne(c => c.TipoDocumento)
                .WithMany()
                .HasForeignKey(c => c.IdTipoDocumento);
            builder.Property(c => c.Arquivo);
            base.Configure(builder);
        }
    }
}
