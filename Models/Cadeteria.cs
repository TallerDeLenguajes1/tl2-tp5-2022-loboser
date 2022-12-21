using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;
using tl2_tp4_2022_loboser.Repositories;
using AutoMapper;

namespace tl2_tp4_2022_loboser.Models
{
    public class Cadeteria {
        private string nombre;
        private string telefono;
        private List<Cadete> cadetes = new List<Cadete>();

        
        public string Nombre { get => nombre; set => nombre = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public List<Cadete> Cadetes { get => cadetes; set => cadetes = value; }

        public Cadeteria(List<Cadete> cadetes)
        {
            this.Nombre = "Rapi";
            this.Telefono = "549381342574";
            this.Cadetes = cadetes;
        }
    }
}