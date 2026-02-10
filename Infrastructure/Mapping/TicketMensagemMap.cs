using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class TicketMensagemMap : BaseModelMap<TicketMensagem>
    {
        public override void Configure(EntityTypeBuilder<TicketMensagem> builder)
        {
            builder.HasKey(c => c.IdTicketMensagem);
            builder.HasOne(c => c.Ticket)
                .WithMany()
                .HasForeignKey(c => c.IdTicket);
            builder.Property(c => c.Mensagem);
            builder.Property(c => c.Arquivo);
            builder.Property(c => c.Data);
            builder.Property(c => c.Usuario);
            base.Configure(builder);
        }
    }
}
