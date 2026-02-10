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
    public class PlanoMap : BaseModelMap<Plano>
    {
        public override void Configure(EntityTypeBuilder<Plano> builder)
        {
            builder.HasKey(c => c.IdPlano);
            builder.Property(c => c.Nome);
            builder.Property(c => c.Valor);
            builder.Property(c => c.ValorAdesao);
            builder.Property(c => c.ValorRepasse);
            builder.Property(c => c.Descricao);
            builder.Property(f => f.QuantidadeVendasInicial);
            builder.Property(f => f.QuantidadeVendasFinal);

            base.Configure(builder);
        }
    }
}
