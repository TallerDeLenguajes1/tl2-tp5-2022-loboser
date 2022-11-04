using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace tl2_tp5_2022_loboser.Views.Pedido
{
    public class EditarPedido : PageModel
    {
        private readonly ILogger<EditarPedido> _logger;

        public EditarPedido(ILogger<EditarPedido> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}