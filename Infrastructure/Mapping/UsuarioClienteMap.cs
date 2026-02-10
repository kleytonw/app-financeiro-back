using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class UsuarioClienteMap : BaseModelMap<UsuarioCliente>
    {
        public override void Configure(EntityTypeBuilder<UsuarioCliente> builder)
        {
            builder.HasKey(c => c.IdUsuarioCliente);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);
            builder.HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.IdUsuario);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}
