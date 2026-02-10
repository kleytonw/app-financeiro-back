using ERP.Infrastructure.Mapping;
using ERP.Models;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ERP_API.Infrastructure.Mapping
{
    public class CategoriaMap : BaseModelMap<Categoria>
    {
        public override void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(c => c.IdCategoria);
            builder.Property(c => c.Nome);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}
