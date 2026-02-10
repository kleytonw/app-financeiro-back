using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Infrastructure.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(c => c.IdUsuario);
            builder.HasOne(c => c.Cliente)
               .WithMany()
               .HasForeignKey(c => c.IdPessoa);
            builder.HasOne(x => x.Consultor)
               .WithMany()
               .HasForeignKey(x => x.IdPessoa);
            builder.HasOne(x => x.Afiliado)
               .WithMany()
               .HasForeignKey(x => x.IdPessoa);

            builder.HasOne(x => x.Pessoa)
               .WithMany()
               .HasForeignKey(x => x.IdPessoa);

            builder.Property(c => c.Nome);
            builder.Property(c => c.Login);
            builder.Property(c => c.Senha);
            builder.Property(c => c.Email);

            builder.Property(c => c.PrimeiroAcesso);


            builder.HasOne(x => x.ERP)
               .WithMany()
               .HasForeignKey(x => x.IdERPs);


            builder.Property(c => c.UsuarioInclusao);
            builder.Property(c => c.DataInclusao);
            builder.Property(c => c.UsuarioAlteracao);
            builder.Property(c => c.DataAlteracao);
            builder.Property(c => c.UsuarioExclusao);
            builder.Property(c => c.DataExclusao);
            builder.Property(c => c.Situacao);
        }
    }
}
