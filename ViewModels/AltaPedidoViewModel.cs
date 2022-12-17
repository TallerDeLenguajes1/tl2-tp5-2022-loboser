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
        [StringLength(100)]
        public string ClienteNombre { get; set; }

        [Required]
        [StringLength(13)]
        public string ClienteTelefono { get; set; }

        [Required]
        [StringLength(100)]
        public string ClienteDireccion { get; set; }

        [AllowNull]
        [StringLength(100)]
        public string ClienteDatosReferenciaDireccion{ get; set; }

        public AltaPedidoViewModel(){}

        public AltaPedidoViewModel(int nro, string obs, string clienteNombre, string clienteTelefono, string clienteDireccion, string clienteDatosReferenciaDireccion) 
        {
            this.Obs = obs;
            this.ClienteNombre = clienteNombre;
            this.ClienteTelefono = clienteTelefono;
            this.ClienteDireccion = clienteDireccion;
            this.ClienteDatosReferenciaDireccion = clienteDatosReferenciaDireccion;
        }
    }
}