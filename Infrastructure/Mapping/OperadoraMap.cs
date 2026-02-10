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
    public class OperadoraMap : BaseModelMap<Operadora>
    {
        public override void Configure(EntityTypeBuilder<Operadora> builder)
        {
            builder.HasKey(c => c.IdOperadora);
            builder.Property(c => c.NomeOperadora);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}

