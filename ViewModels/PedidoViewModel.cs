using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class PedidoViewModel
    {
        [Required]
        [NotNull]
        public int Nro { get; set; }

        [AllowNull]
        [StringLength(100)]
        public string Obs { get; set; }

        [Required]
        [StringLength(100)]
        public string ClienteNombre { get; set; }

        [Required]
        [StringLength(100)]
        public string ClienteDireccion { get; set; }

        [Required]
        [StringLength(100)]
        public string ClienteDatosReferenciaDireccion { get; set; }

        [Required]
        [StringLength(10)]
        public string Estado { get; set; }

        [Required]
        [AllowNull]
        public int IdCadeteAsignado { get; set; }
    
        [Required]
        [StringLength(100)]
        public String NombreCadeteAsignado { get; set; }
    }
}