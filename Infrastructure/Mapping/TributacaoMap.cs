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
    public class TributacaoMap : BaseModelMap<Tributacao>
    {
        public override void Configure(EntityTypeBuilder<Tributacao> builder)
        {
            builder.HasKey(c => c.IdTributacao);
            builder.Property(c => c.Nome);

            base.Configure(builder);
        }
    }
}

