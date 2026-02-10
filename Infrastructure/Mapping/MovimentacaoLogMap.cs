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
    public class MovimentacaoLogMap : BaseModelMap<MovimentacaoLog>
    {
        public override void Configure(EntityTypeBuilder<MovimentacaoLog> builder)
        {
            builder.HasKey(c => c.IdMovimentacaoLog);
            builder.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.IdEmpresa);
            builder.Property(x => x.DataMovimentacaoLog);


            base.Configure(builder);
        }
    }
}
