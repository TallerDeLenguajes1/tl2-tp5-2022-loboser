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
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly ICadeteriaRepository _cadeteriaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;

        public ClienteController(ILogger<ClienteController> logger, ICadeteriaRepository cadeteriaRepository, IUsuarioRepository usuarioRepository, IClienteRepository clienteRepository, IPedidoRepository pedidoRepository, IMapper mapper)
        {
            this._logger = logger;
            this._cadeteriaRepository = cadeteriaRepository;
            this._usuarioRepository = usuarioRepository;
            this._clienteRepository = clienteRepository;
            this._pedidoRepository = pedidoRepository;
            this._mapper = mapper;
        }

        public IActionResult Index()        //Muestra al CLiente logueado una lista de sus pedidos
        {
            if (HttpContext.Session.GetString("rol") == "Cliente")
            {
                var pedidos = _pedidoRepository.GetPedidosByCliente(Convert.ToInt32(HttpContext.Session.GetString("id")));
                ViewBag.Nombre = HttpContext.Session.GetString("nombre");
                var pedidosVM = _mapper.Map<List<PedidoViewModel>>(pedidos);
                pedidosVM.ForEach(t => t.NombreCadete = _cadeteriaRepository.GetCadeteById(t.IdCadete).Nombre);

                return View(pedidosVM);
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpGet]
        public IActionResult Clientes(){            //Lista de Clientes
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var clientesVM = _mapper.Map<List<ClienteViewModel>>(_clienteRepository.GetClientes());

                if (clientesVM.Count>0)
                {
                    clientesVM.ForEach(t=>t.User = _usuarioRepository.GetUsuarioByClienteId(t.Id).User);
                }
                
                return View(clientesVM);
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpGet]
        public IActionResult AltaCliente()
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                return View();
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpPost]

        public IActionResult AltaCliente(AltaClienteViewModel Cliente)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    if (_clienteRepository.GetClienteByTelefono(Cliente.Telefono).Id == 0)
                    {
                        var cliente = _mapper.Map<Cliente>(Cliente);
                        var usuario = new Usuario(cliente);

                        if (_usuarioRepository.GetUsuarioByUser(usuario.User).Id == 0)
                        {
                            _clienteRepository.AltaCliente(cliente);
                            _usuarioRepository.AltaUsuario(usuario);
                        }
                        else
                        {
                            TempData["Alert"] = "Error! Ya existe un usuario con ese nombre de usuario...!";
                        }
                    }
                    else
                    {
                        TempData["Alert"] = "Error! Ya existe un cliente con ese numero de telefono...!";
                    }
                    return RedirectToAction("Clientes");
                }
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpGet]
        public IActionResult EditarCliente(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var cliente = _clienteRepository.GetClienteById(id);

                if (cliente.Id != 0)
                {
                    return View(_mapper.Map<EditarClienteViewModel>(cliente));
                }

                return RedirectToAction("Clientes");
            }
            
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpPost]
        public IActionResult EditarCliente(EditarClienteViewModel Edit)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    var cliente = _mapper.Map<Cliente>(Edit);
                    
                    if (_clienteRepository.GetClienteById(cliente.Id).Id != 0)
                    {
                        _clienteRepository.EditarCliente(cliente);      //Edita el Cadete y el Nombre del Usuario asociado a el
                    }
                    else
                    {
                        TempData["Error"] = "No existe el Cliente a Editar...!";
                    }
                }
                else
                {
                    TempData["Error"] = "Error en la Edición de Cliente...!";
                }
                return RedirectToAction("Clientes");
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpGet]
        public IActionResult BajaCliente(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var cliente = _clienteRepository.GetClienteById(id);

                if (cliente.Id != 0)
                {
                    _clienteRepository.BajaCliente(id);     //Elimina el Cliente y el Usuario asociado a el

                }
                else
                {
                    TempData["Error"] = "No existe el Cliente a Eliminar...!";
                }
                return RedirectToAction("Clientes");
            }
            return RedirectToAction("Redireccion", "Usuario");
        }

        [HttpGet]
        public IActionResult VerPedidosCliente(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var pedidos = _mapper.Map<List<PedidoViewModel>>(_pedidoRepository.GetPedidosByCliente(id));
                pedidos.ForEach(t => t.NombreCadete = _cadeteriaRepository.GetCadeteById(t.IdCadete).Nombre);

                var cliente = _clienteRepository.GetClienteById(id);
                ViewBag.Nombre = cliente.Nombre;

                return View(pedidos);
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