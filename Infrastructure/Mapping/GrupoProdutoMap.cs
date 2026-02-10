using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class GrupoProdutoMap : BaseModelMap<GrupoProduto>
    {
        public override void Configure(EntityTypeBuilder<GrupoProduto> builder)
        {
            builder.HasKey(c => c.IdGrupoProduto);
            builder.Property(c => c.Nome);

            base.Configure(builder);
        }
    }
}
