using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class BlogMap : BaseModelMap<Blog>
    {
        public override void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(c => c.IdBlog);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);
            builder.Property(c => c.Autor);
            builder.Property(c => c.Titulo);
            builder.Property(c => c.Subtitulo);
            builder.Property(c => c.Descricao);
            builder.Property(c => c.Data);
            builder.Property(c => c.LinkFoto);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}
