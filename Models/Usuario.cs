using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace tl2_tp4_2022_loboser.Models
{
    public class Usuario
    {
        private int idUsuario;
        private string nombre;
        private string user;
        private string pass;
        private string rol;
        
        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string User { get => user; set => user = value; }
        public string Pass { get => pass; set => pass = value; }
        public string Rol { get => rol; set => rol = value; }

        public Usuario(){
        }
        public Usuario(Cadete cadete)
        {
            this.nombre = cadete.Nombre;
            this.user = cadete.Nombre.ToLower().Replace(" ", string.Empty);
            this.pass = cadete.Nombre.ToLower().Replace(" ", string.Empty);
            this.rol = "Cadete";
        }
        public Usuario(int idUsuario, string nombre, string user, string pass, string rol)
        {
            this.idUsuario = idUsuario;
            this.nombre = nombre;
            this.user = user;
            this.pass = pass;
            this.rol = rol;
        }
    }
}