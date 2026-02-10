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
    public class ParceiroParametroMap : BaseModelMap<ParceiroParametro>
    {
        public override void Configure(EntityTypeBuilder<ParceiroParametro> builder)
        {
            builder.HasKey(c => c.IdParceiroParametro);
            builder.HasOne(c => c.ParceiroSistema)
                .WithMany()
                .HasForeignKey(c => c.IdParceiroSistema);
            builder.Property(c => c.Chave);
            builder.Property(c => c.Valor);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}
