using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;
using NLog;

namespace tl2_tp4_2022_loboser.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _cadenaConexion;
        private readonly ILogger<UsuarioRepository> _logger;

        public UsuarioRepository(IConexionRepository conexion, ILogger<UsuarioRepository> logger)
        {
            this._cadenaConexion = conexion.GetConnectionString();
            this._logger = logger;
        }
        public Usuario GetUsuario(Usuario Logeo){
            Usuario usuario = new Usuario();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Usuario WHERE usuarioUsuario='" + Logeo.Nombre + "' AND passwordUsuario='" + Logeo.Pass + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {
                                usuario.IdUsuario = Convert.ToInt32(Lector["idUsuario"].ToString());
                                usuario.Nombre = Lector["nombreUsuario"].ToString();
                                usuario.User = Lector["usuarioUsuario"].ToString();
                                usuario.Pass = Lector["passwordUsuario"].ToString();
                                usuario.Rol = Lector["rolUsuario"].ToString();
                                _logger.LogTrace("Logeo Exitoso del {rol} {Nombre}",usuario.Rol, usuario.Nombre);
                            }
                            Conexion.Close();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER a el Usuario {Nombre} ({error})", Logeo.Nombre, ex.Message);
            }
            return usuario;
        }

        public Usuario GetUsuarioLikeUser(string User)
        {
            var usuario = new Usuario();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Usuario WHERE usuarioUsuario LIKE '" + User + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {

                                usuario.IdUsuario = Convert.ToInt32(Lector["idUsuario"].ToString());
                                usuario.Nombre = Lector["nombreUsuario"].ToString();
                                usuario.User = Lector["usuarioUsuario"].ToString();
                                usuario.Pass = Lector["passwordUsuario"].ToString();
                                usuario.Rol = Lector["rolUsuario"].ToString();
                                _logger.LogTrace("Obtencion de Usuario {Nombre} exitosa!", User);
                            }
                            Conexion.Close();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER a el Usuario LIKE {Nombre} ({error})", User, ex.Message);
            }
            return usuario;
        }

        public void AltaUsuario(Usuario Usuario)
        {
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "INSERT INTO Usuario(nombreUsuario, usuarioUsuario, passwordUsuario, rolUsuario) VALUES('" + Usuario.Nombre + "', '" + Usuario.User + "', '" + Usuario.Pass + "', '" + Usuario.Rol + "');";
                        Comando.ExecuteNonQuery();
                        Conexion.Close();
                        _logger.LogTrace("Subida de Usuario {User} exitosa!", Usuario.User);
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando SUBIR a el Usuario {Nombre} - {Usuario} - {Rol} ({error})", Usuario.Nombre, Usuario.User, Usuario.Rol, ex.Message);
            }
        }
        public void EditarUsuario(Usuario Usuario)
        {
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "UPDATE Usuario SET nombreUsuario='" + Usuario.Nombre + "', usuarioUsuario='" + Usuario.User + "', passwordUsuario='" + Usuario.Pass + "', rolUsuario='" + Usuario.Rol + "' WHERE idUsuario='" + Usuario.IdUsuario + "';";
                        Comando.ExecuteNonQuery();
                        Conexion.Close();
                        _logger.LogTrace("Subida de Usuario {User} exitosa!", Usuario.User);
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando SUBIR a el Usuario {Nombre} - {Usuario} - {Rol} ({error})", Usuario.Nombre, Usuario.User, Usuario.Rol, ex.Message);
            }
        }

        public void BajaUsuario(Usuario Usuario)
        {
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "DELETE FROM Usuario WHERE usuarioUsuario='" + Usuario.User + "';";
                        Comando.ExecuteNonQuery();
                        Conexion.Close();
                        _logger.LogTrace("Baja de Usuario {User} exitosa!", Usuario.User);
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando dar de BAJA a el Usuario {Usuario} ({error})", Usuario.User, ex.Message);
            }
        }
    }
}