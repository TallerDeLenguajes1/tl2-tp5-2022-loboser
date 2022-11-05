using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

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
        static Cadeteria Cadeteria = CargarCadeteria();
        static int Id = Cadeteria.Cadetes.Count() + 1;

        [HttpGet]
        public IActionResult AltaCadete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AltaCadete(AltaCadeteViewModel CadeteRecibido)
        {
            if (ModelState.IsValid)
            {
                Cadete Cadete = new Cadete();

                Cadete.Id = Id;
                Cadete.Direccion = CadeteRecibido.Direccion;
                Cadete.Nombre = CadeteRecibido.Nombre;
                Cadete.Telefono = CadeteRecibido.Telefono;
                Id++;
                Cadeteria.Cadetes.Add(Cadete);

                return RedirectToAction("AltaCadete");
            }else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IActionResult ListaDeCadetes()
        {
            return View(Cadeteria);
        }
        

        [HttpPost]
        public IActionResult ListaDeCadetes(int id){
            Cadeteria.Cadetes = Cadeteria.Cadetes.Where(t => t.Id != id).ToList();

            return RedirectToAction("ListaDeCadetes");
        }

        [HttpGet]
        public IActionResult EditarCadete(int id)
        {
            var Cadete = Cadeteria.Cadetes.First(t => t.Id == id);

            return View(new EditarCadeteViewModel(Cadete.Id, Cadete.Nombre, Cadete.Direccion, Cadete.Telefono));
        }

        [HttpPost]
        public IActionResult EditarCadete(EditarCadeteViewModel Edit)
        {
            Cadete Cadete = Cadeteria.Cadetes.First(t => t.Id == Edit.Id);
            
            Cadete.Nombre = Edit.Nombre;
            Cadete.Direccion = Edit.Direccion;
            Cadete.Telefono = Edit.Telefono;

            return RedirectToAction("ListaDeCadetes");
        }

        [HttpGet]
        public IActionResult GuardarCSV()
        {
            string[] lineas = new string[Cadeteria.Cadetes.Count()];
            int i = 0;
            
            foreach (var Cadete in Cadeteria.Cadetes)
            {
                lineas[i] = Cadete.Id + ";" + Cadete.Nombre + ";" + Cadete.Direccion + ";" + Cadete.Telefono;
                i++;
            }

            System.IO.File.WriteAllLines("Datos.csv", lineas);

            return RedirectToAction("ListaDeCadetes");
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