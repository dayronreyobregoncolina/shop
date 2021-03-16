using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop3.Models;

namespace Shop.Context
{
    public class ApplicationDbContext : IdentityDbContext<Usuario, Rol, Guid>
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Orden> Orden { get; set; }

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



            var PasswordHash = new PasswordHasher<Usuario>();

            //Usuario Dayron Rey con Rol Administrador
            Usuario u1 = new Usuario
            {
                Id = Guid.NewGuid(),
                Nombre = "Dayron Rey",
                Apellidos = "Obregon",
                Email = "dayronrey.obregon@gmail.com",
                UserName = "dayronrey"
            };
            Rol r1 = new Rol { Id = Guid.NewGuid(), Name = "administrador", NormalizedName = "ADMINISTRADOR" };
            Rol r2 = new Rol { Id = Guid.NewGuid(), Name = "cliente", NormalizedName = "CLIENTE" };
            Rol r3 = new Rol { Id = Guid.NewGuid(), Name = "vendedor", NormalizedName = "VENDEDOR" };
            u1.PasswordHash = PasswordHash.HashPassword(u1, "12345678");
            builder.Entity<Usuario>().HasData(u1);
            builder.Entity<Rol>().HasData(r1);
            builder.Entity<Rol>().HasData(r2);
            builder.Entity<Rol>().HasData(r3);
            base.OnModelCreating(builder);
        }
    }
}
