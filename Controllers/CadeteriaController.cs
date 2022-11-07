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
        //static Cadeteria Cadeteria = DataBase.CargarCadeteriaDB();
        Cadeteria Cadeteria = DataBase.CargarCadeteriaDB();

        //static int Id = Cadeteria.Cadetes.Max(t => t.Id) + 1;

        [HttpGet]
        public IActionResult ListaDeCadetes()
        {
            return View(Cadeteria);
        }
        
        [HttpPost]
        public IActionResult ListaDeCadetes(int id){
            //Cadeteria.Cadetes = Cadeteria.Cadetes.Where(t => t.Id != id).ToList();
            DataBase.BajaCadeteDB(id);

            return RedirectToAction("ListaDeCadetes");
        }

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
                /*
                Cadete Cadete = new Cadete();

                Cadete.Id = Id;
                Cadete.Direccion = CadeteRecibido.Direccion;
                Cadete.Nombre = CadeteRecibido.Nombre;
                Cadete.Telefono = CadeteRecibido.Telefono;

                Cadeteria.Cadetes.Add(Cadete);
                Id++;
                */
                
                DataBase.AltaCadeteDB(CadeteRecibido);

                return RedirectToAction("AltaCadete");
            }else
            {
                return RedirectToAction("Error");
            }
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
            /*Cadete Cadete = Cadeteria.Cadetes.First(t => t.Id == Edit.Id);
            
            Cadete.Nombre = Edit.Nombre;
            Cadete.Direccion = Edit.Direccion;
            Cadete.Telefono = Edit.Telefono;*/
        
            DataBase.EditarCadeteDB(Edit);

            return RedirectToAction("ListaDeCadetes");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}