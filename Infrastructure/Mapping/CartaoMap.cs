using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class CartaoMap : BaseModelMap<Cartao>
    {
        public override void Configure(EntityTypeBuilder<Cartao> builder)
        {
            builder.HasKey(c => c.IdCartao);
            builder.Property(c => c.Nome);
            builder.Property(c => c.Bandeira);
            builder.Property(c => c.UltimosDigitos);
            builder.Property(c => c.DiaFechamento);
            builder.Property(c => c.DiaVencimento);
            builder.Property(c => c.LimiteTotal);
            base.Configure(builder);
        }
    }
}
