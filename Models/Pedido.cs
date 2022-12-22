using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp4_2022_loboser.Models
{
    public class Pedido
    {
        private int nro;
        private string obs;
        private Cliente cliente;
        private string estado;
        private int idCadete;
        
        public int Nro { get => nro; set => nro = value; }
        public string Obs { get => obs; set => obs = value; }
        public Cliente Cliente { get => cliente; set => cliente = value; }
        public string Estado { get => estado; set => estado = value; }
        public int IdCadete { get => idCadete; set => idCadete= value; }

        public Pedido(){
        }

        public Pedido(int nro, string obs, Cliente cliente){
                Nro = nro;
                Obs = obs;
                Cliente = cliente;
                Estado = "En proceso";
                IdCadete = 0;
        }
    }
}