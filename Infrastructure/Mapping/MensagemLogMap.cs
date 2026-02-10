using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class MensagemLogMap : BaseModelMap<MensagemLog>
    {
        public override void Configure(EntityTypeBuilder<MensagemLog> builder)
        {
            builder.HasKey(c => c.IdMensagemLog);
            builder.HasOne(c => c.Mensagem)
                .WithMany()
                .HasForeignKey(c => c.IdMensagem);
            builder.Property(c => c.LogMensagemErro);
            builder.Property(c => c.Descricao);
        }
    }
}
