using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;

namespace ERP_API.Infrastructure.Mapping
{
    public class ClienteERPMap : BaseModelMap<ClienteERP>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ClienteERP> builder)
        {
            builder.HasKey(c => c.IdClienteERP);
            builder.HasOne(c => c.Cliente)
                .WithMany(c=>c.ERPs)
                .HasForeignKey(c => c.IdCliente);

            builder.HasOne(c => c.ERPs)
                .WithMany()
                .HasForeignKey(c => c.IdERPs);
            
            
            base.Configure(builder);
        }
    }
}
