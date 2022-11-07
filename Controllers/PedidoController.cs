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
    public class PedidoController : Controller
    {
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(ILogger<PedidoController> logger)
        {
            _logger = logger;
        }

        List<Pedido> Pedidos = DataBase.CargarPedidosDB();
        //static int nroPedidos = 1;
        //static int idCliente = 1;

        [HttpGet]
        public IActionResult ListaDePedidos()
        {
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
                /*
                PedidoRecibido.Cliente.Id = idCliente;
                var Pedido = new Pedido(nroPedidos, PedidoRecibido.Obs, PedidoRecibido.Cliente);
                
                idCliente++;
                nroPedidos++;
                */

                DataBase.AltaPedidoDB(PedidoRecibido);

                return RedirectToAction("AltaPedido");
            }else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public IActionResult BajaPedido(int nro)
        {
            //Pedidos = Pedidos.Where(t => t.Nro != nro).ToList();
            DataBase.BajaPedidoDB(nro);

            return RedirectToAction("ListaDePedidos");
        }

        [HttpGet]
        public IActionResult EditarPedido(int nro)
        {
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
            /*
            Pedido Pedido = Pedidos.First(t => t.Nro == Edit.Nro);

            Pedido.Obs = Edit.Obs;
            Pedido.Cliente = Edit.Cliente;
            */

            DataBase.EditarPedidoDB(Edit);
            
            return RedirectToAction("ListaDePedidos");
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}