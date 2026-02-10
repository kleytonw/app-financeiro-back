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
    public class DisciplinaCursoMap : BaseModelMap<DisciplinaCurso>
    {
        public override void Configure(EntityTypeBuilder<DisciplinaCurso> builder)
        {
            builder.HasKey(c => c.IdDisciplinaCurso);
            builder.HasOne(x => x.Curso)
                .WithMany()
                .HasForeignKey(x => x.IdCurso);

            builder.HasOne(x => x.Disciplina)
                .WithMany()
                .HasForeignKey(x => x.IdDisciplina);


            base.Configure(builder);
        }
    }
}
