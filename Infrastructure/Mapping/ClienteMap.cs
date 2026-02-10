using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class ClienteMap : BaseModelMap<Cliente>
    {
        public override void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.IdPessoa);

            builder.HasOne(x => x.Pessoa)
                  .WithMany()
                  .HasForeignKey(x => x.IdPessoa);
            builder.Property(x => x.IdentificadorConciliadora);
            builder.Property(x => x.Senha);
            builder.Property(x => x.SenhaConciliadora);
            builder.Property(x => x.ApiKeyConciliadora);
            builder.Property(x => x.NomeResponsavel).IsRequired(false);
            builder.Property(x => x.CelularResponsavel).IsRequired(false);
            builder.Property(x => x.EmailResponsavel).IsRequired(false);
            builder.Property(x => x.NomeContratante).IsRequired(false);
            builder.Property(x => x.CelularContratante).IsRequired(false);
            builder.Property(x => x.EmailContratante).IsRequired(false);
            builder.HasMany(x => x.ERPs);
            builder.HasOne(x => x.Colaborador)
                  .WithMany()
                  .HasForeignKey(x => x.IdColaborador);
            builder.HasOne(x => x.Afiliado)
                .WithMany()
                .HasForeignKey(x => x.IdAfiliado);


            builder.Property(x => x.IdSacadoUnique);



            base.Configure(builder);
        }
    }
}
