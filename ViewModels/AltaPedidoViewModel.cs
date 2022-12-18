using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class AltaPedidoViewModel
    {
        [AllowNull]
        [StringLength(100)]
        public string Obs { get; set; }

        [Required]
        [StringLength(10)]
        public string Estado { get; set; }

        public AltaPedidoViewModel(){}

        public AltaPedidoViewModel(string obs, string estado) 
        {
            this.Obs = obs;
            this.Estado = estado;
        }
    }
}