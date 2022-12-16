using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly string _cadenaConexion;
        private readonly IClienteRepository _clienteRepository;

        public PedidoRepository(IConexionRepository conexion, IClienteRepository clienteRepository)
        {
            this._cadenaConexion = conexion.GetConnectionString();
            this._clienteRepository = clienteRepository;
        }

        public List<Pedido> GetPedidos(){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Pedido;";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        List<Pedido> Pedidos = new List<Pedido>();
                        while (Lector.Read())
                        {
                            Pedido Pedido = new Pedido();

                            Pedido.Nro = Convert.ToInt32(Lector["nroPedido"].ToString());
                            Pedido.Obs = Lector["Obs"].ToString();
                            Pedido.Estado = Lector["Estado"].ToString();
                            Pedido.IdCadeteAsignado = Convert.ToInt32(Lector["idCadeteAsignado"].ToString());

                            Pedido.Cliente = _clienteRepository.GetCliente(Convert.ToInt32(Lector["idCliente"].ToString()));

                            Pedidos.Add(Pedido);
                        }

                        Conexion.Close();
                        return Pedidos;
                    }
                }
            }
        }
        public List<Pedido> GetPedidosByCadete(int idCadete){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Pedido WHERE idCadeteAsignado='" + idCadete + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        List<Pedido> Pedidos = new List<Pedido>();
                        while (Lector.Read())
                        {
                            Pedido Pedido = new Pedido();

                            Pedido.Nro = Convert.ToInt32(Lector["nroPedido"].ToString());
                            Pedido.Obs = Lector["Obs"].ToString();
                            Pedido.Estado = Lector["Estado"].ToString();
                            Pedido.IdCadeteAsignado = Convert.ToInt32(Lector["idCadeteAsignado"].ToString());

                            Pedido.Cliente = _clienteRepository.GetCliente(Convert.ToInt32(Lector["idCliente"].ToString()));

                            Pedidos.Add(Pedido);
                        }

                        Conexion.Close();
                        return Pedidos;
                    }
                }
            }
        }

        public Pedido GetPedidoByNro(int nro){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Pedido WHERE nroPedido='" + nro + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if (Lector.Read())
                        {
                            Pedido Pedido = new Pedido();

                            Pedido.Nro = Convert.ToInt32(Lector["nroPedido"].ToString());
                            Pedido.Obs = Lector["Obs"].ToString();
                            Pedido.Estado = Lector["Estado"].ToString();
                            Pedido.IdCadeteAsignado = Convert.ToInt32(Lector["idCadeteAsignado"].ToString());
                            
                            Pedido.Cliente = _clienteRepository.GetCliente(Convert.ToInt32(Lector["idCliente"].ToString()));

                            Conexion.Close();
                            return Pedido;
                        }

                        return null;
                    }
                }
            }
        }
        public void AltaPedido(Pedido Pedido){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
            {
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT idCliente FROM Cliente WHERE telefonoCliente='" + Pedido.Cliente.Telefono + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if (Lector.Read() == false)
                        {
                            Lector.Close();
                            _clienteRepository.AltaCliente(Pedido.Cliente);
                        }
                    }
                }
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT idCliente FROM Cliente WHERE telefonoCliente='" + Pedido.Cliente.Telefono + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if (Lector.Read())
                        {
                            using (SqliteCommand Comando2 = Conexion.CreateCommand())
                            {
                                Comando2.CommandText = "INSERT INTO Pedido(Obs, Estado, idCliente, idCadeteAsignado) VALUES('" + Pedido.Obs + "', 'En Proceso', '" + Convert.ToInt32(Lector["idCliente"].ToString()) + "' , '0');";
                                Comando2.ExecuteNonQuery();
                            }
                        }
                    }  
                }
                Conexion.Close();
            }
        }

        public void BajaPedido(int nro)
        {
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "DELETE FROM Pedido WHERE nroPedido='" + nro + "';";
                    Comando.ExecuteNonQuery();

                    Conexion.Close();
                }
            }
        }

        public void EditarPedido(Pedido Pedido)
        {
            using (SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
            {
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Cliente WHERE telefonoCliente='" + Pedido.Cliente.Telefono + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if (Lector.Read() == false)
                        {
                            Lector.Close();
                            _clienteRepository.AltaCliente(Pedido.Cliente);
                        }else
                        {
                            Lector.Close();
                            _clienteRepository.EditarCliente(Pedido.Cliente);
                        }
                    }
                }
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {     
                    Comando.CommandText = "SELECT idCliente FROM Cliente WHERE telefonoCliente='" + Pedido.Cliente.Telefono + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if (Lector.Read())
                        {
                            using (SqliteCommand Comando2 = Conexion.CreateCommand())
                            {
                                Comando2.CommandText = "UPDATE Pedido SET Obs='" + Pedido.Obs + "', idCliente ='" + Convert.ToInt32(Lector["idCliente"].ToString()) + "' WHERE nroPedido='" + Pedido.Nro + "';";
                                Comando2.ExecuteNonQuery();
                            }
                        }
                    }
                    Conexion.Close();
                }
            }
        }
        public void CambiarEstado(int nro)
        {
            using (SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
            {
                Conexion.Open();
                using(SqliteCommand Comando = Conexion.CreateCommand()){
                    Comando.CommandText = "SELECT Estado FROM Pedido WHERE nroPedido='" + nro + "';";
                    string estado;
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if(Lector.Read())
                        {
                            estado = (Lector["Estado"].ToString() == "En Proceso")?"Entregado":"En Proceso";
                            Lector.Close();
                            Comando.CommandText = "UPDATE Pedido SET Estado='" + estado + "' WHERE nroPedido='" + nro + "'";
                            Comando.ExecuteNonQuery();
                        }
                    }
                }
                Conexion.Close();
            }
        }
        public void AsignarPedidoCadete(int nro, int id)
        {
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
            {
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "UPDATE Pedido SET idCadeteAsignado='" + id + "' WHERE nroPedido='" + nro + "';";
                    Comando.ExecuteNonQuery();
                }
                Conexion.Close();
            }
        }
    }
}