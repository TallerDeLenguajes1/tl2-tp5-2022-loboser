using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

#nullable disable

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class RegistroViewModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public string IdCadete { get; set; }

        [Required]
        [StringLength(13)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }

        [AllowNull]
        [StringLength(100)]
        public string DatosReferenciaDireccion{ get; set; }

        [Required]
        [StringLength(100)]
        public string User { get; set; }

        [Required]
        [StringLength(100)]
        public string Pass { get; set; }

        [Required]
        [StringLength(100)]
        public string ConfirmPass { get; set; }

        [Required]
        [StringLength(7)]
        public string Rol { get; set; }
    }
}