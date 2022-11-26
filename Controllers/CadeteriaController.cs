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
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                Cadeteria Cadeteria = _cadeteriaRepository.GetCadeteria();

                return View(Cadeteria);
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }
        
        [HttpPost]
        public IActionResult ListaDeCadetes(int id){
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                _cadeteriaRepository.BajaCadete(id);

                return RedirectToAction("ListaDeCadetes");
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [HttpGet]
        public IActionResult AltaCadete()
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                return View();
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [HttpPost]
        public IActionResult AltaCadete(AltaCadeteViewModel CadeteRecibido)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    _cadeteriaRepository.AltaCadete(CadeteRecibido);

                    return RedirectToAction("AltaCadete");
                }else
                {
                    return RedirectToAction("Error");
                }
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [HttpGet]
        public IActionResult EditarCadete(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                Cadeteria Cadeteria = _cadeteriaRepository.GetCadeteria();

                var Cadete = Cadeteria.Cadetes.First(t => t.Id == id);

                return View(new EditarCadeteViewModel(Cadete.Id, Cadete.Nombre, Cadete.Direccion, Cadete.Telefono));
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [HttpPost]
        public IActionResult EditarCadete(EditarCadeteViewModel Edit)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                _cadeteriaRepository.EditarCadete(Edit);

                return RedirectToAction("ListaDeCadetes");
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}