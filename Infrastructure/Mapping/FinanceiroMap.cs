using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class FinanceiroMap : BaseModelMap<Financeiro> 
    {
        public override void Configure(EntityTypeBuilder<Financeiro> builder)
        {
            builder.HasKey(c => c.IdFinanceiro);

            builder.HasOne(m => m.Pessoa)
              .WithMany()
              .HasForeignKey(m => m.IdPessoa);
            builder.Property(c => c.Nome);
            builder.Property(c => c.CpfCnpj);

            builder.Property(c => c.Tipo);

            builder.Property(c => c.TotalVencimento);
            builder.Property(c => c.TotalDesconto);
            builder.Property(c => c.TotalAcrescimo);
            builder.Property(c => c.TotalAcerto);

            builder.HasMany(x => x.Parcelas);

            base.Configure(builder);


        }
    }
}
