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
    public class UnidadeMap : BaseModelMap<Unidade>
    {
        public override void Configure(EntityTypeBuilder<Unidade> builder)
        {
            builder.HasKey(c => c.IdUnidade);

            builder.HasOne(c => c.Empresa)
                .WithMany(c => c.Unidades)
                .HasForeignKey(c => c.IdEmpresa);

            builder.Property(m => m.Nome);
            builder.Property(m => m.TipoPessoa);
            builder.Property(m => m.RazaoSocial);
            builder.Property(m => m.CpfCnpj);

            builder.HasOne(x => x.GrupoEmpresa)
                .WithMany()
                .HasForeignKey(x => x.IdGrupoEmpresa);

            builder.HasOne(x => x.Regiao)
                .WithMany()
                .HasForeignKey(x => x.IdRegiao);

            builder.HasOne(x => x.RamoAtividade)
                .WithMany()
                .HasForeignKey(x => x.IdRamoAtividade);
            builder.Property(m => m.Telefone1);
            builder.Property(m => m.Telefone2);
            builder.Property(m => m.Email);
            builder.Property(m => m.Cep);
            builder.Property(m => m.Logradouro);
            builder.Property(m => m.Numero);
            builder.Property(m => m.Complemento);
            builder.Property(m => m.Bairro);
            builder.Property(m => m.Cidade);
            builder.Property(m => m.Estado);
            builder.Property(m => m.Referencia);
            builder.Property(m => m.InscricaoEstadual);
            builder.Property(m => m.TokenTecnoSpeed);
            builder.Property(m => m.InscricaoMunicipal);
            builder.Property(m => m.UniqueId);

            base.Configure(builder);
        }
    }
}
