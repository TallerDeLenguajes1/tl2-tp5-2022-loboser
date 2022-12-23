using System.Net;
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
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public CadeteriaController(ILogger<CadeteriaController> logger, ICadeteriaRepository cadeteriaRepository, IPedidoRepository pedidoRepository, IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            this._logger = logger;
            this._cadeteriaRepository = cadeteriaRepository;
            this._pedidoRepository = pedidoRepository;
            this._usuarioRepository = usuarioRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                var idCadete = Convert.ToInt32(HttpContext.Session.GetString("id"));
                var cadeteVM = new CadetePedidosViewModel();

                var pedidosDelCadete = _pedidoRepository.GetPedidosByCadete(idCadete);
                var pedidosSinCadete= _pedidoRepository.GetPedidosByCadete(0);

                cadeteVM.Id = Convert.ToInt32(HttpContext.Session.GetString("id"));
                cadeteVM.Nombre = HttpContext.Session.GetString("nombre");
                cadeteVM.PedidosDelCadete = _mapper.Map<List<PedidoViewModel>>(pedidosDelCadete);
                cadeteVM.PedidosSinCadete = _mapper.Map<List<PedidoViewModel>>(pedidosSinCadete);

                return View(cadeteVM);
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpGet]
        public IActionResult Cadetes()
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var Cadeteria = new Cadeteria(_cadeteriaRepository.GetCadetes());
                var CadetesVM = new List<CadeteViewModel>();

                ViewBag.NombreCadeteria = Cadeteria.Nombre;
                ViewBag.TelefonoCadeteria = Cadeteria.Telefono;

                CadetesVM = _mapper.Map<List<CadeteViewModel>>(Cadeteria.Cadetes);

                if (Cadeteria.Cadetes.Count()>0)
                {
                    CadetesVM.ForEach(t => t.User = _usuarioRepository.GetUsuarioByCadeteId(t.Id).User);
                } 

                return View(CadetesVM);
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpGet]
        public IActionResult AltaCadete()
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                return View();
            }
            return RedirectToAction("Redireccion", "Usuario");
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
                    
                    if (_usuarioRepository.GetUsuarioByUser(usuario.User).Id == 0)
                    {
                        _usuarioRepository.AltaUsuario(usuario);
                        _cadeteriaRepository.AltaCadete(cadete);
                    }

                    return RedirectToAction("Cadetes");
                }
            }
            return RedirectToAction("Redireccion", "Usuario");
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
                return RedirectToAction("Cadetes");
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpPost]
        public IActionResult EditarCadete(EditarCadeteViewModel Edit)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    var cadete = _mapper.Map<Cadete>(Edit);

                    if (_cadeteriaRepository.GetCadeteById(cadete.Id).Id != 0)
                    {
                        _cadeteriaRepository.EditarCadete(cadete);      //Edita el Cadete y el Nombre del Usuario asociado a el
                    }
                }
                return RedirectToAction("Cadetes");
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpGet]
        public IActionResult BajaCadete(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var cadete = _cadeteriaRepository.GetCadeteById(id);

                if (cadete.Id != 0)
                {
                    _cadeteriaRepository.BajaCadete(id);        //Elimina el Cadete y el Usuario asociado a el
                }
                return RedirectToAction("Cadetes");
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpGet]
        public IActionResult VerPedidosCadete(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var cadete = _cadeteriaRepository.GetCadeteById(id);

                if (cadete.Nombre != null)
                {
                    ViewBag.Nombre = cadete.Nombre;
                    ViewBag.Rol = HttpContext.Session.GetString("rol");

                    return View(_mapper.Map<List<PedidoViewModel>>(cadete.Pedidos));
                }
                return RedirectToAction("Cadetes");
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}