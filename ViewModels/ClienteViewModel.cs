using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class ClienteViewModel
    {
        [Required]
        [NotNull]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string User { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(13)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }

        [AllowNull]
        [StringLength(100)]
        public string DatosReferenciaDireccion{ get; set; }

        public ClienteViewModel(){}
        public ClienteViewModel(int id,string user, string nombre, string direccion, string telefono, string datosReferenciaDireccion)
        {
            this.Id = id;
            this.User = user;
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Telefono = telefono;
            this.DatosReferenciaDireccion = datosReferenciaDireccion;
        }
    }
}