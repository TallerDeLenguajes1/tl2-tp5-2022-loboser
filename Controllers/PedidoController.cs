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
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var PedidosVM = _mapper.Map<List<PedidoViewModel>>(_pedidoRepository.GetPedidos());
                
                if (PedidosVM.Count()>0)
                {
                    PedidosVM.ForEach(t => t.NombreCadeteAsignado = _cadeteriaRepository.GetCadeteById(t.IdCadeteAsignado).Nombre);
                }

                return View(PedidosVM);
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

        [HttpGet]
        public IActionResult CambiarEstado(int nro, string aux)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var pedido = _pedidoRepository.GetPedidoByNro(nro);

                if (pedido.Estado != null)
                {
                    pedido.Estado = (pedido.Estado == "En Proceso")?"Entregado":"En Proceso";
                    _pedidoRepository.EditarPedido(pedido);
                }

                var redirect = Redirect(pedido.Cliente.Id, pedido.IdCadeteAsignado, aux);

                return RedirectToAction(redirect[0], "Pedido", new {id = redirect[1]});
            }
            else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                Cadete cadete = _cadeteriaRepository.GetCadetes().First(t=> t.Nombre == HttpContext.Session.GetString("nombre"));

                if (cadete.Pedidos.Where(t => t.Nro == nro).ToList().Count > 0)
                {
                    var pedido = cadete.Pedidos.First(t => t.Nro == nro);
                    pedido.Estado = (pedido.Estado == "En Proceso")?"Entregado":"En Proceso";

                    _pedidoRepository.EditarPedido(pedido);
                }
                return RedirectToAction("VerPedidos", "Pedido", new{ id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }
        
        [HttpGet]
        public IActionResult AltaPedido()
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                return View();
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
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

                    return RedirectToAction("Index", "Pedido");
                }else
                {
                    return RedirectToAction("Error");
                }
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            
        }

        [HttpGet]
        public IActionResult BajaPedido(int nro, string aux)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var pedido = _pedidoRepository.GetPedidoByNro(nro);
                if (pedido.Obs != null)
                {
                    _pedidoRepository.BajaPedido(nro);
                }

                var redirect = Redirect(pedido.Cliente.Id, pedido.IdCadeteAsignado, aux);

                return RedirectToAction(redirect[0], "Pedido", new {id = redirect[1]});
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

        [HttpGet]
        public IActionResult EditarPedido(int nro, string aux)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var pedido = _pedidoRepository.GetPedidoByNro(nro);

                if (pedido.Obs != null)
                {
                    ViewBag.Aux = aux;
                    return View(_mapper.Map<EditarPedidoViewModel>(pedido));
                }else
                {
                    return RedirectToAction("AltaPedido");
                }
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

        [HttpPost]
        public IActionResult EditarPedido(EditarPedidoViewModel Edit)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    var pedido = _mapper.Map<Pedido>(Edit);

                    _pedidoRepository.EditarPedido(pedido);

                    var redirect = Redirect(pedido.Cliente.Id, pedido.IdCadeteAsignado, Edit.Aux);

                    return RedirectToAction(redirect[0], "Pedido", new {id = redirect[1]});
                }
                return RedirectToAction("Index", "Pedido");
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

        [HttpGet]
        public IActionResult AsignarACadete(int nro, string aux)
        {   
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var cadetes = _cadeteriaRepository.GetCadetes();
                ViewBag.Nro = nro;
                ViewBag.Aux = aux;

                return View(_mapper.Map<List<CadeteViewModel>>(cadetes));
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }
        
        [HttpGet]
        public IActionResult AsignarPedidoACadete(int nro, int id, string aux)
        {   
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var pedido = _pedidoRepository.GetPedidoByNro(nro);
                if (pedido.Obs != null)
                {
                    var idCadete = pedido.IdCadeteAsignado;
                    pedido.IdCadeteAsignado = id;

                    _pedidoRepository.EditarPedido(pedido);

                    var redirect = Redirect(pedido.Cliente.Id, idCadete, aux);

                    return RedirectToAction(redirect[0], "Pedido", new {id = redirect[1]});
                }
                return RedirectToAction("Index", "Pedido");
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

        [HttpGet]
        public IActionResult AsignarACliente(int nro, string aux)
        {   
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var clientes = _clienteRepository.GetClientes();
                ViewBag.Nro = nro;
                ViewBag.Aux = aux;

                return View(_mapper.Map<List<ClienteViewModel>>(clientes));
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

        [HttpGet]
        public IActionResult AsignarPedidoACliente(int nro, int id, string aux)
        {   
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var pedido = _pedidoRepository.GetPedidoByNro(nro);
                if (pedido.Obs != null)
                {
                    var idCliente = pedido.Cliente.Id;
                    pedido.Cliente = _clienteRepository.GetClienteById(id);

                    _pedidoRepository.EditarPedido(pedido);

                    var redirect = Redirect(idCliente, pedido.IdCadeteAsignado, aux);

                    return RedirectToAction(redirect[0], "Pedido", new {id = redirect[1]});
                }
                return RedirectToAction("Index", "Pedido");
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

        [HttpGet]
        public IActionResult VerPedidos(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var cadete = _cadeteriaRepository.GetCadeteById(id);

                if (cadete.Nombre != null)
                {
                    ViewBag.Nombre = cadete.Nombre;

                    return View(_mapper.Map<List<PedidoViewModel>>(cadete.Pedidos));
                }else
                {
                    return RedirectToAction("Index", "Cadeteria");
                }
            }
            else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                var cadetes = _cadeteriaRepository.GetCadetes();
                
                var cadete = cadetes.First(t => t.Nombre == HttpContext.Session.GetString("nombre"));   //Como en sesion guardo el nombre del Cadete/Usuario
                ViewBag.Nombre = cadete.Nombre;
                
                return View(_mapper.Map<List<PedidoViewModel>>(cadete.Pedidos));
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
            }
            return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        public string[] Redirect(int idCliente, int idCadete, string aux){      //funcion casera para rediccionar a la pagina que estaba
            string[] redirect = new string[2];
            if (idCadete != 0 && aux == "Cadete")
            {
                redirect[0] = "VerPedidos";
                redirect[1] = idCadete.ToString();
            }
            else if (idCliente != 0 && aux == "Cliente")
            {
                redirect[0] = "VerPedidosCliente";
                redirect[1] = idCliente.ToString();
            }
            else
            {
                redirect[0] = "Index";
                redirect[1] = "0";
            }
            return redirect;
        }
    }
}