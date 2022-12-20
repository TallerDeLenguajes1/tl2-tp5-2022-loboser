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
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteController(ILogger<ClienteController> logger, IClienteRepository clienteRepository, IMapper mapper)
        {
            this._logger = logger;
            this._clienteRepository = clienteRepository;
            this._mapper = mapper;
        }

        public IActionResult Index()                    //Lista de Clientes
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var Clientes = _clienteRepository.GetClientes();
                
                return View(_mapper.Map<List<ClienteViewModel>>(Clientes));
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");  
        }

        [HttpGet]
        public IActionResult AltaCliente()
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

        public IActionResult AltaCliente(AltaClienteViewModel Cliente)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    if (_clienteRepository.GetClienteByTelefono(Cliente.Telefono).Nombre == null)
                    {
                        _clienteRepository.AltaCliente(_mapper.Map<Cliente>(Cliente));
                    }

                    return RedirectToAction("Index", "Cliente");
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
        public IActionResult EditarCliente(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var cliente = _clienteRepository.GetClienteById(id);

                if (cliente.Nombre != null)
                {
                    return View(_mapper.Map<EditarClienteViewModel>(cliente));
                }
                return RedirectToAction("Index");
            }else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("VerPedidos", "Pedido", new{id = 0});
            }
            return RedirectToAction("Index", "Logeo");
        }

        [HttpPost]
        public IActionResult EditarCliente(EditarClienteViewModel Edit)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    if (_clienteRepository.GetClienteById(Edit.Id).Nombre != null)
                    {
                        _clienteRepository.EditarCliente(_mapper.Map<Cliente>(Edit));
                    }
                    return RedirectToAction("Index");
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
        public IActionResult BajaCliente(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (_clienteRepository.GetClienteById(id).Nombre != null)
                {
                    _clienteRepository.BajaCliente(id);
                }
                return RedirectToAction("Index");
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