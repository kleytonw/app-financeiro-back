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
    public class MeioPagamentoMap : BaseModelMap<MeioPagamento>
    {
        public override void Configure(EntityTypeBuilder<MeioPagamento> builder)
        {
            builder.HasKey(c => c.IdMeioPagamento);
            builder.Property(c => c.NomeMeioPagamento);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}

