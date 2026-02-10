using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Infrastructure.Mapping
{

    public class ConectorOpenFinanceMap : IEntityTypeConfiguration<ConectorOpenFinance>
    {
        public void Configure(EntityTypeBuilder<ConectorOpenFinance> builder)
        {
            builder.ToTable("ConectoresOpenFinance");

            // PK
            builder.HasKey(x => x.IdConector);

            builder.Property(x => x.IdConector)
                   .HasColumnName("IdConector")
                   .ValueGeneratedNever();

            // Dados principais
            builder.Property(x => x.Nome)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(x => x.CorPrimaria)
                   .HasMaxLength(20);

            builder.Property(x => x.InstitutionUrl)
                   .HasMaxLength(500);

            builder.Property(x => x.ImageUrl)
                   .HasMaxLength(500);

            builder.Property(x => x.Pais)
                   .HasMaxLength(2)
                   .IsRequired();

            builder.Property(x => x.Tipo)
                   .HasMaxLength(50)
                   .IsRequired();

            // Flags
            builder.Property(x => x.PossuiMFA)
                   .IsRequired();

            builder.Property(x => x.OAuth)
                   .IsRequired();

            builder.Property(x => x.IsSandbox)
                   .IsRequired();

            builder.Property(x => x.IsOpenFinance)
                   .IsRequired();

            builder.Property(x => x.SuportaIniciacaoPagamento)
                   .IsRequired();

            builder.Property(x => x.SuportaPagamentosAgendados)
                   .IsRequired();

            builder.Property(x => x.SuportaSmartTransfer)
                   .IsRequired();

            builder.Property(x => x.SuportaBoleto)
                   .IsRequired();

            // Health
            builder.Property(x => x.StatusHealth)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.HealthStage)
                   .HasMaxLength(20);

            // Produtos (CSV)
            builder.Property(x => x.Produtos)
                   .HasMaxLength(500);

            // Datas
            builder.Property(x => x.DataCriacao)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(x => x.DataAtualizacao)
                   .HasColumnType("datetime2")
                   .IsRequired();

            // Índices úteis
            builder.HasIndex(x => x.Nome);
            builder.HasIndex(x => x.Tipo);
            builder.HasIndex(x => x.StatusHealth);
            builder.HasIndex(x => x.IsOpenFinance);
        }
    }
}
