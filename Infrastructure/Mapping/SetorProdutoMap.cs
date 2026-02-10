using Microsoft.EntityFrameworkCore;
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
    public class SetorProdutoMap : BaseModelMap<SetorProduto>
    {
        public override void Configure(EntityTypeBuilder<SetorProduto> builder)
        {
            builder.HasKey(c => c.IdSetorProduto);
            builder.HasOne(x => x.Produto)
                .WithMany()
                .HasForeignKey(x => x.IdProduto);

            builder.HasOne(x => x.Setor)
                .WithMany()
                .HasForeignKey(x => x.IdSetor);


            base.Configure(builder);
        }
    }
}
