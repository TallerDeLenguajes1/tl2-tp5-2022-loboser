using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class AltaCadeteViewModel
    {
        [Required]
        [StringLength(40)]
        [DisplayName("Nombre: ")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(40)]
        [DisplayName("Dirección: ")]
        public string Direccion { get; set; }

        [Required]
        [StringLength(40)]
        [DisplayName("Teléfono: ")]
        public string Telefono { get; set; }
        public AltaCadeteViewModel(){}
        public AltaCadeteViewModel(string nombre, string direccion, string telefono)
        {
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Telefono = telefono;
        }
    }

    public class EditarCadeteViewModel
    {
        [Required]
        [NotNull]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        [DisplayName("Nombre: ")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(40)]
        [DisplayName("Teléfono: ")]
        public string Telefono { get; set; }

        [Required]
        [StringLength(40)]
        [DisplayName("Dirección: ")]
        public string Direccion { get; set; }

        public EditarCadeteViewModel(){}
        public EditarCadeteViewModel(int id, string nombre, string direccion, string telefono)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Telefono = telefono;
        }
    }
}