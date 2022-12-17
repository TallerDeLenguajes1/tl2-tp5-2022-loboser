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

        [Required]
        [NotNull]
        public int idCadeteAsignado { get; set; }

        public EditarPedidoViewModel(){}
        public EditarPedidoViewModel(int nro, string obs, string estado, string clienteNombre, string clienteTelefono, string clienteDireccion, string clienteDatosReferenciaDireccion, int idCadeteAsignado) 
        {
            this.Nro = nro;
            this.Obs = obs;
            this.Estado = estado;
            this.ClienteNombre = clienteNombre;
            this.ClienteTelefono = clienteTelefono;
            this.ClienteDireccion = clienteDireccion;
            this.ClienteDatosReferenciaDireccion = clienteDatosReferenciaDireccion;
            this.idCadeteAsignado = idCadeteAsignado;

        }
    
    }
}