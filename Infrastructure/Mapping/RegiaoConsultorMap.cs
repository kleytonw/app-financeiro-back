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
    public class RegiaoConsultorMap : BaseModelMap<RegiaoConsultor>
    {
        public override void Configure(EntityTypeBuilder<RegiaoConsultor> builder)
        {
            builder.HasKey(c => c.IdRegiaoConsultor);
            builder.HasOne(x => x.Regiao)
                .WithMany()
                .HasForeignKey(x => x.IdRegiao);

            builder.HasOne(x => x.Consultor)
                .WithMany()
                .HasForeignKey(x => x.IdPessoa);


            base.Configure(builder);
        }
    }
}
