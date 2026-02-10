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
    public class TabelaPrecoMap : BaseModelMap<TabelaPreco>
    {
        public override void Configure(EntityTypeBuilder<TabelaPreco> builder)
        {
            builder.HasKey(c => c.IdTabelaPreco);
            builder.Property(c => c.Nome);
            builder.Property(c => c.DataInicio);
            builder.Property(c => c.DataTermino);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}

