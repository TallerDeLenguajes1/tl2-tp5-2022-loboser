using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

#nullable disable
namespace tl2_tp4_2022_loboser.ViewModels
{
    public class PedidoViewModel
    {
        [Required]
        [NotNull]
        public int Nro { get; set; }

        [Required]
        [StringLength(100)]
        public string Obs { get; set; }

        [Required]
        public int ClienteId { get; set; }

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
        public int IdCadete { get; set; }
    
        [Required]
        [StringLength(100)]
        public String NombreCadete { get; set; }
    }
}