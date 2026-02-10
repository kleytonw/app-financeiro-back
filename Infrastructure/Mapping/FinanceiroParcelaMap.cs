using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class FinanceiroParcelaMap : BaseModelMap<FinanceiroParcela>   
    {
        public override void Configure(EntityTypeBuilder<FinanceiroParcela> builder)
        {
            builder.HasKey(c => c.IdFinanceiroParcela);

            builder.HasOne(m => m.Financeiro)
             .WithMany(x => x.Parcelas)
             .HasForeignKey(m => m.IdFinanceiro);

            builder.Property(c => c.Numero);
            builder.Property(c => c.DataVencimento);
            builder.Property(c => c.ValorVencimento);
            builder.Property(c => c.ValorDesconto);
            builder.Property(c => c.ValorAcrescimo);
            builder.Property(c => c.DataAcerto);
            builder.Property(c => c.ValorAcerto);

            builder.HasOne(m => m.MeioPagamento)
            .WithMany()
            .HasForeignKey(m => m.IdMeioPagamento);

            builder.Property(c => c.NumeroNf);
            builder.Property(c => c.IdentificadorBoletoUnique);

            builder.Property(c => c.Observacao);

            builder.Property(c => c.UsuarioBaixa);
            builder.Property(c => c.DataBaixa);
            builder.Property(c => c.DataEnvioLembrete);
            builder.Property(c => c.DataEnvioLembreteVencimento);
            builder.HasOne(c => c.PlanoConta)
                .WithMany()
                .HasForeignKey(c => c.IdPlanoConta);

            base.Configure(builder);
        }
    }
}
