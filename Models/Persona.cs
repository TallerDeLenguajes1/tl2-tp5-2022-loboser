using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace tl2_tp4_2022_loboser.Models
{
    public class Persona
    {
        
        private int id;
        [Required]
        [StringLength(40)]
        private string telefono;
        [Required]
        [StringLength(40)]
        private string nombre;
        [Required]
        [StringLength(40)]
        private string direccion;

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Telefono { get => telefono; set => telefono = value; }
    }
}