using System;
using System.Collections.Generic;

namespace Shop3.Models
{
    public class Producto
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Descripción { get; set; }
        public int Cantidad { get; set; }
        public string Slug { get; set; }
        public float Precio { get; set; }
        public Guid UsuarioId { get; set; }

        public ICollection<Orden> Ordenes { get; set; }
        //Usuario que creo el producto
        public Usuario Usuario { get; set; }

    }
}
