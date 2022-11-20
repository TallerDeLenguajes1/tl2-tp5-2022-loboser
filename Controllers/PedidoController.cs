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
            List<Pedido> Pedidos = _pedidoRepository.GetPedidos();
            return View(Pedidos);
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult AltaPedido()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AltaPedido(AltaPedidoViewModel PedidoRecibido)
        {
            if (ModelState.IsValid)
            {
                _pedidoRepository.AltaPedido(PedidoRecibido);

                return RedirectToAction("AltaPedido");
            }else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public IActionResult BajaPedido(int nro)
        {
            _pedidoRepository.BajaPedido(nro);

            return RedirectToAction("ListaDePedidos");
        }

        [HttpGet]
        public IActionResult EditarPedido(int nro)
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
        }

        [HttpPost]
        public IActionResult EditarPedido(EditarPedidoViewModel Edit)
        {
            _pedidoRepository.EditarPedido(Edit);
            
            return RedirectToAction("ListaDePedidos");
        }

        [HttpGet]
        public IActionResult AsignarACadete(int nro)
        {   
            Cadeteria Cadeteria = _cadeteriaRepository.GetCadeteria();
            ViewBag.Nro = nro;
            return View(Cadeteria);
        }

        [HttpGet]
        public IActionResult AsignarPedido(int nro, int id)
        {   
            _pedidoRepository.AsignarPedidoCadete(nro, id);
            return RedirectToAction("ListaDePedidos");
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}