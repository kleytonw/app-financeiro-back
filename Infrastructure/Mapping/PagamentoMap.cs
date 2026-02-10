using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class PagamentoMap : BaseModelMap<Pagamento>
    {
        public override void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(c => c.IdPagamento);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);
            builder.HasOne(c => c.Operadora)
                .WithMany()
                .HasForeignKey(c => c.IdOperadora);
            builder.HasOne(c => c.Unidade)
                .WithMany()
                .HasForeignKey(c => c.IdUnidade);
            builder.HasOne(c => c.Diagnostico)
                .WithMany()
                .HasForeignKey(c => c.IdDiagnostico);
            builder.Property(c => c.DataPagamento);
            builder.HasOne(c => c.Banco)
                    .WithMany()
                    .HasForeignKey(c => c.IdBanco);
            builder.Property(c => c.CodigoBanco);
            builder.Property(c => c.NomeBanco);
            builder.Property(c => c.Agencia);
            builder.Property(c => c.Conta);
            builder.HasOne(c => c.Bandeira)
                .WithMany()
                .HasForeignKey(c => c.IdBandeira);
            builder.Property(c => c.CodigoBandeira);
            builder.Property(c => c.NomeBandeira);
            builder.Property(c => c.RazaoSocial);
            builder.Property(c => c.ValorPagamento);
            builder.Property(c => c.StatusPagamento);
            builder.Property(c => c.StatusConciliado);
            builder.Property(c => c.TipoPagamento);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}
