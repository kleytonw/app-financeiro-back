using ERP.Infrastructure.Mapping;
using ERP.Models;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class EtapaMap : BaseModelMap<Etapa>
    {
        public override void Configure(EntityTypeBuilder<Etapa> builder)
        {
            builder.HasKey(c => c.IdEtapa);
            builder.Property(c => c.Nome);
            builder.Property(c => c.Descricao);
            builder.Property(c => c.EtapaConcluida);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}
