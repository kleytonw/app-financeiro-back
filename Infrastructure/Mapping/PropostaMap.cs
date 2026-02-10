using System;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class PropostaMap : BaseModelMap<Proposta>
    {
        public override void Configure(EntityTypeBuilder<Proposta> builder)
        {
            builder.HasKey(x => x.IdProposta);
            builder.Property(m => m.Nome);
            builder.Property(m => m.Sexo);
            builder.Property(m => m.DataNascimento);
            builder.Property(m => m.Mae);
            builder.Property(m => m.Pai);
            builder.Property(m => m.TipoPessoa);
            builder.Property(m => m.RazaoSocial);
            builder.Property(m => m.CpfCnpj);
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
            builder.Property(m => m.InscricaoMunicipal);
            builder.HasOne(x => x.Plano)
                .WithMany()
                .HasForeignKey(x => x.IdPlano);
            builder.HasOne(x => x.Vendedor)
                .WithMany()
                .HasForeignKey(x => x.IdVendedor);
            builder.Property(x => x.DataInicio);
            builder.Property(x => x.DataTermino);
            builder.Property(x => x.StatusProposta);
            base.Configure(builder);
        }
    }
}
