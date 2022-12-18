using System.Net;
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
using AutoMapper;

namespace tl2_tp4_2022_loboser.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ILogger<PedidoController> _logger;
        private readonly ICadeteriaRepository _cadeteriaRepository;
        private readonly Repositories.IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public PedidoController(ILogger<PedidoController> logger, ICadeteriaRepository cadeteriaRepository, IPedidoRepository pedidoRepository, IClienteRepository clienteRepository, IMapper mapper)
        {
            this._cadeteriaRepository = cadeteriaRepository;
            this._pedidoRepository = pedidoRepository;
            this._clienteRepository = clienteRepository;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult ListaDePedidos()
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var Pedidos = _mapper.Map<List<PedidoViewModel>>(_pedidoRepository.GetPedidos());
                Pedidos.ForEach(t => t.NombreCadeteAsignado = _cadeteriaRepository.GetCadeteById(t.IdCadeteAsignado).Nombre);

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
                var pedido = _pedidoRepository.GetPedidoByNro(nro);

                pedido.Estado = (pedido.Estado == "En Proceso")?"Entregado":"En Proceso";

                _pedidoRepository.EditarPedido(pedido);
                return RedirectToAction("VerPedidos", "Pedido", new{ id = pedido.IdCadeteAsignado});
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                Cadete cadete = _cadeteriaRepository.GetCadetes().First(t=> t.Nombre == HttpContext.Session.GetString("nombre"));
                if (cadete.Pedidos.Where(t => t.Nro == nro).ToList().Count > 0)
                {
                    var pedido = _pedidoRepository.GetPedidoByNro(nro);
                    pedido.Estado = (pedido.Estado == "En Proceso")?"Entregado":"En Proceso";

                    _pedidoRepository.EditarPedido(pedido);
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
                    var pedido = _mapper.Map<Pedido>(PedidoRecibido);
                    pedido.Estado = "En Proceso";
                    _pedidoRepository.AltaPedido(pedido);

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
                var pedido = _pedidoRepository.GetPedidoByNro(nro);

                if (pedido is not null)
                {
                    return View(_mapper.Map<EditarPedidoViewModel>(pedido));
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
                if (ModelState.IsValid)
                {
                    var Pedido = _mapper.Map<Pedido>(Edit);
                    var pedidoAux = _pedidoRepository.GetPedidoByNro(Edit.Nro);

                    Pedido.IdCadeteAsignado = pedidoAux.IdCadeteAsignado;
                    Pedido.Cliente = pedidoAux.Cliente;

                    _pedidoRepository.EditarPedido(Pedido);
                }else
                {
                    return RedirectToAction("Error");
                }
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
                var cadetes = _cadeteriaRepository.GetCadetes();
                ViewBag.Nro = nro;

                return View(_mapper.Map<List<CadeteViewModel>>(cadetes));
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }
        
        [HttpGet]
        public IActionResult AsignarPedidoACadete(int nro, int id)
        {   
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var pedido = _pedidoRepository.GetPedidoByNro(nro);
                pedido.IdCadeteAsignado = id;

                _pedidoRepository.EditarPedido(pedido);
                
                return RedirectToAction("ListaDePedidos");
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [HttpGet]
        public IActionResult AsignarACliente(int nro)
        {   
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var clientes = _clienteRepository.GetClientes();
                ViewBag.Nro = nro;

                return View(_mapper.Map<List<ClienteViewModel>>(clientes));
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [HttpGet]
        public IActionResult AsignarPedidoACliente(int nro, int id)
        {   
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var pedido = _pedidoRepository.GetPedidoByNro(nro);
                pedido.Cliente = _clienteRepository.GetClienteById(id);

                _pedidoRepository.EditarPedido(pedido);

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
                var cadetes = _cadeteriaRepository.GetCadetes();
                var cadete = cadetes.First(t => t.Nombre == HttpContext.Session.GetString("nombre"));   //Como en sesion guardo el nombre del Cadete/Usuario
                ViewBag.Nombre = cadete.Nombre;
                
                return View(_mapper.Map<List<PedidoViewModel>>(cadete.Pedidos));
            }else if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var cadete = _cadeteriaRepository.GetCadeteById(id);

                if (cadete is not null && id !=0)
                {
                    ViewBag.Nombre = cadete.Nombre;

                    return View(_mapper.Map<List<PedidoViewModel>>(cadete.Pedidos));
                }else
                {
                    return RedirectToAction("ListaDeCadetes", "Cadeteria");
                }
            }
            return RedirectToAction("Index","Logeo");
        }

        [HttpGet]
        public IActionResult VerPedidosCliente(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var pedidos = _mapper.Map<List<PedidoViewModel>>(_pedidoRepository.GetPedidosByCliente(id));
                pedidos.ForEach(t => t.NombreCadeteAsignado = _cadeteriaRepository.GetCadeteById(t.IdCadeteAsignado).Nombre);

                var cliente = _clienteRepository.GetClienteById(id);
                ViewBag.Nombre = cliente.Nombre;

                return View(pedidos);
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
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