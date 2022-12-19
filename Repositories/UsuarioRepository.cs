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
        public Usuario GetUsuario(Usuario Logeo){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Usuario WHERE usuarioUsuario='" + Logeo.Nombre + "' AND passwordUsuario='" + Logeo.Pass + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        Usuario usuario = new Usuario();
                        if (Lector.Read())
                        {
                            usuario.IdUsuario = Convert.ToInt32(Lector["idUsuario"].ToString());
                            usuario.Nombre = Lector["nombreUsuario"].ToString();
                            usuario.User = Lector["usuarioUsuario"].ToString();
                            usuario.Pass = Lector["passwordUsuario"].ToString();
                            usuario.Rol = Lector["rolUsuario"].ToString();
                        }
                        Conexion.Close();
                        return usuario;
                    }
                }
            }
        }

        public Usuario GetUsuarioLikeUser(string User)
        {
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Usuario WHERE usuarioUsuario LIKE '" + User + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if (Lector.Read())
                        {
                            var usuario = new Usuario();

                            usuario.IdUsuario = Convert.ToInt32(Lector["idUsuario"].ToString());
                            usuario.Nombre = Lector["nombreUsuario"].ToString();
                            usuario.User = Lector["usuarioUsuario"].ToString();
                            usuario.Pass = Lector["passwordUsuario"].ToString();
                            usuario.Rol = Lector["rolUsuario"].ToString();
                            
                            Conexion.Close();
                            return usuario;
                        }
                        return null;
                    }
                }
            }
        }

        public void AltaUsuario(Usuario Usuario)
        {
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "INSERT INTO Usuario(nombreUsuario, usuarioUsuario, passwordUsuario, rolUsuario) VALUES('" + Usuario.Nombre + "', '" + Usuario.User + "', '" + Usuario.Pass + "', '" + Usuario.Rol + "');";
                    Comando.ExecuteNonQuery();
                    Conexion.Close();
                }
            }
        }

        public void BajaUsuario(Usuario Usuario)
        {
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "DELETE FROM Usuario WHERE usuarioUsuario='" + Usuario.User + "';";
                    Comando.ExecuteNonQuery();
                    Conexion.Close();
                }
            }
        }
    }
}