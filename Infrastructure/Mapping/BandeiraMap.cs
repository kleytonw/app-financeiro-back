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
    public class BandeiraMap : BaseModelMap<Bandeira>
    {
        public override void Configure(EntityTypeBuilder<Bandeira> builder)
        {
            builder.HasKey(c => c.IdBandeira);
            builder.Property(c => c.NomeBandeira);
            builder.Property(c => c.CodigoBandeiraCartao);
            builder.Property(c => c.CodigoBandeiraCartaoRede);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}
