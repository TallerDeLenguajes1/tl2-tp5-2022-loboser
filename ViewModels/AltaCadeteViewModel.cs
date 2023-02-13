using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class AltaCadeteViewModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }

        [Required]
        [StringLength(12)]
        [RegularExpression(@"[0-9]{10,12}$")]
        public string Telefono { get; set; }
        public AltaCadeteViewModel(){}
        public AltaCadeteViewModel(string nombre, string direccion, string telefono)
        {
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Telefono = telefono;
        }
    }
}