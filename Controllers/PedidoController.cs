using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;
using tl2_tp4_2022_loboser.Repositories;

namespace tl2_tp4_2022_loboser.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ILogger<PedidoController> _logger;
        private readonly ICadeteriaRepository _cadeteriaRepository;
        private readonly Repositories.IPedidoRepository _pedidoRepository;

        public PedidoController(ILogger<PedidoController> logger, ICadeteriaRepository cadeteriaRepository, IPedidoRepository pedidoRepository, IClienteRepository clienteRepository)
        {
            this._cadeteriaRepository = cadeteriaRepository;
            this._pedidoRepository = pedidoRepository;
            this._logger = logger;
        }

        [HttpGet]
        public IActionResult ListaDePedidos()
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                List<Pedido> Pedidos = _pedidoRepository.GetPedidos();
                return View(Pedidos);
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }else
            {
                return RedirectToAction("Index", "Logeo");
            } 
        }

        [HttpGet]
        public IActionResult CambiarEstado(int nro)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                _pedidoRepository.CambiarEstado(nro);
                return RedirectToAction("VerPedidos", "Pedido", new{ id = 0});
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                Cadete cadete = _cadeteriaRepository.GetCadeteria().Cadetes.First(t=> t.Nombre == HttpContext.Session.GetString("nombre"));
                if (cadete.Pedidos.Where(t => t.Nro == nro).ToList().Count > 0)
                {
                    _pedidoRepository.CambiarEstado(nro);
                }
                return RedirectToAction("VerPedidos", "Pedido", new{ id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult AltaPedido()
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
        public IActionResult AltaPedido(AltaPedidoViewModel PedidoRecibido)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    _pedidoRepository.AltaPedido(PedidoRecibido);

                    return RedirectToAction("AltaPedido");
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

        [HttpPost]
        public IActionResult BajaPedido(int nro)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    _pedidoRepository.BajaPedido(nro);

                    return RedirectToAction("ListaDePedidos");
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
        public IActionResult EditarPedido(int nro)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                List<Pedido> Pedidos = _pedidoRepository.GetPedidos();

                if (Pedidos.Count()> 0)
                {
                    var PedidoAEditar = Pedidos.First(t => t.Nro == nro);
                    return View(new EditarPedidoViewModel(PedidoAEditar.Nro, PedidoAEditar.Obs, PedidoAEditar.Cliente));
                }else
                {
                    return RedirectToAction("AltaPedido");
                }
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [HttpPost]
        public IActionResult EditarPedido(EditarPedidoViewModel Edit)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                _pedidoRepository.EditarPedido(Edit);
            
                return RedirectToAction("ListaDePedidos");
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [HttpGet]
        public IActionResult AsignarACadete(int nro)
        {   
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                Cadeteria Cadeteria = _cadeteriaRepository.GetCadeteria();
                ViewBag.Nro = nro;
                return View(Cadeteria);
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [HttpGet]
        public IActionResult AsignarPedido(int nro, int id)
        {   
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                _pedidoRepository.AsignarPedidoCadete(nro, id);
                return RedirectToAction("ListaDePedidos");
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [HttpGet]
        public IActionResult VerPedidos(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                Cadeteria Cadeteria = _cadeteriaRepository.GetCadeteria();

                id = Cadeteria.Cadetes.First(t => t.Nombre == HttpContext.Session.GetString("nombre")).Id;
                var Cadete = Cadeteria.Cadetes.First(t => t.Id == id);

                return View(Cadete.Pedidos);
            }else if (HttpContext.Session.GetString("rol") == "Admin")
            {
                return RedirectToAction("ListaDeCadetes","Cadeteria");
            }
            return RedirectToAction("Index","Logeo");
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}