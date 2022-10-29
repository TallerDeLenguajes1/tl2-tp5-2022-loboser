using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tl2_tp4_2022_loboser.Models
{
    public class Cadete : Persona {
        private List<Pedido> pedidos = new List<Pedido>();

        public List<Pedido> Pedidos { get => pedidos; set => pedidos = value; }

        int JornalACobrar(){
            return Pedidos.Where(t => t.Estado == "Entregado").Count()*300;
        }
    }
}