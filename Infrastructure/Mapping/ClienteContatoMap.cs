using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ClienteContatoMap : BaseModelMap<ClienteContato>
    {
        public override void Configure(EntityTypeBuilder<ClienteContato> builder)
        {
            builder.HasKey(c => c.IdClienteContato);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);
            builder.Property(c => c.Nome);
            builder.Property(c => c.DataNascimento);
            builder.Property(c => c.Email);
            builder.Property(c => c.Telefone);
            builder.Property(c => c.Cargo);
            builder.Property(c => c.Observacao);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}
