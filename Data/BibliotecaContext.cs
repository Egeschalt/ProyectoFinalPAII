using ProyectoFinalPAII.Models;
using Microsoft.EntityFrameworkCore;
namespace ProyectoFinalPAII.Data
{
    public class BibliotecaContext :DbContext
    {
        public BibliotecaContext(DbContextOptions opciones) : base(opciones)
        {
        }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Prestamos> Prestamo { get; set; }
        public DbSet<Devoluciones> Devoluciones { get; set; }
        public DbSet<Editorial> Editorial { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libros>().ToTable("Libros");
            modelBuilder.Entity<Autor>().ToTable("Proveedores");
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Prestamos>().ToTable("Prestamos");
            modelBuilder.Entity<Devoluciones>().ToTable("Devoluciones");
            modelBuilder.Entity<Editorial>().ToTable("Editorial");
        }
    }
}
