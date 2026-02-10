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
    public class ExtratoMap : BaseModelMap<Extrato>
    {
        public override void Configure(EntityTypeBuilder<Extrato> builder)
        {
            builder.HasKey(c => c.IdExtrato);
            builder.HasOne(c => c.ClienteContaBancaria)
                .WithMany()
                .HasForeignKey(c => c.IdClienteContaBancaria);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);
            builder.Property(c => c.Descricao);
            builder.Property(c => c.Valor);
            builder.Property(c => c.Tipo)
                .HasConversion<string>();
            builder.Property(c => c.DataLancamento);
            builder.Property(c => c.UniqueId);
            builder.Property(c => c.Situacao);
            builder.Property(c => c.Pagador);
            builder.Property(c => c.CpfCnpjPagador);
            builder.Property(c => c.Categoria);
            builder.Property(c => c.Banco);
            builder.Property(c => c.MetodoPagamento);

            base.Configure(builder);
        }
    }
}

