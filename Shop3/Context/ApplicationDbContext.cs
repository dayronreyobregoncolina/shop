using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop3;
using Shop3.Models;

namespace Shop.Context
{
    public class ApplicationDbContext : IdentityDbContext<Usuario, Rol, string>
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Orden> Ordenes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {


            builder.Entity<Orden>()
            .HasKey(t => new { t.UsuarioId, t.ProductoId });

            builder.Entity<Orden>()
                .HasOne(o => o.Usuario)
                .WithMany(u => u.Ordenes)
                .HasForeignKey(o => o.UsuarioId);

            builder.Entity<Orden>()
               .HasOne(o => o.Producto)
               .WithMany(p => p.Ordenes)
               .HasForeignKey(o => o.ProductoId);


            /*
            var PasswordHash = new PasswordHasher<Usuario>();

            
            Usuario u1 = new Usuario
            {
                Id = "1",
                Nombre = "Dayron Rey",
                Apellidos = "Obregon",
                Email = "dayronrey@test.com",
                UserName = "dayronrey"
            };

            Producto p1=new Producto
            {
                Nombre="Producto-1",

            }
            Rol r1 = new Rol { Id = "1", Name = "administrador", NormalizedName = "ADMINISTRADOR" };
            Rol r2 = new Rol { Id = "2", Name = "cliente", NormalizedName = "CLIENTE" };
            Rol r3 = new Rol { Id = "3", Name = "vendedor", NormalizedName = "VENDEDOR" };
            u1.PasswordHash = PasswordHash.HashPassword(u1, "12345678");
            builder.Entity<Usuario>().HasData(u1);
            builder.Entity<Rol>().HasData(r1);
            builder.Entity<Rol>().HasData(r2);
            builder.Entity<Rol>().HasData(r3);
            */
            base.OnModelCreating(builder);
        }
    }
}
