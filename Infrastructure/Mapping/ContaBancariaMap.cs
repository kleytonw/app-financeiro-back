using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class ContaBancariaMap : BaseModelMap<ContaBancaria>
    {
        public override void Configure(EntityTypeBuilder<ContaBancaria> builder)
        {
            builder.HasKey(c => c.IdContaBancaria);
            builder.HasOne(c => c.Banco)
                .WithMany()
                .HasForeignKey(c => c.IdBanco);
            builder.Property(c => c.Agencia);
            builder.Property(c => c.DigitoAgencia);
            builder.Property(c => c.Conta);
            builder.Property(c => c.DigitoConta);
            builder.Property(c => c.CodigoSistema);
            builder.HasOne(c => c.Unidade)
                .WithMany()
                .HasForeignKey(c => c.IdUnidade);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);
            builder.HasOne(c => c.Operadora)
                .WithMany()
                .HasForeignKey(c => c.IdOperadora);
            builder.Property(c => c.HashDaConta);
            builder.Property(c => c.Saldo);
            base.Configure(builder);
        }
    }
}
