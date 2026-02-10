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
    public class ServicoMap : BaseModelMap<Servico>
    {
        public override void Configure(EntityTypeBuilder<Servico> builder)
        {
            builder.HasKey(c => c.IdServico);
            builder.Property(c => c.Nome);
            builder.Property(c => c.Valor);
            builder.Property(c => c.Descricao);


            base.Configure(builder);
        }
    }
}

