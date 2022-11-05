using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class AltaPedidoViewModel
    {
        public string Obs { get; set; }

        public Cliente Cliente { get; set; }
        public AltaPedidoViewModel(){}
        public AltaPedidoViewModel(string obs,  Cliente cliente)
        {
            this.Obs = obs;
            this.Cliente = cliente;
        }
    }

    public class EditarPedidoViewModel
    {
        [Required]
        [NotNull]
        public int Nro { get; set; }

        [AllowNull]
        [StringLength(40)]
        [DisplayName("Observaciones: ")]
        public string Obs { get; set; }
        public Cliente Cliente { get; set; }

        public EditarPedidoViewModel(){}
        public EditarPedidoViewModel(int nro, string obs,  Cliente cliente)
        {
            this.Nro = nro;
            this.Obs = obs;
            this.Cliente = cliente;
        }
    }
}