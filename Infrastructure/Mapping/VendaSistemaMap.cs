using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;

namespace ERP_API.Infrastructure.Mapping
{
    public class VendaSistemaMap : BaseModelMap<VendaSistema>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<VendaSistema> builder)
        {
            builder.HasKey(v => v.IdVendaSistema);
            builder.HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.IdCliente);
            builder.HasOne(v => v.ERPs)
                .WithMany()
                .HasForeignKey(v => v.IdERPs);
            builder.Property(v => v.Data);
            builder.Property(v => v.Arquivo);
            builder.Property(v => v.NomeArquivo);
            builder.Property(v => v.CodigoResposta);
            builder.Property(v => v.MensagemResposta);
            builder.Property(v => v.XMLErrosResposta);
            builder.Property(v => v.SucessoResposta);
            builder.Property(v => v.Situacao);
            base.Configure(builder);
        }
    }
}
