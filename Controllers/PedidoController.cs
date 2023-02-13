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
        public IActionResult Index()            //Lista de Pedidos
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var PedidosVM = _mapper.Map<List<PedidoViewModel>>(_pedidoRepository.GetPedidos());
                
                if (PedidosVM.Count()>0)
                {
                    PedidosVM.ForEach(t => t.NombreCadete = _cadeteriaRepository.GetCadeteById(t.IdCadete).Nombre);
                }

                return View(PedidosVM);
            } 
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpGet]
        public IActionResult CambiarEstado(int nro, string aux)
        {
            var pedido = _pedidoRepository.GetPedidoByNro(nro);

            if (HttpContext.Session.GetString("rol") == "Admin" && pedido.Nro != 0)
            {
                CambiarEstado(pedido);

                return RedirectToAction("Redireccion", "Usuario", new {idCadete = pedido.IdCadete, idCliente = pedido.Cliente.Id, aux = aux});
            }
            else if (HttpContext.Session.GetString("rol") == "Cadete" && pedido.Nro != 0)
            {
                var cadete = _cadeteriaRepository.GetCadeteById(Convert.ToInt32(HttpContext.Session.GetString("id")));

                if (cadete.Pedidos.Where(t => t.Nro == nro).ToList().Count > 0)
                {
                    CambiarEstado(pedido);
                }
            }
            return RedirectToAction("Redireccion", "Usuario");
        }
        
        [HttpGet]
        public IActionResult AltaPedido()
        {
            if (HttpContext.Session.GetString("rol") == "Admin" || HttpContext.Session.GetString("rol") == "Cliente")
            {
                ViewBag.Rol = HttpContext.Session.GetString("rol");
                ViewBag.Id = HttpContext.Session.GetString("id");

                var altaPedido = new AltaPedidoViewModel();
                altaPedido.Cadetes = _mapper.Map<List<CadeteViewModel>>(_cadeteriaRepository.GetCadetes());
                altaPedido.Clientes = _mapper.Map<List<ClienteViewModel>>(_clienteRepository.GetClientes());

                return View(altaPedido);
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpPost]
        public IActionResult AltaPedido(AltaPedidoViewModel PedidoRecibido)
        {
            if (HttpContext.Session.GetString("rol") == "Admin" || HttpContext.Session.GetString("rol") == "Cliente")
            {
                if (ModelState.IsValid)
                {
                    var pedido = _mapper.Map<Pedido>(PedidoRecibido);
                    _pedidoRepository.AltaPedido(pedido);
                }
                else
                {
                    TempData["Error"] = "Error en la subida de Pedido Exitosa...!";
                }
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpGet]
        public IActionResult BajaPedido(int nro, string aux)           // el cliente solo puede dar de baja a su pedido                                       
        {                                                              // si el pedido no esta asignado a un cadete
            var pedido = _pedidoRepository.GetPedidoByNro(nro);
            if (pedido.Nro != 0 && (HttpContext.Session.GetString("rol") == "Admin" || (HttpContext.Session.GetString("rol") == "Cliente" && pedido.Cliente.Id == Convert.ToInt32(HttpContext.Session.GetString("id")) && pedido.IdCadete == 0)))
            {
                _pedidoRepository.BajaPedido(nro);
            }
            else
            {
                TempData["Error"] = "No existe el Pedido a Eliminar...!";
            }
            return RedirectToAction("Redireccion", "Usuario", new{idCadete = pedido.IdCadete, idCliente = pedido.Cliente.Id, aux = aux});
        }

        [HttpGet]
        public IActionResult EditarPedido(int nro, string aux)
        {
            var pedido = _pedidoRepository.GetPedidoByNro(nro);         //El cliente solo puede editar el pedido si es suyo y si no fue asignado a un cadete

            if (pedido.Nro != 0 && (HttpContext.Session.GetString("rol") == "Admin" || (HttpContext.Session.GetString("rol") == "Cliente" && pedido.Cliente.Id == Convert.ToInt32(HttpContext.Session.GetString("id")) && pedido.IdCadete == 0)))
            {
                var editPedidoVM = _mapper.Map<EditarPedidoViewModel>(pedido);
                editPedidoVM.Clientes = _mapper.Map<List<ClienteViewModel>>(_clienteRepository.GetClientes());
                editPedidoVM.Cadetes = _mapper.Map<List<CadeteViewModel>>(_cadeteriaRepository.GetCadetes());
                ViewBag.Aux = aux;
                ViewBag.Rol = HttpContext.Session.GetString("rol");

                return View(editPedidoVM);
            }

            return RedirectToAction("Redireccion", "Usuario", new{idCadete = pedido.IdCadete, idCliente = pedido.Cliente.Id, aux = aux});
        }

        [HttpPost]
        public IActionResult EditarPedido(EditarPedidoViewModel Edit)
        {
            if (HttpContext.Session.GetString("rol") == "Admin" || (HttpContext.Session.GetString("rol") == "Cliente" && Edit.ClienteId == Convert.ToInt32(HttpContext.Session.GetString("id")) && Edit.IdCadete == 0))
            {
                if (ModelState.IsValid)
                {
                    var pedido = _mapper.Map<Pedido>(Edit);

                    if (_pedidoRepository.GetPedidoByNro(Edit.Nro).Nro != 0)
                    {
                        _pedidoRepository.EditarPedido(pedido);
                    }
                    else
                    {
                        TempData["Error"] = "No existe el Pedido a Editar...!";
                    }
                }
                else
                {
                    TempData["Error"] = "Error! al editar el Pedido...!";
                }
            }
            return RedirectToAction("Redireccion", "Usuario", new{idCadete = Edit.IdCadete, idCliente = Edit.ClienteId, aux = Edit.Aux});
        }
        
        [HttpGet]
        public IActionResult AsignarPedido(int nro, string aux)
        {
            bool desasignar = true;
            var pedido = _pedidoRepository.GetPedidoByNro(nro);
            int idCadete = pedido.IdCadete;

            if (HttpContext.Session.GetString("rol") == "Admin")        //Solo usado para desasignar siendo admin
            {
                Asignar(desasignar, 0, pedido);
            }
            else if (HttpContext.Session.GetString("rol") == "Cadete")         //Usado para asignar y desasignar pedidos
            {
                idCadete = Convert.ToInt32(HttpContext.Session.GetString("id"));

                var cadete = _cadeteriaRepository.GetCadeteById(idCadete);
                desasignar = cadete.Pedidos.Where(t => t.Nro == nro).Count()>0;

                Asignar(desasignar, idCadete, pedido);  
            }
            return RedirectToAction("Redireccion", "Usuario", new{idCadete = idCadete, idCliente = pedido.Cliente.Id, aux = aux});
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        private void CambiarEstado(Pedido pedido)             //cambia de estado al pedido
        {
            pedido.Estado = (pedido.Estado == "En Proceso")?"Entregado":"En Proceso";
            _pedidoRepository.EditarPedido(pedido);
        }
        private void Asignar(bool desasignar, int idCadete, Pedido pedido)          //asigna o desasigna el pedido a un cadete
        {
            if (desasignar)
            {
                pedido.IdCadete = 0;
                pedido.Estado = "En Proceso";
                _pedidoRepository.EditarPedido(pedido);
            }
            else if(pedido.IdCadete == 0)
            {
                pedido.IdCadete = idCadete;
                _pedidoRepository.EditarPedido(pedido);
            }
        }
    }
}