using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;
using NLog;

#nullable disable
namespace tl2_tp4_2022_loboser.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly string _cadenaConexion;
        private readonly IClienteRepository _clienteRepository;
        private readonly ILogger<PedidoRepository> _logger;

        public PedidoRepository(IConexionRepository conexion, IClienteRepository clienteRepository, ILogger<PedidoRepository> logger)
        {
            this._cadenaConexion = conexion.GetConnectionString();
            this._clienteRepository = clienteRepository;
            this._logger = logger;
        }

        public List<Pedido> GetPedidos(){
            List<Pedido> Pedidos = new List<Pedido>();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Pedido;";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            while (Lector.Read())
                            {
                                Pedido Pedido = new Pedido();

                                Pedido.Nro = Convert.ToInt32(Lector["nroPedido"].ToString());
                                Pedido.Obs = Lector["Obs"].ToString();
                                Pedido.Estado = Lector["Estado"].ToString();
                                Pedido.IdCadeteAsignado = Convert.ToInt32(Lector["idCadeteAsignado"].ToString());

                                Pedido.Cliente = _clienteRepository.GetClienteById(Convert.ToInt32(Lector["idCliente"].ToString()));

                                Pedidos.Add(Pedido);
                            }
                            _logger.LogTrace("Obtención de Lista de Pedidos exitosa!");
                            Conexion.Close();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER Lista de Pedidos ({error})", ex.Message);
            }
            return Pedidos;
        }

        public List<Pedido> GetPedidosByCadete(int idCadete){
            List<Pedido> Pedidos = new List<Pedido>();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Pedido WHERE idCadeteAsignado='" + idCadete + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        while (Lector.Read())
                        {
                            Pedido Pedido = new Pedido();

                            Pedido.Nro = Convert.ToInt32(Lector["nroPedido"].ToString());
                            Pedido.Obs = Lector["Obs"].ToString();
                            Pedido.Estado = Lector["Estado"].ToString();
                            Pedido.IdCadeteAsignado = Convert.ToInt32(Lector["idCadeteAsignado"].ToString());

                            Pedido.Cliente = _clienteRepository.GetClienteById(Convert.ToInt32(Lector["idCliente"].ToString()));

                            Pedidos.Add(Pedido);

                        }

                        Conexion.Close();
                        _logger.LogTrace("Obtención de Lista de Pedidos del Cadete de id = {id} exitosa!", idCadete);
                    }
                }
            }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER Lista de Pedidos del Cadete de Id = {id} ({error})", idCadete, ex.Message);
            }
            return Pedidos;
        }

        public List<Pedido> GetPedidosByCliente(int idCliente){
            List<Pedido> Pedidos = new List<Pedido>();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Pedido WHERE idCliente='" + idCliente + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            while (Lector.Read())
                            {
                                Pedido Pedido = new Pedido();

                                Pedido.Nro = Convert.ToInt32(Lector["nroPedido"].ToString());
                                Pedido.Obs = Lector["Obs"].ToString();
                                Pedido.Estado = Lector["Estado"].ToString();
                                Pedido.IdCadeteAsignado = Convert.ToInt32(Lector["idCadeteAsignado"].ToString());

                                Pedido.Cliente = _clienteRepository.GetClienteById(Convert.ToInt32(Lector["idCliente"].ToString()));

                                Pedidos.Add(Pedido);
                            }

                            Conexion.Close();
                            _logger.LogTrace("Obtención de Lista de Pedidos del Cliente de id = {id} exitosa!", idCliente);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER Lista de Pedidos del Cliente de Id = {id} ({error})", idCliente, ex.Message);
            }
            return Pedidos;
        }

        public Pedido GetPedidoByNro(int nro){
            Pedido Pedido = new Pedido();
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "SELECT * FROM Pedido WHERE nroPedido='" + nro + "';";
                        using (SqliteDataReader Lector = Comando.ExecuteReader())
                        {
                            if (Lector.Read())
                            {

                                Pedido.Nro = Convert.ToInt32(Lector["nroPedido"].ToString());
                                Pedido.Obs = Lector["Obs"].ToString();
                                Pedido.Estado = Lector["Estado"].ToString();
                                Pedido.IdCadeteAsignado = Convert.ToInt32(Lector["idCadeteAsignado"].ToString());
                                
                                Pedido.Cliente = _clienteRepository.GetClienteById(Convert.ToInt32(Lector["idCliente"].ToString()));
                            }
                            Conexion.Close();
                            _logger.LogTrace("Obtención del Pedido de Nro = {nro} exitosa!", nro);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando OBTENER el Pedido de NRO = {nro} ({error})", Pedido.Nro, ex.Message);
            }
            return Pedido;
        }
        public void AltaPedido(Pedido Pedido){
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        
                        Comando.CommandText = "INSERT INTO Pedido(Obs, Estado, idCliente, idCadeteAsignado) VALUES('" + Pedido.Obs + "', '" +  Pedido.Estado + "', '0' , '0');";
                        Comando.ExecuteNonQuery();
                    }
                    Conexion.Close();
                    _logger.LogTrace("Alta de Pedido {obs} - {estado} de exitosa!", Pedido.Obs, Pedido.Estado);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando SUBIR el Pedido con los datos {Obs} - {Estado} ({error})", Pedido.Obs, Pedido.Estado, ex.Message);
            }
        }

        public void BajaPedido(int nro)
        {
            try
            {
                using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {
                        Comando.CommandText = "DELETE FROM Pedido WHERE nroPedido='" + nro + "';";
                        Comando.ExecuteNonQuery();

                        Conexion.Close();
                        _logger.LogTrace("Baja del Pedido de Nro = {nro} exitoso!", nro);
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando dar de BAJA a el Pedido de NRO = {Nro} ({error})", nro, ex.Message);
            }
        }

        public void EditarPedido(Pedido Pedido)
        {
            try
            {
                using (SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
                {
                    Conexion.Open();
                    using (SqliteCommand Comando = Conexion.CreateCommand())
                    {     
                        Comando.CommandText = "UPDATE Pedido SET Obs='" + Pedido.Obs + "', idCliente ='" + Pedido.Cliente.Id + "', Estado='" + Pedido.Estado + "', idCadeteAsignado='" + Pedido.IdCadeteAsignado + "' WHERE nroPedido='" + Pedido.Nro + "';";
                        Comando.ExecuteNonQuery();
                    }
                    Conexion.Close();
                    _logger.LogTrace("Edicion del Pedido de Nro = {nro} exitoso!", Pedido.Nro);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("Error intentando EDITAR a el Pedido de NRO = {Nro}, con los datos {Obs} - {Estado} - idCliente: {idCliente} - idCadete {idCadete} ({error})", Pedido.Nro, Pedido.Obs, Pedido.Estado, Pedido.Cliente.Id, Pedido.IdCadeteAsignado, ex.Message);
            }
        }
    }
}