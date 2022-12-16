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
            var Clientes = _clienteRepository.GetClientes();

            return View(_mapper.Map<List<ClienteViewModel>>(Clientes));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}