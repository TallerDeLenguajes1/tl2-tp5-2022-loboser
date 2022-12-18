using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class EditarPedidoViewModel
    {
        [Required]
        [NotNull]
        public int Nro { get; set; }

        [AllowNull]
        [StringLength(100)]
        public string Obs { get; set; }

        [Required]
        [StringLength(10)]
        public string Estado { get; set; }


        public EditarPedidoViewModel(){}
        public EditarPedidoViewModel(int nro, string obs, string estado) 
        {
            this.Nro = nro;
            this.Obs = obs;
            this.Estado = estado;
        }
    
    }
}