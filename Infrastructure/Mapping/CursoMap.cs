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
    public class CursoMap : BaseModelMap<Curso>
    {
        public override void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.HasKey(c => c.IdCurso);
            builder.Property(c => c.NomeCurso);
            builder.Property(c => c.Valor);

            base.Configure(builder);
        }
    }
}

