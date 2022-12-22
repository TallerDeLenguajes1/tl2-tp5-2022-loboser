using tl2_tp4_2022_loboser.Models;
using Microsoft.Data.Sqlite;
using NLog;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _cadenaConexion;
        private readonly ILogger<ClienteRepository> _logger;
        private readonly IUsuarioRepository _usuarioRepository;

        public ClienteRepository(IConexionRepository conexion, ILogger<ClienteRepository> logger, IUsuarioRepository usuarioRepository)
        {
            this._cadenaConexion = conexion.GetConnectionString();
            this._logger = logger;
            this._usuarioRepository = usuarioRepository;
        }

        public List<Cliente> GetClientes(){
            List<Cliente> Clientes = new List<Cliente>();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Cliente;";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            while(Lector.Read())
                            {
                                var Cliente = new Cliente();

                                Cliente.Id = Convert.ToInt32(Lector["idCliente"].ToString());
                                Cliente.Nombre = Lector["nombreCliente"].ToString();
                                Cliente.Direccion = Lector["direccionCliente"].ToString();
                                Cliente.Telefono = Lector["telefonoCliente"].ToString();
                                Cliente.DatosReferenciaDireccion = Lector["datosReferenciaDireccion"].ToString();

                                Clientes.Add(Cliente);
                            }
                            Conexion.Close();
                            _logger.LogTrace("Obtención de Lista de Clientes exitosa!");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER Lista de Clientes ({error})", ex.Message);
            }
            return Clientes;
        }

        public Cliente GetClienteById(int id)
        {
            Cliente Cliente = new Cliente();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Cliente WHERE idCliente='" + id + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {
                                Cliente.Id = Convert.ToInt32(Lector["idCliente"].ToString());
                                Cliente.Nombre = Lector["nombreCliente"].ToString();
                                Cliente.Direccion = Lector["direccionCliente"].ToString();
                                Cliente.Telefono = Lector["telefonoCliente"].ToString();
                                Cliente.DatosReferenciaDireccion = Lector["datosReferenciaDireccion"].ToString();
                            }

                            Conexion.Close();
                        }
                    }
                }   
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER Cliente de Id = {id} ({error})", id, ex.Message);
            }
            return Cliente;
        }
        public Cliente GetClienteByTelefono(string telefono)
        {
            Cliente Cliente = new Cliente();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Cliente WHERE telefonoCliente='" + telefono + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {
                                Cliente.Id = Convert.ToInt32(Lector["idCliente"].ToString());
                                Cliente.Nombre = Lector["nombreCliente"].ToString();
                                Cliente.Direccion = Lector["direccionCliente"].ToString();
                                Cliente.Telefono = Lector["telefonoCliente"].ToString();
                                Cliente.DatosReferenciaDireccion = Lector["datosReferenciaDireccion"].ToString();
                            }

                            Conexion.Close();
                        }
                    }
                }  
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER Cliente de Telefono = {telefono} ({error})", Cliente.Telefono, ex.Message);
            }
            return Cliente;
        }

        public void AltaCliente(Cliente cliente)
        {
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "INSERT INTO Cliente(nombreCliente, direccionCliente, telefonoCliente, datosReferenciaDireccion) VALUES('" + cliente.Nombre + "', '" + cliente.Direccion + "', '" + cliente.Telefono + "', '" + cliente.DatosReferenciaDireccion + "');";
                        Comando.ExecuteNonQuery();
                    }
                    Conexion.Close();
                }
            }
            catch (System.Exception ex)
            {    
                _logger.LogDebug("Error intentando SUBIR Cliente de con los datos {nombre} - {direccion} - {telefono} - {datosReferencia} ({error})", cliente.Nombre, cliente.Direccion, cliente.Telefono, cliente.DatosReferenciaDireccion, ex.Message);
            }
        }

        public void EditarCliente(Cliente cliente)
        {
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "UPDATE Usuario SET nombreUsuario='" + cliente.Nombre + "' WHERE idCliente='" + cliente.Id + "';";
                        Comando.ExecuteNonQuery();

                        Comando.CommandText = "UPDATE Cliente SET direccionCliente='" + cliente.Direccion + "', datosReferenciaDireccion='" + cliente.DatosReferenciaDireccion + "', nombreCliente='" + cliente.Nombre + "', telefonoCliente='" + cliente.Telefono + "' WHERE idCliente='" + cliente.Id + "';";
                        Comando.ExecuteNonQuery();
                    }
                    _logger.LogTrace("Edición de Cliente {Nombre} exitosa!", cliente.Nombre);
                    Conexion.Close();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando EDITAR al Cliente de Id = {id}, con los datos {nombre} - {direccion} - {telefono} - {datosReferencia} ({error})",cliente.Id, cliente.Nombre, cliente.Direccion, cliente.Telefono, cliente.DatosReferenciaDireccion, ex.Message);
            }
        }

        public void BajaCliente(int id)
        {
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "DELETE FROM Pedido WHERE idCliente='" + id + "';";
                        Comando.ExecuteNonQuery();
                        Comando.CommandText = "DELETE FROM Cliente WHERE idCliente='" + id + "';";
                        Comando.ExecuteNonQuery();  

                        var usuario = _usuarioRepository.GetUsuarioByClienteId(id);
                        if (usuario.Id != 0)
                        {
                            usuario.IdCliente = 0;
                            _usuarioRepository.BajaUsuario(usuario);
                        }
                        
                    }
                    Conexion.Close();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando de dar de BAJA al Cliente de Id = {id} ({error})", id, ex.Message);
            }
        }
    }
}