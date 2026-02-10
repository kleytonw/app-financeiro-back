using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Models;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class BlogComentarioMap : BaseModelMap<BlogComentario>
    {
        public override void Configure(EntityTypeBuilder<BlogComentario> builder)
        {
            builder.HasKey(c => c.IdBlogComentario);
            builder.HasOne(c => c.Blog)
                   .WithMany()
                   .HasForeignKey(c => c.IdBlog);
            builder.Property(c => c.Nome);
            builder.Property(c => c.Email);
            builder.Property(c => c.Comentario);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}