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


namespace tl2_tp4_2022_loboser.Controllers
{
    public class CadeteriaController : Controller
    {
        private readonly ILogger<CadeteriaController> _logger;
        private readonly Repositories.ICadeteriaRepository cadeteriaRepository;

        public CadeteriaController(ILogger<CadeteriaController> logger, ICadeteriaRepository cadeteriaRepository)
        {
            this.cadeteriaRepository = cadeteriaRepository;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //static Cadeteria Cadeteria = DataBase.CargarCadeteriaDB();

        //static int Id = Cadeteria.Cadetes.Max(t => t.Id) + 1;

        [HttpGet]
        public IActionResult ListaDeCadetes()
        {
            //Cadeteria Cadeteria = DataBase.CargarCadeteriaDB();
            
            Cadeteria Cadeteria = cadeteriaRepository.GetCadeteria();

            return View(Cadeteria);
        }
        
        [HttpPost]
        public IActionResult ListaDeCadetes(int id){
            //Cadeteria.Cadetes = Cadeteria.Cadetes.Where(t => t.Id != id).ToList();
            cadeteriaRepository.BajaCadete(id);

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
                
                cadeteriaRepository.AltaCadete(CadeteRecibido);

                return RedirectToAction("AltaCadete");
            }else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IActionResult EditarCadete(int id)
        {
            Cadeteria Cadeteria = cadeteriaRepository.GetCadeteria();

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
        
            cadeteriaRepository.EditarCadete(Edit);

            return RedirectToAction("ListaDeCadetes");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}