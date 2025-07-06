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
    /// <summary>
    /// Clase persistance que representa el contexto de base de datos en PostgreSQL del Microservicio Producto.
    /// </summary>
    public class SubastaDbContext : DbContext
    {
       public SubastaDbContext(DbContextOptions<SubastaDbContext> options) : base(options) { }

        /// <summary>
        /// Atributo que corresponde a la tabla de producto en la base de datos PostgreSQL.
        /// </summary>
        public DbSet<ProductoPostgreSQL> Producto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la tabla de categoria en la base de datos PostgreSQL.
        /// </summary>
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
