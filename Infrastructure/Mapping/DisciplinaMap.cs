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
    public class DisciplinaMap : BaseModelMap<Disciplina>
    {
        public override void Configure(EntityTypeBuilder<Disciplina> builder)
        {
            builder.HasKey(c => c.IdDisciplina);
            builder.Property(c => c.Nome);

            base.Configure(builder);
        }
    }
}

