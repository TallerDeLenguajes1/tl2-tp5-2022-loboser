using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;


namespace tl2_tp4_2022_loboser.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly string _cadenaConexion;
        public UsuarioRepository(IConexionRepository conexion)
        {
            this._cadenaConexion = conexion.GetConnectionString();
        }
        public Usuario Logear(LogeoViewModel Logeo){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Usuario WHERE usuarioUsuario='" + Logeo.nombre + "' AND passwordUsuario='" + Logeo.pass + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if (Lector.Read())
                        {
                            Usuario usuario = new Usuario();
                            usuario.IdUsuario = Convert.ToInt32(Lector["idUsuario"].ToString());
                            usuario.Nombre = Lector["nombreUsuario"].ToString();
                            usuario.User = Lector["usuarioUsuario"].ToString();
                            usuario.Pass = Lector["passwordUsuario"].ToString();
                            usuario.Rol = Lector["rolUsuario"].ToString();
                            Conexion.Close();
                            return usuario;
                        }else
                        {
                            Conexion.Close();
                            return null;
                        }
                    }
                }
            }
        }
    }
}