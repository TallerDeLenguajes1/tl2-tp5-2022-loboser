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

namespace tl2_tp4_2022_loboser.Controllers
{
    public class CadeteriaController : Controller
    {
        private readonly ILogger<CadeteriaController> _logger;
        private readonly Repositories.ICadeteriaRepository _cadeteriaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public CadeteriaController(ILogger<CadeteriaController> logger, ICadeteriaRepository cadeteriaRepository, IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            this._logger = logger;
            this._cadeteriaRepository = cadeteriaRepository;
            this._usuarioRepository = usuarioRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var Cadeteria = new Cadeteria(_cadeteriaRepository.GetCadetes());
                var CadetesVM = new List<CadeteViewModel>();

                ViewBag.NombreCadeteria = Cadeteria.Nombre;
                ViewBag.TelefonoCadeteria = Cadeteria.Telefono;

                if (Cadeteria.Cadetes.Count()>0)
                {
                    CadetesVM = _mapper.Map<List<CadeteViewModel>>(Cadeteria.Cadetes);
                } 

                return View(CadetesVM);
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            
        }


        [HttpGet]
        public IActionResult AltaCadete()
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                return View();
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            
        }

        [HttpPost]
        public IActionResult AltaCadete(AltaCadeteViewModel CadeteRecibido)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    var cadete = _mapper.Map<Cadete>(CadeteRecibido);
                    var usuario = new Usuario(cadete);
                    
                    if (_usuarioRepository.GetUsuarioLikeUser(usuario.User).Nombre == null)
                    {
                        _usuarioRepository.AltaUsuario(usuario);
                        _cadeteriaRepository.AltaCadete(cadete);
                    }

                    return RedirectToAction("Index", "Cadeteria");
                }
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

        [HttpGet]
        public IActionResult EditarCadete(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var Cadete = _cadeteriaRepository.GetCadeteById(id);

                if (Cadete.Nombre != null)
                {
                    return View(_mapper.Map<EditarCadeteViewModel>(Cadete));
                }
                return RedirectToAction("Index", "Cadeteria");
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

        [HttpPost]
        public IActionResult EditarCadete(EditarCadeteViewModel Edit)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    var cadete = _mapper.Map<Cadete>(Edit);

                    var oldCadete = _cadeteriaRepository.GetCadeteById(Edit.Id);
                    var oldUsuario = _usuarioRepository.GetUsuarioLikeUser(oldCadete.Nombre.ToLower().Replace(" ", string.Empty));

                    if (oldUsuario.IdUsuario != 0 && _cadeteriaRepository.GetCadeteById(cadete.Id).Nombre != null)
                    {
                        var usuario = new Usuario(cadete);
                        usuario.IdUsuario = oldUsuario.IdUsuario;

                        _usuarioRepository.EditarUsuario(usuario);
                        _cadeteriaRepository.EditarCadete(cadete);
                    }
                    
                    return RedirectToAction("Index", "Cadeteria");
                }
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

        [HttpGet]
        public IActionResult BajaCadete(int id){
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (_cadeteriaRepository.GetCadeteById(id).Nombre != null)
                {
                    _cadeteriaRepository.BajaCadete(id);
                }

                return RedirectToAction("Index", "Cadeteria");
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}