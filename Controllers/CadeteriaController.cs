using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tl2_tp4_2022_loboser.Models;

namespace tl2_tp4_2022_loboser.Controllers
{
    public class CadeteriaController : Controller
    {
        private readonly ILogger<CadeteriaController> _logger;

        public CadeteriaController(ILogger<CadeteriaController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ListaDeCadetes()
        {
            Cadeteria Cadeteria = CargarCadeteria();
            return View(Cadeteria);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        static Cadeteria CargarCadeteria(){
            Cadeteria Cadeteria = new Cadeteria();

            Cadeteria.Nombre = "Cadeteria Of";
            Cadeteria.Telefono = "+549381342574";
            Cadeteria.Cadetes = new List<Cadete>();
 
            Cadeteria.Cadetes = CargarCadetes();

            return Cadeteria;
        }

        static List<Cadete> CargarCadetes(){
            List<Cadete> Cadetes = new List<Cadete>();

            if(System.IO.File.Exists("Datos.csv") && new FileInfo("Datos.csv").Length > 0){
                string[] lineas = System.IO.File.ReadAllLines("Datos.csv");

                foreach (var linea in lineas)
                {
                    Cadete Cadete = new Cadete();

                    string[] dato = linea.Split(';');

                    Cadete.Id = Convert.ToInt32(dato[0]);
                    Cadete.Nombre = dato[1];
                    Cadete.Direccion = dato[2];
                    Cadete.Telefono = dato[3];
                    Cadete.Pedidos = new List<Pedido>();

                    Cadetes.Add(Cadete);
                }
            }else
            {
                Console.WriteLine("El Archivo Datos.csv no existe!");
            }
            return Cadetes;
        }
    }
}