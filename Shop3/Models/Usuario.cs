using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Shop3.Models
{
    public class Usuario : IdentityUser
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }

        public ICollection<Orden> Ordenes { get; set; }

        //Relacion para obtner el listado de productos de un usuario
        public ICollection<Producto> Productos { get; set; }
    }
}
