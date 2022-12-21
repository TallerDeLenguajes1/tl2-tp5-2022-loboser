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

        public Cadeteria GetCadeteria(){
            Cadeteria Cadeteria = new Cadeteria();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT nombreCadeteria, telefonoCadeteria FROM Cadeteria WHERE idCadeteria='1';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            while (Lector.Read())
                            {
                                Cadeteria.Nombre = Lector["nombreCadeteria"].ToString();
                                Cadeteria.Telefono = Lector["telefonoCadeteria"].ToString();
                            }
                            Cadeteria.Cadetes = GetCadetes();

                            Conexion.Close();
                        }
                    }
                }
            }catch(System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER Cadeteria ({error})", ex.Message);
            }
            return Cadeteria;
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
                        Comando.CommandText = "SELECT * FROM Cadete INNER JOIN CadeteCadeteria ON Cadete.idCadete = CadeteCadeteria.idCadete WHERE CadeteCadeteria.idCadeteria='1';";
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

                        Comando.CommandText = "SELECT * FROM Cadete WHERE nombreCadete='" + Cadete.Nombre + "' AND telefonoCadete='" + Cadete.Telefono + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            using (SqliteCommand Comando2 = Conexion.CreateCommand())
                            {
                                if(Lector.Read())
                                {
                                    int idCadeteria = 1;
                                    int id = Convert.ToInt32(Lector["idCadete"].ToString());
                                    Comando2.CommandText = "INSERT INTO CadeteCadeteria(idCadeteria, idCadete) VALUES ('" + idCadeteria + "', '" + id + "');";
                                    Comando2.ExecuteNonQuery();   
                                }
                                Conexion.Close();
                            }  
                        }
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
                        _usuarioRepository.BajaUsuario(new Usuario(this.GetCadeteById(id)));

                        Comando.CommandText = "DELETE FROM CadeteCadeteria WHERE idCadete='" + id + "';";
                        Comando.ExecuteNonQuery();

                        Comando.CommandText = "DELETE FROM Cadete WHERE idCadete='" + id + "';";
                        Comando.ExecuteNonQuery();

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
                        Comando.CommandText = "UPDATE Cadete SET nombreCadete='" + Cadete.Nombre + "', direccionCadete='" + Cadete.Direccion + "', telefonoCadete='" + Cadete.Telefono + "' WHERE idCadete='" + Cadete.Id + "';";
                        Comando.ExecuteNonQuery();
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