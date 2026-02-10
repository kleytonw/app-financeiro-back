using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ClienteContaBancariaMap: BaseModelMap<ClienteContaBancaria>
    {
        public override void Configure(EntityTypeBuilder<ClienteContaBancaria> builder)
        {
            builder.HasKey(c => c.IdClienteContaBancaria);
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente);
            builder.HasOne(c => c.Banco)
                .WithMany()
                .HasForeignKey(c => c.IdBanco);
            builder.Property(c => c.Agencia);
            builder.Property(c => c.Conta);
            builder.Property(c => c.DigitoConta);
            builder.Property(c => c.DigitoAgencia);
            builder.Property(c => c.Descricao);
            builder.Property(c => c.Saldo);
            builder.Property(c => c.DataDoSaldo);

            builder.Property(c => c.AccountIdOpenFinance);
            builder.Property(c => c.ItemIdOpenFinance);
            builder.Property(c => c.IdentificadorConta);
            builder.Property(c => c.UrlAtivacaoConta);


            builder.Property(c => c.Tipo);
            builder.Property(c => c.SubTipo);
            builder.Property(c => c.Nome);
            builder.Property(c => c.Numero);
            builder.Property(c => c.TransferNumber);

            base.Configure(builder);
        }
    }
}
