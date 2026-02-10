using ERP.Infrastructure.Mapping;
using ERP_API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class MovimentacaoDiariaMap : BaseModelMap<MovimentacaoDiaria>
    {
        public void Configure(EntityTypeBuilder<MovimentacaoDiaria> builder)
        { 
            builder.HasKey(x => x.IdMovimentacaoDiaria);

            builder.Property(x => x.IdCliente)
                .IsRequired();

            builder.Property(x => x.TipoMovimentacao)
              .HasConversion<string>() // salva o enum como varchar
              .HasMaxLength(20)        // tamanho do campo no banco
              .IsRequired();

            builder.Property(x => x.DataMovimentacao)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(x => x.FornecedorCliente)
                .HasMaxLength(150);

            builder.Property(x => x.NotaFiscal)
                .HasMaxLength(30);

            builder.Property(x => x.Produto)
                .HasMaxLength(150);

            builder.Property(x => x.Categoria)
                .HasMaxLength(100);

            builder.Property(x => x.SKU)
                .HasMaxLength(50);

            builder.Property(x => x.NCM)
                .HasMaxLength(20);

            builder.Property(x => x.CFOP)
                .HasMaxLength(10);

            builder.Property(x => x.UnidadeMedida)
                .HasMaxLength(10);

            builder.Property(x => x.CodigoBarras)
                .HasMaxLength(60);

            builder.Property(x => x.Observacao)
                .HasMaxLength(500);

            // Valores monetários
            builder.Property(x => x.ValorUnitario)
                .HasPrecision(18, 4);

            builder.Property(x => x.ValorDesconto)
                .HasPrecision(18, 4);

            builder.Property(x => x.ValorTotal)
                .HasPrecision(18, 4)
                .IsRequired();

            builder.Property(x => x.CMV_Contabil)
           .HasPrecision(18, 4);

            builder.Property(x => x.CMV_Aquisicao)
                .HasPrecision(18, 4);


            builder.Property(x => x.CMV_Tributos)
                .HasPrecision(18, 4);


            builder.Property(x => x.CMV_Total)
        .HasPrecision(18, 4);


            builder.Property(x => x.Margem)
                .HasPrecision(5, 2);

            builder.Property(x => x.CpfCnpjFornecedorCliente)
    .HasMaxLength(20);

            builder.Property(x => x.Promocao)
                .HasConversion<int>() // ou bool → 0/1
                .IsRequired();

            builder.Property(x => x.Quantidade)
    .IsRequired();


            // Índices
            builder.HasIndex(x => x.DataMovimentacao);
            builder.HasIndex(x => x.IdCliente);
            builder.HasIndex(x => x.SKU);
            builder.HasIndex(x => x.NotaFiscal);
        }
    }
}