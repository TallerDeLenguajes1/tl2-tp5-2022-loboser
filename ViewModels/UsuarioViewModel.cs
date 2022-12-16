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
        [StringLength(40)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(40)]
        public string Pass { get; set; }
    }
}