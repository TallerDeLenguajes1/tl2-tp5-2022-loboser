using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp4_2022_loboser.Models
{
    public class Cadeteria {
        [Required][StringLength(100)]
        private string nombre;
        [Required][Phone]
        private string telefono;
        private List<Cadete> cadetes = new List<Cadete>();

        public string Nombre { get => nombre; set => nombre = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public List<Cadete> Cadetes { get => cadetes; set => cadetes = value; }
    }
}