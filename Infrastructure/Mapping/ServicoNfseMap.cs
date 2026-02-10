using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ServicoNfseMap : BaseModelMap<ServicoNfse>
    {
        public override void Configure(EntityTypeBuilder<ServicoNfse> builder)
        {
            builder.HasKey(s => s.IdServicoNfse);
            builder.Property(s => s.Codigo);
            builder.Property(s => s.CodigoNBS);
            builder.Property(s => s.Nome);
            builder.Property(s => s.AliquotaISS);
            builder.Property(s => s.DescricaoServico);
            builder.Property(s => s.Situacao);
            base.Configure(builder);
        }
    }
}
