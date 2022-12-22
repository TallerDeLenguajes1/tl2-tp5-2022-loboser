using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

#nullable disable
namespace tl2_tp4_2022_loboser.ViewModels
{
    public class AltaPedidoViewModel
    {
        [Required]
        [StringLength(100)]
        public string Obs { get; set; }

        [Required]
        [StringLength(10)]
        public string Estado { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int IdCadete { get; set; }

        public List<CadeteViewModel> Cadetes {get; set;}

        public List<ClienteViewModel> Clientes {get; set;}
    }
}