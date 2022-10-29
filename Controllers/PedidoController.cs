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
    public class PedidoController : Controller
    {
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(ILogger<PedidoController> logger)
        {
            _logger = logger;
        }

        static List<Pedido> Pedidos = new List<Pedido>();
        static int nroPedidos = 1;
        static int idCliente = 1;
        
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
        public IActionResult AltaPedido(string obs, Cliente Cliente)
        {
            Cliente.Id = idCliente;
            var Pedido = new Pedido(nroPedidos, obs, Cliente);
            
            idCliente++;
            nroPedidos++;
            
            Pedidos.Add(Pedido);

            return RedirectToAction("AltaPedido");
        }

        [HttpGet]
        public IActionResult BajaPedido()
        {
            return View(Pedidos);
        }

        [HttpPost]
        public IActionResult BajaPedido(int nro)
        {
            Pedidos = Pedidos.Where(t => t.Nro != nro).ToList();
            return RedirectToAction("BajaPedido");
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}