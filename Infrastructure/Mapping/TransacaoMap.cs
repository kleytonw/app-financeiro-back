using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class TransacaoMap : BaseModelMap<Transacao>
    {
        public override void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.HasKey(c => c.IdTransacao);
            builder.Property(c => c.Cliente);
            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa);
            builder.HasOne(c => c.Unidade)
               .WithMany()
               .HasForeignKey(c => c.IdUnidade);
            builder.Property(c => c.ValorBruto);
            builder.Property(c => c.Taxa);
            builder.Property(c => c.Despesa);
            builder.Property(c => c.ValorLiquido);
            builder.Property(c => c.DataVenda);
            builder.Property(c => c.DataMovimentacao);
            builder.Property(c => c.MeioPagamento);
            builder.Property(c => c.Bandeira);
            builder.HasOne(c => c.Operadora)
                .WithMany()
                .HasForeignKey(c => c.IdOperadora);
            builder.Property(c => c.NomeOperadora);
            builder.Property(c => c.StatusConciliacao);
            builder.Property(c => c.ValorPagoConciliacao);
            builder.Property(c => c.ValorTarifaConciliacao);
            builder.Property(c => c.Observacao);
            builder.Property(c => c.Identificador);
            builder.Property(c => c.QuantidadeParcela);
            builder.Property(c => c.NumeroVenda);
            builder.Property(c => c.StatusTransacao);
            builder.Property(c => c.Terminal);
            builder.Property(c => c.ChargebackStatus);
            builder.Property(c => c.TipoCaptura);
            builder.Property(c => c.Flex);
            builder.Property(c => c.FlexAmount);
            builder.Property(c => c.ValorTotalTaxa);
            builder.Property(c => c.ValorEmbarqueTransacao);
            builder.Property(c => c.Tokenizado);
            builder.Property(c => c.Tid);
            builder.Property(c => c.NumeroDoPedido);
            builder.Property(c => c.NumeroCartao);

            builder.Property(c => c.Produto);
            builder.Property(c => c.DescricaoProduto);
            builder.Property(c => c.TipoProduto);
            builder.Property(c => c.CodigoProduto);

            builder.Property(c => c.Situacao);

            builder.Property(c => c.Diagnostico);

            base.Configure(builder);
        }
    }
}
