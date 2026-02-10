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
    public class AfiliadoMap : BaseModelMap<Afiliado>
    {
        public override void Configure(EntityTypeBuilder<Afiliado> builder)
        {
            builder.HasKey(c => c.IdPessoa);

            builder.HasOne(x => x.Pessoa)
                  .WithMany()
                  .HasForeignKey(x => x.IdPessoa);

            base.Configure(builder);
        }
    }
}
