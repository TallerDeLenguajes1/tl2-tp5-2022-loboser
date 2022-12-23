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
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICadeteriaRepository _cadeteriaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository, ICadeteriaRepository cadeteriaRepository, IClienteRepository clienteRepository, IMapper mapper)
        {
            this._logger = logger;
            this._usuarioRepository = usuarioRepository;
            this._cadeteriaRepository = cadeteriaRepository;
            this._clienteRepository = clienteRepository;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("nombre") != null){
                var nombre = HttpContext.Session.GetString("nombre");
                HttpContext.Session.Clear();
                TempData["Info"] = "Deslogeo Exitoso...!";

                _logger.LogTrace("Deslogeo de {User} exitoso!", nombre);
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult Logear(LogeoViewModel Logeo)
        {
            if (ModelState.IsValid)
            {
                var logeo = _mapper.Map<Usuario>(Logeo);
                Usuario usuario = _usuarioRepository.GetUsuario(logeo);

                if (usuario.User != null)
                {
                    HttpContext.Session.SetString("id", (usuario.IdCliente != 0)?usuario.IdCliente.ToString():usuario.IdCadete.ToString());
                    HttpContext.Session.SetString("nombre", usuario.Nombre);
                    HttpContext.Session.SetString("rol", usuario.Rol);

                    if (usuario.Rol == "Admin")
                    {
                        return RedirectToAction("Cadetes", "Cadeteria");
                        
                    }else if (usuario.Rol == "Cadeteria")
                    {
                        return RedirectToAction("Index", "Cadeteria");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Cliente");
                    }
                }
                TempData["Error"] = "Usuario o Contraseña incorrectos...!";

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            if (HttpContext.Session.GetString("nombre") == null)
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Registro(RegistroViewModel registroUsuario)
        {
            if (ModelState.IsValid)
            {
                if (registroUsuario.ConfirmPass == registroUsuario.Pass)
                {
                    var usuario = _mapper.Map<Usuario>(registroUsuario);
                    var cliente = _mapper.Map<Cliente>(registroUsuario);

                    if (_usuarioRepository.GetUsuarioByUser(usuario.User).Id == 0 && _clienteRepository.GetClienteByTelefono(cliente.Telefono).Id == 0)
                    {
                        _clienteRepository.AltaCliente(cliente);
                        cliente = _clienteRepository.GetClienteByTelefono(cliente.Telefono);

                        usuario.IdCliente = cliente.Id;
                        _usuarioRepository.AltaUsuario(usuario);
                        TempData["Info"] = "Registro Exitoso...!";
                    }
                    else if (_usuarioRepository.GetUsuarioByUser(usuario.User).Id != 0)
                    {
                        TempData["Error"] = "No se pudo porque ya existe ese nombre de usuario...!";
                    }else
                    {
                        TempData["Error"] = "No se pudo porque ya se registró un cliente con ese número de telefono...!";
                    }
                }
                else
                {
                    TempData["Error"] = "Las Contraseñas no son las mismas...!";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Usuarios()
        {
            if(HttpContext.Session.GetString("rol") == "Admin")
            {
                var usuarios = _mapper.Map<List<UsuarioViewModel>>(_usuarioRepository.GetUsuarios());

                foreach (var usuario in usuarios)
                {
                    if (usuario.IdCadete != 0)
                    {
                        var cadete = _cadeteriaRepository.GetCadeteById(usuario.IdCadete);

                        usuario.Direccion = cadete.Direccion;
                        usuario.Telefono = cadete.Telefono;
                    }
                    else if(usuario.IdCliente!= 0)
                    {
                        var cliente = _clienteRepository.GetClienteById(usuario.IdCliente);

                        usuario.Direccion = cliente.Direccion;
                        usuario.Telefono = cliente.Telefono;
                    }
                }
                return View(usuarios);
            }
            
            return RedirectToAction("Redireccion");
        }

        [HttpGet]
        public IActionResult EditarUsuario(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var usuario = _usuarioRepository.GetUsuarioById(id);

                if (usuario.Id != 0)
                {
                    return View(_mapper.Map<EditarUsuarioViewModel>(usuario));
                }
                return RedirectToAction("Usuarios");
            }
            return RedirectToAction("Redireccion");
        }

        [HttpPost]
        public IActionResult EditarUsuario(EditarUsuarioViewModel usuarioEdit)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    var usuario = _mapper.Map<Usuario>(usuarioEdit);

                    if (_usuarioRepository.GetUsuarioById(usuario.Id).Id != 0)
                    {
                        _usuarioRepository.EditarUsuario(usuario);      //Edita el Usuario y el Nombre del Cadete/Cliente asociado a el
                    }
                    else
                    {
                        TempData["Error"] = "Error al Editar usuario...!";
                        
                    }
                    return RedirectToAction("Usuarios");
                }
            }
            return RedirectToAction("Redireccion");
        }

        [HttpGet]
        public IActionResult BajaUsuario(int id)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                var usuario = _usuarioRepository.GetUsuarioById(id);

                if (usuario.Id != 0)
                {
                    _usuarioRepository.BajaUsuario(usuario);        //Elimina el Usuario y el Cadete/Cliente asociado a el
                }
                TempData["Error"] = "No se encontró el usuario...!";

                return RedirectToAction("Usuarios");
            }
            return RedirectToAction("Redireccion");
        }

        public IActionResult Redireccion(int idCadete,int idCliente, string aux)
        {
            if (HttpContext.Session.GetString("rol") == "Admin")
            {
                if (aux == "Cadete" || aux == "Cliente")
                {
                    return RedirectToAction("VerPedidos" + aux, (aux=="Cadete")?"Cadeteria":aux, new {id = (aux=="Cadete")?idCadete:idCliente});
                }
                return RedirectToAction("Index", "Pedido");
            }
            else if (HttpContext.Session.GetString("rol") == "Cadete")
            {
                return RedirectToAction("Index", "Cadeteria");
            }
            else if (HttpContext.Session.GetString("rol") == "Cliente")
            {
                return RedirectToAction("Index", "Cliente");
            }
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}