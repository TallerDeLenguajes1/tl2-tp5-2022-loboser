using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

#nullable disable

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class UsuarioViewModel
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public int IdCadete { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        [StringLength(100)]
        public string User { get; set; }

        [Required]
        [StringLength(7)]
        public string Rol { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(13)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }
    }
}