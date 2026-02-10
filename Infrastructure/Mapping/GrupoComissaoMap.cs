using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class GrupoComissaoMap : BaseModelMap<GrupoComissao>
    {
        public override void Configure(EntityTypeBuilder<GrupoComissao> builder)
        {
            builder.HasKey(c => c.IdGrupoComissao);
            builder.Property(c => c.Nome);

            base.Configure(builder);
        }
    }
}