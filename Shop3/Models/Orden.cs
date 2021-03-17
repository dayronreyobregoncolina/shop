using System;
namespace Shop3.Models
{
    public class Orden
    {
        public long Id { get; set; }
        public string UsuarioId { get; set; }
        public long ProductoId { get; set; }
        public DateTime Fecha { get; set; }
        public int Cantidad { get; set; }
        public string Estado { get; set; }

        public Usuario Usuario { get; set; }
        public Producto Producto { get; set; }
    }
}
