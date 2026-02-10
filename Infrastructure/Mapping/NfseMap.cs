using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class NfseMap : BaseModelMap<Nfse>
    {
        public override void Configure(EntityTypeBuilder<Nfse> builder)
        {
            builder.HasKey(n => n.IdNfse);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);
            builder.Property(n => n.NomeCliente);
            builder.Property(n => n.ChaveAcesso);
            builder.Property(n => n.NumeroRPS);
            builder.Property(n => n.Serie);
            builder.Property(n => n.DataHoraInclusao);
            builder.Property(n => n.DataHoraCancelamento);
            builder.Property(n => n.StatusNotaFiscal);
            builder.HasOne(s => s.ServicoNfse)
                .WithMany()
                .HasForeignKey(s => s.IdServicoNfse);
            builder.Property(n => n.Valor);
            builder.Property(n => n.CodigoServico);
            builder.Property(n => n.CodigoNBS);
            builder.Property(n => n.ObservacoesNotaFiscal);

            base.Configure(builder);
        }
    }
}
