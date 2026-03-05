using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class DependenteMap : BaseModelMap<Dependente>
    {
        public override void Configure(EntityTypeBuilder<Dependente> builder)
        {
            builder.HasKey(c => c.IdDependente);

            builder.Property(c => c.Nome);

            base.Configure(builder);
        }
    }
}
