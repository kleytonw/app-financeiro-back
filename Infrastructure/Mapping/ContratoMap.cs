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
    public class ContratoMap : BaseModelMap<Contrato>
    {
        public override void Configure(EntityTypeBuilder<Contrato> builder)
        {
            builder.HasKey(c => c.IdContrato);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);
            builder.HasOne(x => x.Financeiro)
                .WithMany()
                .HasForeignKey(x => x.IdFinanceiro);
            builder.HasOne(x => x.Vendedor)
                .WithMany()
                .HasForeignKey(x => x.IdVendedor);
            builder.Property(c => c.DataInicio);
            builder.Property(c => c.DataTermino);
            builder.Property(c => c.ValorMensalidade);
            builder.Property(c => c.LinkContrato);
            builder.Property(c => c.Descricao);
            builder.HasOne(c => c.Plano)
                .WithMany()
                .HasForeignKey(c => c.IdPlano);
            builder.Property(c => c.ValorAdesao);
            builder.Property(c => c.DataAdesao);
            builder.Property(c => c.ContratoAdesao);
            builder.Property(c => c.DataPrimeiraMensalidade);
            builder.Property(c => c.ValorTotal);
            builder.Property(c => c.NumeroParcelas);
            builder.Property(c => c.ResponsavelNome);
            builder.Property(c => c.ResponsavelCpf);
            builder.Property(c => c.ResponsavelCargo);
            builder.Property(c => c.ResponsavelEmail);
            builder.Property(c => c.ResponsavelTelefone);
            builder.Property(c => c.ResponsavelCelular);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}