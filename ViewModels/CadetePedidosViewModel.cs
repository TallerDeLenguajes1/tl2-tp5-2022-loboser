using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

#nullable disable

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class CadetePedidosViewModel
    {
        public int Id {get; set;}
        public string Nombre {get; set;}
        public List<PedidoViewModel> PedidosDelCadete {get; set;}
        public List<PedidoViewModel> PedidosSinCadete {get; set;}
    }
}