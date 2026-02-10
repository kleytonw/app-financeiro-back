using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class TicketMap : BaseModelMap<Ticket>
    {
        public override void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(c => c.IdTicket);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdPessoa);
            builder.HasOne(c => c.TipoSuporte)
                .WithMany()
                .HasForeignKey(c => c.IdTipoSuporte);
            builder.Property(c => c.Titulo);
            builder.Property(c => c.Mensagem);
            builder.Property(c => c.Status);
            builder.Property(c => c.Situacao);
            builder.Property(c => c.DataAbertura);
            builder.Property(c => c.DataConclusao);
            builder.Property(c => c.DataAndamento);
            builder.Property(c => c.UsuarioAtendimento);
            builder.Property(c => c.UsuarioConclusao);

            base.Configure(builder);
        }
    }
}
