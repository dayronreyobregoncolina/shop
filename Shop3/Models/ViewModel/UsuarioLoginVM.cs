using System;
using System.ComponentModel.DataAnnotations;

namespace Shop3.Models.ViewModel
{
    public class UsuarioLoginVM
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
