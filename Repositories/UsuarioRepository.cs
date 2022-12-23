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

        public List<Usuario> GetUsuarios(){
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Usuario;";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            while (Lector.Read())
                            {
                                var usuario = new Usuario();
                                
                                usuario.Id = Convert.ToInt32(Lector["idUsuario"].ToString());
                                usuario.IdCliente= Convert.ToInt32(Lector["idCliente"].ToString());
                                usuario.IdCadete = Convert.ToInt32(Lector["idCadete"].ToString());
                                usuario.Nombre = Lector["nombreUsuario"].ToString();
                                usuario.User = Lector["usuarioUsuario"].ToString();
                                usuario.Pass = Lector["passwordUsuario"].ToString();
                                usuario.Rol = Lector["rolUsuario"].ToString();

                                usuarios.Add(usuario);
                            }
                            Conexion.Close();
                            _logger.LogTrace("Obtención de Lista de Usuarios exitosa!");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER Lista de Usuarios ({error})", ex.Message);
            }
            return usuarios; 
        }
        public Usuario GetUsuario(Usuario Logeo){
            Usuario usuario = new Usuario();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Usuario WHERE usuarioUsuario='" + Logeo.User + "' AND passwordUsuario='" + Logeo.Pass + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {
                                usuario.Id = Convert.ToInt32(Lector["idUsuario"].ToString());
                                usuario.IdCliente= Convert.ToInt32(Lector["idCliente"].ToString());
                                usuario.IdCadete = Convert.ToInt32(Lector["idCadete"].ToString());
                                usuario.Nombre = Lector["nombreUsuario"].ToString();
                                usuario.User = Lector["usuarioUsuario"].ToString();
                                usuario.Pass = Lector["passwordUsuario"].ToString();
                                usuario.Rol = Lector["rolUsuario"].ToString();

                                _logger.LogTrace("Obtencion Exitosa del {rol} {Nombre}",usuario.Rol, usuario.Nombre);
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

        public Usuario GetUsuarioByUser(string User)
        {
            var usuario = new Usuario();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Usuario WHERE usuarioUsuario = '" + User + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {

                                usuario.Id = Convert.ToInt32(Lector["idUsuario"].ToString());
                                usuario.IdCliente= Convert.ToInt32(Lector["idCliente"].ToString());
                                usuario.IdCadete = Convert.ToInt32(Lector["idCadete"].ToString());
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
                _logger.LogDebug("Error intentando OBTENER a el Usuario {Nombre} ({error})", User, ex.Message);
            }
            return usuario;
        }
        public Usuario GetUsuarioById(int id)
        {
            var usuario = new Usuario();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Usuario WHERE idUsuario = '" + id + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {
                                usuario.Id = Convert.ToInt32(Lector["idUsuario"].ToString());
                                usuario.IdCliente= Convert.ToInt32(Lector["idCliente"].ToString());
                                usuario.IdCadete = Convert.ToInt32(Lector["idCadete"].ToString());
                                usuario.Nombre = Lector["nombreUsuario"].ToString();
                                usuario.User = Lector["usuarioUsuario"].ToString();
                                usuario.Pass = Lector["passwordUsuario"].ToString();
                                usuario.Rol = Lector["rolUsuario"].ToString();

                                _logger.LogTrace("Obtencion del Usuario del Cliente de Id = {id} exitosa!", id);
                            }
                            Conexion.Close();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER a el Usuario de Id = {id} ({error})", id, ex.Message);
            }
            return usuario;
        }
        public Usuario GetUsuarioByClienteId(int idCliente)
        {
            var usuario = new Usuario();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Usuario WHERE idCliente = '" + idCliente + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {
                                usuario.Id = Convert.ToInt32(Lector["idUsuario"].ToString());
                                usuario.IdCliente= Convert.ToInt32(Lector["idCliente"].ToString());
                                usuario.IdCadete = Convert.ToInt32(Lector["idCadete"].ToString());
                                usuario.Nombre = Lector["nombreUsuario"].ToString();
                                usuario.User = Lector["usuarioUsuario"].ToString();
                                usuario.Pass = Lector["passwordUsuario"].ToString();
                                usuario.Rol = Lector["rolUsuario"].ToString();

                                _logger.LogTrace("Obtencion del Usuario del Cliente de Id = {id} exitosa!", idCliente);
                            }
                            Conexion.Close();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER a el Usuario del Cliente de Id = {id} ({error})", idCliente, ex.Message);
            }
            return usuario;
        }

        public Usuario GetUsuarioByCadeteId(int idCadete)
        {
            var usuario = new Usuario();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Usuario WHERE idCadete = '" + idCadete + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {
                                usuario.Id = Convert.ToInt32(Lector["idUsuario"].ToString());
                                usuario.IdCliente= Convert.ToInt32(Lector["idCliente"].ToString());
                                usuario.IdCadete = Convert.ToInt32(Lector["idCadete"].ToString());
                                usuario.Nombre = Lector["nombreUsuario"].ToString();
                                usuario.User = Lector["usuarioUsuario"].ToString();
                                usuario.Pass = Lector["passwordUsuario"].ToString();
                                usuario.Rol = Lector["rolUsuario"].ToString();

                                _logger.LogTrace("Obtencion del Usuario del Cadete de Id = {id} exitosa!", idCadete);
                            }
                            Conexion.Close();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER a el Usuario del Cadete de Id = {id} ({error})", idCadete, ex.Message);
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
                        Comando.CommandText = "INSERT INTO Usuario(nombreUsuario, usuarioUsuario, passwordUsuario, rolUsuario, idCliente, idCadete) VALUES('" + Usuario.Nombre + "', '" + Usuario.User + "', '" + Usuario.Pass + "', '" + Usuario.Rol + "', '" + Usuario.IdCliente + "', '" + Usuario.IdCadete + "');";
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
                        Comando.CommandText = "UPDATE Usuario SET nombreUsuario='" + Usuario.Nombre + "', usuarioUsuario='" + Usuario.User + "', passwordUsuario='" + Usuario.Pass + "', rolUsuario='" + Usuario.Rol + "', idCliente='" + Usuario.IdCliente + "', idCadete='" + Usuario.IdCadete + "' WHERE idUsuario='" + Usuario.Id + "';";
                        Comando.ExecuteNonQuery();

                        if (Usuario.IdCadete != 0)
                        {
                            Comando.CommandText = "UPDATE Cadete SET nombreCadete='" + Usuario.Nombre + "' WHERE idCadete='" + Usuario.IdCadete + "';";
                            Comando.ExecuteNonQuery();
                        }
                        if (Usuario.IdCliente != 0)
                        {
                            Comando.CommandText = "UPDATE Cliente SET nombreCliente='" + Usuario.Nombre + "' WHERE idCliente='" + Usuario.IdCliente + "';";
                            Comando.ExecuteNonQuery();
                        }
                        
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
                        Comando.CommandText = "DELETE FROM Usuario WHERE idUsuario='" + Usuario.Id + "';";
                        Comando.ExecuteNonQuery();

                        if (Usuario.IdCadete != 0)
                        {
                            Comando.CommandText = "DELETE FROM Cadete WHERE idCadete='" + Usuario.IdCadete + "';";
                            Comando.ExecuteNonQuery();
                        }

                        if (Usuario.IdCliente != 0)
                        {
                            Comando.CommandText = "DELETE FROM Cliente WHERE idCliente='" + Usuario.IdCliente + "';";
                            Comando.ExecuteNonQuery();
                        }

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