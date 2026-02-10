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
    public class BancoMap : BaseModelMap<Banco>
    {
        public override void Configure(EntityTypeBuilder<Banco> builder)
        {
            builder.HasKey(c => c.IdBanco);
            builder.Property(c => c.NomeBanco);
            builder.Property(c => c.CodigoBancoTecnoSpeed);
            builder.Property(c => c.PossuiIntegracaoTecnospeed);
            builder.Property(c => c.Situacao);
            builder.Property(c => c.Situacao);
            builder.Property(c => c.IdOpenFinance);

            base.Configure(builder);
        }
    }
}

