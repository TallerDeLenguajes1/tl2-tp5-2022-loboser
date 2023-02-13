using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class AltaClienteViewModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(12)]
        [RegularExpression(@"[0-9]{10,12}$")]
        public string Telefono { get; set; }

        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }

        [AllowNull]
        [StringLength(100)]
        public string DatosReferenciaDireccion{ get; set; }

        public AltaClienteViewModel(){}
        public AltaClienteViewModel(int id, string nombre, string direccion, string telefono, string datosReferenciaDireccion)
        {
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Telefono = telefono;
            this.DatosReferenciaDireccion = datosReferenciaDireccion;
        }
    }
}