using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public class ProdutoMap : BaseModelMap<Produto>
    {
        public override void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(c => c.IdProduto);
            builder.Property(c => c.NomeProduto);
            builder.Property(c => c.CodigoProduto);

            builder.HasOne(x => x.GrupoProduto)
                .WithMany()
                .HasForeignKey(x => x.IdGrupoProduto);
            builder.HasOne(x => x.Fornecedor)
                .WithMany()
                .HasForeignKey(c => c.IdFornecedor);
            builder.Property(x => x.PermitirCompra);
            builder.HasOne(x => x.UnidadeMedidaCompra)
                 .WithMany()
                 .HasForeignKey(x => x.IdUnidadeMedidaCompra);
            builder.Property(x => x.PrecoDeCompra);
            builder.Property(x => x.PermitirVenda);
            builder.HasOne(x => x.UnidadeMedidaVenda)
                 .WithMany()
                 .HasForeignKey(x => x.IdUnidadeMedidaVenda);
            builder.Property(x => x.PrecoDeVenda);
            builder.Property(x => x.ControleDeEstoque);
            builder.Property(x => x.QuantidadeDeEstoque);
            builder.Property(x => x.EstoqueMinimo);
            builder.HasOne(x => x.UnidadeMedidaArmazenamento)
           .WithMany()
           .HasForeignKey(x => x.IdUnidadeMedidaArmazenamento);
            builder.Property(x => x.LinkFoto);
            builder.Property(x => x.PrecoDeVenda);
            builder.Property(c => c.Ean);
            builder.Property(c => c.ValorVenda);
            builder.Property(c => c.ValorCusto);


            base.Configure(builder);
        }
    }
}
