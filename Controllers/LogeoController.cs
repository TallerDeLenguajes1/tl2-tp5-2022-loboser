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
    // [Route("[controller]")]
    public class LogeoController : Controller
    {
        private readonly ILogger<LogeoController> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public LogeoController(ILogger<LogeoController> logger, IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            this._logger = logger;
            this._usuarioRepository = usuarioRepository;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logear(UsuarioViewModel Logeo)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = _usuarioRepository.GetUsuario(_mapper.Map<Usuario>(Logeo));

                if (usuario.User is not null)
                {
                    HttpContext.Session.SetString("nombre", usuario.Nombre);
                    HttpContext.Session.SetString("rol", usuario.Rol);
                    if (usuario.Rol == "Admin")
                    {
                        return RedirectToAction("ListaDeCadetes", "Cadeteria");
                        
                    }else
                    {
                        return RedirectToAction("VerPedidos", "Pedido", new{ id = 0});
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Logear(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}