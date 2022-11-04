using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp4_2022_loboser.Models
{
    public class Cliente : Persona
    {
        [Required][StringLength(500)]
        private string datosReferenciaDireccion;
        public string DatosReferenciaDireccion { get => datosReferenciaDireccion; set => datosReferenciaDireccion = value; }
    }
}