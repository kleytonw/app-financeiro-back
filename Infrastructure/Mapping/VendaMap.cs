using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class VendaMap : BaseModelMap<Venda>
    {
        public override void Configure(EntityTypeBuilder<Venda> builder)
        {
            builder.HasKey(c => c.IdVenda);
            builder.Property(c => c.DataVenda);
            builder.Property(c => c.DataPrevPagamento);
            builder.Property(c => c.DataPagamento);
            builder.Property(c => c.Cliente);
            builder.Property(c => c.ValorBruto);
            builder.Property(c => c.ValorDespesa);
            builder.Property(c => c.ValorLiquido);
            builder.Property(c => c.ValorPagamento);
            builder.Property(c => c.Taxa);
            builder.Property(c => c.MeioPagamento);
            builder.HasOne(c => c.Bandeira)
                .WithMany()
                .HasForeignKey(c => c.IdBandeira);
            builder.Property(c => c.NomeBandeira);
            builder.HasOne(c => c.Operadora)
              .WithMany()
              .HasForeignKey(c => c.IdOperadora);
            builder.HasOne(c => c.ContaRecebimento)
                .WithMany()
                .HasForeignKey(c => c.IdContaRecebimento);
            builder.HasOne(c => c.ContaGravame)
                .WithMany()
                .HasForeignKey(c => c.IdContaGravame);
            builder.Property(c => c.NomeOperadora);
            builder.Property(c => c.Gravame);
            builder.Property(c => c.StatusConciliacao);
            builder.Property(c => c.StatusVenda);
            builder.HasOne(c => c.Unidade)
               .WithMany()
               .HasForeignKey(c => c.IdUnidade);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);
            builder.Property(c => c.Identificador);
            builder.Property(c => c.Produto);
            builder.Property(c => c.ProdutoCliente);
            builder.Property(c => c.Modalidade);
            builder.Property(c => c.Observacao);
            builder.Property(c => c.Parcela);
            builder.Property(c => c.Terminal);
            builder.Property(c => c.Autorizacao);
            builder.Property(c => c.Situacao);

            base.Configure(builder);
        }
    }
}
