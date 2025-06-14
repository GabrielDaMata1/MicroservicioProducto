using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models.PostgreSQL;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    public class SubastaDbContext : DbContext
    {
       public SubastaDbContext(DbContextOptions<SubastaDbContext> options) : base(options) { }


        public DbSet<ProductoPostgreSQL> Producto { get; set; }
        public DbSet<CategoriaPostgreSQL> Categoria { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductoPostgreSQL>()
                .HasIndex(u => u.Id)
                .IsUnique();
            modelBuilder.Entity<CategoriaPostgreSQL>()
                .HasIndex(u => u.Id)
                .IsUnique();

            base.OnModelCreating(modelBuilder);

        }
    }
}
