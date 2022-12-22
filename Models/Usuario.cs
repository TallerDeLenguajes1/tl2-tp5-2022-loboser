using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace tl2_tp4_2022_loboser.Models
{
    public class Usuario
    {
        private int id;
        private int idCliente;
        private int idCadete;
        private string nombre;
        private string user;
        private string pass;
        private string rol;
        
        public int Id{ get => id; set => id = value; }
        public int IdCliente { get => idCliente; set => idCliente = value; }
        public int IdCadete { get => idCadete; set => idCadete = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string User { get => user; set => user = value; }
        public string Pass { get => pass; set => pass = value; }
        public string Rol { get => rol; set => rol = value; }

        public Usuario(){
        }
        public Usuario(Cadete cadete)
        {
            this.idCadete = cadete.Id;
            this.idCliente = 0;
            this.nombre = cadete.Nombre;
            this.user = cadete.Nombre.ToLower().Replace(" ", string.Empty);
            this.pass = cadete.Nombre.ToLower().Replace(" ", string.Empty);
            this.rol = "Cadete";
        }

        public Usuario(Cliente cliente)
        {
            this.idCliente = cliente.Id;
            this.idCadete = 0;
            this.nombre = cliente.Nombre;
            this.user = cliente.Nombre.ToLower().Replace(" ", string.Empty);
            this.pass = cliente.Nombre.ToLower().Replace(" ", string.Empty);
            this.rol = "Cliente";
        }
        public Usuario(int id,int idCadete,int idCliente, string nombre, string user, string pass, string rol)
        {
            this.id = id;
            this.idCadete = idCadete;
            this.idCliente = idCliente;
            this.nombre = nombre;
            this.user = user;
            this.pass = pass;
            this.rol = rol;
        }
    }
}