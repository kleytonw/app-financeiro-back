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
    public class CaracteristicaMap : BaseModelMap<Caracteristica>
    {
        public override void Configure(EntityTypeBuilder<Caracteristica> builder)
        {
            builder.HasKey(c => c.IdCaracteristica);
            builder.Property(c => c.Nome);

            base.Configure(builder);
        }
    }
}

