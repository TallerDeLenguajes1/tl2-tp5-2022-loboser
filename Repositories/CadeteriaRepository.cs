using Microsoft.Data.Sqlite;
using NLog;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{

    public class CadeteriaRepository : ICadeteriaRepository
    {
        private readonly string _cadenaConexion;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<CadeteriaRepository> _logger;

        public CadeteriaRepository(IConexionRepository conexion, IPedidoRepository pedidoRepository, IUsuarioRepository usuarioRepository, ILogger<CadeteriaRepository> logger)
        {
            this._cadenaConexion = conexion.GetConnectionString();
            this._pedidoRepository = pedidoRepository;
            this._usuarioRepository = usuarioRepository;
            this._logger = logger;
        }

        public List<Cadete> GetCadetes(){
            List<Cadete> Cadetes = new List<Cadete>();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Cadete;";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            while (Lector.Read())
                            {
                                Cadete Cadete = new Cadete();

                                Cadete.Id = Convert.ToInt32(Lector["idCadete"].ToString());
                                Cadete.Nombre = Lector["nombreCadete"].ToString();
                                Cadete.Direccion = Lector["direccionCadete"].ToString();
                                Cadete.Telefono = Lector["telefonoCadete"].ToString();
                                Cadete.Pedidos = _pedidoRepository.GetPedidosByCadete(Convert.ToInt32(Lector["idCadete"].ToString()));

                                Cadetes.Add(Cadete);
                            }
                            Conexion.Close();
                            _logger.LogTrace("Obtención de Lista de Cadete exitosa!");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER Lista de Cadetes ({error})", ex.Message);
            }
            return Cadetes; 
        }

        public Cadete GetCadeteById(int id){
            Cadete Cadete = new Cadete();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Cadete WHERE idCadete='" + id + "'";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {
                                Cadete.Id = Convert.ToInt32(Lector["idCadete"].ToString());
                                Cadete.Nombre = Lector["nombreCadete"].ToString();
                                Cadete.Direccion = Lector["direccionCadete"].ToString();
                                Cadete.Telefono = Lector["telefonoCadete"].ToString();
                                Cadete.Pedidos = _pedidoRepository.GetPedidosByCadete(Convert.ToInt32(Lector["idCadete"].ToString()));
                                _logger.LogTrace("Obtención de Cadete de id = {id} exitosa!", id);
                            }
                            Conexion.Close();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER a el Cadete de Id = {id} ({error})", id, ex.Message);
            }
            return Cadete;
        }

        public Cadete GetCadeteByTelefono(string telefono){
            Cadete Cadete = new Cadete();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Cadete WHERE telefonoCadete='" + telefono + "'";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {
                                Cadete.Id = Convert.ToInt32(Lector["idCadete"].ToString());
                                Cadete.Nombre = Lector["nombreCadete"].ToString();
                                Cadete.Direccion = Lector["direccionCadete"].ToString();
                                Cadete.Telefono = Lector["telefonoCadete"].ToString();
                                Cadete.Pedidos = _pedidoRepository.GetPedidosByCadete(Convert.ToInt32(Lector["idCadete"].ToString()));
                                _logger.LogTrace("Obtención de Cadete de telefono = {telefono} exitosa!", telefono);
                            }
                            Conexion.Close();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER a el Cadete de Id = {telefono} ({error})", telefono, ex.Message);
            }
            return Cadete;
        }

        public void AltaCadete(Cadete Cadete)
        {
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "INSERT INTO Cadete(nombreCadete, direccionCadete, telefonoCadete) VALUES ('" + Cadete.Nombre + "', '" + Cadete.Direccion + "' ,'" + Cadete.Telefono + "');";
                        Comando.ExecuteNonQuery();
                        _logger.LogTrace("Alta de Cadete {nombre} exitosa!", Cadete.Nombre);
                        Conexion.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando SUBIR el Cadete {nombre} - {direccion} - {telefono} ({error})", Cadete.Nombre, Cadete.Direccion, Cadete.Telefono, ex.Message);
            }
        }

        public void BajaCadete(int id)
        {
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "UPDATE Pedido SET idCadeteAsignado='0' WHERE idCadeteAsignado='" + id + "';";
                        Comando.ExecuteNonQuery();

                        Comando.CommandText = "DELETE FROM Cadete WHERE idCadete='" + id + "';";
                        Comando.ExecuteNonQuery();

                        var usuario = _usuarioRepository.GetUsuarioByCadeteId(id);

                        if (usuario.Id != 0)
                        {
                            usuario.IdCadete = 0;
                            _usuarioRepository.BajaUsuario(usuario);
                        }

                        
                        _logger.LogTrace("Baja de Cadete de id = {id} exitosa!", id);

                        Conexion.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando dar de BAJA a el Cadete de Id = {id} ({error})", id, ex.Message);
            }
        }

        public void EditarCadete(Cadete Cadete){
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "UPDATE Usuario SET nombreUsuario='" + Cadete.Nombre + "' WHERE idCadete='" + Cadete.Id + "';";
                        Comando.ExecuteNonQuery();

                        Comando.CommandText = "UPDATE Cadete SET nombreCadete='" + Cadete.Nombre + "', direccionCadete='" + Cadete.Direccion + "', telefonoCadete='" + Cadete.Telefono + "' WHERE idCadete='" + Cadete.Id + "';";
                        Comando.ExecuteNonQuery();

                        _logger.LogTrace("Edición de Cadete {Nombre} exitosa!", Cadete.Nombre);
                        Conexion.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando EDITAR a el Cadete de Id = {id}, con los datos {nombre} - {direccion} - {telefono} ({error})", Cadete.Id, Cadete.Nombre, Cadete.Direccion, Cadete.Telefono, ex.Message);
            }
        }
    }
}