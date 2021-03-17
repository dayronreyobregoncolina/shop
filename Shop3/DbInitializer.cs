using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Shop.Context;
using Shop3.Models;

namespace Shop3
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, IServiceProvider services)
        {

            // Make sure the database is created
            // We already did this in the previous step
            context.Database.EnsureCreated();

            if (context.Productos.Any())
            {
                //La Base de Datos ya tiene Data
                return;
            }

            //Creacion de usuarios
            var PasswordHash = new PasswordHasher<Usuario>();
            Usuario u1 = new Usuario
            {
                Id = "1",
                Nombre = "Dayron Rey",
                Apellidos = "Obregon Colina",
                Email = "dayronrey@test.com",
                UserName = "dayronrey",
            };
            Usuario u2 = new Usuario
            {
                Id = "2",
                Nombre = "Diana",
                Apellidos = "Obregon Colina",
                Email = "diana@test.com",
                UserName = "diana",
            };
            Usuario u3 = new Usuario
            {
                Id = "3",
                Nombre = "Juan",
                Apellidos = "Obregon Colina",
                Email = "juan@test.com",
                UserName = "juan",
            };

            u1.PasswordHash = PasswordHash.HashPassword(u1, "123456781");
            u2.PasswordHash = PasswordHash.HashPassword(u2, "123456782");
            u3.PasswordHash = PasswordHash.HashPassword(u3, "123456783");
            context.Usuarios.Add(u1);
            context.Usuarios.Add(u2);
            context.Usuarios.Add(u3);
            context.SaveChanges();

            //Creacion de roles
            var rolList = new List<Rol>() {
                new Rol { Id = "1", Name = "administrador", NormalizedName = "ADMINISTRADOR" },
                new Rol { Id = "2", Name = "cliente", NormalizedName = "CLIENTE" },
                new Rol { Id = "3", Name = "vendedor", NormalizedName = "VENDEDOR" }
            };
            foreach (var rol in rolList)
            {
                context.Rols.Add(rol);
            }
            context.SaveChanges();

            //Asignando Roles a usuarios
            context.UserRoles.Add(new IdentityUserRole<string> { RoleId = "1", UserId = "1" });
            context.UserRoles.Add(new IdentityUserRole<string> { RoleId = "2", UserId = "2" });
            context.UserRoles.Add(new IdentityUserRole<string> { RoleId = "3", UserId = "3" });
            context.SaveChanges();

            //Creacion de Productos
            var productList = new List<Producto>()
            {
                new Producto
                {
                    Nombre = "Producto-1",
                    Descripción = "Descripción Producto 1",
                    Cantidad=3,
                    Slug="producto-1",
                    Precio=10,
                    UsuarioId="1"
                },
                new Producto
                {
                    Nombre = "Producto-2",
                    Descripción = "Descripción Producto 2",
                    Cantidad=6,
                    Slug="producto-2",
                    Precio=10,
                    UsuarioId="1"
                },
                new Producto
                {
                    Nombre = "Producto-3",
                    Descripción = "Descripción Producto 3",
                    Cantidad=9,
                    Slug="producto-3",
                    Precio=10,
                    UsuarioId="2"
                },

            };
            foreach (var product in productList)
            {
                context.Productos.Add(product);
            }
            context.SaveChanges();

            //Creacion de Ordenes o compras
            var orderList = new List<Orden>()
            {
                new Orden
                {
                    Id = 1,
                    UsuarioId = "2",
                    ProductoId=1,
                    Cantidad=3,
                    Fecha=DateTime.Now,
                    Estado="created",

                },
                 new Orden
                {
                    Id = 2,
                    UsuarioId = "2",
                    ProductoId=2,
                    Cantidad=6,
                    Fecha=DateTime.Now,
                    Estado="created",

                },


            };
            foreach (var orden in orderList)
            {
                context.Ordenes.Add(orden);
            }
            context.SaveChanges();


        }
    }
}
