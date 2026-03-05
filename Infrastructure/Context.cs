using Microsoft.EntityFrameworkCore;
using ERP.Infrastructure.Mapping;
using ERP.Domain;
using ERP_API.Domain.Entidades;
using ERP_API.Infrastructure.Mapping;
using System.Collections.Generic;
using ERP_API.Domain;

namespace ERP.Infra
{
    public class Context : DbContext
    {

        public DbSet<Dependente> Dependente { get; set; }
        public DbSet<Cartao> Cartao { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Transacao> Transacao { get; set; }
        public DbSet<Usuario> Usuario { get; set; }


        public Context (DbContextOptions options) : base(options)
        {
        }   
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dependente>(new DependenteMap().Configure);
            modelBuilder.Entity<Cartao>(new CartaoMap().Configure);
            modelBuilder.Entity<Categoria>(new CategoriaMap().Configure);
            modelBuilder.Entity<Transacao>(new TransacaoMap().Configure);
            modelBuilder.Entity<Usuario>(new UsuarioMap().Configure);

        }
    }   
}
