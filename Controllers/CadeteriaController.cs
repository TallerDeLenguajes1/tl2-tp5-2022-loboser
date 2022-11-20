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
        private readonly Repositories.ICadeteriaRepository _cadeteriaRepository;

        public CadeteriaController(ILogger<CadeteriaController> logger, ICadeteriaRepository cadeteriaRepository)
        {
            this._cadeteriaRepository = cadeteriaRepository;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListaDeCadetes()
        {
            Cadeteria Cadeteria = _cadeteriaRepository.GetCadeteria();

            return View(Cadeteria);
        }
        
        [HttpPost]
        public IActionResult ListaDeCadetes(int id){
            _cadeteriaRepository.BajaCadete(id);

            return RedirectToAction("ListaDeCadetes");
        }

        [HttpGet]
        public IActionResult VerPedidos(int id)
        {
            Cadeteria Cadeteria = _cadeteriaRepository.GetCadeteria();

            var Cadete = Cadeteria.Cadetes.First(t => t.Id == id);

            return View(Cadete.Pedidos);
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
                _cadeteriaRepository.AltaCadete(CadeteRecibido);

                return RedirectToAction("AltaCadete");
            }else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IActionResult EditarCadete(int id)
        {
            Cadeteria Cadeteria = _cadeteriaRepository.GetCadeteria();

            var Cadete = Cadeteria.Cadetes.First(t => t.Id == id);

            return View(new EditarCadeteViewModel(Cadete.Id, Cadete.Nombre, Cadete.Direccion, Cadete.Telefono));
        }

        [HttpPost]
        public IActionResult EditarCadete(EditarCadeteViewModel Edit)
        {
            _cadeteriaRepository.EditarCadete(Edit);

            return RedirectToAction("ListaDeCadetes");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}