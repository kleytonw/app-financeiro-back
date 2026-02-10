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
    public class PessoaMap : BaseModelMap<Pessoa>
    {
        public override void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.HasKey(c => c.IdPessoa);
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

            base.Configure(builder);
        }
    }
}